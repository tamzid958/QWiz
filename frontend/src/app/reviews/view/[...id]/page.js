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
import { formatDate } from "@/utils/common";
import { Check, PanTool } from "@mui/icons-material";
import { Cancel } from "axios";

const Confirmation = ({ approved, open, handleClose, mutate }) => {
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
            url: "/ApprovalLog",
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

const createApproversWithLog = (approvers, approvalLogs) => {
  return approvers && approvalLogs
    ? approvers.map((o) => ({
        id: o.id,
        fullName: o.appUser.fullName,
        log: approvalLogs.find((x) => x.createdById === o.appUserId) ?? null,
      }))
    : [];
};

const Page = () => {
  const params = useParams();
  const { data, mutate } = useSWR({ url: `/Question/${params.id}` });
  const htmlToReactParser = new HtmlToReactParser();
  const [approved, setApproved] = useState(false);
  const [dialogOpen, setDialogOpen] = useState(false);
  const { data: approverData } = useSWR(
    data
      ? {
          url: "/Approver",
          params: {
            categoryId: data?.categoryId,
          },
        }
      : null,
  );

  const { data: approvalLogData } = useSWR(
    data
      ? {
          url: "/ApprovalLog",
          params: {
            questionId: data?.id,
          },
        }
      : null,
  );
  console.log(createApproversWithLog(approverData, approvalLogData));
  const handleApproval = (approved) => {
    setApproved(approved);
    setDialogOpen(true);
  };

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
        {data.isAddedToQuestionBank == null && (
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

        <Stepper orientation="vertical">
          {createApproversWithLog(approverData, approvalLogData).map(
            (approver) => (
              <Step key={approver.id}>
                <StepLabel
                  StepIconComponent={
                    approver.log === null
                      ? PanTool
                      : approver.log.isApproved
                        ? Check
                        : Cancel
                  }
                  StepIconProps={{
                    className: `w-6 h-6 p-0.5 rounded-full ${approver.log === null ? "bg-yellow-500" : approver.log.isApproved ? "bg-green-500" : "bg-red-800"} text-white`,
                  }}
                  optional={
                    <Typography variant="caption">
                      {approver.log === null
                        ? "not reviewed yet"
                        : formatDate(approver.log.createdAt)}
                    </Typography>
                  }
                >
                  {approver.fullName}
                </StepLabel>
                <StepContent>
                  <Typography>
                    {approver.log === null
                      ? "review is still in pending"
                      : approver.log.comment}
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
