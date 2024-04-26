"use client";

import {
    Avatar,
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    DialogContentText,
    DialogTitle,
    TextField,
    Typography,
} from "@mui/material";
import {Logout, Password} from "@mui/icons-material";
import {signOut} from "next-auth/react";
import {toast} from "react-toastify";
import * as React from "react";
import {useState} from "react";
import {LoadingButton} from "@mui/lab";
import {requestApi} from "@/utils/axios.settings";
import useSWR from "swr";
import {getLettersFromString, textToDarkLightColor} from "@/utils/common";

const ChangePasswordDialog = ({user, open, handleClose}) => {
    const [loading, setLoading] = useState(false);

    return (
        <Dialog
            open={open}
            onClose={handleClose}
            PaperProps={{
                component: "form",
                onSubmit: async (event) => {
                    setLoading(true);

                    event.preventDefault();
                    const formData = new FormData(event.currentTarget);
                    const formJson = Object.fromEntries(formData.entries());

                    if (formJson.newPassword !== formJson.confirmPassword) {
                        toast.error("New Password and Confirm Password Mismatched");
                        setLoading(false);
                    } else {
                        await requestApi({
                            method: "PUT",
                            url: "/Authentication/ChangePassword",
                            data: {
                                oldPassword: formJson.oldPassword,
                                newPassword: formJson.newPassword,
                                userName: user.userName,
                            },
                        }).then(({error}) => {
                            setLoading(false);
                            error
                                ? toast.error("Password change failed")
                                : toast.success("Password changed successfully");

                            !error && handleClose();
                            !error &&
                            signOut().then(() => toast.success("Successfully signed out"));
                        });
                    }
                },
            }}
        >
            <DialogTitle>Change Password</DialogTitle>
            <DialogContent>
                <DialogContentText>
                    {`Your security is our priority. To enhance your account's protection,
            you can change your password here. Please enter your current password
            along with the new one. Ensure your new password is strong and unique,
            containing a mix of letters, numbers, and symbols. Once changed,
            remember to keep it confidential and avoid sharing it with anyone.
            Thank you for helping us keep your account secure.`}
                </DialogContentText>
                <TextField
                    autoFocus
                    type="password"
                    margin="dense"
                    id="oldPassword"
                    name="oldPassword"
                    label="Old Password"
                    fullWidth
                    variant="standard"
                    required
                />
                <TextField
                    autoFocus
                    type="password"
                    margin="dense"
                    id="newPassword"
                    name="newPassword"
                    label="New Password"
                    fullWidth
                    variant="standard"
                    required
                />
                <TextField
                    autoFocus
                    type="password"
                    margin="dense"
                    id="confirmPassword"
                    name="confirmPassword"
                    label="Confirm Password"
                    fullWidth
                    variant="standard"
                    required
                />
            </DialogContent>
            <DialogActions>
                <Button onClick={handleClose}>Cancel</Button>
                <LoadingButton type="submit" loading={loading}>
                    Change Password
                </LoadingButton>
            </DialogActions>
        </Dialog>
    );
};

const Page = () => {
    const [open, setOpen] = useState(false);
    const {data} = useSWR({url: "/Authentication/UserInfo"});

    const {data: rolesData} = useSWR(
        data && {
            url: `/Authentication/Roles/${data.id}`,
        },
    );

    return (
        <>
            <div className="flex gap-2">
                {data && (
                    <Button
                        startIcon={<Password/>}
                        variant="contained"
                        onClick={() => setOpen(true)}
                    >
                        Change Password
                    </Button>
                )}
                <Button
                    endIcon={<Logout/>}
                    variant="contaied"
                    className="bg-red-800 text-white hover:bg-red-900 hover:text-white"
                    onClick={() =>
                        signOut().then(() => toast.success("Successfully signed out"))
                    }
                >
                    Sign out
                </Button>
            </div>
            {data && (
                <div className="w-full mt-2">
                    <Typography variant="h5" className="font-bold">
                        Account Overview
                    </Typography>

                    <div className="flex items-center mt-5 gap-2">
                        <Avatar
                            sx={{width: 100, height: 100}}
                            style={{
                                backgroundColor: textToDarkLightColor(data.fullName ?? "")
                                    .darkColor,
                            }}
                        >
                            <Typography className="text-white font-bold font-sm">
                                {getLettersFromString(data.fullName ?? "N/A")}
                            </Typography>
                        </Avatar>
                        <Typography variant="h6" className="font-bold">
                            {data.fullName}
                        </Typography>
                    </div>
                    <div className="mt-5 pl-1">
                        <p className="flex gap-2">
                            <span className="font-bold">Username:</span>
                            <span>{data.userName}</span>
                        </p>

                        <p className="flex gap-2">
                            <span className="font-bold">Email:</span>
                            <span>{data.email}</span>
                        </p>

                        <p className="flex gap-2">
                            <span className="font-bold">Phone:</span>
                            <span>{data.phoneNumber}</span>
                        </p>

                        <p className="flex gap-2">
                            <span className="font-bold">Roles:</span>
                            <span>{rolesData?.join(",")}</span>
                        </p>
                    </div>
                </div>
            )}
            <ChangePasswordDialog
                user={data}
                open={open}
                handleClose={() => setOpen(false)}
            />
        </>
    );
};

export default Page;
