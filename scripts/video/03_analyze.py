#!/usr/bin/env python3
"""ROTF dungeon recovery from gameplay video — analysis stage.

Sends a video (Gemini, native) or a batch of frames (OpenAI/Anthropic) to a vision model
with the ROTF extraction prompt and writes a structured dungeon spec JSON.

Setup:  pip install google-generativeai          # for --provider gemini  (EASIEST)
        pip install openai                        # for --provider openai (frames)
        pip install anthropic                     # for --provider anthropic (frames)
Keys:   GEMINI_API_KEY / OPENAI_API_KEY / ANTHROPIC_API_KEY in env.

Usage:
  # Gemini, whole video (recommended):
  GEMINI_API_KEY=... python3 03_analyze.py --provider gemini --dungeon Asgard \
      --video videos/asgard.mp4 --out docs/video-recovery/Asgard.json
  # Frames (extract first with 02_frames.sh), any image LLM:
  OPENAI_API_KEY=... python3 03_analyze.py --provider openai --dungeon Asgard \
      --frames frames/asgard --out docs/video-recovery/Asgard.json
"""
import os, sys, json, argparse, glob, base64

PROMPT = """You are analyzing gameplay footage of the RotMG private server "Revenge of the Fallen" to
reconstruct the dungeon "{dungeon}". RotMG is a top-down 2D bullet-hell. On screen: the player is
centered; bosses are large sprites; projectiles are small bright bullets; the MINIMAP (top-right)
shows the dungeon layout; the CHAT BOX (bottom-left) shows text — system messages, boss TAUNTS
(often "BossName: ..." or a colored announcement), and LOOT notifications.

Return ONLY this JSON (no prose):
{{
 "dungeon": "{dungeon}",
 "map": "from the minimap: shape (arena/linear/branching), #rooms, boss-room location, biome/tile colors/theme",
 "bosses": [{{"name":"", "phases":[""], "attacks":["concrete: count+shape+direction"], "minions":[""], "colorFlashes":"", "taunts":["VERBATIM chat text in quotes"]}}],
 "drops": ["item names seen in loot bags/chat"],
 "timestamps": "where key events occur",
 "uncertain": "anything not clearly readable"
}}
Transcribe chat/taunt text EXACTLY (do not paraphrase). Describe attacks concretely. If unsure, put
it in "uncertain" rather than inventing."""

def run_gemini(dungeon, video, model):
    import google.generativeai as genai, time
    genai.configure(api_key=os.environ["GEMINI_API_KEY"])
    f = genai.upload_file(video)
    while f.state.name == "PROCESSING":
        time.sleep(3); f = genai.get_file(f.name)
    resp = genai.GenerativeModel(model).generate_content([f, PROMPT.format(dungeon=dungeon)])
    return resp.text

def run_frames(dungeon, frames_dir, provider, model, max_frames):
    imgs = sorted(glob.glob(os.path.join(frames_dir, "*.png")))[:max_frames]
    if not imgs:
        sys.exit(f"no frames in {frames_dir}")
    b64 = [base64.b64encode(open(p, "rb").read()).decode() for p in imgs]
    if provider == "openai":
        from openai import OpenAI
        cli = OpenAI()
        content = [{"type": "text", "text": PROMPT.format(dungeon=dungeon)}] + \
            [{"type": "image_url", "image_url": {"url": f"data:image/png;base64,{b}"}} for b in b64]
        return cli.chat.completions.create(model=model, messages=[{"role": "user", "content": content}]).choices[0].message.content
    else:  # anthropic
        import anthropic
        cli = anthropic.Anthropic()
        content = [{"type": "image", "source": {"type": "base64", "media_type": "image/png", "data": b}} for b in b64] + \
            [{"type": "text", "text": PROMPT.format(dungeon=dungeon)}]
        return cli.messages.create(model=model, max_tokens=4096, messages=[{"role": "user", "content": content}]).content[0].text

def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--provider", choices=["gemini", "openai", "anthropic"], default="gemini")
    ap.add_argument("--dungeon", required=True)
    ap.add_argument("--video"); ap.add_argument("--frames")
    ap.add_argument("--model", default=None)
    ap.add_argument("--max-frames", type=int, default=40)
    ap.add_argument("--out", required=True)
    a = ap.parse_args()
    model = a.model or {"gemini": "gemini-2.0-flash", "openai": "gpt-4o", "anthropic": "claude-sonnet-4-6"}[a.provider]
    if a.provider == "gemini":
        if not a.video: sys.exit("--video required for gemini")
        txt = run_gemini(a.dungeon, a.video, model)
    else:
        if not a.frames: sys.exit("--frames required for openai/anthropic")
        txt = run_frames(a.dungeon, a.frames, a.provider, model, a.max_frames)
    # extract JSON
    import re
    m = re.search(r"\{.*\}", txt, re.S)
    data = json.loads(m.group(0)) if m else {"raw": txt}
    os.makedirs(os.path.dirname(a.out), exist_ok=True)
    json.dump(data, open(a.out, "w"), indent=1)
    print(f"wrote {a.out}\n", json.dumps(data, indent=1)[:800])

if __name__ == "__main__":
    main()
