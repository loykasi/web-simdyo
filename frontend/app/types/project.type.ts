export interface UploadProjectRequest {
    title: string;
    description: string;
    category: string;
    projectFile: File;
    thumbnailFile: File;
}

export interface ProjectResponse {
    id: string;
    title: string;
    description: string;
    category: string;
    projectLink: string;
    thumbnailLink: string;
}