<script setup lang="ts">
import * as z from "zod";
import type { FormSubmitEvent } from "@nuxt/ui";
import { useAuth } from "~/composables/useAuth";
import type { RegisterRequest } from "~/types/auth.type";

const toast = useToast();
const form = useTemplateRef("form");
const isRegisterSuccess = ref(false);
const registering = ref(false);

// const schema = z.object({
//     username: z.string('Username is required'),
//     email: z.email('Invalid email'),
//     password: z.string('Password is required').min(6, 'Must be at least 6 characters'),
//     confirmPassword: z.string('Required')
// }).refine((data) => data.password === data.confirmPassword, {
//     message: "Passwords don't match",
//     path: ["confirmPassword"],
// });

const schema = z.object({
  username: z.string("Username is required"),
  email: z.email("Invalid email"),
});
type Schema = z.output<typeof schema>;
const state = reactive<Partial<Schema>>({
  email: undefined,
});

const { register } = useAuth();

async function onSubmit(event: FormSubmitEvent<Schema>) {
  try {
    registering.value = true;
    const payload: RegisterRequest = {
      username: event.data.username,
      email: event.data.email,
    };

    await register(payload);
    isRegisterSuccess.value = true;
  } catch (err: any) {
    if (!err.data) {
      toast.add({
        title: "Something went wrong. Please try again.",
        color: "error",
      });
      return;
    }

    if (err.data[0].code === "User.DuplicateUsername") {
      const errors = [];
      errors.push({ name: "username", message: "Username taken! Try another" });
      form.value?.setErrors(errors);
    }

    if (err.data[0].code === "User.DuplicateEmail") {
      const errors = [];
      errors.push({ name: "email", message: "Email already in use" });
      form.value?.setErrors(errors);
    }
  } finally {
    registering.value = false;
  }
}

definePageMeta({
  middleware: ["redirect-if-logged-in"],
});
</script>
<template>
  <div v-if="!isRegisterSuccess">
    <UCard
      variant="outline"
      class="mt-8 mx-auto max-w-md"
      :ui="{
        header: 'flex flex-col text-center',
      }"
    >
      <template #header>
        <h1 class="text-2xl font-bold text-center">
          {{ $t("auth.register.title") }}
        </h1>
        <p class="mt-1 text-base">{{ $t("auth.register.description") }}</p>
      </template>
      <div class="w-full">
        <UForm
          ref="form"
          :schema="schema"
          :state="state"
          class="space-y-4"
          @submit="onSubmit"
        >
          <UFormField :label="$t('common.fields.username')" name="username">
            <UInput
              v-model="state.username"
              :placeholder="$t('auth.register.placeholders.username')"
              class="w-full mt-1"
            />
          </UFormField>

          <UFormField :label="$t('common.fields.email')" name="email">
            <UInput
              v-model="state.email"
              :placeholder="$t('auth.register.placeholders.email')"
              class="w-full mt-1"
            />
          </UFormField>

          <UButton
            type="submit"
            class="flex w-full py-2 justify-center"
            :loading="registering"
          >
            {{ $t("common.actions.continue") }}
          </UButton>
        </UForm>
      </div>
    </UCard>
  </div>
  <div v-else>
    <UCard
      variant="outline"
      class="mt-8 mx-auto max-w-md"
      :ui="{
        header: 'flex flex-col text-center',
      }"
    >
      <template #header>
        <h1 class="text-2xl font-bold text-center">
          {{ $t("auth.register.success.title") }}
        </h1>
      </template>
      <div class="w-full">
        <div class="flex flex-col justify-center items-center">
          <div>{{ $t("auth.register.success.message") }}</div>
        </div>
      </div>
    </UCard>
  </div>
</template>
