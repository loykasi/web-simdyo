<script setup lang="ts">
import * as z from 'zod';
import type { FormSubmitEvent } from '@nuxt/ui';
import { useAuthStore } from '~/stores/auth.store';
import type { LoginOTPRequest, LoginRequest } from '~/types/auth.type';

const prop = defineProps<{
  email: string
}>();

const emit = defineEmits<{
  backToRequestForm: []
}>();

const loading = ref(false);
const error = ref("");
const showPassword = ref(false);
const isLoginSuccess = ref(false);
const { loginOTP } = useLogin();
const { user } = useAuthStore();
const toast = useToast();

const isLogged = useCookie("isLogged", {
	default: () => false,
});

const schema = z.object({
	password: z.string('Invalid password').min(6, 'Must be at least 6 characters')
});
type Schema = z.output<typeof schema>;
const state = reactive<Partial<Schema>>({
	password: ""
});

async function onSubmit(event: FormSubmitEvent<Schema>) {
  try {
    loading.value = true;
    const payload: LoginOTPRequest = {
        email: prop.email,
        code: event.data.password
    };
    const res = await loginOTP(payload);

    user.value = {
			email: res.email,
			username: res.username,
			permissions: res.permissions
		};
		
		isLoginSuccess.value = true;
		isLogged.value = true;

		toast.add({
			title: `Welcome, ${res.username}`,
			color: 'success'
		})
  } catch (err: any) {
    if (!err.data) {
			toast.add({
				title: `Server error! Try again.`,
				color: 'error'
			})
		} else if (err.data[0].code === "Auth.AccountBanned") {
			error.value = "Account has been suspended due to a violation of community guidelines."
    } else if (err.data[0].code === "Auth.InvalidCredentials") {
      error.value = "Incorrect email or password"
    }
  } finally {
    loading.value = false;
  }
}
</script>
<template>
  <div class="p-4 w-xs">
    <UForm :schema="schema" :state="state" class="space-y-4" @submit="onSubmit" :validateOn="[]">
      <UFormField :label="$t('login.email')" name="email" >
        <UInput :model-value="email" class="w-full mt-1" disabled />
      </UFormField>

      <UFormField :label="$t('login.password')" name="password">
        <UInput
          v-model="state.password"
          :type="showPassword ? 'text' : 'password'"
          class="w-full mt-1"
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

      <UButton type="submit" :loading="loading" class="flex justify-center w-full">
        {{ $t('login.login') }}
      </UButton>

      <UButton color="neutral" variant="subtle" class="flex justify-center w-full" @click="emit('backToRequestForm')">
        Back
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