<script setup lang="ts">
import * as z from 'zod'
import type { FormSubmitEvent } from '@nuxt/ui'

const toast = useToast();
const { forgotPassword } = useAuth();

const isSendSuccess = ref(false);

const schema = z.object({
  email: z.email('Invalid email')
});

type Schema = z.output<typeof schema>
const state = reactive<Partial<Schema>>({
  email: undefined,
})

async function onSubmit(event: FormSubmitEvent<Schema>) {
    isSendSuccess.value = false;
    await forgotPassword(event.data.email)
        .then(res => {
            isSendSuccess.value = true;
        })
        .catch(err => {
            toast.add({ title: 'Failed', description: 'Server error! Try again,', color: 'error' })
        });
}

</script>
<template>
    <UCard variant="subtle" class="mx-auto w-[400px]">
        <template #header>
            <h1 class="text-2xl font-bold text-center">Forgot password</h1>
        </template>

        <template v-if="!isSendSuccess">
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
        </template>
        <template v-else>
            <div class="flex justify-center items-center">
                We have sent a email to {{ state.email }}.
            </div>
        </template>
    </UCard>
</template>