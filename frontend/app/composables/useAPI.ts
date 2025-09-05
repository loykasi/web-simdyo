export function useAPI<T>(
    url: string, 
    userOptions: { method?: string, body?: any } = {}
) {
    const config = useRuntimeConfig();

    return $fetch<T>(url, {
        baseURL: `${config.public.baseUrl}`,
        method: userOptions.method as any || "GET",
        body: userOptions.body
    })
}