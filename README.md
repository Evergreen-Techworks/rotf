# Ordinary Client — Open-Source ROTF Revival (RotMG Private Server)

[![License](https://img.shields.io/badge/license-GPL--3.0-14b8a6)](LICENSE)
[![Server](https://img.shields.io/badge/server-.NET%208-512BD4?logo=dotnet&logoColor=white)](server/)
[![Client](https://img.shields.io/badge/client-AS3%20%2F%20Flash-CC2927)](client/)
[![Database](https://img.shields.io/badge/db-redis-DC382D?logo=redis&logoColor=white)](https://redis.io)
[![Discord](https://img.shields.io/badge/discord-join-5865F2?logo=discord&logoColor=white)](https://discord.gg/uEKPPWz9k4)
[![Stars](https://img.shields.io/github/stars/Evergreen-Techworks/rotf?style=social)](https://github.com/Evergreen-Techworks/rotf/stargazers)
[![Buy Me A Coffee](https://img.shields.io/badge/buy_me_a_coffee-support-FFDD00?logo=buymeacoffee&logoColor=black)](https://www.buymeacoffee.com/egtw)

**Ordinary Client** is an open-source revival of **ROTF — "Revenge of the Fallen"**, a beloved ~2017–2018 **Realm of the Mad God** private server. ROTF's original source and assets never survived publicly, so this is a ground-up **reconstruction**: a modern RotMG server + client, re-skinned and re-content-ed into ROTF, with the lost dungeons, bosses, and items rebuilt from surviving wikis and gameplay footage.

> **TL;DR:** A full RotMG private server stack — C# server, AS3/Flash client, redis, AWS-ready — plus a pipeline that reverse-engineers ROTF's lost content from old YouTube videos. Clone it, run it locally, help rebuild the realm.

> *The name "Ordinary Client" is deliberately boring. The realm underneath it is not.*

💬 **Discord:** [discord.gg/uEKPPWz9k4](https://discord.gg/uEKPPWz9k4)  ·  🐙 **Repo:** [Evergreen-Techworks/rotf](https://github.com/Evergreen-Techworks/rotf)  ·  🗺️ **Status:** [ROADMAP.md](ROADMAP.md)

---

## ✨ What's inside

### The server
- **C# RotMG 7.0 server** (NR-CORE lineage), ported to **.NET 8** and runs on Linux.
- **App server** (HTTP, accounts/chars/vaults) + **world server** (TCP `:2050`, the live game).
- **redis-only** persistence — no SQL, no RDS. Local redis or AWS ElastiCache.
- One-command local boot via `./run-local.sh`.

### The client → *Ordinary Client*
- **AS3 / Flash 7.0 client**, built to a `WebMain.swf` and played in the standalone **Adobe Flash Projector** (Ruffle's AS3 support isn't complete enough to run the full RotMG client reliably — not recommended).
- ROTF sprite rip imported into the embedded sheets.
- Cross-platform build script (`client/build-client.sh` / `.bat`) that fetches the Flex SDK and compiles.

### The reconstructed ROTF content
- Lost **dungeons, bosses, items, classes & runes** re-authored as server XML in `server/common/resources/xml/`.
- Recovered specs live in `docs/` (`DUNGEON_SPECS.json`, `build_progress.md`, the `video-recovery/` analysis set).

### The recovery pipeline
- A reproducible toolchain (`scripts/`) that mines surviving sources — wikis, Reddit, dev blogs, and **YouTube footage** — frame-by-frame to recover boss attack patterns, taunts, loot tables, maps, and stats. See [`docs/VIDEO_RECOVERY_PIPELINE.md`](docs/VIDEO_RECOVERY_PIPELINE.md).

---

## 🧱 Repository layout

| Path | What |
|---|---|
| [`server/`](server/) | C# server: app server (HTTP) + world server (TCP `:2050`) + `DungeonGen` + `common`. Config in `server/server/server.json`, `server/wServer/wServer.json`. ROTF content authored in `common/resources/xml/`. |
| [`client/`](client/) | AS3 Flash client (becomes *Ordinary Client*). Sprite sheets under `src/kabam/rotmg/assets/`. Build with `build-client.sh` / `build-client.bat`. |
| [`web/`](web/) | Next.js launcher / account / download site (egtw-style). Serves the SWF + Flash Projector. |
| [`assets/`](assets/) | The ROTF sprite rip — raw atlases, index-renamed copies, and a `contact_sheet.png` inventory. |
| [`scripts/`](scripts/) | The content-recovery & build pipeline (video frame analysis, wiki mining, XML/map generators, importers). |
| [`docs/`](docs/) | Reconstruction notes & recovered specs. Start at [`docs/README.md`](docs/README.md). |
| [`deployment/`](deployment/) | systemd units, nginx, GitHub Actions deploy — mirrors the egtw AWS pattern. See [`deployment/AWS_DEPLOY.md`](deployment/AWS_DEPLOY.md). |

---

## 🚀 Quick Start (run it locally)

**Prereqs:** Linux / WSL, [.NET 8 SDK](https://dotnet.microsoft.com/download), `redis-server`, and the standalone [Adobe Flash Projector](https://www.adobe.com/support/flashplayer/debug_downloads.html) to play.

**1. Boot the backend** (redis + app server `:8080` + world server `:2050`):
```bash
./run-local.sh           # builds .NET 8, starts redis + both servers; Ctrl+C stops all
```

**2. Build the client** (produces `client/bin-debug/WebMain.swf`):
```bash
cd client && ./build-client.sh      # Linux/WSL  (build-client.bat on Windows)
```

**3. Play:** open `client/bin-debug/WebMain.swf` in the Flash Projector, register an account, and connect. Full walkthrough in [`docs/HOW_TO_TEST.md`](docs/HOW_TO_TEST.md).

**Make yourself admin** (for `/spawn` etc., after registering once):
```bash
./make-admin.sh you@email.com        # then re-login in the client
```

---

## 🗺️ Project status

The backend is real: the server builds & boots on .NET 8, accounts/chars/vaults persist in redis, worlds load, and the client connects over the network. The content-recovery pipeline has analyzed the surviving ROTF footage and the reconstructed dungeons/bosses are being authored in. **The current frontier is in [ROADMAP.md](ROADMAP.md)** — that's the best place to find what's done and what's open.

Want to help? See **[CONTRIBUTING.md](CONTRIBUTING.md)** for the build details, where content lives, and how the recovery pipeline works.

---

## ❓ FAQ

**Is this RotMG itself?**
No. It's an independent, non-commercial fan revival of a *private server* (ROTF) that shut down years ago. Realm of the Mad God and its original art/IP belong to their owners (DECA / formerly Deca Games & Kabam).

**Why reconstruct instead of restore?**
ROTF's source code and assets were never released and didn't survive publicly. The only record of its custom content is old wikis and YouTube videos — so the content is *reverse-engineered* from those, frame by frame.

**Why Flash in 2026?**
The original RotMG client is AS3/Flash, and the standalone Flash Projector still runs it perfectly. Ruffle isn't a substitute — its AS3 (AVM2) support isn't complete enough to run the full client reliably, so we don't recommend it. A modern client is a long-term maybe, not a blocker.

**Can I run my own server / fork this?**
Yes — that's the point. It's GPL-3.0 (see below). Run it, fork it, rebuild content with us. Just keep the copyleft and don't claim the parts you didn't write.

**How do I report a bug or pick up work?**
Open an issue, grab something off the [ROADMAP](ROADMAP.md), or hop into the [Discord](https://discord.gg/uEKPPWz9k4).

---

## 🤝 Contributing

PRs welcome — rebuild a boss, author a dungeon, improve the pipeline, or get the client rendering. Start with **[CONTRIBUTING.md](CONTRIBUTING.md)** and the [ROADMAP](ROADMAP.md).

---

## ⚖️ Provenance & License

This project is **GPL-3.0** (see [LICENSE](LICENSE)). It is built on:

- **Server** — vendored from [moistosaurus/realm-src](https://github.com/moistosaurus/realm-src) (GPL-3.0, NR-CORE / Nilly's Realm lineage). See [`server/VENDORED.md`](server/VENDORED.md).
- **Client** — vendored from [moistosaurus/realm-cli](https://github.com/moistosaurus/realm-cli) (AS3 RotMG client). See [`client/VENDORED.md`](client/VENDORED.md).

Running a server is unrestricted; distributing modified server binaries carries GPL copyleft obligations. **Realm of the Mad God and all original RotMG art, names, and IP belong to their respective owners.** This is a non-commercial fan project with no affiliation or endorsement.

---

<details>
<summary><strong>Keywords (for search indexing)</strong></summary>

rotf, revenge of the fallen, rotf rotmg, rotmg private server, open source rotmg server, realm of the mad god private server, rotmg server source, rotmg server reconstruction, ordinary client, rotmg flash client, AS3 rotmg client, realm-src, realm-cli, NR-CORE, nilly's realm, rotmg .NET 8 server, redis rotmg, rotmg AWS deploy, rotmg content recovery, rotmg dungeon reconstruction, private server revival, rotmg fan server
</details>
