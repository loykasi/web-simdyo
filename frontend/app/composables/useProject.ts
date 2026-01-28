import type { ProjectResponse, UploadProjectRequest, UploadProjectResponse } from "~/types/project.type";

export function useProject() {
    async function upload(payload: UploadProjectRequest) {
        return useAPI<UploadProjectResponse>("projects/upload", {
            method: "POST",
            body: payload
        });
    }

    async function update(publicId: string, payload: FormData) {
        return useAPI<ProjectResponse>(`projects/${publicId}`, {
            method: "PUT",
            body: payload
        });
    }

    return {
        upload,
        update
    }
}