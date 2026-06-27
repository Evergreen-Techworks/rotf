export default function Home() {
  return (
    <main style={{ maxWidth: 760, margin: "0 auto", padding: "64px 24px" }}>
      <h1 style={{ fontSize: 44, margin: 0, color: "#9fe7ff" }}>Ordinary Client</h1>
      <p style={{ fontSize: 18, opacity: 0.8 }}>Revenge of the Fallen — reborn.</p>

      <section style={{ marginTop: 40, padding: 24, background: "#141821", borderRadius: 12 }}>
        <h2 style={{ marginTop: 0 }}>Play</h2>
        <ol style={{ lineHeight: 1.8 }}>
          <li>Download the <strong>standalone Adobe Flash Projector</strong> (32.0.0.363).</li>
          <li>Download <a href="/WebMain.swf" style={{ color: "#9fe7ff" }}>WebMain.swf</a>.</li>
          <li>Trust the SWF folder (see the setup guide), open it in the Projector, register and play.</li>
        </ol>
        <p style={{ opacity: 0.7, fontSize: 14 }}>
          Ruffle is not supported (no raw TCP sockets). Use the standalone Projector.
        </p>
      </section>

      <section style={{ marginTop: 24, padding: 24, background: "#141821", borderRadius: 12 }}>
        <h2 style={{ marginTop: 0 }}>News</h2>
        <ul style={{ lineHeight: 1.8 }}>
          <li>Hunt the <strong>Illusionist</strong> for Fallen-tier loot.</li>
          <li>Custom dungeons, runes &amp; galactic zones coming soon.</li>
        </ul>
      </section>

      <footer style={{ marginTop: 40, opacity: 0.5, fontSize: 13 }}>
        Non-commercial fan project. RotMG and related assets belong to their owners.
      </footer>
    </main>
  );
}
