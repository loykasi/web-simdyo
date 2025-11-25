import type { Pagination } from "~/types/pagination.type";
import type { UserResponse } from "~/types/user.type";

export const useAdminUsersStore = () => {
    const users = useState<Pagination<UserResponse>>("adminUsers");
    const pending = useState<boolean>("adminUsersPending", () => true);
    const pageSize = 10;

    async function fetch(page: number, signal: AbortSignal) {
        pending.value = true;
        useAPI<Pagination<UserResponse>>(`users`, {
            method: "GET",
            query: {
                pageNumber: page,
                limit: pageSize,
            },
            signal: signal
        }).then(res => {
            users.value = res;
            pending.value = false;
        })
    }

    async function update(update: (pagination: Pagination<UserResponse>) => Pagination<UserResponse>) {
        users.value = update({...users.value} as Pagination<UserResponse>);
    }
    
    return {
        users,
        pending,
        pageSize,
        fetch,
        update
    }
}