"use client";

import { Link, Paper } from "@mui/material";
import Logo from "/public/logo.png";
import Image from "next/image";
import { FormContainer, TextFieldElement } from "react-hook-form-mui";
import { signIn } from "next-auth/react";
import { LoadingButton } from "@mui/lab";
import { useState } from "react";
import { toast, ToastContainer } from "react-toastify";

const Page = () => {
  const [loading, setLoading] = useState(false);
  return (
    <div className="h-screen flex items-center justify-center">
      <ToastContainer />
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
            setLoading(true);
            await signIn("credentials", {
              username: data.username,
              password: data.password,
              redirect: true,
              callbackUrl: "/",
            })
              .then(() => toast.success("Logged in successfully"))
              .catch((e) => setLoading(false));
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
              <LoadingButton
                variant="contained"
                type="submit"
                loading={loading}
              >
                Login
              </LoadingButton>
              <Link href="#">Forget Password?</Link>
            </div>
          </div>
        </FormContainer>
      </Paper>
    </div>
  );
};

export default Page;
