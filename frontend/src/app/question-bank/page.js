"use client";

import {
  Accordion,
  AccordionDetails,
  AccordionSummary,
  Button,
  FormControlLabel,
  Switch,
  Table,
  TableBody,
  TableCell,
  TableHead,
  TablePagination,
  TableRow,
  Typography,
} from "@mui/material";
import { ExpandMore, Visibility } from "@mui/icons-material";
import { useEffect, useState } from "react";
import useSWR from "swr";
import { formatDate } from "@/utils/common";
import { useRouter } from "next/navigation";

const QuestionsByCategory = ({ categoryId, reviewMode }) => {
  const [params, setParams] = useState({
    page: 1,
  });
  const { data } = useSWR({
    url: "/Question",
    params: {
      ...params,
      categoryId,
      isReadyForAddingQuestionBank: true,
      issAddedToQuestionBank: !reviewMode,
    },
  });
  const router = useRouter();

  useEffect(() => {
    if (categoryId) {
      setParams({ page: 1 });
    }
  }, [categoryId]);

  return (
    <div className="w-full">
      <Table>
        <TableHead className="bg-gray-300 border-1 border-black border-solid">
          <TableRow>
            <TableCell>ID</TableCell>
            <TableCell>Title</TableCell>
            <TableCell align="center">Type</TableCell>
            <TableCell align="center">Created by</TableCell>
            <TableCell>Created at</TableCell>
            <TableCell align="center">Actions</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {(data?.data ?? []).map((datum) => (
            <TableRow key={datum.id}>
              <TableCell>{datum.id}</TableCell>
              <TableCell>{datum.title}</TableCell>
              <TableCell align="center">{datum.questionType}</TableCell>
              <TableCell align="center">{datum.createdBy?.fullName}</TableCell>
              <TableCell>{formatDate(datum.createdAt)}</TableCell>
              <TableCell className="flex justify-center items-center gap-2">
                <Button
                  startIcon={<Visibility />}
                  variant="conatined"
                  onClick={() => router.push(`/question-bank/view/${datum.id}`)}
                >
                  View
                </Button>
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
      <TablePagination
        component="div"
        onPageChange={(e, page) => {
          setParams({ ...params, page });
        }}
        page={data?.page - 1 ?? 0}
        count={data?.totalRecords ?? 1}
        rowsPerPage={20}
        rowsPerPageOptions={[]}
      />
    </div>
  );
};

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
        label={reviewMode ? "Review Mode" : "Bank Mode"}
        onChange={handleChange}
      />
      <div className="w-full">
        {(data ?? []).map((datum) => (
          <Accordion
            key={datum.id}
            expanded={expanded === datum.id}
            onChange={() =>
              setExpanded(expanded === datum.id ? null : datum.id)
            }
          >
            <AccordionSummary expandIcon={<ExpandMore />}>
              <Typography sx={{ width: "33%", flexShrink: 0 }}>
                {datum.name}
              </Typography>
            </AccordionSummary>
            <AccordionDetails>
              {expanded === datum.id && (
                <QuestionsByCategory
                  {...{ categoryId: expanded, reviewMode }}
                />
              )}
            </AccordionDetails>
          </Accordion>
        ))}
      </div>
    </>
  );
};

export default Page;
