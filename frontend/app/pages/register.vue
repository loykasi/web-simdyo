<script setup lang="ts">
import * as z from 'zod'
import type { FormSubmitEvent } from '@nuxt/ui'
import { useAuth } from '~/composables/useAuth';
import type { RegisterRequest } from '~/types/auth.type';

const schema = z.object({
    username: z.string('Invalid username'),
  email: z.email('Invalid email'),
  password: z.string('Invalid password').min(6, 'Must be at least 6 characters'),
  confirmPassword: z.string('Invalid')
}).refine((data) => data.password === data.confirmPassword, {
    message: "Passwords don't match",
    path: ["confirmPassword"],
});

type Schema = z.output<typeof schema>

const state = reactive<Partial<Schema>>({
  email: undefined,
  password: undefined
})

const showPassword = ref(false);
const showConfirmPassword = ref(false);

const email = ref("");
const { isRegisterSuccess, register } = useAuth();
async function onSubmit(event: FormSubmitEvent<Schema>) {
    const payload: RegisterRequest = {
        username: event.data.username,
        email: event.data.email,
        password: event.data.password,
    };
    email.value = payload.email;
    register(payload);
}
</script>
<template>
    <UCard variant="subtle" class="mx-auto w-[400px]">
        <template #header>
            <h1 class="text-2xl font-bold text-center">Register</h1>
        </template>

        <div class="w-full">
            <template v-if="!isRegisterSuccess">
                <UForm :schema="schema" :state="state" class="space-y-4" @submit="onSubmit">
                    <UFormField label="Username" name="username">
                        <UInput v-model="state.username" placeholder="Username" class="w-full mt-3" />
                    </UFormField>

                    <UFormField label="Email" name="email">
                        <UInput v-model="state.email" placeholder="Email address" class="w-full mt-3" />
                    </UFormField>

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
                        Register
                    </UButton>
                </UForm>
            </template>
            <template v-else>
                <div class="flex justify-center items-center">
                    We have sent a verification email to {{ email }}.
                </div>
            </template>
        </div>
    </UCard>
</template>