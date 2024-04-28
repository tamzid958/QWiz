"use client";

import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  Table,
  TableBody,
  TableCell,
  TableHead,
  TablePagination,
  TableRow,
} from "@mui/material";
import useSWR from "swr";
import { Add, Delete, Edit, Reviews } from "@mui/icons-material";
import { useRouter } from "next/navigation";
import DeleteConfirm from "@/components/DeleteConfirmation";
import { useState } from "react";
import { requestApi } from "@/utils/axios.settings";
import { toast } from "react-toastify";
import { createReviewersWithLog, formatDate } from "@/utils/common";
import { DialogBody } from "next/dist/client/components/react-dev-overlay/internal/components/Dialog";
import ReviewLog from "@/components/ReviewLog";

const CheckReview = ({
  questionId,
  categoryId,
  isAddedToQuestionBank,
  open = false,
  handleClose,
}) => {
  const { data: reviewerData } = useSWR(
    categoryId
      ? {
          url: "/Reviewer",
          params: {
            categoryId,
          },
        }
      : null,
  );

  const { data: reviewLogData } = useSWR(
    questionId
      ? {
          url: "/ReviewLog",
          params: {
            questionId,
          },
        }
      : null,
  );

  const reviewerWithLog = createReviewersWithLog(reviewerData, reviewLogData);

  return (
    <Dialog
      open={open}
      onClose={() => {
        handleClose(false);
      }}
      aria-labelledby="alert-dialog-title"
      aria-describedby="alert-dialog-description"
    >
      <DialogTitle id="alert-dialog-title">{"Review Log"}</DialogTitle>
      <DialogContent>
        <DialogContentText id="alert-dialog-description">
          Here, you can monitor and assess all recorded reviews within the
          system. Our comprehensive log provides detailed insights about the
          question.
        </DialogContentText>

        <DialogBody className="mt-4">
          <ReviewLog
            reviewerWithLog={reviewerWithLog}
            isAddedToQuestionBank={isAddedToQuestionBank}
          />
        </DialogBody>
      </DialogContent>
      <DialogActions>
        <Button onClick={() => handleClose(false)}>Close</Button>
      </DialogActions>
    </Dialog>
  );
};

const Questions = () => {
  const [params, setParams] = useState({
    page: 1,
    size: 20,
  });
  const { data, mutate } = useSWR({ url: "/Question", params });
  const router = useRouter();
  const [deletion, setDeletion] = useState({
    dialog: false,
    id: null,
  });

  const [checkReview, setCheckReview] = useState({
    dialog: false,
    questionId: null,
    categoryId: null,
    isAddedToQuestionBank: null,
  });

  return (
    <>
      <Button
        variant="contained"
        startIcon={<Add />}
        onClick={() => router.push("/questions/create")}
      >
        Create
      </Button>
      <div className="w-full">
        <Table>
          <TableHead className="bg-gray-300 border-1 border-black border-solid">
            <TableRow>
              <TableCell>ID</TableCell>
              <TableCell>Title</TableCell>
              <TableCell align="center">Type</TableCell>
              <TableCell align="center">Question Bank</TableCell>
              <TableCell align="center">Category</TableCell>
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
                <TableCell align="center">
                  {datum.isAddedToQuestionBank === null
                    ? "Pending"
                    : datum.isAddedToQuestionBank
                      ? "Yes"
                      : "No"}
                </TableCell>
                <TableCell align="center">{datum.category.name}</TableCell>
                <TableCell align="center">
                  {datum.createdBy?.fullName}
                </TableCell>
                <TableCell>{formatDate(datum.createdAt)}</TableCell>

                <TableCell>
                  {datum?.isAddedToQuestionBank == null && (
                    <Button
                      size="small"
                      startIcon={<Delete />}
                      variant="conatined"
                      onClick={() =>
                        setDeletion({ id: datum.id, dialog: true })
                      }
                    >
                      Delete
                    </Button>
                  )}
                  {datum.reviewLogs.length === 0 && (
                    <Button
                      size="small"
                      startIcon={<Edit />}
                      onClick={() =>
                        router.push(`/questions/update/${datum.id}`)
                      }
                      variant="conatined"
                    >
                      Edit
                    </Button>
                  )}
                  <Button
                    size="small"
                    startIcon={<Reviews />}
                    variant="conatined"
                    onClick={() =>
                      setCheckReview({
                        dialog: true,
                        questionId: datum.id,
                        categoryId: datum.categoryId,
                        isAddedToQuestionBank: datum.isAddedToQuestionBank,
                      })
                    }
                  >
                    Check
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
          rowsPerPage={data?.size ?? 20}
          rowsPerPageOptions={[20, 40, 60]}
          onRowsPerPageChange={(e) => {
            setParams({ size: parseInt(e.target.value, 10), page: 1 });
          }}
        />
      </div>
      <CheckReview
        questionId={checkReview.questionId}
        categoryId={checkReview.categoryId}
        isAddedToQuestionBank={checkReview.isAddedToQuestionBank}
        open={checkReview.dialog}
        handleClose={() => {
          setCheckReview({
            questionId: null,
            categoryId: null,
            dialog: false,
            isAddedToQuestionBank: null,
          });
        }}
      />
      <DeleteConfirm
        open={deletion.dialog}
        handleClose={async (confirmation) => {
          setDeletion({
            id: null,
            dialog: false,
          });
          if (confirmation) {
            await requestApi({
              url: `/Question/${deletion.id}`,
              method: "DELETE",
            }).then(({ error }) => {
              error
                ? toast.error("Question Deletion Failed")
                : toast.success("Question Deleted Successfully");
              !error && mutate();
            });
          }
        }}
      />
    </>
  );
};

export default Questions;
