export function useAPI<T>(
    url: string, 
    userOptions: { method?: string, body?: any } = {}
) {
    const config = useRuntimeConfig();

    return $fetch<T>(url, {
        baseURL: `${config.public.baseUrl}`,
        method: userOptions.method as any || "GET",
        body: userOptions.body,
        credentials: "include",
        retry: 1,
        retryStatusCodes: [401],

        onResponseError: async({ request, options, response }) => {
            if (response.status == 401) {
                try {
                    await $fetch("auth/refresh", {
                        baseURL: `${config.public.baseUrl}`,
                        method: "POST",
                        credentials: "include",
                    });
                } catch (error) {
                    options.retry = false;
                    console.error("Token expired");
                }
            }
        }
    })
}