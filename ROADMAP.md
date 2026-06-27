# Ordinary Client / ROTF — Build Roadmap

Phased plan from empty repo → playable locally → ROTF-themed → live on AWS.

---

## Phase 0 — Scaffold & base  ✅ DONE (2026-05-31)

- [x] Monorepo created (`server/ client/ assets/ web/ deployment/ docs/`)
- [x] Vendored working base: `realm-src` (C# server) + `realm-cli` (AS3 client)
- [x] ROTF sprite rip organized into `assets/sprites/` + contact sheet built
- [x] Confirmed config + data surface (ports, redis, resources XML, embedded sheets)

---

## Phase 1 — Vanilla stack playable locally  ⬅ NEXT (the "is it real?" milestone)

Goal: log into an unmodified copy and walk around a Nexus, proving the whole
client↔server↔redis loop works before we change anything.

**Server (C#) — ported to .NET 8 on Linux. ✅ App server builds, boots & serves.**
- [x] .NET 8 SDK installed to `~/.dotnet` (8.0.421).
- [x] All 4 projects converted to SDK-style net8.0 (originals kept as `*.csproj.netfx.bak`;
      shared settings in `server/Directory.Build.props`). Dropped vestigial refs
      (System.ServiceModel/WCF, PerformanceCounter, MEF — zero source usage) and the
      net472 System.* polyfills (built into net8). Fixes: `MimeMapping` helper
      (`server/MimeMapping.cs`), `GetValueOrDefault` disambiguation.
- [x] **Anna → HttpListener shim** (`server/Http/AnnaShim.cs`). The original net40 Anna
      HTTP lib needs `System.UriTemplate` (in the dead `System.ServiceModel`), which
      doesn't exist on net8. Anna was a thin HttpListener wrapper, so the shim restores
      the exact API with **no handler changes**; modern System.Reactive supplies Rx.
- [x] **Verified end-to-end**: `dotnet build` 0 errors → app server boots, connects
      redis, loads 214 tiles / 960 items / 1593 objects, listens on HTTP, and answers
      requests (`POST /account/register` → `<Error>Invalid email</Error>`; 404 routing OK).
- [x] **DB needs NO seeding** — redis auto-inits counters (`StringIncrement`). Verified:
      `POST /account/register` → `<Success/>`, `POST /account/verify` → full `<Account>` XML.
      (NOTE: the game uses **redis only**, not SQL/Aurora/RDS. On AWS → ElastiCache for
      Redis, or a systemd redis on the EC2 box. The egtw-main RDS is NOT used by the game.)
- [x] **World server (`wServer`) builds & runs**: loads 9 worlds + 33 behavior sets,
      RealmManager + logic/network tick loops running, Oryx spawning enemies,
      **`Listening on port 2050`**, joined the inter-server bus. ✅
  - ⚠️ Flash **socket-policy server on port 843** can't bind as non-root (privileged port).
    Non-fatal locally; for the client handshake run with `CAP_NET_BIND_SERVICE`/root, or
    handle via the Projector. Address during client bring-up / AWS deploy.

**→ Server backend is DONE on .NET 8 Linux. Only the client (Phase 1 client step) stands
between here and walking around in-world.**

Build/run (Linux):
```
export PATH="$HOME/.dotnet:$PATH" DOTNET_ROOT="$HOME/.dotnet"
redis-server --daemonize yes
cd server && dotnet build realm-src.sln -c Debug
cd server/bin/Debug/net8.0 && dotnet server.dll      # app server (port from server.json)
```
Local note: app server port set to 8080 for non-root testing (HttpListener can't bind
:80 without privileges); prod uses :80 behind nginx like egtw.

**Client (AS3 → SWF) — BUILDS. ✅** (`client/bin-debug/WebMain.swf`, ~2.4 MB, 0 errors)
- [x] Toolchain: **Apache Flex SDK 4.16.1** + **playerglobal 11.2** (Adobe's URL is dead;
      pulled from the Wayback Machine). Compiles fine under **Java 17**.
- [x] Font fix (one-time, committed): the 4 embedded fonts are **CFF OpenType** (`.otf`),
      which the SDK's Batik transcoder can't read (no `loca` table). Converted
      `MyriadPro/MyriadProBold.otf → .ttf` (otf2ttf/cu2qu) and repointed the `[Embed]`s in
      `src/com/company/ui/fonts/*.as`. Now transcodes cleanly.
- [x] **`client/build-client.bat`** — double-click on Windows: auto-downloads the SDK +
      playerglobal into `client/.buildsdk/`, then compiles with the validated args. Runs
      straight from a `\\wsl$` path. (A prebuilt `WebMain.swf` already exists from the
      Linux validation.)
- [ ] Point the client at the server host/port and connect.

**Play:**
- [ ] Download the **standalone Adobe Flash Projector** (`flashplayer_*_sa`).
- [ ] Open the SWF in Projector → register an account → create a character →
      spawn in the Nexus and move around. ✅ **Playable.**

---

## Phase 2 — Transform into Ordinary Client / ROTF

**Rebrand:**
- [ ] Client title/logo/menu → "Ordinary Client"; server name strings → ROTF.

**Import the ROTF sprites** (the asset pipeline):
- [ ] Map each of the 79 ripped sheets in `assets/sprites/` to a client sheet in
      `client/src/kabam/rotmg/assets/EmbeddedAssets_*Embed_.png` (match by cell
      size: 8×8 / 16×16 / 32×32 / 64×64; use the contact sheet to identify).
- [ ] Register each via `AssetLibrary.addImageSet("<sheet>", ..., W, H)` and the
      `EmbeddedAssets` / `AssetLoader` entries.
- [ ] Wire sprites to objects: in `server/common/resources/xml/EmbeddedData_*.xml`,
      set each `<Object>`'s `<Texture><File>` = sheet name, `<Index>` = cell.
      **Keep server XML and client XML/sheets aligned on object type IDs.**
- [ ] Investigate the stray face photo (`assets/sprites/raw/...`) — portrait/loading
      asset or easter egg; place or drop it.

**Re-author ROTF content** (reconstruct from surviving sources — see `docs/`):
- [ ] Custom dungeons, bosses, items, classes/runes ROTF was known for, authored as
      XML in `common/resources/xml/`.

Sources (no ROTF source survived; reconstruct from these):
- Fandom: https://revenge-of-the-fallen-rotmg.fandom.com/ and https://rotfserver.fandom.com/
- Wikidot: http://revengeofthefallen.wikidot.com/
- YouTube footage of original gameplay/loot.

---

## Phase 3 — Deploy on AWS (mirror `../egtw-main`)

egtw pattern = EC2 Ubuntu 22.04 → systemd services → nginx + certbot → GitHub
Actions (SSH/rsync). We add the world server + game port:

- [ ] EC2 Ubuntu 22.04; install **.NET 8 runtime** + **redis** (or AWS ElastiCache).
- [ ] systemd units in `deployment/`:
      `rotf-appserver` (HTTP, behind nginx) and `rotf-worldserver` (TCP :2050).
- [ ] **Security group:** 22 (your IP), 80/443 (web/account/launcher), **2050 TCP**
      (game), redis bound to localhost only.
- [ ] nginx reverse proxy + certbot SSL for the account/launcher site.
- [ ] `web/`: small **Next.js launcher/download site** (egtw-style) serving the
      Ordinary Client SWF + Flash Projector + a one-click launcher, plus account
      pages. This is the part that looks/deploys just like egtw-main.
- [ ] GitHub Actions deploy → `github.com/Evergreen-Techworks/rotf`.

---

## Open decisions (defaults chosen; change anytime)

- **Server build OS:** *default Linux + .NET 8* (matches egtw, cheaper EC2).
  Windows EC2 is more authentic/zero-port but pricier and off-pattern.
- **License:** server is GPL-3.0 — fine for running a server; matters only if
  distributing modified server binaries. `Zolmex/alloy-server` (MIT, + native C#
  OpenTK client, no Flash) is the pivot if Flash/GPL becomes a blocker.
