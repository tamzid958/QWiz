import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
} from "@mui/material";

const LockConfirmation = ({ lockoutEnabled, open = false, handleClose }) => {
  return (
    <Dialog
      open={open}
      onClose={() => {
        handleClose(false);
      }}
      aria-labelledby="alert-dialog-title"
      aria-describedby="alert-dialog-description"
    >
      <DialogTitle id="alert-dialog-title">
        {lockoutEnabled ? "Confirm Unblock" : "Confirm Block"}
      </DialogTitle>
      <DialogContent>
        <DialogContentText id="alert-dialog-description">
          {lockoutEnabled
            ? `Double - check! Unlocking the user will cause the user to able to login again.
            Are you absolutely sure?`
            : `Double - check! Locking the user will cause the user to unable to login.
            Are you absolutely sure?`}
        </DialogContentText>
      </DialogContent>
      <DialogActions>
        <Button onClick={() => handleClose(false)}>Cancel</Button>
        <Button
          onClick={() => handleClose(true)}
          autoFocus
          variant="contained"
          className="bg-red-800 text-white hover:bg-red-900 hover:text-white"
        >
          {lockoutEnabled ? "Unlock" : "Lock"}
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export default LockConfirmation;
