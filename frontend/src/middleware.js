import { NextResponse } from "next/server";
import { getToken } from "next-auth/jwt";

const protectedUrls = [
  {
    url: "/dashboard",
    access: ["Admin", "Reviewer", "QuestionSetter"],
  },
  {
    url: "/question-bank",
    access: ["Admin"],
  },
  {
    url: "/questions",
    access: ["QuestionSetter", "Admin"],
  },
  {
    url: "/reviews",
    access: ["Reviewer", "Admin"],
  },
  {
    url: "/categories",
    access: ["Admin"],
  },
  {
    url: "/user-management",
    access: ["Admin"],
  },
  {
    url: "/account",
    access: ["Admin", "Reviewer", "QuestionSetter"],
  },
];

const checkAccess = (pathname, userRoles) => {
  // Find the navigation link whose URL matches the start of the pathname
  const foundNavigationLink = protectedUrls.find((o) =>
    pathname.startsWith(o.url),
  );

  // If no matching navigation link is found, return false
  if (!foundNavigationLink) {
    return false;
  }

  // Find the intersection of the found navigation link's access array and userRoles
  const intersection = foundNavigationLink.access.filter((role) =>
    userRoles.includes(role),
  );

  // Check if the intersection array contains any elements
  return intersection.length > 0;
};

export async function middleware(request, event) {
  const response = NextResponse.next();

  const token = await getToken({
    req: request,
    secret: process.env.NEXTAUTH_SECRET,
  });
  const pathname = request.nextUrl.pathname;

  const roles = token?.user?.roles ?? [];

  if (!token) {
    const signInUrl = new URL("/api/auth/signin", request.url);
    signInUrl.searchParams.set("callbackUrl", pathname);
    return NextResponse.redirect(signInUrl, {
      status: 303,
    });
  }
  if (pathname === "/") {
    return NextResponse.redirect(new URL(`/dashboard`, request.url), {
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
    "/((?!api|_next/static|_next/image|favicon.ico|auth/login|auth/logout|not-found).*)",
  ],
};
