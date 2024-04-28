"use client";

import { useEffect, useState } from "react";
import useSWR from "swr";
import { useRouter } from "next/navigation";
import {
  Accordion,
  AccordionDetails,
  AccordionSummary,
  Button,
  Table,
  TableBody,
  TableCell,
  TableHead,
  TablePagination,
  TableRow,
  Typography,
} from "@mui/material";
import { formatDate } from "@/utils/common";
import { ExpandMore, Visibility } from "@mui/icons-material";

const QuestionsByCategory = ({
  categoryId,
  reviewed,
  mode,
  isAddedToQuestionBank,
}) => {
  const [params, setParams] = useState({
    page: 1,
  });
  const router = useRouter();

  const { data } = useSWR({
    url: "/Question",
    params: {
      ...params,
      categoryId,
      reviewed,
      isAddedToQuestionBank,
    },
  });

  const [dialog, setDialog] = useState({ open: false, question: null });

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
                  onClick={() => {
                    if (mode === "review") {
                      router.push(`/reviews/view/${datum.id}`);
                    } else if (mode === "questionBank") {
                      router.push(`/question-bank/view/${datum.id}`);
                    }
                  }}
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

const CategoryAccordion = ({
  datum,
  expanded,
  onChange,
  reviewed,
  isAddedToQuestionBank,
  mode,
}) => (
  <Accordion expanded={expanded === datum.id} onChange={onChange}>
    <AccordionSummary expandIcon={<ExpandMore />}>
      <Typography sx={{ width: "33%", flexShrink: 0 }}>{datum.name}</Typography>
    </AccordionSummary>
    <AccordionDetails>
      {expanded === datum.id && (
        <QuestionsByCategory
          {...{ categoryId: expanded, reviewed, mode, isAddedToQuestionBank }}
        />
      )}
    </AccordionDetails>
  </Accordion>
);

export default CategoryAccordion;
