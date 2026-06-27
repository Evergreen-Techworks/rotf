#!/usr/bin/env python3
"""
Extract embedded data (DefineBinaryData) from a RotMG/ROTF client .swf.
RotMG clients embed game data as ByteArrayAsset XML (objects, ground, etc.) in
DefineBinaryData (tag 87) blobs, named via SymbolClass (tag 76).

Usage: python3 scripts/swf_extract.py <client.swf> <out_dir>
Outputs every embedded blob; XML blobs get .xml, others .bin. Prints a manifest.
"""
import sys, os, zlib, struct

def decompress_swf(data):
    sig = data[:3]
    if sig == b'FWS':            # uncompressed
        return data
    if sig == b'CWS':            # zlib
        return b'FWS' + data[3:8] + zlib.decompress(data[8:])
    if sig == b'ZWS':            # LZMA
        import lzma
        # SWF LZMA: 4-byte magic, 4-byte file len, 4-byte uncompressed size, 5-byte props, stream
        props = data[12:17]
        comp = data[17:]
        dec = lzma.LZMADecompressor(format=lzma.FORMAT_RAW,
                                    filters=[{"id": lzma.FILTER_LZMA1, "dict_size": 1 << 23}])
        # rebuild a raw lzma stream using props
        import struct as _s
        return b'FWS' + data[3:8] + dec.decompress(props + comp)
    raise ValueError(f"unknown SWF signature {sig!r}")

def parse_tags(body):
    """Yield (tag_code, tag_data) after skipping the SWF header (rect + frame info)."""
    # skip rect
    nbits = body[0] >> 3
    rect_bits = 5 + nbits * 4
    rect_bytes = (rect_bits + 7) // 8
    pos = rect_bytes + 4  # + frame rate (2) + frame count (2)
    while pos < len(body):
        if pos + 2 > len(body): break
        rec = struct.unpack_from("<H", body, pos)[0]; pos += 2
        code = rec >> 6
        length = rec & 0x3F
        if length == 0x3F:
            if pos + 4 > len(body): break
            length = struct.unpack_from("<I", body, pos)[0]; pos += 4
        tag = body[pos:pos+length]; pos += length
        yield code, tag
        if code == 0:  # End
            break

def main():
    if len(sys.argv) < 3:
        print(__doc__); sys.exit(1)
    swf, out = sys.argv[1], sys.argv[2]
    os.makedirs(out, exist_ok=True)
    raw = open(swf, "rb").read()
    data = decompress_swf(raw)
    body = data[8:]  # after FWS + version(1) + filelen(4) = 8 bytes

    names = {}        # charId -> symbol name (from SymbolClass)
    blobs = {}        # charId -> bytes (from DefineBinaryData)
    for code, tag in parse_tags(body):
        if code == 76:  # SymbolClass
            cnt = struct.unpack_from("<H", tag, 0)[0]; p = 2
            for _ in range(cnt):
                cid = struct.unpack_from("<H", tag, p)[0]; p += 2
                end = tag.index(b'\x00', p)
                names[cid] = tag[p:end].decode("utf-8", "replace"); p = end + 1
        elif code == 87:  # DefineBinaryData
            cid = struct.unpack_from("<H", tag, 0)[0]
            blobs[cid] = tag[6:]  # skip charId(2) + reserved(4)

    manifest = []
    for cid, payload in blobs.items():
        nm = names.get(cid, f"bin_{cid}")
        safe = nm.replace(".", "_").replace("/", "_").replace(":", "_")
        is_xml = payload[:200].lstrip()[:1] in (b'<',)
        ext = "xml" if is_xml else "bin"
        fp = os.path.join(out, f"{safe}.{ext}")
        open(fp, "wb").write(payload)
        manifest.append((nm, len(payload), ext))
    manifest.sort(key=lambda x: -x[1])
    print(f"extracted {len(blobs)} binary blobs ({sum(1 for m in manifest if m[2]=='xml')} XML) -> {out}\n")
    for nm, sz, ext in manifest:
        print(f"  {sz:>9}  {ext}  {nm}")

if __name__ == "__main__":
    main()
