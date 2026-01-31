<script setup lang="ts">
import * as z from "zod";
import type { FormSubmitEvent } from "@nuxt/ui";
import type { LoginRequest } from "~/types/auth.type";
import { useAuthStore } from "~/stores/auth.store";

const route = useRoute();
const redirectRoute = computed(() =>
  decodeURIComponent((route.query.redirect as string) || ""),
);

const toast = useToast();
const { user } = useAuthStore();
const { login } = useLogin();
const isLoginSuccess = ref(false);
const loading = ref(false);
const error = ref("");
const showPassword = ref(false);

const isLogged = useCookie("isLogged", {
  default: () => false,
});

const schema = z.object({
  email: z.email("Invalid Email").nonempty("This field is required"),
  password: z
    .string("Invalid password")
    .min(6, "Must be at least 6 characters"),
});
type Schema = z.output<typeof schema>;
const state = reactive<Partial<Schema>>({
  email: user.value?.email,
  password: "",
});

async function onSubmit(event: FormSubmitEvent<Schema>) {
  try {
    loading.value = true;
    const payload: LoginRequest = {
      email: event.data.email,
      password: event.data.password,
    };
    const res = await login(payload);

    user.value = {
      email: res.email,
      username: res.username,
      isUseOTP: res.isUseOTP,
      permissions: res.permissions,
    };

    isLoginSuccess.value = true;
    isLogged.value = true;

    console.log("redirect " + redirectRoute.value);
    navigateTo(redirectRoute.value);
  } catch (err: any) {
    if (!err.data) {
      toast.add({
        title: `Server error! Try again.`,
        color: "error",
      });
    } else if (err.data[0].code === "Login.Ban") {
      error.value =
        "Account has been suspended due to a violation of community guidelines.";
    } else if (err.data[0].code === "Login.ValidationFailed") {
      error.value = "Incorrect username or password";
    }
  } finally {
    loading.value = false;
  }
}

definePageMeta({
  middleware: ["authenticated"],
});
</script>
<template>
  <UCard
    variant="outline"
    class="mt-8 mx-auto max-w-md"
    :ui="{
      header: 'flex flex-col text-center',
    }"
  >
    <template #header>
      <h1 class="text-2xl font-bold text-center">Login</h1>
    </template>
    <div class="w-full">
      <UForm
        ref="form"
        :schema="schema"
        :state="state"
        class="space-y-4"
        @submit="onSubmit"
      >
        <UFormField label="Email" name="email">
          <UInput
            v-model="state.email"
            placeholder="Your email address"
            class="w-full mt-1"
          />
        </UFormField>

        <UFormField label="Password" name="password">
          <UInput
            v-model="state.password"
            placeholder="******"
            :type="showPassword ? 'text' : 'password'"
            class="w-full mt-1"
          >
            <template #trailing>
              <UButton
                color="neutral"
                variant="link"
                size="sm"
                :icon="showPassword ? 'i-lucide-eye-off' : 'i-lucide-eye'"
                :aria-label="
                  showPassword ? 'Hide password' : 'showPassword password'
                "
                :aria-pressed="showPassword"
                aria-controls="password"
                @click="showPassword = !showPassword"
              />
            </template>
          </UInput>
        </UFormField>

        <UButton
          type="submit"
          class="flex w-full py-2 justify-center"
          :loading="loading"
        >
          Login
        </UButton>
      </UForm>
    </div>
  </UCard>
</template>
