export interface UploadProjectRequest {
    title: string;
    description: string;
    category: string;
    projectFile: File;
    thumbnailFile: File;
}

export interface ProjectResponse {
    publicId: string;
    title: string;
    description: string;
    category: string;
    projectLink: string;
    thumbnailLink: string;
    username: string;
    createdAt: string;
}

export interface ProjectsResponse {
    projects: ProjectResponse[]
}