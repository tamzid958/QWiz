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
import { useParams, useRouter } from "next/navigation";
import useSWR from "swr";
import Loader from "@/components/Loader";

const Page = () => {
  const params = useParams();

  const { data, mutate } = useSWR({ url: "/AppUser/" + params.id });

  const [loading, setLoading] = useState(false);
  const router = useRouter();

  return !data ? (
    <div className="w-full">
      <Loader />
    </div>
  ) : (
    <>
      <Typography
        variant="h5"
        component="h5"
        className="font-bold text-blue-800"
      >
        Update User
      </Typography>
      <div className="w-full">
        <FormContainer
          defaultValues={{
            ...data,
            roles: data.userRoles?.[0]?.role.name,
          }}
          onSuccess={async (data) => {
            setLoading(true);
            await requestApi({
              method: "PATCH",
              url: `/AppUser/${params.id}`,
              data: {
                ...data,
                roles: [data.roles],
              },
            }).then(({ error }) => {
              setLoading(false);
              error
                ? toast.error("User Update Failed")
                : toast.success("User Updated Successfully");
              !error && mutate();
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
