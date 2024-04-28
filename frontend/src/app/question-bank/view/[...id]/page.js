"use client";

import { useParams } from "next/navigation";
import useSWR from "swr";
import Loader from "@/components/Loader";
import {
  Button,
  Card,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  TextField,
  Typography,
} from "@mui/material";
import { Parser as HtmlToReactParser } from "html-to-react";
import { requestApi } from "@/utils/axios.settings";
import { toast } from "react-toastify";
import { createReviewersWithLog } from "@/utils/common";
import ReviewLog from "@/components/ReviewLog";

const AddToBankConfirmation = ({ open, approved, handleClose, mutate }) => {
  const params = useParams();

  return (
    <Dialog
      open={open}
      onClose={handleClose}
      PaperProps={{
        component: "form",
        onSubmit: async (event) => {
          event.preventDefault();

          await requestApi({
            method: "PATCH",
            url: `/Question/AddToBank/${params.id}/${approved}`,
          }).then(({ error }) => {
            error
              ? toast.error(
                  approved ? "Add question to bank failed" : "Rejection failed",
                )
              : toast.success(
                  approved ? `Added in question bank` : `Question is rejected`,
                );

            !error && mutate();
          });

          handleClose();
        },
      }}
    >
      <DialogTitle>Confirmation</DialogTitle>
      <DialogContent>
        <DialogContentText>
          {approved
            ? `By accepting this question for the set, you are confirming its
          inclusion. Proceeding will finalize this action.`
            : `By rejecting this question, you are confirming its
          exclusion. Proceeding will finalize this action.`}
        </DialogContentText>
      </DialogContent>
      <DialogActions>
        <Button onClick={handleClose}>Cancel</Button>
        <Button type="submit">Confirm</Button>
      </DialogActions>
    </Dialog>
  );
};

const Confirmation = ({
  approved,
  open,
  handleClose,
  mutate,
  reviewLogMutate,
}) => {
  const params = useParams();

  return (
    <Dialog
      open={open}
      onClose={handleClose}
      PaperProps={{
        component: "form",
        onSubmit: async (event) => {
          event.preventDefault();
          const formData = new FormData(event.currentTarget);
          const formJson = Object.fromEntries(formData.entries());
          const comment = formJson.comment;

          await requestApi({
            method: "POST",
            url: "/ReviewLog",
            data: {
              questionId: Number(params.id),
              isApproved: approved,
              comment: comment,
            },
          }).then(({ error }) => {
            error
              ? toast.error("Review creation  failed")
              : toast.success("Review successfully added");

            !error && mutate();
            !error && reviewLogMutate();
          });

          handleClose();
        },
      }}
    >
      <DialogTitle>Add Comment</DialogTitle>
      <DialogContent>
        <DialogContentText>
          Start by clearly summarizing your main points or reactions to the
          content. Provide specific examples or evidence to support your
          comments. Conclude by offering constructive feedback or suggestions
          for improvement.
        </DialogContentText>
        <TextField
          autoFocus
          margin="dense"
          id="comment"
          name="comment"
          label="Comment"
          fullWidth
          variant="standard"
        />
      </DialogContent>
      <DialogActions>
        <Button onClick={handleClose}>Cancel</Button>
        <Button type="submit">Confirm</Button>
      </DialogActions>
    </Dialog>
  );
};

const Page = () => {
  const params = useParams();
  const { data, mutate } = useSWR({ url: `/Question/${params.id}` });
  const htmlToReactParser = new HtmlToReactParser();

  const { data: reviewerData } = useSWR(
    data
      ? {
          url: "/Reviewer",
          params: {
            categoryId: data?.categoryId,
          },
        }
      : null,
  );

  const { data: reviewLogData } = useSWR(
    data
      ? {
          url: "/ReviewLog",
          params: {
            questionId: data?.id,
          },
        }
      : null,
  );

  const reviewerWithLog = createReviewersWithLog(reviewerData, reviewLogData);

  return !data ? (
    <div className="w-full">
      <Loader />
    </div>
  ) : (
    <div className="w-full flex justify-between gap-4">
      <Card className="flex flex-col gap-0.5 w-1/2 p-4 bg-gray-50">
        <Typography variant="h4" gutterBottom>
          <span className="font-bold">Question:</span> {data.title}
        </Typography>
        <Typography variant="subtitle2" gutterBottom>
          <span className="font-bold">Category:</span> {data.category.name}
        </Typography>
        <div className="max-h-96 overflow-auto text-sm">
          {htmlToReactParser.parse(data.description)}
        </div>
      </Card>
      <Card className="w-1/2 p-4 bg-gray-50">
        <Typography variant="h4" gutterBottom className="mb-2">
          Reviews
        </Typography>
        <ReviewLog
          reviewerWithLog={reviewerWithLog}
          isAddedToQuestionBank={data.isAddedToQuestionBank}
        />
      </Card>
    </div>
  );
};

export default Page;
