import type { ChangePasswordRequest } from "~/types/changePassword.type";
import type { Profile } from "~/types/profile.type";

export function useAccount() {
  async function changePassword(payload: ChangePasswordRequest) {
    return useAPI("account/change-password", {
      method: "POST",
      body: payload,
    });
  }

  async function getProfileDetail(userName: string) {
    return useAPI<Profile>(`account/${userName}`, {
      method: "GET",
    });
  }

  return {
    changePassword,
    getProfileDetail,
  };
}
