"use client";
import {
  AutocompleteElement,
  FormContainer,
  TextFieldElement,
} from "react-hook-form-mui";
import { Typography } from "@mui/material";
import { LoadingButton } from "@mui/lab";
import { useState } from "react";
import { useRouter } from "next/navigation";
import useSWR from "swr";
import _ from "lodash";
import { requestApi } from "@/utils/axios.settings";
import { toast } from "react-toastify";

const Page = () => {
  const [params, setParams] = useState({});
  const [loading, setLoading] = useState(false);
  const router = useRouter();
  const { data, isValidating, isLoading } = useSWR({ url: "/AppUser", params });
  const [selected, setSelected] = useState([]);
  return (
    <>
      <Typography
        variant="h5"
        component="h5"
        className="font-bold text-blue-800"
      >
        Create new Category
      </Typography>
      <div className="w-full">
        <FormContainer
          onSuccess={async (data) => {
            setLoading(true);
            await requestApi({
              method: "POST",
              url: "/Category",
              data: {
                name: data.name,
                approver: {
                  appUserIds: data.approvers.map((o) => o.id),
                },
              },
            }).then(({ error }) => {
              error
                ? toast.error("Category Creation Failed")
                : toast.success("Category Created Successfully"),
                !error && router.back();
            });
          }}
        >
          <div className="flex flex-col justify-between items-end gap-4">
            <TextFieldElement
              name="name"
              required
              label="Full Name"
              fullWidth
              autoComplete="off"
            />
            <div className="w-full">
              <AutocompleteElement
                label="Approvers"
                loading={isValidating || isLoading}
                multiple
                name="approvers"
                autocompleteProps={{ onChange: (e, v) => setSelected(v) }}
                textFieldProps={{
                  onChange: (event) =>
                    setParams({ fullName: event.target.value }),
                }}
                options={
                  _.uniqBy(
                    data?.data
                      ?.map((datum) => ({
                        id: datum.id,
                        label: `${datum.fullName} (${datum.userName})`,
                      }))
                      .concat(selected),
                    "id",
                  ) ?? []
                }
                showCheckbox
                showChips
                required
              />
            </div>
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
