import { useAuthStore } from "~/stores/auth.store"
import type { AuthUser } from "~/types/auth.type";

export default defineNuxtPlugin(async () => {
    const { user } = useAuthStore();

    async function getUser() {
        const isLogged = useCookie("isLogged", {
            default: () => false
        });
        
        if (user.value || !isLogged.value) {
            return;
        }

        await useAPI<AuthUser>("auth/user", {
            method: "GET",
            headers: useRequestHeaders(['cookie'])
        }).then(res => {
            user.value = {
                email: res.email,
                username: res.username,
                permissions: res.permissions
            } as AuthUser;
            
            isLogged.value = true;
            console.log("SUCCESS get current user");
        }).catch(err => {
            console.log("FAIL get current user");
        });

        console.log("DONE get current user");
    }

    await getUser();
})