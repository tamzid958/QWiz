"use client";

import {
  Button,
  Chip,
  Switch,
  Table,
  TableBody,
  TableCell,
  TableHead,
  TablePagination,
  TableRow,
} from "@mui/material";
import useSWR from "swr";
import { Add, Edit } from "@mui/icons-material";
import { useRouter } from "next/navigation";
import { useState } from "react";
import LockConfirmation from "@/components/LockConfirmation";
import { requestApi } from "@/utils/axios.settings";
import { toast } from "react-toastify";
import { useSession } from "next-auth/react";

const UserManagement = () => {
  const [params, setParams] = useState({
    page: 1,
    size: 20,
  });
  const { data, mutate } = useSWR({ url: "/AppUser", params });
  const router = useRouter();
  const [lockDialog, setLockDialog] = useState({
    dialog: false,
    id: null,
    lockoutEnabled: null,
  });
  const session = useSession();

  return (
    <>
      <Button
        variant="contained"
        startIcon={<Add />}
        onClick={() => router.push("/user-management/create")}
      >
        Create
      </Button>
      <div className="w-full">
        <Table>
          <TableHead className="bg-gray-300 border-1 border-black border-solid">
            <TableRow>
              <TableCell>ID</TableCell>
              <TableCell>Username</TableCell>
              <TableCell>Full Name</TableCell>
              <TableCell align="center">Role</TableCell>
              <TableCell>Email</TableCell>
              <TableCell>Phone Number</TableCell>
              <TableCell align="center">Blocked</TableCell>
              <TableCell align="center">Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {(data?.data ?? []).map((datum) => (
              <TableRow key={datum.id}>
                <TableCell>{datum.id}</TableCell>
                <TableCell>{datum.userName}</TableCell>
                <TableCell>{datum.fullName}</TableCell>
                <TableCell align="center">
                  {datum.userRoles.map((roles, index) => (
                    <Chip key={index} label={roles.role.name} />
                  ))}
                </TableCell>
                <TableCell>{datum.email}</TableCell>
                <TableCell>{datum.phoneNumber ?? "not available"}</TableCell>
                <TableCell align="center">
                  <Switch
                    checked={datum.lockoutEnabled}
                    disabled={session.data?.user.userName === datum.userName}
                    label="Blocked"
                    onChange={() =>
                      setLockDialog({
                        dialog: true,
                        id: datum.id,
                        lockoutEnabled: datum.lockoutEnabled,
                      })
                    }
                  />
                </TableCell>
                <TableCell align="center">
                  <Button
                    startIcon={<Edit />}
                    onClick={() =>
                      router.push(`/user-management/update/${datum.id}`)
                    }
                    variant="conatined"
                  >
                    Edit
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
      <LockConfirmation
        open={lockDialog.dialog}
        lockoutEnabled={lockDialog.lockoutEnabled}
        handleClose={async (confirmation) => {
          setLockDialog({
            dialog: false,
            id: null,
            lockoutEnabled: null,
          });
          if (confirmation) {
            await requestApi({
              url: `/Authentication/LockAccount/${lockDialog.id}`,
              method: "PATCH",
            }).then(({ data, error }) => {
              error
                ? toast.error("User Lock Failed")
                : toast.success(
                    data.lockoutEnabled
                      ? "User Locked Successfully"
                      : "User Unlocked Successfully",
                  );
              !error && mutate();
            });
          }
        }}
      />
    </>
  );
};

export default UserManagement;
