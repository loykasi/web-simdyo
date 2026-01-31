<script setup lang="ts">
import * as locales from "@nuxt/ui/locale";

const { locale } = useI18n();
const toaster = { position: "bottom-center" };

useHead({
  titleTemplate: (titleChunk) => {
    return titleChunk ? `${titleChunk} - Simdyo` : "Simdyo";
  },
});
</script>
<template>
  <UApp :toaster="toaster" :locale="locales[locale]">
    <NuxtLayout />
  </UApp>
</template>
