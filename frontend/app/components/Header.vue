<script setup lang="ts">
import { UColorModeButton } from '#components';
import type { NavigationMenuItem } from '@nuxt/ui'
import { useAuthStore } from '~/stores/auth.store';
import { en, vi } from '@nuxt/ui/locale'

const route = useRoute();
const { isLoggedIn, isPermitted } = useAuthStore();
const { locale, setLocale, locales } = useI18n();

const items = computed<NavigationMenuItem[]>(() => [
	{
		label: $t('nav.create'),
		to: '/create'
	},
	{
		label: $t('nav.explore'),
		to: '/explore'
	},
	...(isLoggedIn.value ? [ 
		{
			label: $t('nav.upload'),
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
				:placeholder="$t('nav.search')"
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
				<ULocaleSelect
					:model-value="locale"
					:locales="[en, vi]"
					@update:model-value="setLocale($event)"
				/>
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
						{{ $t('nav.register') }}
					</UButton>
				</template>
			</ClientOnly>
		</template>
	</UHeader>
</template>
