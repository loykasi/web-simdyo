interface ErrorType {
  errors: Record<string, string[]>;
}

// export async function useAPI<T>(
//     url: string,
//     userOptions: {
//         method?: string,
//         body?: any,
//         query?: Record<string, any> | undefined,
//         signal?: AbortSignal,
//         headers?: any
//     } = {}
// ) {
//     const config = useRuntimeConfig();
//     const isLogged = useCookie("isLogged", {
//         default: () => false
//     });

//     try {
//         return await $fetch<T>(url, {
//             baseURL: `${config.public.baseUrl}`,
//             method: userOptions.method as any || "GET",
//             headers: userOptions.headers,
//             body: userOptions.body,
//             query: userOptions.query,
//             signal: userOptions.signal,
//         });
//     } catch (error: any) {
//         const status = error.response.status;
//         if (status == 401) {
//             try {
//                 await $fetch("auth/refresh", {
//                     baseURL: `${config.public.baseUrl}`,
//                     method: "POST",
//                     headers: userOptions.headers,
//                 });
//                 console.log("Token refreshed");

//                 return await $fetch<T>(url, {
//                     baseURL: `${config.public.baseUrl}`,
//                     method: userOptions.method as any || "GET",
//                     headers: userOptions.headers,
//                     body: userOptions.body,
//                     query: userOptions.query,
//                     signal: userOptions.signal,
//                 });
//             } catch (error: any) {
//                 console.error(error);

//                 if (isLogged.value) {
//                     isLogged.value = false;
//                     navigateTo("/");
//                 }

//                 throw error;
//             }
//         }

//         throw error;
//     }
// }

type OnErrorActionType = "refreshTokenAndRetry" | "doNothing";

export function useAPI<T>(
  url: string,
  userOptions: {
    method?: string;
    body?: any;
    query?: Record<string, any>;
    signal?: AbortSignal;
    headers?: any;
    onErrorAction?: OnErrorActionType;
  },
) {
  const config = useRuntimeConfig();
  const isLogged = useCookie("isLogged", {
    default: () => false,
  });

  if (userOptions.onErrorAction === undefined) {
    userOptions.onErrorAction = "refreshTokenAndRetry";
  }

  return $fetch<T>(url, {
    baseURL: `${config.public.baseUrl}`,
    method: (userOptions.method as any) || "GET",
    headers: userOptions.headers,
    body: userOptions.body,
    query: userOptions.query,
    credentials: "include",
    signal: userOptions.signal,
    retry: 1,
    retryStatusCodes: [401],

    onResponseError: async ({ request, options, response }) => {
      if (
        response.status == 401 &&
        userOptions.onErrorAction === "refreshTokenAndRetry"
      ) {
        // console.error(userOptions.headers);
        console.log("try refresh");
        try {
          await $fetch("auth/refresh", {
            baseURL: `${config.public.baseUrl}`,
            method: "POST",
            headers: userOptions.headers,
            credentials: "include",
          });
          console.log("Token refreshed");
        } catch (error: any) {
          options.retry = false;
          console.error(error);

          if (isLogged.value) {
            isLogged.value = false;
            navigateTo("/");
          }
        }
      } else {
        options.retry = false;
      }

      // const statusCode = response.status || 500;
      // const statusMessage = response.statusText || '';
      // throw createError({ statusCode: statusCode, statusMessage: statusMessage })
    },
  });
}
