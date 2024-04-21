"use client"; // Error components must be Client Components

import { useEffect } from "react";
import { Button } from "@mui/material";

export default function Error({ error, reset }) {
  useEffect(() => {
    // Log the error to an error reporting service
    console.error(error);
  }, [error]);

  return (
    <div className="w-full flex flex-col items-center justify-center gap-2">
      <h2>Something went wrong!</h2>
      <Button
        variant="contained"
        onClick={
          // Attempt to recover by trying to re-render the segment
          () => reset()
        }
      >
        Try again
      </Button>
    </div>
  );
}
