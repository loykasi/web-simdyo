<script setup lang="ts">

const items = [
  {
    label: 'Account',
    icon: 'material-symbols:person-outline',
    slot: 'account'
  },
  {
    label: 'Password',
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
        <UPageHeader title="Account" />

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
