# How to test Ordinary Client / ROTF locally

You're on WSL; the **servers run in WSL**, and you open the **client in the Windows
standalone Flash Projector** (WSL2 forwards `localhost`, so the Projector reaches the
WSL servers at `127.0.0.1`).

> **Important:** a local SWF can't open the game socket (2050) until you mark it
> **trusted** via Flash Player's Trust folder — otherwise Flash blocks the socket
> (it wants a policy file on port 843, which we don't run). The Trust step (below)
> bypasses that, so no root / no 843 server needed. HTTP to the app server works
> because the server serves `crossdomain.xml`.

## 1. Start the servers (in WSL)

```bash
cd ~/rotf
./run-local.sh
```
This launches **redis**, the **app server** on `:8080` (accounts/login), and the
**world server** on `:2050` (gameplay). Leave it running; `Ctrl+C` stops everything.
You should see `Listening at address *:8080` and `Listening on port 2050`.

## 2. Build the client SWF (once)

A prebuilt `client/bin-debug/WebMain.swf` already exists (already copied to `C:\rotf` and
your **Downloads**). To rebuild after changes, on **Windows** double-click
**`client\build-flash.bat`** — it builds the SWF, copies it to your **Downloads**, and
trusts it there automatically (needs Java on PATH). (`build-client.bat` builds in place.)

## 3. Get the standalone Flash Player Projector (Windows, one-time)

Use **version 32.0.0.363** (or earlier) — the last build WITHOUT the EOL kill-switch that
blocks Flash content after Jan 2021. **Do NOT use 32.0.0.465+** (kill-switch). Prefer the
**content debugger** SA build — it shows AS3 errors, which helps debug the connection.

- Internet Archive (363 Win/Mac/Linux SA + debug): https://archive.org/details/flashplayer32_0r0_363_win_sa
- Collection: https://archive.org/details/standaloneflashplayers  /  https://archive.org/details/flash-projectors
- GitHub debug mirror: https://github.com/Grubsic/Adobe-Flash-Player-Debug-Downloads-Archive

Single `.exe`, no install. Needs Flash Player ≥ 11.2 (Starling/Stage3D); 363 is fine.
(Ruffle will NOT work — it can't do RotMG's TCP sockets.)

## 4. Copy the SWF to Windows and TRUST it (one-time)

Copy the SWF to `C:\rotf` **from WSL** (robust — avoids the `\\wsl$\<distro>` path; this
machine's distro is `Debian`, not `Ubuntu`):
```bash
mkdir -p /mnt/c/rotf && cp ~/rotf/client/bin-debug/WebMain.swf /mnt/c/rotf/
```
Then **trust** that folder so Flash allows the game socket — in **PowerShell** (one-time):
```powershell
$d="$env:APPDATA\Macromedia\Flash Player\#Security\FlashPlayerTrust"
New-Item -ItemType Directory -Force $d | Out-Null
"C:\rotf" | Out-File -Encoding ascii "$d\rotf.cfg"
```
(Re-run the WSL `cp` line whenever the SWF is rebuilt.)

## 5. Play

1. Open the Projector → **File ▸ Open** → `C:\rotf\WebMain.swf`.
2. The client points at `http://127.0.0.1:8080` (app server) and connects to the world
   server on `127.0.0.1:2050`.
3. **Register** an account (any email + password), **create a character**, and you should
   spawn in the **Nexus** and be able to walk around (WASD) and shoot (mouse).

Alternative to the Trust step: run the 843 policy server as root in WSL
(`sudo setcap cap_net_bind_service=+ep $(readlink -f ~/.dotnet/dotnet)` then restart) —
but the Trust folder is simpler.

## What works right now

- Full login/account/character flow (redis-backed).
- Vanilla world (Nexus/Realm/etc.), movement, combat, the base RotMG content.
- The **80 ROTF items** are loaded server-side (type IDs `0x8000+`). They currently use
  placeholder sprites (real ROTF art import is the next step) and drop via loot tables
  once dungeons are wired. To eyeball one now, an admin `/give` command (if enabled) can
  spawn an item by name.

## Fight the ROTF boss (once you're in-game)

1. Register + create a character in the client.
2. In WSL: `./make-admin.sh your@email.com`, then **re-login** in the client.
3. In-game press Enter and type: `/spawn The Illusionist` (or `/spawn Illusion Mirage` for
   an easy one). Kill it → ROTF loot drops. (Boss uses a placeholder sprite for now.)

## Troubleshooting

**Black screen but it says connected / you can type** (the current known issue):
see **`MORNING_TODO.md`** — the 3 things to check (is the menu black too? any
`SendFailure -> "reason"` on the server console? is this RDP/VM?). The GPU renderer is
forced on; also try right-click client → **Settings → Display → Enable hardware
acceleration**.

**Doesn't connect at all:**
- Confirm `run-local.sh` shows both `:8080` and `:2050` listening.
- From Windows, `curl http://127.0.0.1:8080/app/init` should return XML (proves the app
  server is reachable across the WSL boundary).
- Make sure no other process holds 8080/2050; `run-local.sh` auto-frees them on start.
- Account/login issues → check redis (`redis-cli ping` → PONG).
