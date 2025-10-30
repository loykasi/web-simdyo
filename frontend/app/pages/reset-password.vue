<script setup lang="ts">
import * as z from 'zod'
import type { FormSubmitEvent } from '@nuxt/ui'
import type { ResetPasswordRequest } from '~/types/auth.type';

const toast = useToast();
const route = useRoute();
const { resetPassword } = useAuth();

const showPassword = ref(false);
const showConfirmPassword = ref(false);
const isSuccess = ref(false);

const schema = z.object({
  password: z.string('Invalid password').min(6, 'Must be at least 6 characters'),
  confirmPassword: z.string('Invalid')
}).refine((data) => data.password === data.confirmPassword, {
    message: "Passwords don't match",
    path: ["confirmPassword"],
});

type Schema = z.output<typeof schema>
const state = reactive<Partial<Schema>>({
    password: undefined,
    confirmPassword: undefined
})


async function onSubmit(event: FormSubmitEvent<Schema>) {
    const email: string = route.query.email as string;
    const token: string = route.query.token as string;

    const payload: ResetPasswordRequest = {
        token,
        email,
        password: event.data.password,
        confirmPassword: event.data.confirmPassword,
    }

    console.log(payload);

    await resetPassword(payload)
        .then(res => {
            isSuccess.value = true;
        })
        .catch(err => {
            toast.add({ title: 'Failed', description: 'Invalid Token or server error. Try again!', color: 'error' })
        });
}
</script>
<template>
    <UCard variant="subtle" class="mx-auto w-[400px]">
        <template #header>
            <h1 class="text-2xl font-bold text-center">Reset password</h1>
        </template>

        <template v-if="!isSuccess">
            <div class="w-full">
                <h2 class="text-center text-gray-400">Enter your email and a reset password link will be sent to your email.</h2>
                <UForm :schema="schema" :state="state" class="mt-4 space-y-4" @submit="onSubmit">
                    <UFormField label="Password" name="password">
                        <UInput
                            v-model="state.password"
                            placeholder="Password"
                            :type="showPassword ? 'text' : 'password'"
                            class="w-full mt-3"
                        >
                            <template #trailing>
                            <UButton
                                color="neutral"
                                variant="link"
                                size="sm"
                                :icon="showPassword ? 'i-lucide-eye-off' : 'i-lucide-eye'"
                                :aria-label="showPassword ? 'Hide password' : 'showPassword password'"
                                :aria-pressed="showPassword"
                                aria-controls="password"
                                @click="showPassword = !showPassword"
                            />
                            </template>
                        </UInput>
                    </UFormField>

                    <UFormField label="Re-enter Password" name="confirmPassword">
                        <UInput
                            v-model="state.confirmPassword"
                            placeholder="Type password again"
                            :type="showConfirmPassword ? 'text' : 'password'"
                            class="w-full mt-3"
                        >
                            <template #trailing>
                            <UButton
                                color="neutral"
                                variant="link"
                                size="sm"
                                :icon="showConfirmPassword ? 'i-lucide-eye-off' : 'i-lucide-eye'"
                                :aria-label="showConfirmPassword ? 'Hide password' : 'showConfirmPassword password'"
                                :aria-pressed="showConfirmPassword"
                                aria-controls="password"
                                @click="showConfirmPassword = !showConfirmPassword"
                            />
                            </template>
                        </UInput>
                    </UFormField>

                    <UButton type="submit" class="flex w-full py-2 justify-center">
                        Submit
                    </UButton>
                </UForm>
            </div>
        </template>
        <template v-else>
            <div class="flex justify-center items-center">
                Password Reset complelete
            </div>
        </template>
    </UCard>
</template>