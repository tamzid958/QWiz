"use client";

import { useParams } from "next/navigation";
import useSWR from "swr";
import Loader from "@/components/Loader";
import { Button, Typography } from "@mui/material";
import { Parser as HtmlToReactParser } from "html-to-react";

const Page = () => {
  const params = useParams();
  const { data } = useSWR({ url: `/Question/${params.id}` });
  const htmlToReactParser = new HtmlToReactParser();

  return !data ? (
    <div className="w-full">
      <Loader />
    </div>
  ) : (
    <div className="w-full flex justify-between gap-4">
      <div className="flex flex-col gap-0.5 w-1/2 border-2 rounded p-2 border-gray-300">
        <Typography variant="h4" gutterBottom>
          <span className="font-bold">Question:</span> {data.title}
        </Typography>
        <Typography variant="subtitle2" gutterBottom>
          <span className="font-bold">Category:</span> {data.category.name}
        </Typography>
        <Typography
          variant="body2"
          gutterBottom
          className="max-h-96 overflow-auto"
        >
          {htmlToReactParser.parse(data.description)}
        </Typography>
        <div className="flex gap-2 mt-5">
          <Button variant="contained" color="primary" onClick={() => {}}>
            Approve
          </Button>
          <Button variant="contained" color="error" onClick={() => {}}>
            Reject
          </Button>
        </div>
      </div>
      <div className="w-1/2 border-2 border-gray-300 p-2 rounded">
        <Typography variant="h4" gutterBottom>
          Reviews
        </Typography>
      </div>
    </div>
  );
};

export default Page;
