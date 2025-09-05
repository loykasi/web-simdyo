import { AuthState } from "~/stores/auth";
import type { LoginRequest, LoginResponse, User } from "~/types/auth.type";

export function useLogin() {
    const isLoginSuccess = ref(false);

    async function login(request: LoginRequest) {
        const toast = useToast();
        const { isLoggedIn, user } = AuthState();
        useAPI<LoginResponse>("auth/login", {
            method: "POST",
            body: request
        }).then((res) => {
            console.log("success: " + res);
            user.value = {
                email: res.email,
                username: res.username
            } as User;
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

    return { isLoginSuccess, login }
}