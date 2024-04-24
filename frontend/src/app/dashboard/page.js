"use client";

import { Card, CardContent, CardHeader, Typography } from "@mui/material";
import useSWR from "swr";
import navigationLinks from "@/utils/navigation.link";
import _ from "lodash";
import { useSession } from "next-auth/react";

export default function Home() {
  const session = useSession();
  const roles = session?.data?.user?.roles ?? [];
  const { data } = useSWR({
    url: "/Analytics",
  });

  return (
    <div className="w-full">
      <div className="w-full flex gap-4 flex-wrap justify-around">
        <Card className="flex-grow h-36 bg-stone-950 text-white">
          <CardHeader disableTypography title={"Category"} />
          <CardContent className="flex justify-center">
            <p className="text-5xl font-black">{data?.categoryCount ?? ".."}</p>
          </CardContent>
        </Card>
        <Card className="flex-grow h-36 bg-stone-950 text-white">
          <CardHeader disableTypography title={"Questions"} />
          <CardContent className="flex justify-center">
            <p className="text-5xl font-black">{data?.questionCount ?? ".."}</p>
          </CardContent>
        </Card>
        <Card className="flex-grow h-36 bg-stone-950 text-white">
          <CardHeader disableTypography title={"Reviewers"} />
          <CardContent className="flex justify-center">
            <p className="text-5xl font-black">{data?.reviewerCount ?? ".."}</p>
          </CardContent>
        </Card>
        <Card className="flex-grow h-36 bg-stone-950 text-white">
          <CardHeader disableTypography title={"Question Setter"} />
          <CardContent className="flex justify-center">
            <p className="text-5xl font-black">
              {data?.questionSetterCount ?? ".."}
            </p>
          </CardContent>
        </Card>
      </div>
      <div className="mt-8">
        <Typography variant="h4">Instruction Manual</Typography>
        <div className="grid grid-cols-2 gap-4 mt-8">
          {navigationLinks
            .filter((n) => !n.hidden && n.description)
            .filter((o) => _.intersection(o.access, roles).length > 0)
            .map((instruction, index) => (
              <Card key={index} className="bg-gray-50">
                <CardHeader title={instruction.title} />
                <CardContent className="text-wrap text-justify text-sm">
                  {instruction.description}
                </CardContent>
              </Card>
            ))}
        </div>
      </div>
    </div>
  );
}
