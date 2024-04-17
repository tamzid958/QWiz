"use client";

import { Button, Link, Paper } from "@mui/material";
import Logo from "/public/logo.png";
import Image from "next/image";
import { FormContainer, TextFieldElement } from "react-hook-form-mui";
import { signIn } from "next-auth/react";

const Page = () => {
  return (
    <div className="h-screen flex items-center justify-center">
      <Paper elevation={1} className="p-3 w-1/4 flex flex-col gap-5 bg-gray-50">
        <div className="flex flex-row justify-between items-center">
          <div className="flex flex-col gap-1">
            <p className="text-5xl text-blue-600">Hello,</p>
            <p className="text-5xl text-blue-900 font-black">welcome!</p>
          </div>
          <Image src={Logo} alt="logo" className="w-16 h-16" />
        </div>
        <FormContainer
          onSuccess={async (data) => {
            await signIn("credentials", {
              username: data.username,
              password: data.password,
              redirect: true,
              callbackUrl: "/",
            });
          }}
        >
          <div className="flex flex-col justify-between items-end gap-4">
            <TextFieldElement
              name="username"
              required
              label="Username"
              fullWidth
            />

            <TextFieldElement
              name="password"
              type="password"
              label="Password"
              required
              fullWidth
            />

            <div className="flex justify-between items-end w-full">
              <div className="flex gap-2">
                <Button variant="contained" type="submit">
                  Login
                </Button>
                <Button variant="outlined">Sign up</Button>
              </div>
              <Link href="#">Forget Password?</Link>
            </div>
          </div>
        </FormContainer>
      </Paper>
    </div>
  );
};

export default Page;
