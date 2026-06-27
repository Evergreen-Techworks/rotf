// Minimal HttpListener-based reimplementation of the slice of the "Anna" reactive
// HTTP-server library that the app/account server uses. The original Anna 1.0.1008 is
// a net40 package whose route matching depends on System.UriTemplate (in the old
// System.ServiceModel assembly), which does not exist on .NET 8. Anna was itself just a
// thin wrapper over System.Net.HttpListener, so this shim restores identical behaviour
// for the static routes this server registers, with no changes to any request handler.
//
// Surface intentionally matches Anna so the rest of the server compiles unchanged:
//   Anna.HttpServer(prefix) : IObservable<RequestContext> GET/POST(uri), IDisposable
//   Anna.Request.RequestContext : Request, Response(string|byte[]), Respond(string,int)
//   Anna.Request.Request : Url, HttpMethod, Headers, InputStream, ContentEncoding, ListenerRequest
//   Anna.Responses.Response : Headers["Content-Type"]=..., Send()

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Anna.Responses
{
    public class Response
    {
        private readonly HttpListenerResponse _resp;
        private readonly byte[] _body;

        public IDictionary<string, string> Headers { get; } =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public int StatusCode { get; set; } = 200;

        internal Response(HttpListenerResponse resp, byte[] body)
        {
            _resp = resp;
            _body = body;
        }

        public void Send()
        {
            try
            {
                foreach (var h in Headers)
                {
                    // Content-Type is a restricted header on HttpListenerResponse and
                    // must go through the dedicated property, not the Headers collection.
                    if (string.Equals(h.Key, "Content-Type", StringComparison.OrdinalIgnoreCase))
                        _resp.ContentType = h.Value;
                    else
                        try { _resp.Headers[h.Key] = h.Value; } catch { /* skip restricted */ }
                }

                _resp.StatusCode = StatusCode;
                if (_body != null)
                {
                    _resp.ContentLength64 = _body.Length;
                    _resp.OutputStream.Write(_body, 0, _body.Length);
                }
            }
            catch { /* client gone */ }
            finally
            {
                try { _resp.OutputStream.Close(); } catch { }
                try { _resp.Close(); } catch { }
            }
        }
    }
}

namespace Anna.Request
{
    public class Request
    {
        private readonly HttpListenerRequest _r;

        public Request(HttpListenerRequest r)
        {
            _r = r;
            var headers = new Dictionary<string, IEnumerable<string>>(StringComparer.OrdinalIgnoreCase);
            foreach (string key in _r.Headers.AllKeys)
            {
                if (key == null) continue;
                headers[key] = _r.Headers.GetValues(key) ?? Array.Empty<string>();
            }
            Headers = headers;
        }

        public HttpListenerRequest ListenerRequest => _r;
        public Uri Url => _r.Url;
        public string HttpMethod => _r.HttpMethod;
        public Stream InputStream => _r.InputStream;
        public Encoding ContentEncoding => _r.ContentEncoding;
        public IDictionary<string, IEnumerable<string>> Headers { get; }
    }

    public class RequestContext
    {
        private readonly HttpListenerContext _ctx;

        public Request Request { get; }

        internal RequestContext(HttpListenerContext ctx)
        {
            _ctx = ctx;
            Request = new Request(ctx.Request);
        }

        public Anna.Responses.Response Response(string body)
            => new Anna.Responses.Response(_ctx.Response, Encoding.UTF8.GetBytes(body ?? string.Empty));

        public Anna.Responses.Response Response(byte[] body)
            => new Anna.Responses.Response(_ctx.Response, body);

        public void Respond(string body, int statusCode = 200)
        {
            var r = Response(body);
            r.StatusCode = statusCode;
            r.Headers["Content-Type"] = "text/plain";
            r.Send();
        }
    }
}

namespace Anna
{
    using Anna.Request;

    public class HttpServer : IDisposable
    {
        private readonly HttpListener _listener = new HttpListener();
        private readonly Dictionary<string, Subject<RequestContext>> _routes =
            new Dictionary<string, Subject<RequestContext>>(StringComparer.OrdinalIgnoreCase);
        private volatile bool _running;

        public HttpServer(string prefix)
        {
            _listener.Prefixes.Add(prefix);
            _listener.Start();
            _running = true;
            Task.Run(AcceptLoop);
        }

        public IObservable<RequestContext> GET(string uri) => Route("GET", uri);
        public IObservable<RequestContext> POST(string uri) => Route("POST", uri);

        private IObservable<RequestContext> Route(string method, string uri)
        {
            var key = method + " " + uri;
            if (!_routes.TryGetValue(key, out var subject))
            {
                subject = new Subject<RequestContext>();
                _routes[key] = subject;
            }
            return subject;
        }

        private async Task AcceptLoop()
        {
            while (_running)
            {
                HttpListenerContext ctx;
                try { ctx = await _listener.GetContextAsync(); }
                catch { if (!_running) break; continue; }
                ThreadPool.QueueUserWorkItem(_ => Dispatch(ctx));
            }
        }

        private void Dispatch(HttpListenerContext ctx)
        {
            var key = ctx.Request.HttpMethod + " " + ctx.Request.Url.LocalPath;
            if (_routes.TryGetValue(key, out var subject))
            {
                try { subject.OnNext(new RequestContext(ctx)); }
                catch { TryClose(ctx, 500); }
            }
            else
            {
                TryClose(ctx, 404);
            }
        }

        private static void TryClose(HttpListenerContext ctx, int status)
        {
            try { ctx.Response.StatusCode = status; ctx.Response.Close(); } catch { }
        }

        public void Dispose()
        {
            _running = false;
            try { _listener.Stop(); } catch { }
            try { _listener.Close(); } catch { }
            foreach (var s in _routes.Values)
            {
                try { s.OnCompleted(); } catch { }
            }
        }
    }
}
