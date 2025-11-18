import type { ProjectResponse, UploadProjectRequest } from "~/types/project.type";

export function useProject() {
    async function upload(payload: FormData) {
        return useAPI<ProjectResponse>("projects/upload", {
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