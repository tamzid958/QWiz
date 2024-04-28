import NextAuth from "next-auth";
import CredentialsProvider from "next-auth/providers/credentials";
import { requestApi } from "@/utils/axios.settings";
import _ from "lodash";
import { parseJwt } from "@/utils/common";

const refreshAccessToken = async ({ accessToken, refreshToken }) => {
  try {
    const response = await requestApi({
      baseURL: process.env.NEXT_SERVER_BASE_URL,
      url: "/Authentication/RefreshToken",
      method: "POST",
      data: {
        accessToken,
        refreshToken,
      },
    });
    const data = response?.data;
    if (!response?.error) {
      return {
        accessToken: data.token,
        refreshToken: data.refreshToken ?? refreshToken,
        accessTokenExpires: new Date() + parseJwt(data?.token).exp * 1000,
        user: {
          ..._.pick(data["appUser"], ["fullName", "id", "userName"]),
          roles: data.roles,
        },
      };
    }
  } catch (e) {
    console.error(e);
    return { error: "RefreshAccessTokenError" };
  }
};

const handler = NextAuth({
  secret: process.env.NEXTAUTH_SECRET,
  providers: [
    CredentialsProvider({
      name: "Credentials",
      credentials: {
        username: {
          label: "Username",
          type: "text",
          placeholder: "Enter Your Username",
        },
        password: {
          label: "Password",
          type: "password",
          placeholder: "Enter Your Password",
        },
      },
      async authorize(credentials, req) {
        const response = await requestApi({
          baseURL: process.env.NEXT_SERVER_BASE_URL,
          url: "/Authentication/Login",
          method: "POST",
          data: {
            username: credentials.username,
            password: credentials.password,
          },
        });

        if (!response.error) {
          return response.data;
        }
        return null;
      },
    }),
  ],
  debug: process.env.NODE_ENV !== "production",
  callbacks: {
    async jwt({ token, user, account }) {
      if (user && account) {
        token = {
          accessToken: user?.token,
          refreshToken: user?.refreshToken,
          accessTokenExpires: Date.now() + parseJwt(user?.token).exp * 1000,
          user: {
            ..._.pick(user["appUser"], ["fullName", "id", "userName"]),
            roles: user?.roles,
          },
        };
        return token;
      }
      console.log(Date.now(), token?.accessTokenExpires);
      if (Date.now() < token?.accessTokenExpires) {
        return token;
      }

      return refreshAccessToken({
        accessToken: token?.accessToken,
        refreshToken: token?.refreshToken,
      });
    },
    async session({ session, token }) {
      if (typeof token !== typeof undefined) {
        session.user = token?.user;
        session.accessToken = token?.accessToken;
      }
      return session;
    },
  },
  pages: {
    signIn: "/auth/login",
    signOut: "/auth/logout",
  },
});

export { handler as GET, handler as POST };
