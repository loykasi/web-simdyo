import type { ChangePasswordRequest } from "~/types/changePassword.type";

export function useAccount() {
    async function changePassword(payload: ChangePasswordRequest) {
        return useAPI("account/change-password", {
            method: "POST",
            body: payload
        });
    }

    return {
        changePassword,
    }
}