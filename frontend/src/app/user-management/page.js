"use client";

import {
  Button,
  Chip,
  Table,
  TableBody,
  TableCell,
  TableHead,
  TablePagination,
  TableRow,
} from "@mui/material";
import useSWR from "swr";
import { Add } from "@mui/icons-material";
import { useRouter } from "next/navigation";
import { useState } from "react";

const UserManagement = () => {
  const [params, setParams] = useState({
    page: 1,
    size: 20,
  });
  const { data } = useSWR({ url: "/AppUser", params });
  const router = useRouter();

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
    </>
  );
};

export default UserManagement;
