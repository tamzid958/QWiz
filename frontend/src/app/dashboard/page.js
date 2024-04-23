"use client";

import { Card, CardContent, CardHeader } from "@mui/material";
import useSWR from "swr";

export default function Home() {
  const { data } = useSWR({
    url: "/Analytics",
  });

  return (
    <div className="w-full flex gap-4 flex-wrap justify-between">
      <Card className="flex-grow h-36">
        <CardHeader disableTypography title={"Category"} />
        <CardContent className="flex justify-center">
          <p className="text-5xl font-black">
            {data?.categoryCount ?? "loading"}
          </p>
        </CardContent>
      </Card>
      <Card className="flex-grow h-36">
        <CardHeader disableTypography title={"Questions"} />
        <CardContent className="flex justify-center">
          <p className="text-5xl font-black">
            {data?.questionCount ?? "loading"}
          </p>
        </CardContent>
      </Card>
      <Card className="flex-grow h-36">
        <CardHeader disableTypography title={"Reviewers"} />
        <CardContent className="flex justify-center">
          <p className="text-5xl font-black">
            {data?.reviewerCount ?? "loading"}
          </p>
        </CardContent>
      </Card>
      <Card className="flex-grow h-36">
        <CardHeader disableTypography title={"Question Setter"} />
        <CardContent className="flex justify-center">
          <p className="text-5xl font-black">
            {data?.questionSetterCount ?? "loading"}
          </p>
        </CardContent>
      </Card>
    </div>
  );
}
