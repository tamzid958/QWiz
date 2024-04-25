import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
} from "@mui/material";

const LockConfirmation = ({ open = false, handleClose }) => {
  return (
    <Dialog
      open={open}
      onClose={() => {
        handleClose(false);
      }}
      aria-labelledby="alert-dialog-title"
      aria-describedby="alert-dialog-description"
    >
      <DialogTitle id="alert-dialog-title">Confirm Block</DialogTitle>
      <DialogContent>
        <DialogContentText id="alert-dialog-description">
          Double-check! Locking the user will cause the user to unable to login.
          Are you absolutely sure?
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
          Lock
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export default LockConfirmation;
