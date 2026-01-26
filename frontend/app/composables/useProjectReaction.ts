import type { projectReactionType } from "~/types/projectReaction.type";

export function useProjectReaction() {
    async function addReaction(projectPublicId: string, type: projectReactionType) {
        return useAPI(`projects/${projectPublicId}/reaction`, {
            method: "POST",
            body: {
                type: type
            }
        });
    }

    async function deleteReaction(projectPublicId: string) {
        return useAPI(`projects/${projectPublicId}/reaction`, {
            method: "DELETE"
        });
    }

    return {
        addReaction,
        deleteReaction
    }
}