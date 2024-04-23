"use client";

import {
  FormContainer,
  SelectElement,
  TextFieldElement,
} from "react-hook-form-mui";
import { Typography } from "@mui/material";
import { LoadingButton } from "@mui/lab";
import { useState } from "react";
import { toast } from "react-toastify";
import { requestApi } from "@/utils/axios.settings";
import { useRouter } from "next/navigation";

const Page = () => {
  const [loading, setLoading] = useState(false);
  const router = useRouter();
  return (
    <>
      <Typography
        variant="h5"
        component="h5"
        className="font-bold text-blue-800"
      >
        Create new User
      </Typography>
      <div className="w-full">
        <FormContainer
          onSuccess={async (data) => {
            setLoading(true);
            await requestApi({
              method: "POST",
              url: "/Authentication/Register",
              data: {
                ...data,
                roles: [data.roles],
              },
            }).then(({ error }) => {
              error
                ? toast.error("User Creation Failed")
                : toast.success("User Created Successfully");
              !error && router.back();
            });
          }}
        >
          <div className="flex flex-col justify-between items-end gap-4">
            <TextFieldElement
              name="fullName"
              required
              label="Full Name"
              fullWidth
              autoComplete="off"
            />
            <TextFieldElement
              name="email"
              required
              label="Email"
              type="email"
              fullWidth
              autoComplete="off"
            />
            <TextFieldElement
              name="phoneNumber"
              required
              label="Phone Number"
              type="tel"
              fullWidth
              autoComplete="off"
            />
            <TextFieldElement
              name="password"
              required
              label="Password"
              type="password"
              fullWidth
              autoComplete="off"
            />
            <SelectElement
              label="Roles"
              name="roles"
              options={["QuestionSetter", "Reviewer", "Admin"].map((o) => ({
                label: o,
                id: o,
              }))}
              showChips
              showCheckbox
              fullWidth
              required
            />
            <LoadingButton variant="contained" type="submit" loading={loading}>
              Submit
            </LoadingButton>
          </div>
        </FormContainer>
      </div>
    </>
  );
};
export default Page;
