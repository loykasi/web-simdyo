<script setup lang="ts">
import type { ButtonProps } from "@nuxt/ui";

const links = ref<ButtonProps[]>([
  {
    label: $t("home.hero.create"),
    to: "/create",
  },
]);
</script>
<template>
  <UPageHero
    :title="$t('home.hero.title')"
    :description="$t('home.hero.description')"
    :links="links"
  />
</template>
