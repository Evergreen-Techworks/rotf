export const metadata = {
  title: "Ordinary Client — ROTF",
  description: "Revenge of the Fallen, reborn.",
};

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="en">
      <body style={{ margin: 0, background: "#0b0d12", color: "#e6edf3",
        fontFamily: "system-ui, sans-serif" }}>
        {children}
      </body>
    </html>
  );
}
