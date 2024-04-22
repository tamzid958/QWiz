"use client";
import {
  AutocompleteElement,
  FormContainer,
  TextFieldElement,
} from "react-hook-form-mui";
import { Typography } from "@mui/material";
import { LoadingButton } from "@mui/lab";
import { useEffect, useState } from "react";
import { useParams, useRouter } from "next/navigation";
import useSWR from "swr";
import _ from "lodash";
import { requestApi } from "@/utils/axios.settings";
import { toast } from "react-toastify";
import Loader from "@/components/Loader";

const Page = () => {
  const queryParams = useParams();

  const [params, setParams] = useState({});
  const [loading, setLoading] = useState(false);
  const router = useRouter();
  const { data, isValidating, isLoading } = useSWR({
    url: "/AppUser",
    params: { ...params, size: 100 },
  });
  const { data: prevData, mutate } = useSWR({
    url: `/Category/${queryParams.id}`,
  });

  const [selected, setSelected] = useState([]);

  useEffect(() => {
    setSelected(prevData?.reviewers.map((o) => o.appUser?.userName) ?? []);
  }, [prevData?.reviewers]);

  return !prevData ? (
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
        Update category
      </Typography>
      <div className="w-full">
        <FormContainer
          defaultValues={{
            name: prevData?.name,
            reviewers: prevData?.reviewers.map((o) => o.appUser?.userName),
          }}
          onSuccess={async (data) => {
            setLoading(true);
            await requestApi({
              method: "PUT",
              url: `/Category/${queryParams.id}`,
              data: {
                name: data.name,
                reviewers: {
                  appUserNames: data.reviewers,
                },
              },
            }).then(({ error }) => {
              setLoading(false);
              error
                ? toast.error("Category Creation Failed")
                : toast.success("Category Created Successfully");
              !error && mutate();
              !error && router.back();
            });
          }}
        >
          <div className="flex flex-col justify-between items-end gap-4">
            <TextFieldElement
              name="name"
              required
              label="Name"
              fullWidth
              autoComplete="off"
            />
            <div className="w-full">
              <AutocompleteElement
                label="Reviewers"
                loading={isValidating || isLoading}
                multiple
                name="reviewers"
                autocompleteProps={{ onChange: (e, v) => setSelected(v) }}
                textFieldProps={{
                  onChange: (event) =>
                    setParams({ userName: event.target.value }),
                }}
                options={
                  _.uniq(
                    data?.data?.map((datum) => datum.userName).concat(selected),
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
