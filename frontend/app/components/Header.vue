<script setup lang="ts">
import { UColorModeButton } from '#components';
import type { NavigationMenuItem } from '@nuxt/ui'
import { useAuthStore } from '~/stores/auth.store';

const route = useRoute()
const { isLoggedIn, user } = useAuthStore();

const items = computed<NavigationMenuItem[]>(() => [
	{
		label: 'Home',
		to: '/'
	},
	{
		label: 'Create',
		to: '/create'
	},
	{
		label: 'Explorer',
		to: '/explore'
	},
	...(isLoggedIn.value ? [ 
		{
			label: 'Upload',
			to: '/upload'
		}
	] : [])
]);
</script>

<template>
	<UHeader>
		<template #left>
			<NuxtLink
				to="/"
				class="focus-visible:outline-primary hover:text-default transition-colors shrink-0 font-bold text-xl text-highlighted flex items-end gap-1.5 me-4"
			>VisualBlock</NuxtLink>
			<UNavigationMenu :items="items" />
		</template>

		<template #right>
			<UColorModeButton />
			<ClientOnly>
				<template v-if="isLoggedIn">
					<AccountDropdown />
				</template>
				<template v-else>
					<LoginPopover />

					<UButton
						to="/register"
						color="neutral"
						variant="subtle"
					>
						Register
					</UButton>
				</template>
			</ClientOnly>
		</template>
	</UHeader>
</template>
