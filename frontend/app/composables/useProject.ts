import type { ProjectResponse, UploadProjectRequest } from "~/types/project.type";

export function useProject() {
    async function upload(payload: FormData) {
        return useAPI<ProjectResponse>("projects/upload", {
            method: "POST",
            body: payload
        });
    }

    return {
        upload,
    }
}