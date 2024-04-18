"use client";

import { Button } from "@mui/material";
import { Logout } from "@mui/icons-material";
import { signOut } from "next-auth/react";
import { toast } from "react-toastify";

const Page = () => {
  return (
    <div>
      <Button
        endIcon={<Logout />}
        variant="contaied"
        className="bg-red-800 text-white hover:bg-red-900 hover:text-white"
        onClick={() =>
          signOut().then(() => toast.success("Successfully signed out"))
        }
      >
        Sign out
      </Button>
    </div>
  );
};

export default Page;
