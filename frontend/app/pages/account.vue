<script setup lang="ts">
const items = [
  {
    label: $t("settings.sections.account"),
    icon: "material-symbols:person-outline",
    slot: "account",
  },
  {
    label: $t("settings.sections.password"),
    icon: "material-symbols:lock-outline",
    slot: "password",
  },
];

definePageMeta({
  middleware: ["admin-authorization"],
});
</script>

<template>
  <UPage>
    <UPageHeader :title="$t('settings.title')" />

    <UTabs
      :items="items"
      :ui="{
        content: 'mt-4',
      }"
    >
      <template #account>
        <AccountTab />
      </template>

      <template #password>
        <AccountPasswordTab />
      </template>
    </UTabs>
  </UPage>
</template>
