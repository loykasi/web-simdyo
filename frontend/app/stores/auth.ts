import type { User } from "~/types/auth.type"

export const AuthState = () => {
    const user = useState<User | null>("userData", () => null);
    const isLoggedIn = computed(() => user.value !== null);

    return {
        user,
        isLoggedIn
    }
}