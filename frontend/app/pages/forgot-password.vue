<script setup lang="ts">
import * as z from "zod";
import type { FormSubmitEvent } from "@nuxt/ui";

const toast = useToast();
const { forgotPassword } = useAuth();

const isSendSuccess = ref(false);

const schema = z.object({
  email: z.email("Invalid email"),
});

type Schema = z.output<typeof schema>;
const state = reactive<Partial<Schema>>({
  email: undefined,
});

async function onSubmit(event: FormSubmitEvent<Schema>) {
  isSendSuccess.value = false;
  await forgotPassword(event.data.email)
    .then(() => {
      isSendSuccess.value = true;
    })
    .catch(() => {
      toast.add({
        title: "Failed",
        description: "Server error! Try again,",
        color: "error",
      });
    });
}
</script>
<template>
  <UCard variant="outline" class="mt-8 mx-auto max-w-md">
    <template #header>
      <h1 class="text-2xl font-bold text-center">
        {{ $t("forgot_password") }}
      </h1>
      <h2 class="mt-4 text-center text-gray-400">
        {{ $t("forgot_password.description") }}
      </h2>
    </template>

    <template v-if="!isSendSuccess">
      <div class="w-full">
        <UForm
          :schema="schema"
          :state="state"
          class="space-y-4"
          @submit="onSubmit"
        >
          <UFormField :label="$t('forgot_password.email')" name="email">
            <UInput
              v-model="state.email"
              :placeholder="$t('forgot_password.placeholder')"
              class="w-full mt-1"
            />
          </UFormField>

          <UButton type="submit" class="flex w-full py-2 justify-center">
            {{ $t("forgot_password.continue") }}
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
