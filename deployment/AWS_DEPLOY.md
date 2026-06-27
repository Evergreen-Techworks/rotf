# ROTF / Ordinary Client — AWS deployment guide

Mirrors the `../egtw-main` pattern: EC2 Ubuntu + systemd + nginx + certbot, deployed via
GitHub Actions. The only extra vs. egtw is a second service (the world server) and the
raw game ports.

## Architecture on AWS
```
GitHub (main) → GitHub Actions (build .NET 8 + launcher) → rsync → EC2
  EC2 (Ubuntu 22.04):
    nginx (:80/:443, certbot) ──► rotf-appserver (127.0.0.1:8080)  [accounts/HTTP]
                              └──► web launcher    (127.0.0.1:3000)  [Next.js]
    rotf-worldserver (:2050 TCP, :843 policy)  ◄── Flash clients connect directly
    redis (127.0.0.1:6379)  [accounts, chars, vaults — the ONLY datastore]
```
**The game uses redis only — no RDS/Aurora.** Use a local redis on the box (simplest) or
ElastiCache for Redis. Your `egtw-main` RDS is unrelated and untouched.

## One-time EC2 setup
1. Launch **Ubuntu 22.04**, t3.small+, 20 GB. Elastic IP.
2. **Security group:** 22 (your IP), 80 + 443 (web/accounts), **2050 TCP** (game), **843
   TCP** (Flash policy), redis NOT exposed (localhost only).
3. SSH in, then:
   ```bash
   sudo apt update && sudo apt install -y redis-server nginx
   # .NET 8 runtime
   curl -fsSL https://dot.net/v1/dotnet-install.sh | bash -s -- --channel 8.0 --runtime aspnetcore --install-dir /usr/lib/dotnet
   sudo ln -s /usr/lib/dotnet/dotnet /usr/bin/dotnet
   sudo mkdir -p /opt/rotf/appserver /opt/rotf/worldserver && sudo chown -R ubuntu /opt/rotf
   ```
4. Install services + nginx:
   ```bash
   sudo cp deployment/rotf-appserver.service deployment/rotf-worldserver.service /etc/systemd/system/
   sudo systemctl daemon-reload && sudo systemctl enable rotf-appserver rotf-worldserver
   sudo cp deployment/nginx/rotf.conf /etc/nginx/sites-available/rotf
   sudo ln -s /etc/nginx/sites-available/rotf /etc/nginx/sites-enabled/ && sudo nginx -t && sudo systemctl reload nginx
   sudo certbot --nginx -d play.rotf.example     # your domain
   ```
5. In the deployed `/opt/rotf/appserver/server.json` set `bindAddress` `127.0.0.1`, port
   `8080`; in `/opt/rotf/worldserver/wServer.json` set the world `address` to your **public
   IP/domain** (clients connect there) and keep redis at `127.0.0.1:6379`.

## Deploy
- Add GitHub secrets `EC2_HOST`, `EC2_USER`, `EC2_SSH_KEY`, copy `deployment/deploy.yml` to
  `.github/workflows/deploy.yml`, push to `main`. It publishes both servers, rsyncs to
  `/opt/rotf/*`, and restarts the services.

## Client distribution
- The web launcher serves the **`WebMain.swf`** + the standalone Flash Projector download +
  a one-click `.bat`/launcher. The world `address` in `wServer.json` must be the public
  host so the SWF connects to the right server.

## Notes
- Build the client SWF (`client/build-flash.bat`) and host it via the launcher's `public/`.
- Scale: run multiple `rotf-worldserver` instances against one app server + redis later.
