import { useAuthStore } from "~/stores/auth.store";
import type { LoginRequest, LoginResponse, User } from "~/types/auth.type";

export function useLogin() {
    const isLoginSuccess = ref(false);

    async function login(request: LoginRequest) {
        const toast = useToast();
        const { user, setExpiresAt } = useAuthStore();
        useAPI<LoginResponse>("auth/login", {
            method: "POST",
            body: request
        }).then((res) => {
            user.value = {
                email: res.email,
                username: res.userName
            } as User;
            
            setExpiresAt(res.expiresAt);
            isLoginSuccess.value = true;
            
            toast.add({
                title: "Success",
                description: "You have successfully logged in!",
                color: "success",
            })
        }).catch((err) => {
            console.log("error: " + err);
            toast.add({
                title: "Failed",
                description: "Something wrong!",
                color: "error",
            })
        })
    }

    async function logout() {
        const { user, stopRefreshTokenTimer } = useAuthStore();

        useAPI("auth/logout");
        user.value = null;
        stopRefreshTokenTimer();
    }

    return { isLoginSuccess, login }
}