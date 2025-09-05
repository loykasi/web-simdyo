<script setup lang="ts">
import * as z from 'zod'
import type { FormSubmitEvent } from '@nuxt/ui'

const schema = z.object({
  email: z.email('Invalid email'),
  password: z.string('Invalid password').min(8, 'Must be at least 8 characters'),
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

const toast = useToast()
async function onSubmit(event: FormSubmitEvent<Schema>) {
  toast.add({ title: 'Success', description: 'The form has been submitted.', color: 'success' })
  console.log(event.data)
}

</script>
<template>
    <UCard variant="subtle" class="mx-auto w-[400px]">
        <template #header>
            <h1 class="text-2xl font-bold text-center">Forgot password</h1>
        </template>

        <div class="w-full">
            <h2 class="text-center text-gray-400">Enter your email and a reset password link will be sent to your email.</h2>
            <UForm :schema="schema" :state="state" class="mt-4 space-y-4" @submit="onSubmit">
                <UFormField label="Email" name="email">
                    <UInput v-model="state.email" placeholder="Email address" class="w-full mt-3" />
                </UFormField>

                <UButton type="submit" class="flex w-full py-2 justify-center">
                    Submit
                </UButton>
            </UForm>
        </div>
    </UCard>
</template>