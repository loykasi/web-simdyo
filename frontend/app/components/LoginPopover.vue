<script setup lang="ts">
import * as z from 'zod'
import type { FormSubmitEvent } from '@nuxt/ui'
import { useAuthStore } from '~/stores/auth.store'
import { useLogin } from '~/composables/useLogin';
import type { LoginRequest } from '~/types/auth.type';

const { user } = useAuthStore();
const { isLoginSuccess, login } = useLogin();

const schema = z.object({
	email: z.email('Invalid email'),
	password: z.string('Invalid password').min(6, 'Must be at least 6 characters')
})

type Schema = z.output<typeof schema>

const state = reactive<Partial<Schema>>({
	email: undefined,
	password: undefined
})

async function onSubmit(event: FormSubmitEvent<Schema>) {
    const payload: LoginRequest = {
        email: event.data.email,
        password: event.data.password
    };
    login(payload);
}

const showPassword = ref(false);
</script>

<template>
	<UPopover
		:content="{
			align: 'end'
		}"
        arrow
	>
		<UButton
			label="Login"
			color="neutral"
			variant="ghost"
		/>
		<template #content>
			<div class="p-4">
				<UForm :schema="schema" :state="state" class="space-y-4" @submit="onSubmit">
					<UFormField label="Email" name="email" >
						<UInput v-model="state.email" placeholder="Email address" class="w-full mt-3" />
					</UFormField>

					<UFormField label="Password" name="password">
						<UInput
							v-model="state.password"
							placeholder="Password"
							:type="showPassword ? 'text' : 'password'"
							class="w-full mt-3"
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
						<ULink to="/forgotPassword">Forgot password?</ULink>
					</div>

					<UButton type="submit">
						Submit
					</UButton>
				</UForm>
			</div>
		</template>
	</UPopover>
</template>

