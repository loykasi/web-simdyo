<script setup lang="ts">
import * as z from "zod";
import type { FormSubmitEvent } from "@nuxt/ui";

const prop = defineProps<{
  email: string;
}>();

const emit = defineEmits<{
  toLoginForm: [value: string];
}>();

onMounted(() => {
  state.email = prop.email;
});

const toast = useToast();
const loading = ref(false);
const error = ref("");
const { sendOTP } = useLogin();

const schema = z.object({
  email: z.email("Invalid email"),
});
type Schema = z.output<typeof schema>;
const state = reactive<Partial<Schema>>({});

async function onSubmit(event: FormSubmitEvent<Schema>) {
  try {
    loading.value = true;
    const email = event.data.email;
    await sendOTP(email);
    emit("toLoginForm", email);
  } catch (err: any) {
    if (!err.data) {
      toast.add({
        title: `Server error! Try again.`,
        color: "error",
      });
    } else if (err.data[0].code === "Account.NotFound") {
      error.value = "Account not found email";
    } else if (err.data[0].code === "Auth.AccountBanned") {
      error.value =
        "Account has been suspended due to a violation of community guidelines.";
    }
  } finally {
    loading.value = false;
  }
}
</script>
<template>
  <div class="p-4 w-xs">
    <UForm
      :schema="schema"
      :state="state"
      class="space-y-4"
      :validate-on="[]"
      @submit="onSubmit"
    >
      <UFormField :label="$t('login.email')" name="email">
        <UInput v-model="state.email" class="w-full mt-1" />
      </UFormField>

      <UButton
        type="submit"
        :loading="loading"
        class="flex justify-center w-full"
      >
        <!-- {{ $t('login.login') }} -->
        Send OTP
      </UButton>

      <div
        v-if="error !== ''"
        class="bg-error-100 dark:bg-error-600 p-2 rounded-md text-base"
      >
        {{ error }}
      </div>
    </UForm>
  </div>
</template>
