export interface ProjectReportResponse {
    id: string,
    reason: string,
    description: string,
    projectPublicId: string,
    username: string,
    createdAt: string
}

export interface ReportProjectRequest {
    reason: string,
    description: string,
}