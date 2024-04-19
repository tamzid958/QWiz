import { Link } from "@mui/material";

export default function NotFound() {
  return (
    <div className="w-full mx-auto flex flex-col items-center justify-center gap-4">
      <p className="text-3xl font-bold text-center">Resources not found!</p>
      <Link href="/">Return Home</Link>
    </div>
  );
}
