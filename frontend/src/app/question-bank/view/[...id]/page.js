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

const Confirmation = ({ open, handleClose, mutate }) => {
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
            url: `/Question/AddToBank/${params.id}`,
          }).then(({ error }) => {
            error
              ? toast.error("Add question to bank failed")
              : toast.success("Added in question bank");

            !error && mutate();
          });

          handleClose();
        },
      }}
    >
      <DialogTitle>Confirmation</DialogTitle>
      <DialogContent>
        <DialogContentText>
          By accepting this question for the set, you are confirming its
          inclusion. Proceeding will finalize this action.
        </DialogContentText>
      </DialogContent>
      <DialogActions>
        <Button onClick={handleClose}>Cancel</Button>
        <Button type="submit">Confirm</Button>
      </DialogActions>
    </Dialog>
  );
};

const createReviewersWithLog = (reviewers, reviewLogs) => {
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
  const [dialogOpen, setDialogOpen] = useState(false);
  const [activeStep, setActiveStep] = useState(null);
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

  const handleApproval = () => {
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
        {data.isReadyForAddingQuestionBank &&
          data.IsAddedToQuestionBank === null && (
            <div className="mt-5">
              <Button
                variant="contained"
                color="primary"
                onClick={() => handleApproval()}
              >
                Add to Question Bank
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
                        ? "not reviewed yet"
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
          handleClose: () => {
            setDialogOpen(false);
          },
        }}
      />
    </div>
  );
};

export default Page;
