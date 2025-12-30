<script setup lang="ts">
import * as z from 'zod'
import type { FormSubmitEvent } from '@nuxt/ui'
import { useAuth } from '~/composables/useAuth';
import type { RegisterRequest } from '~/types/auth.type';

const toast = useToast();

const form = useTemplateRef("form");
const isRegisterSuccess = ref(false);
const showPassword = ref(false);

const schema = z.object({
    username: z.string('Username is required'),
    email: z.email('Invalid email'),
    password: z.string('Password is required').min(6, 'Must be at least 6 characters'),
    confirmPassword: z.string('Required')
}).refine((data) => data.password === data.confirmPassword, {
    message: "Passwords don't match",
    path: ["confirmPassword"],
});

type Schema = z.output<typeof schema>

const state = reactive<Partial<Schema>>({
  email: undefined,
  password: undefined
})

const { register } = useAuth();
async function onSubmit(event: FormSubmitEvent<Schema>) {
    const payload: RegisterRequest = {
        username: event.data.username,
        email: event.data.email,
        password: event.data.password,
    };
    register(payload)
    .then((res) => {
        isRegisterSuccess.value = true;
    }).catch((err) => {
        if (err.data === undefined) {
            console.log("server error");
            return;
        }
        if (err.data[0].code === "User.UsernameDuplicate") {
            const errors = [];
            errors.push({ name: "username", message: "Username taken! Try another" });
            form.value?.setErrors(errors);
        }

        if (err.data[0].code === "User.EmailDuplicate") {
            const errors = [];
            errors.push({ name: "email", message: "Email already in use" });
            form.value?.setErrors(errors);
        }
    })
}

definePageMeta({
    middleware: ['redirect-if-logged-in']
})
</script>
<template>
    <template v-if="!isRegisterSuccess">
        <UCard
            variant="outline"
            class="mt-8 mx-auto max-w-md"
            :ui="{
                header: 'flex flex-col text-center'
            }"
        >
            <template #header>
                <h1 class="text-2xl font-bold text-center">{{ $t('register.title') }}</h1>
                <p class="mt-1 text-base">{{ $t('register.description') }}</p>
            </template>

            <div class="w-full">
                <UForm ref="form" :schema="schema" :state="state" class="space-y-4" @submit="onSubmit">
                    <UFormField :label="$t('register.username')" name="username">
                        <UInput v-model="state.username" :placeholder="$t('register.username_placeholder')" class="w-full mt-1" />
                    </UFormField>

                    <UFormField :label="$t('register.email')" name="email">
                        <UInput v-model="state.email" :placeholder="$t('register.email_placeholder')" class="w-full mt-1" />
                    </UFormField>

                    <UFormField :label="$t('register.password')" name="password">
                        <UInput
                            v-model="state.password"
                            placeholder="********"
                            :type="showPassword ? 'text' : 'password'"
                            class="w-full mt-1"
                        >
                            <template #trailing>
                            <UButton
                                color="neutral"
                                variant="link"
                                size="sm"
                                :icon="showPassword ? 'i-lucide-eye-off' : 'i-lucide-eye'"
                                :aria-label="showPassword ? 'Hide password' : 'show password'"
                                :aria-pressed="showPassword"
                                aria-controls="password"
                                @click="showPassword = !showPassword"
                            />
                            </template>
                        </UInput>
                    </UFormField>

                    <UFormField :label="$t('register.confirm_password')" name="confirmPassword">
                        <UInput
                            v-model="state.confirmPassword"
                            placeholder="********"
                            :type="showPassword ? 'text' : 'password'"
                            class="w-full mt-1"
                        >
                            <template #trailing>
                            <UButton
                                color="neutral"
                                variant="link"
                                size="sm"
                                :icon="showPassword ? 'i-lucide-eye-off' : 'i-lucide-eye'"
                                :aria-label="showPassword ? 'Hide password' : 'show password'"
                                :aria-pressed="showPassword"
                                aria-controls="password"
                                @click="showPassword = !showPassword"
                            />
                            </template>
                        </UInput>
                    </UFormField>

                    <UButton type="submit" class="flex w-full py-2 justify-center">
                        {{ $t('register.continue') }}
                    </UButton>
                </UForm>
            </div>
        </UCard>
    </template>

    <template v-else>
        <UCard
            variant="outline"
            class="mt-8 mx-auto max-w-md"
            :ui="{
                header: 'flex flex-col text-center'
            }"
        >
            <template #header>
                <h1 class="text-2xl font-bold text-center">Check your email</h1>
            </template>

            <div class="w-full">
                <div class="flex flex-col justify-center items-center">
                    <div>A verification email was sent to <span>{{ state.email }}</span></div>
                    <div>Click on the link in that email to verify your account.</div>
                </div>
            </div>
        </UCard>
    </template>
</template>