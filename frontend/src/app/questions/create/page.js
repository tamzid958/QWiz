"use client";
import {
  FormContainer,
  SelectElement,
  TextFieldElement,
} from "react-hook-form-mui";
import { Typography } from "@mui/material";
import { LoadingButton } from "@mui/lab";
import { useEffect, useRef, useState } from "react";
import { useRouter } from "next/navigation";
import { requestApi } from "@/utils/axios.settings";
import { toast } from "react-toastify";
import useSWR from "swr";
import _ from "lodash";

const Page = () => {
  const editorRef = useRef();
  const { data } = useSWR({ url: "/Category" });
  const { data: questionType } = useSWR({ url: "/Question/Types" });
  const [loading, setLoading] = useState(false);
  const router = useRouter();
  const [description, setDescription] = useState("");
  const [editorLoaded, setEditorLoaded] = useState(false);
  const { CKEditor, ClassicEditor } = editorRef.current || {};

  useEffect(() => {
    editorRef.current = {
      CKEditor: require("@ckeditor/ckeditor5-react").CKEditor, //Added .CKEditor
      ClassicEditor: require("@ckeditor/ckeditor5-build-classic"),
    };
    setEditorLoaded(true);
  }, []);

  return (
    <>
      <Typography
        variant="h5"
        component="h5"
        className="font-bold text-blue-800"
      >
        Create new question
      </Typography>
      <div className="w-full">
        <FormContainer
          onSuccess={async (data) => {
            setLoading(true);
            await requestApi({
              method: "POST",
              url: "/Question",
              data: {
                title: data.title,
                description: description,
                questionType: data.questionType,
                categoryId: data.category,
              },
            }).then(({ error }) => {
              setLoading(false);
              error
                ? toast.error("Question Creation Failed")
                : toast.success("Question Created Successfully");
              !error && router.back();
            });
          }}
        >
          <div className="flex flex-col justify-between items-end gap-4">
            <SelectElement
              label="Category"
              name="category"
              options={(data ?? []).map((o) => ({ id: o.id, label: o.name }))}
              fullWidth
              required
            />

            <TextFieldElement
              name="title"
              required
              label="Title"
              fullWidth
              autoComplete="off"
            />
            <div className="w-full flex flex-col gap-1 h-60">
              <label>Description</label>
              {editorLoaded && (
                <CKEditor
                  editor={ClassicEditor}
                  config={{
                    toolbar: [
                      "bold",
                      "italic",
                      "bulletedList",
                      "numberedList",
                      "blockQuote",
                      "link",
                    ],
                  }}
                  data={description}
                  onChange={(event, editor) => {
                    setDescription(editor.getData());
                  }}
                />
              )}
            </div>
            <SelectElement
              label="Question Type"
              name="questionType"
              options={(questionType ?? []).map((o) => ({
                id: o,
                label: _.replace(o, /([A-Z])/g, " $1").trim(),
              }))}
              fullWidth
              required
            />
            <LoadingButton variant="contained" type="submit" loading={loading}>
              Submit
            </LoadingButton>
          </div>
        </FormContainer>
      </div>
    </>
  );
};
export default Page;
