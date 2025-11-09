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
    likeCount: number;
    createdAt: string;
    deletedAt: string | null;
}

export interface ProjectsResponse {
    projects: ProjectResponse[]
}