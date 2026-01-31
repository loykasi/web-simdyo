<script setup lang="ts">
import type { FormSubmitEvent } from "@nuxt/ui";
import * as z from "zod";
import type { ChangePasswordRequest } from "~/types/changePassword.type";

const toast = useToast();
const { changePassword } = useAccount();

const schema = z
  .object({
    currentPassword: z
      .string("Invalid password")
      .min(6, "Must be at least 6 characters"),
    newPassword: z
      .string("Invalid password")
      .min(6, "Must be at least 6 characters"),
    confirmPassword: z.string("Invalid"),
  })
  .refine((data) => data.newPassword === data.confirmPassword, {
    message: "Passwords don't match",
    path: ["confirmPassword"],
  });
type Schema = z.output<typeof schema>;
const state = reactive<Partial<Schema>>({});

async function onSubmit(event: FormSubmitEvent<Schema>) {
  const payload: ChangePasswordRequest = {
    currentPassword: event.data.currentPassword,
    newPassword: event.data.newPassword,
  };

  try {
    await changePassword(payload);

    state.currentPassword = "";
    state.newPassword = "";
    state.confirmPassword = "";

    toast.add({
      title: "Success",
      description: "Password has been changed!",
      color: "success",
    });
  } catch {
    toast.add({
      title: "Failed",
      description: "Something wrong! Try again.",
      color: "error",
    });
  }
}
</script>
<template>
  <UCard>
    <UForm :state="state" class="flex flex-col gap-4" @submit="onSubmit">
      <UFormField
        :label="$t('settings.password.current')"
        name="current"
        required
      >
        <UInput
          v-model="state.currentPassword"
          type="password"
          required
          class="w-full"
        />
      </UFormField>
      <UFormField :label="$t('settings.password.new')" name="new" required>
        <UInput
          v-model="state.newPassword"
          type="password"
          required
          class="w-full"
        />
      </UFormField>
      <UFormField
        :label="$t('settings.password.confirm')"
        name="confirm"
        required
      >
        <UInput
          v-model="state.confirmPassword"
          type="password"
          required
          class="w-full"
        />
      </UFormField>

      <div>
        <UButton type="submit" class="px-4">{{
          $t("settings.password.submit")
        }}</UButton>
      </div>
    </UForm>
  </UCard>
</template>
