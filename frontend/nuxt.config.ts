import tailwindcss from "@tailwindcss/vite";

export default defineNuxtConfig({
  compatibilityDate: '2025-07-15',
  devtools: { enabled: true },
  runtimeConfig: {
    public: {
      baseUrl: process.env.BASE_URL,
    }
  },
  vite: {
    plugins: [
      tailwindcss(),
    ],
  },
  css: ['~/assets/css/main.css'],
  ssr: true,
  modules: [
    '@nuxt/ui',
    '@nuxt/icon',
    '@nuxt/fonts'
  ],
  routeRules: {
    // '/explore': { redirect: '/explore/default' },
    '/create': { ssr: false },
    '/confirm-email': { ssr: false },
    '/projects/**/edit': { ssr: false },
    '/dashboard/**': { ssr: false },
  },
})