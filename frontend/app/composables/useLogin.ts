import { useAuthStore } from "~/stores/auth.store";
import type { LoginRequest, LoginResponse, User } from "~/types/auth.type";

export function useLogin() {
    async function login(request: LoginRequest) {
        const { user, setExpiresAt } = useAuthStore();
        return useAPI<LoginResponse>("auth/login", {
            method: "POST",
            body: request
        });
    }

    async function fetchProfile() {
        const { user } = useAuthStore();

        useAPI<User>("auth/profile", {
            method: "GET",
        }).then(res => {
            user.value = {
                email: res.email,
                username: res.username
            } as User;
            // console.log("Fetch profile successful");
        }).catch(err => {
            // console.log(err)
        });
    }

    async function logout() {
        const { user } = useAuthStore();

        useAPI("auth/logout", {
            method: "POST"
        });
        user.value = null;

        navigateTo('/');
    }

    return { login, fetchProfile, logout }
}