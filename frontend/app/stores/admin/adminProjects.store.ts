import type { Pagination } from "~/types/pagination.type";
import type { ProjectResponse } from "~/types/project.type";

export const useAdminProjectsStore = () => {
    const projects = useState<Pagination<ProjectResponse>>("adminProjects");
    const pending = useState<boolean>("adminProjectsPending", () => true);
    const pageSize = 10;

    async function fetch(page: number, signal: AbortSignal) {
        pending.value = true;
        useAPI<Pagination<ProjectResponse>>(`admin/projects`, {
            method: "GET",
            query: {
                page: page,
                limit: pageSize,
            },
            signal: signal
        }).then(res => {
            projects.value = res;
            pending.value = false;
        })
    }

    async function update(update: (pagination: Pagination<ProjectResponse>) => Pagination<ProjectResponse>) {
        projects.value = update({...projects.value} as Pagination<ProjectResponse>);
    }
    
    return {
        projects,
        pending,
        pageSize,
        fetch,
        update
    }
}