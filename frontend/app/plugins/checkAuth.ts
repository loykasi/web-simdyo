import { callWithNuxt } from "#app";
import { useAuthStore } from "~/stores/auth.store"
import type { AuthUser } from "~/types/auth.type";
import { appendResponseHeader } from 'h3';
import type { H3Event } from 'h3';

export default defineNuxtPlugin(async () => {
    const { user } = useAuthStore();

    async function getUser() {
        const isLogged = useCookie("isLogged", {
            default: () => false
        });
        
        if (user.value || !isLogged.value) {
            return;
        }
        
        console.log("get current user")
        await useAPI<AuthUser>("auth/user", {
            method: "GET",
            headers: useRequestHeaders(['cookie']),
            onErrorAction: "refreshTokenAndRetry"
        }).then(res => {
            user.value = {
                email: res.email,
                username: res.username,
                permissions: res.permissions
            } as AuthUser;
            
            isLogged.value = true;
            // console.log("SUCCESS get current user");
        }).catch(err => {
            console.log("FAIL get current user");
        });

        // console.log("DONE get current user");
    }

    const nuxtApp = useNuxtApp();

    function getHeader() {
        const cookie = useRequestHeaders(['cookie']);
        console.log(cookie);
        return cookie;
    }

    async function fetchUser() {
        const config = useRuntimeConfig();
        const cookieHeader = useRequestHeaders(['cookie']);

        try {
            return await useAPI<AuthUser>("auth/user", {
                method: "GET",
                headers: cookieHeader,
                onErrorAction: "doNothing"
            })
        } catch (error: any) {
            if (error.response.status != 401) throw error;

            const event = await callWithNuxt(nuxtApp, useRequestEvent);

            try {
                const res = await $fetch.raw("auth/refresh", {
                    baseURL: `${config.public.baseUrl}`,
                    method: "POST",
                    headers: cookieHeader,
                    credentials: "include"
                });

                let header = "";
                if (event) {
                    const cookies = res.headers.getSetCookie();
                    for (const cookie of cookies) {
                        appendResponseHeader(event, 'set-cookie', cookie);
                        
                        const setCookie = cookie.split(";")[0];
                        if (setCookie) header += setCookie + ";";
                    }
                }
                
                cookieHeader.cookie = header;
                
                return await nuxtApp.runWithContext(() =>
                    useAPI<AuthUser>("auth/user", {
                        method: "GET",
                        headers: cookieHeader,
                        onErrorAction: "doNothing"
                    })
                );
            } catch (error: any) {
                throw error;
            }            
        }
    }

    async function getCurrentUser() {
        const isLogged = useCookie("isLogged", {
            default: () => false
        });

        if (import.meta.client) return;
        
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
            isLogged.value = false;
            console.error(err);
            // console.log("FAIL get current user");
        });
    }

    await getCurrentUser();
})