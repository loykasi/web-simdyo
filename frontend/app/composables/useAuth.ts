import { useAuthStore } from "~/stores/auth.store";
import type { AuthUser, RegisterRequest, ResetPasswordRequest } from "~/types/auth.type";

export function useAuth() {

    async function register(request: RegisterRequest) {
        return useAPI("auth/register", {
            method: "POST",
            body: request
        });
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

    const { user } = useAuthStore();

    async function me() {
        const nuxtApp = useNuxtApp();
        const isLogged = useCookie("isLogged", {
            default: () => false
        });
        
        if (user.value || !isLogged.value) {
            return;
        }
        
        console.log("get current user");

        await fetchUser().then(res => {
            user.value = {
                email: res.email,
                username: res.username,
                permissions: res.permissions
            } as AuthUser;
            
            isLogged.value = true;
            // console.log("SUCCESS get current user");
        }).catch(err => {
            console.error(err)
            // console.log("FAIL get current user");
        });
    }

    async function fetchUser() {
        const config = useRuntimeConfig();
        const isLogged = useCookie("isLogged", {
            default: () => false
        });

        try {
            return await useAPI<AuthUser>("auth/user", {
                method: "GET",
                headers: useRequestHeaders(['cookie']),
                onErrorAction: "doNothing"
            })
        } catch (error: any) {
            if (error.response.status !== 401) throw error;

            console.log("try refresh");
            try {
                await $fetch("auth/refresh", {
                    baseURL: `${config.public.baseUrl}`,
                    method: "POST",
                    headers: useRequestHeaders(['cookie']),
                });
                console.log("Token refreshed");
                
                return await useAPI<AuthUser>("auth/user", {
                    method: "GET",
                    headers: useRequestHeaders(['cookie']),
                    onErrorAction: "doNothing"
                })
            } catch (error: any) {
                throw error;
            }            
        }
    }

    return {
        register,
        isVerifying,
        isVerifySuccess,
        confirmEmail,
        forgotPassword,
        resetPassword,
        me
    }
}