<script setup lang="ts">
import { UColorModeButton } from '#components';
import type { NavigationMenuItem } from '@nuxt/ui'
import { useAuthStore } from '~/stores/auth.store';
import type { FormSubmitEvent } from '@nuxt/ui';

const route = useRoute()
const { isLoggedIn, isPermitted } = useAuthStore();

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
		label: 'Explore',
		to: '/explore'
	},
	...(isLoggedIn.value ? [ 
		{
			label: 'Upload',
			to: '/upload'
		}
	] : []),
	...(isPermitted(["dashboard_access"]) ? [ 
		{
			label: 'Dashboard',
			to: '/dashboard/users'
		}
	] : []),
]);

///

const searchTerm = ref(route.query.q || "");

function onSearch() {
	navigateTo({
		path: '/explore',
		query: {
			q: searchTerm.value,
		},
	})
}

</script>
<template>
	<UHeader>
		<template #left>
			<NuxtLink
				to="/"
				class="focus-visible:outline-primary hover:text-default transition-colors shrink-0 font-bold text-xl text-highlighted flex items-end gap-1.5 me-4"
			>
				CodeVisdoo
			</NuxtLink>
			<UNavigationMenu :items="items" variant="pill" />
		</template>

		<form @submit.prevent="onSearch" class="flex items-center bg-accented rounded-md">
			<input
				id="search"
				type="text"
				v-model="searchTerm"
				placeholder="Search for projects or creators"
				required
				class="px-2.5 py-1.5 min-w-sm text-sm"
			/>
			<button
				type="submit"
				class="flex justify-center items-center px-2 hover:cursor-pointer">
				<UIcon name="material-symbols:search" class="size-5" />
			</button>
		</form>

		<template #right>
			<ClientOnly>
				<UColorModeButton />
				<template v-if="isLoggedIn">
					<AccountDropdown />
				</template>
				<template v-else>
					<LoginPopover />

					<UButton
						to="/register"
						variant="subtle"
					>
						Register
					</UButton>
				</template>
			</ClientOnly>
		</template>
	</UHeader>
</template>
