<script setup lang="ts">

const items = [
  {
    label: 'Account',
    icon: 'i-lucide-user',
    slot: 'account'
  },
  {
    label: 'Password',
    icon: 'i-lucide-lock',
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
