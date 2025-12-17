<script setup lang="ts">
const route = useRoute();
const email: string = route.query.email as string;
const token: string = route.query.token as string;

const { isVerifying, isVerifySuccess, confirmEmail } = useAuth();
confirmEmail(token, email);
</script>
<template>
    <UCard
        variant="outline"
        class="mt-8 mx-auto max-w-md"
    >
        <template v-if="isVerifying">
            <h1 class="text-2xl font-bold text-center">Verifing</h1>
            <UProgress class="mt-2" />
        </template>
        <template v-else-if="isVerifySuccess">
            <h1 class="text-2xl font-bold text-center">Verified!</h1>
            <div class="mt-2 text-center">Your email has been verified.</div>
            <div class="text-center">You can now sign in to your account.</div>
        </template>
        <template v-else>
            <h1 class="text-2xl font-bold text-center">Something went wrong!</h1>
            <p class="text-center">Check the link again.</p>
        </template>
    </UCard>
</template>