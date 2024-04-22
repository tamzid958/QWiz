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
import DeleteConfirm from "@/components/DeleteConfirmation";
import { useState } from "react";
import { requestApi } from "@/utils/axios.settings";
import { toast } from "react-toastify";
import { formatDate } from "@/utils/common";

const Category = () => {
  const { data, mutate } = useSWR({ url: "/Category" });
  const router = useRouter();
  const [deletion, setDeletion] = useState({
    dialog: false,
    id: null,
  });

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
              <TableCell align="center">Reviewers</TableCell>
              <TableCell>Created by</TableCell>
              <TableCell>Created at</TableCell>
              <TableCell align="center">Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {(data ?? []).map((datum) => (
              <TableRow key={datum.id}>
                <TableCell>{datum.id}</TableCell>
                <TableCell>{datum.name}</TableCell>
                <TableCell align="center">
                  {datum.reviewers.map((o) => (
                    <Chip
                      className="mx-1"
                      key={o.id}
                      label={o.appUser.fullName}
                      variant="outlined"
                    />
                  ))}
                </TableCell>
                <TableCell>{datum.createdBy.fullName}</TableCell>
                <TableCell>{formatDate(datum.createdAt)}</TableCell>

                <TableCell className="flex gap-2 justify-center items-center">
                  <Button
                    startIcon={<Delete />}
                    variant="conatined"
                    disabled={datum.name === "Uncategorized"}
                    onClick={() => setDeletion({ id: datum.id, dialog: true })}
                  >
                    Delete
                  </Button>
                  <Button
                    startIcon={<Edit />}
                    onClick={() =>
                      router.push(`/categories/update/${datum.id}`)
                    }
                    variant="conatined"
                    disabled={datum.name === "Uncategorized"}
                  >
                    Edit
                  </Button>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </div>
      <DeleteConfirm
        open={deletion.dialog}
        handleClose={async (confirmation) => {
          setDeletion({
            id: null,
            dialog: false,
          });
          if (confirmation) {
            await requestApi({
              url: `/Category/${deletion.id}`,
              method: "DELETE",
            }).then(({ error }) => {
              error
                ? toast.error("Category Deletion Failed")
                : toast.success("Category Deleted Successfully");
              !error && mutate();
            });
          }
        }}
      />
    </>
  );
};

export default Category;
