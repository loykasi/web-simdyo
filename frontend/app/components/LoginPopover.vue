<script setup lang="ts">
import * as z from 'zod'
import type { FormSubmitEvent } from '@nuxt/ui'
import { useAuthStore } from '~/stores/auth.store'
import { useLogin } from '~/composables/useLogin';
import type { LoginRequest } from '~/types/auth.type';

const toast = useToast();
const { user } = useAuthStore();
const { login } = useLogin();
const isLoginSuccess = ref(false);
const loading = ref(false);
const error = ref("");
const open = ref(false);

const isLogged = useCookie("isLogged", {
	default: () => false,
});

const schema = z.object({
	username: z.string('Invalid username').nonempty("This field is required").min(2, 'Must be at least 2 characters'),
	password: z.string('Invalid password').min(6, 'Must be at least 6 characters')
})

type Schema = z.output<typeof schema>

const state = reactive<Partial<Schema>>({
	username: "",
	password: ""
})

async function onSubmit(event: FormSubmitEvent<Schema>) {
	loading.value = true;
    const payload: LoginRequest = {
        username: event.data.username,
        password: event.data.password
    };
    login(payload)
	.then((res) => {
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
	})
	.catch((err) => {
		if (!err.data) {
			toast.add({
				title: `Server error! Try again.`,
				color: 'error'
			})
		} else if (err.data[0].code === "Login.Ban") {
			error.value = "Account has been suspended due to a violation of community guidelines."
        } else if (err.data[0].code === "Login.ValidationFailed") {
			error.value = "Incorrect username or password"
        }
	}).finally(() => {
		loading.value = false;
	})
}

const showPassword = ref(false);
</script>

<template>
	<UPopover
		v-model:open="open"
		:content="{
			align: 'end'
		}"
        arrow
	>
		<UButton
			:label="$t('nav.login')"
			color="neutral"
			variant="ghost"
		/>
		<template #content>
			<div class="p-4 w-xs">
				<UForm :schema="schema" :state="state" class="space-y-4" @submit="onSubmit" :validateOn="[]">
					<UFormField :label="$t('login.username')" name="username" >
						<UInput v-model="state.username" class="w-full mt-1" />
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
						
					<div>
						<ULink to="/forgot-password" @click="() => {open = false}">{{ $t('login.forgot') }}</ULink>
					</div>

					<UButton type="submit" :loading="loading" class="flex justify-center w-24">
						{{ $t('login.login') }}
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
	</UPopover>
</template>

