"use client";

import { useEffect } from "react";
import { useSession } from "next-auth/react";
import { usePathname, useRouter } from "next/navigation";
import _ from "lodash";
import Loader from "@/components/Loader";

const publicPages = ["/auth/login"];

const AuthWrapper = ({ children }) => {
  const session = useSession();
  const router = useRouter();
  const pathname = usePathname();

  useEffect(() => {
    if (
      session?.status === "unauthenticated" &&
      !_.includes(publicPages, pathname)
    ) {
      router.push("/auth/login");
    }
  }, [pathname, router, session]);

  if (pathname.startsWith("/auth")) {
    return <>{children}</>;
  }

  return (session.status === "loading" || session.data == null) &&
    !_.includes(publicPages, pathname) ? (
    <Loader />
  ) : (
    <>{children}</>
  );
};

export default AuthWrapper;
