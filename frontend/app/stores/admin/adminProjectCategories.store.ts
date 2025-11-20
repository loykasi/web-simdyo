import type { ProjectCategory } from "~/types/projectCategory.type";

export const useAdminProjectCategoriesStore = () => {
    const categories = useState<ProjectCategory[]>("adminProjectCategories");
    const pending = useState<boolean>("adminProjectCategoriesPending", () => true);

    async function fetch(signal: AbortSignal) {
        pending.value = true;
        useAPI<ProjectCategory[]>(`projects/categories`, {
            method: "GET",
            signal: signal
        }).then(res => {
            categories.value = res;
            pending.value = false;
        })
    }

    return {
        categories,
        pending,
        fetch,
    }
}