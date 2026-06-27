# ROTF ripped sprite sheets — content catalog & import method

The 78 PNGs in `assets/sprites/raw/` (= `New folder.zip`) were ripped from the original
ROTF client SWF. They have **no names and no cell-size metadata** (filenames are
decompiler junk like `Nothing found _6A93_53_png_1.png`). The trailing `_NN_png_1` number
is the **index (idx)** used below. There is **no original `objects.xml`** mapping objects
to cells, so wiring is done by eye against this catalog + the wiki stats (`docs/rotf-wiki/`).

## How to import a sheet SAFELY (avoid the old black-screen)

The earlier black screen was caused by `scripts/sprites/import_sheets.py` registering **all
78 sheets at BOTH 8x8 and 16x16** — `AssetLibrary.addImageSet` eagerly slices the whole
bitmap into WxH cells, so a 256x1600 sheet at 8x8 = 6400 cells × 78 sheets × 2 = blowup.
**Do NOT bulk-register.** Instead, per sheet you actually use:

1. `cp assets/sprites/raw/<file> client/src/kabam/rotmg/assets/EmbeddedAssets_<name>Embed_.png`
2. Create `EmbeddedAssets_<name>Embed_.as` (BitmapAsset embed; see `EmbeddedAssets_rotfBossEmbed_.as`).
3. Add `public static var <name>Embed_:Class = EmbeddedAssets_<name>Embed_;` to `EmbeddedAssets.as`
   (must be **public** — `AssetLoader` accesses it cross-package).
4. Add `AssetLibrary.addImageSet("<name>",new EmbeddedAssets.<name>Embed_().bitmapData,CW,CH);`
   to `AssetLoader.addImages()` (use the **correct** cell size from the catalog).
5. Reference it from object/item XML: `<Texture><File><name></File><Index>cell</Index></Texture>`
   (cell index = row*cols + col, cols = sheetWidth/CW). Use plain `<Texture>` for these
   static-grid sheets, NOT `<AnimatedTexture>` (these aren't in the animated-char format).
6. Sync the client's embedded copy: `cp` the server XML to the matching
   `client/.../assets/EmbeddedData_*.dat`, then `client/build-client.sh`.

A sheet at correct cell size is cheap (e.g. 256x208 @16 = 208 cells). Register only what you use.

## Catalog (idx → content, confirmed cell size where checked)

REGISTERED so far:
- **idx 53** (256x208, **16x16**) → `rotfBoss`. Dungeon deco + characters. Notable cells:
  **51 = robed wizard** (→ The Illusionist), 52-55 wizard variants, 40-46 fiery demons/phoenixes,
  56-61 green frog creatures, 68-69 skulls, 72-77 blue aliens, 88-93 crystal/demon faces,
  104-109 demon trees, **124-126 white ghosts** (→ Illusion Mirage), 192-202 big-eyed creatures.

Identified, NOT yet registered (cell size = best estimate, confirm with an 8px/16px grid overlay):
- idx 33 (128x512) — **dense monster sheet** (blobs, ghosts, skulls, golems, goblins, jellyfish). ~8x8.
- idx 41 (256x256, 16x16) — red demon/imp humanoids, candelabras. Boss-tier enemies.
- idx 52 (256x512, 16x16) — castle towers + bearded wizard heads + blue/white spell orbs.
- idx 22 (256x256) — staff/wand weapon, black dragon, lightning-bolt projectiles. 8x8 + some 16.
- idx 64 (256x256, 8x8) — red ring/targeting items + red staff + green zombies.
- idx 16 (256x256, 8x8) — lightning-bolt projectiles.
- idx 20 (256x256, 16x16) — HUD/UI icons (lock, skull, swords, HP/MP labels).
- idx 63 (256x256, 16x16) — objects: crystals, trees, benches, lamps.
- idx 72 (256x880, 16x16) — emote faces (smileys, monkey, flame).
- idx 79 (128x2400, 8x8) — small items (fish/mushroom/rose) + biome ground tiles.
- idx 40 (256x1280) — environment: palm trees, sunflowers, beach/water, rope bridges, tents, fire.
- idx 42 (128x800, 8x8) — wall/floor TILES (brick, stone, wood, dirt, lava+flame).
- idx 75 (256x1600) — trees (cherry blossom/green), lamps, fences, snow/ice tiles.
- idx 70 (256x512) — skull-staffs, trees, directional arrows, blue beetles, slimes, lava.
- idx 53/56 effects, idx 58 (271x184) = a PHOTO (loading/credits image, not sprites), idx 05-08/11 = tiny color swatches.

Full dims for every idx: see `docs/rotf-wiki/parsed/sprite_catalog.json` (auto) or run
`identify` on `assets/sprites/raw/*`. Probe montages can be regenerated under `/tmp`.
