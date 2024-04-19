"use client";

import {
  Button,
  Table,
  TableBody,
  TableCell,
  TableHead,
  TablePagination,
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

const Questions = () => {
  const [params, setParams] = useState({
    page: 1,
    size: 20,
  });
  const { data, mutate } = useSWR({ url: "/Question", params });
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
        onClick={() => router.push("/questions/create")}
      >
        Create
      </Button>
      <div className="w-full">
        <Table>
          <TableHead className="bg-gray-300 border-1 border-black border-solid">
            <TableRow>
              <TableCell>ID</TableCell>
              <TableCell>Title</TableCell>
              <TableCell align="center">Type</TableCell>
              <TableCell align="center">Category</TableCell>
              <TableCell align="center">Created by</TableCell>
              <TableCell>Created at</TableCell>
              <TableCell align="center">Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {(data?.data ?? []).map((datum) => (
              <TableRow key={datum.id}>
                <TableCell>{datum.id}</TableCell>
                <TableCell>{datum.title}</TableCell>
                <TableCell align="center">{datum.questionType}</TableCell>
                <TableCell align="center">{datum.category.name}</TableCell>
                <TableCell align="center">
                  {datum.createdBy?.fullName}
                </TableCell>
                <TableCell>{formatDate(datum.createdAt)}</TableCell>

                <TableCell className="flex gap-2 justify-center items-center">
                  <Button
                    startIcon={<Delete />}
                    variant="conatined"
                    onClick={() => setDeletion({ id: datum.id, dialog: true })}
                  >
                    Delete
                  </Button>
                  <Button
                    startIcon={<Edit />}
                    onClick={() => router.push(`/questions/update/${datum.id}`)}
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
      <DeleteConfirm
        open={deletion.dialog}
        handleClose={async (confirmation) => {
          setDeletion({
            id: null,
            dialog: false,
          });
          if (confirmation) {
            await requestApi({
              url: `/Question/${deletion.id}`,
              method: "DELETE",
            }).then(({ error }) => {
              error
                ? toast.error("Question Deletion Failed")
                : toast.success("Question Deleted Successfully");
              !error && mutate();
            });
          }
        }}
      />
    </>
  );
};

export default Questions;
