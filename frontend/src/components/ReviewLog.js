import { useState } from "react";
import {
  Step,
  StepContent,
  StepLabel,
  Stepper,
  Typography,
} from "@mui/material";
import { Cancel, Check, PanTool } from "@mui/icons-material";
import { formatDate } from "@/utils/common";
import Box from "@mui/material/Box";
import _ from "lodash";

const ReviewLog = ({ reviewerWithLog = [], isAddedToQuestionBank }) => {
  const [activeStep, setActiveStep] = useState(-1);
  return (
    <Stepper nonLinear orientation="vertical" activeStep={activeStep}>
      {_.concat(reviewerWithLog, [
        {
          id: 0,
          log:
            isAddedToQuestionBank == null
              ? null
              : {
                  isApproved: isAddedToQuestionBank,
                  comment: isAddedToQuestionBank ? "Accepted" : "Rejected",
                },
          fullName: "Question Bank",
        },
      ]).map((reviewer, index) => (
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
                {reviewer.id === 0
                  ? ""
                  : reviewer.log === null
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
      ))}
    </Stepper>
  );
};
export default ReviewLog;
