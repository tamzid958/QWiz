import { NextResponse } from "next/server";
import { getToken } from "next-auth/jwt";
import { checkAccess } from "@/utils/common";

export async function middleware(request, event) {
  const response = NextResponse.next();

  const token = await getToken({ req: request });
  const pathname = request.nextUrl.pathname;
  const roles = token?.user.roles ?? [];

  if (!token) {
    const signInUrl = new URL("/api/auth/signin", request.url);
    signInUrl.searchParams.set("callbackUrl", pathname);
    return NextResponse.redirect(signInUrl, {
      status: 303,
    });
  } else if (checkAccess(pathname, roles)) {
    return response;
  } else {
    return NextResponse.redirect(new URL(`/not-found`, request.url), {
      status: 303,
    });
  }
}

export const config = {
  matcher: [
    "/((?!api|_next/static|_next/image|favicon.ico|auth/login|not-found).*)",
  ],
};
