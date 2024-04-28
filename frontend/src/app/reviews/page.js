"use client";

import { FormControlLabel, Switch } from "@mui/material";
import { useState } from "react";
import useSWR from "swr";
import CategoryAccordion from "@/components/CategoryAccordion";

const Page = () => {
  const { data } = useSWR({ url: "/Category" });

  const [expanded, setExpanded] = useState(null);
  const [reviewMode, setReviewMode] = useState(false);

  const handleChange = (event) => {
    setReviewMode(event.target.checked);
  };

  return (
    <>
      <FormControlLabel
        checked={reviewMode}
        control={<Switch />}
        label={reviewMode ? "Reviewed" : "Under Review"}
        onChange={handleChange}
      />
      <div className="w-full">
        {(data ?? []).map((datum) => (
          <CategoryAccordion
            mode="review"
            key={datum.id}
            expanded={expanded}
            datum={datum}
            onChange={() =>
              setExpanded(expanded === datum.id ? null : datum.id)
            }
            reviewed={reviewMode}
          />
        ))}
      </div>
    </>
  );
};

export default Page;
