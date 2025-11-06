
export function useLikeProject() {
    async function likeProject(projectPublicId: string) {
        return useAPI(`projects/${projectPublicId}/like`, {
            method: "POST"
        });
    }

    async function unlikeProject(projectPublicId: string) {
        return useAPI(`projects/${projectPublicId}/like`, {
            method: "DELETE"
        });
    }

    return {
        likeProject,
        unlikeProject
    }
}