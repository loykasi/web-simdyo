export interface ProjectComment {
  id: number;
  content: string;
  parentId: number | null;
  username: string;
  repliedUsername: string | null;
  createdAt: string;
  totalReplies: number;
  replies: ProjectComment[];
  replyCount: number;
}

export interface AddProjectCommentRequest {
  content: string;
  parentId: number | null;
  repliedUsername: string | null;
}
