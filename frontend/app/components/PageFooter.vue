<script setup lang="ts">
import type { NavigationMenuItem } from "@nuxt/ui";

const items: NavigationMenuItem[] = [
  {
    label: $t("footer.rules"),
    to: "/rules",
  },
  {
    label: $t("footer.terms"),
    to: "/terms",
  },
  {
    label: $t("footer.feedback"),
    to: "https://forms.gle/bNBGvWZTGqBd74pi9",
    target: "_blank",
  },
];
</script>
<template>
  <UFooter>
    <template #left>
      <p class="text-muted text-sm">v0.1</p>
    </template>

    <UNavigationMenu :items="items" variant="link" />

    <template #right>
      <p class="text-muted text-sm">
        Copyright © {{ new Date().getFullYear() }}
      </p>
    </template>
  </UFooter>
</template>
