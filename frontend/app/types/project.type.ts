export interface UploadProjectRequest {
    title: string;
    shortDescription: string;
    description: string;
    category: string | null;
    projectLength: string;
    thumbnailLength: string;
}

export interface UploadProjectResponse {
    publicId: string,
    projectPresignedUrl: string,
    thumbnaiPresignedUrl: string
}

export interface ProjectResponse {
    publicId: string;
    title: string;
    shortDescription: string;
    description: string;
    category: string | null;
    projectLink: string;
    thumbnailLink: string;
    username: string;
    likeCount: number;
    okayCount: number;
    isBanned: boolean;
    createdAt: string;
    deletedAt: string | null;
}

export interface ProjectsResponse {
    projects: ProjectResponse[]
}