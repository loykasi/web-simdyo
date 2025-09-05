<script setup lang="ts">
const route = useRoute();
const email: string = route.query.email as string;
const token: string = route.query.token as string;

const { isVerifying, isVerifySuccess, confirmEmail } = useAuth();
confirmEmail(token, email);
</script>
<template>
    <UCard variant="subtle" class="mx-auto w-[400px]">
        <template v-if="isVerifying">
            <h2 class="text-center text-xl font-bold mb-4">Verifing</h2>
            <UProgress />
        </template>
        <template v-else-if="isVerifySuccess">
            <h2 class="text-center text-xl font-bold mb-4">Verified!</h2>
            <p class="text-center">You have successfully verified account.</p>
        </template>
        <template v-else>
            <h2 class="text-center text-xl font-bold mb-4">Failed to verify!</h2>
            <p class="text-center">Check the link again.</p>
        </template>
    </UCard>
</template>