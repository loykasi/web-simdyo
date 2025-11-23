import { useAuthStore } from "~/stores/auth.store"

export default defineNuxtRouteMiddleware((to, from) => {
    const { isLoggedIn, user } = useAuthStore();

    if (!isLoggedIn.value && !user.value?.permissions.includes("dashboard_access")) {
        return navigateTo('/');
    }
})