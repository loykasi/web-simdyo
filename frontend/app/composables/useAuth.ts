import type { RegisterRequest, ResetPasswordRequest } from "~/types/auth.type";

export function useAuth() {

    const isRegisterSuccess = ref(false);

    async function register(request: RegisterRequest) {
        const toast = useToast();
        useAPI("auth/register", {
            method: "POST",
            body: request
        }).then((res) => {
            console.log("success: " + res);
            isRegisterSuccess.value = true;
        }).catch((err) => {
            console.log("error: " + err);
            toast.add({
                title: "Failed",
                description: "Something wrong!",
                color: "error",
            })
        })
    }

    const isVerifying = ref(true);
    const isVerifySuccess = ref(false);

    async function confirmEmail(token: string, email: string) {
        useAPI("auth/confirm-email", {
            method: "POST",
            body: {
                token: token,
                email: email
            }
        }).then((res) => {
            console.log("success: " + res);
            isVerifySuccess.value = true;
        }).catch((err) => {
            console.log("error: " + err);
        }).finally(() => {
            isVerifying.value = false;
        })
    }

    async function forgotPassword(email: string) {
        return useAPI("auth/forgot-password", {
            method: "POST",
            body: {
                email
            }
        })
    }

    async function resetPassword(payload: ResetPasswordRequest) {
        return useAPI("auth/reset-password", {
            method: "POST",
            body: payload
        })
    }

    return {
        isRegisterSuccess,
        register,
        isVerifying,
        isVerifySuccess,
        confirmEmail,
        forgotPassword,
        resetPassword
    }
}