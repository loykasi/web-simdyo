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
    category: string | null;
    projectLink: string;
    thumbnailLink: string;
    username: string;
    likeCount: number;
    isBanned: boolean;
    createdAt: string;
    deletedAt: string | null;
}

export interface ProjectsResponse {
    projects: ProjectResponse[]
}