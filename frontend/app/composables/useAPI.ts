interface ErrorType {
	errors: Record<string, string[]>;
}

export function useAPI<T>(
    url: string, 
    userOptions: {
        method?: string,
        body?: any,
        query?: Record<string, any> | undefined,
        signal?: AbortSignal,
        headers?: any
    } = {}
) {
    const config = useRuntimeConfig();

    return $fetch<T>(url, {
        baseURL: `${config.public.baseUrl}`,
        method: userOptions.method as any || "GET",
        headers: userOptions.headers,
        body: userOptions.body,
        query: userOptions.query,
        signal: userOptions.signal,
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
                    navigateTo("/");
                }
            }

            const statusCode = response.status || 500;
			const statusMessage = response.statusText || '';

            // throw createError({ statusCode: statusCode, statusMessage: statusMessage })
        }
    })
}