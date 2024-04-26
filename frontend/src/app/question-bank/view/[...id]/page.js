"use client";

import {useParams} from "next/navigation";
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
    Typography,
} from "@mui/material";
import {Parser as HtmlToReactParser} from "html-to-react";
import {useState} from "react";
import {requestApi} from "@/utils/axios.settings";
import {toast} from "react-toastify";
import {createReviewersWithLog} from "@/utils/common";
import ReviewLog from "@/components/ReviewLog";

const Confirmation = ({open, handleClose, mutate}) => {
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
                    }).then(({error}) => {
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

const Page = () => {
    const params = useParams();
    const {data, mutate} = useSWR({url: `/Question/${params.id}`});
    const htmlToReactParser = new HtmlToReactParser();
    const [dialogOpen, setDialogOpen] = useState(false);
    const {data: reviewerData} = useSWR(
        data
            ? {
                url: "/Reviewer",
                params: {
                    categoryId: data?.categoryId,
                },
            }
            : null,
    );

    const {data: reviewLogData} = useSWR(
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
            <Loader/>
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
            </Card>
            <Card className="w-1/2 p-4 bg-gray-50">
                <Typography variant="h4" gutterBottom className="mb-2">
                    Reviews
                </Typography>

                <ReviewLog reviewerWithLog={reviewerWithLog}/>
            </Card>
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
