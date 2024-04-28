"use client";

import { FormControlLabel, Switch } from "@mui/material";
import { useState } from "react";
import useSWR from "swr";
import CategoryAccordion from "@/components/CategoryAccordion";

const Page = () => {
  const { data } = useSWR({ url: "/Category" });

  const [expanded, setExpanded] = useState(null);

  const [isAddedToQuestionBank, setIsAddedToQuestionBank] = useState(true);

  const handleChange = (event) => {
    setIsAddedToQuestionBank(event.target.checked);
  };

  return (
    <>
      <FormControlLabel
        checked={isAddedToQuestionBank}
        control={<Switch />}
        label={isAddedToQuestionBank ? "Added" : "Rejected"}
        onChange={handleChange}
      />
      <div className="w-full">
        {(data ?? []).map((datum) => (
          <CategoryAccordion
            mode="questionBank"
            key={datum.id}
            expanded={expanded}
            datum={datum}
            onChange={() =>
              setExpanded(expanded === datum.id ? null : datum.id)
            }
            isAddedToQuestionBank={isAddedToQuestionBank}
          />
        ))}
      </div>
    </>
  );
};

export default Page;
