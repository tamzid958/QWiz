"use client";

import {
  Button,
  Chip,
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableRow,
} from "@mui/material";
import useSWR from "swr";
import { Add, Delete, Edit } from "@mui/icons-material";
import { useRouter } from "next/navigation";

const Category = () => {
  const { data } = useSWR({ url: "/Category" });
  const router = useRouter();
  console.log(data);
  return (
    <>
      <Button
        variant="contained"
        startIcon={<Add />}
        onClick={() => router.push("/categories/create")}
      >
        Create
      </Button>
      <div className="w-full">
        <Table>
          <TableHead className="bg-gray-300 border-1 border-black border-solid">
            <TableRow>
              <TableCell>ID</TableCell>
              <TableCell>Name</TableCell>
              <TableCell>Approvers</TableCell>
              <TableCell>Created By</TableCell>
              <TableCell>Created At</TableCell>
              <TableCell align="center">Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {(data ?? []).map((datum) => (
              <TableRow key={datum.id}>
                <TableCell>{datum.id}</TableCell>
                <TableCell>{datum.name}</TableCell>
                <TableCell>
                  {datum.approvers.map((o) => (
                    <Chip
                      key={o.id}
                      label={o.appUser.fullName}
                      variant="outlined"
                    />
                  ))}
                </TableCell>
                <TableCell>{datum.createdBy.fullName}</TableCell>
                <TableCell>{datum.createdAt}</TableCell>
                <TableCell className="flex gap-2 justify-center items-center">
                  <Button startIcon={<Delete />} variant="conatined">
                    Delete
                  </Button>
                  <Button startIcon={<Edit />} variant="conatined">
                    Edit
                  </Button>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </div>
    </>
  );
};

export default Category;
