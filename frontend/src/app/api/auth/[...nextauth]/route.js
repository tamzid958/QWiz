import NextAuth from "next-auth";
import CredentialsProvider from "next-auth/providers/credentials";
import { requestApi } from "@/utils/axios.settings";
import _ from "lodash";

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
          url: "/Authentication/Login",
          method: "POST",
          data: credentials,
        });

        if (!response.error) {
          return response.data;
        }
        return null;
      },
    }),
  ],
  debug: true,
  callbacks: {
    async jwt({ token, user }) {
      if (typeof user !== typeof undefined) {
        token = {
          accessToken: user?.token,
          user: {
            ..._.pick(user["appUser"], [
              "email",
              "fullName",
              "id",
              "phoneNumber",
              "userName",
            ]),
          },
        };
      }
      return token;
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
    signOut: "/auth/signout",
  },
});

export { handler as GET, handler as POST };
