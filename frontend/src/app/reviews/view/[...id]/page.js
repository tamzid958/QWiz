"use client";

import { useParams } from "next/navigation";
import useSWR from "swr";
import Loader from "@/components/Loader";
import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  Step,
  StepContent,
  StepLabel,
  Stepper,
  TextField,
  Typography,
} from "@mui/material";
import { Parser as HtmlToReactParser } from "html-to-react";
import { useState } from "react";
import { requestApi } from "@/utils/axios.settings";
import { toast } from "react-toastify";
import Box from "@mui/material/Box";
import { formatDate, sortByBooleanProperty } from "@/utils/common";
import { Check, PanTool } from "@mui/icons-material";
import { Cancel } from "axios";
import { useSession } from "next-auth/react";

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

export const createReviewersWithLog = (reviewers, reviewLogs) => {
  return reviewers && reviewLogs
    ? reviewers.map((o) => ({
        id: o.id,
        appUserId: o.appUserId,
        fullName: o.appUser.fullName,
        log: reviewLogs.find((x) => x.createdById === o.appUserId) ?? null,
      }))
    : [];
};

const Page = () => {
  const params = useParams();
  const { data, mutate } = useSWR({ url: `/Question/${params.id}` });
  const htmlToReactParser = new HtmlToReactParser();
  const [approved, setApproved] = useState(false);
  const [dialogOpen, setDialogOpen] = useState(false);
  const [activeStep, setActiveStep] = useState(null);
  const { data: userData } = useSession();
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

  const { data: reviewLogData, mutate: reviewLogMutate } = useSWR(
    data
      ? {
          url: "/ReviewLog",
          params: {
            questionId: data?.id,
          },
        }
      : null,
  );

  const handleApproval = (approved) => {
    setApproved(approved);
    setDialogOpen(true);
  };

  const reviewerWithLog = createReviewersWithLog(reviewerData, reviewLogData);

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
        <div className="max-h-96 overflow-auto text-sm">
          {htmlToReactParser.parse(data.description)}
        </div>
        {reviewerWithLog.find((o) => o.appUserId === userData.user.id)?.log ===
          null && (
          <div className="flex gap-2 mt-5">
            <Button
              variant="contained"
              color="primary"
              onClick={() => handleApproval(true)}
            >
              Approve
            </Button>
            <Button
              variant="contained"
              color="error"
              onClick={() => handleApproval(false)}
            >
              Reject
            </Button>
          </div>
        )}
      </div>
      <div className="w-1/2 border-2 border-gray-300 p-2 rounded">
        <Typography variant="h4" gutterBottom className="mb-2">
          Reviews
        </Typography>

        <Stepper nonLinear orientation="vertical" activeStep={activeStep}>
          {sortByBooleanProperty(reviewerWithLog, "review.log.isApproved").map(
            (reviewer, index) => (
              <Step key={reviewer.id}>
                <StepLabel
                  onClick={() => setActiveStep(index)}
                  StepIconComponent={
                    reviewer.log === null
                      ? PanTool
                      : reviewer.log.isApproved
                        ? Check
                        : Cancel
                  }
                  StepIconProps={{
                    className: `w-6 h-6 p-0.5 rounded-full ${
                      reviewer.log === null
                        ? "bg-yellow-500"
                        : reviewer.log.isApproved
                          ? "bg-green-500"
                          : "bg-red-800"
                    } text-white cursor-pointer`,
                  }}
                  optional={
                    <Typography variant="caption">
                      {reviewer.log === null
                        ? "time will be shown"
                        : formatDate(reviewer.log.createdAt)}
                    </Typography>
                  }
                >
                  {reviewer.fullName}
                </StepLabel>
                <StepContent>
                  <Typography>
                    {reviewer.log === null
                      ? "review is still in pending"
                      : reviewer.log.comment}
                  </Typography>
                  <Box sx={{ mb: 2 }}></Box>
                </StepContent>
              </Step>
            ),
          )}
        </Stepper>
      </div>
      <Confirmation
        {...{
          open: dialogOpen,
          mutate,
          reviewLogMutate,
          approved,
          handleClose: () => {
            setDialogOpen(false);
          },
        }}
      />
    </div>
  );
};

export default Page;
