<script setup lang="ts">

const items = [
  {
    label: $t('settings.account'),
    icon: 'material-symbols:person-outline',
    slot: 'account'
  },
  {
    label: $t('settings.password'),
    icon: 'material-symbols:lock-outline',
    slot: 'password'
  }
];

definePageMeta({
    middleware: ['authenticated']
})
</script>

<template>
    <UPage>
        <UPageHeader :title="$t('settings')" />

        <UTabs
            :items="items"
            :ui="{
                content: 'mt-4'
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
