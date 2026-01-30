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
    '@nuxt/fonts',
    '@nuxtjs/i18n'
  ],
  i18n: {
    strategy: 'no_prefix',
    defaultLocale: 'en',
    locales: [
      { code: 'en', name: 'English', file: 'en.json' },
      { code: 'vi', name: 'Tiếng Việt', file: 'vi-VN.json' }
    ]
  },
  routeRules: {
    // '/explore': { redirect: '/explore/default' },
    '/': { redirect: '/explore' },
    '/explore': { redirect: '/explore/all' },
    '/dashboard': { redirect: '/dashboard/users' },
    '/create': { ssr: false },
    '/confirm-email': { ssr: false },
    '/projects/**/edit': { ssr: false },
    '/dashboard/**': { ssr: false },
  },
  nitro: {
    preset: "cloudflare-pages",
    cloudflare: {
      pages: {
        routes: {
          exclude: [
            "/engine/*",
            "/game/*"
          ]
        }
      }
    }
  }
})