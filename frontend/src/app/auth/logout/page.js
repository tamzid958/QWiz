"use client";

import Loader from "@/components/Loader";
import { useEffect } from "react";
import { signOut } from "next-auth/react";
import { useRouter } from "next/navigation";

const Page = () => {
  const router = useRouter();
  useEffect(() => {
    signOut().then(() => router.push("/auth/login"));
  }, [router]);
  return <Loader />;
};

export default Page;
