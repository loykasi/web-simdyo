<script setup lang="ts">
import type { NavigationMenuItem, DropdownMenuItem } from '@nuxt/ui';
import { useAuthStore } from '~/stores/auth.store';

const { isLoggedIn, user } = useAuthStore();

const items = computed<NavigationMenuItem[][]>(() => [
    [
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
    ],
    isLoggedIn.value
    ? [
        {
            slot: 'auth' as const,
            ui: {
                link: "p-0"
            }
        }
    ]
    : [
        {
            slot: 'auth' as const,
            ui: {
                link: "p-0"
            }
        },
        {
            label: 'Register',
            to: '/register'
        },
    ]
]);

const open = ref(false);
const loginArchorRef = ref<HTMLElement>();
function loginClicked(event: MouseEvent)
{
	open.value = !open.value;
}

const openAccountMenu = ref(false);
function accountClicked(event: MouseEvent)
{
    openAccountMenu.value = !openAccountMenu.value;
}
</script>

<template>
    <UNavigationMenu
        highlight
        highlight-color="primary"
        color="primary"
        variant="pill"
        :items="items"
        class="w-full"
    >
        <template #auth="{ item }">
            <div ref="loginArchorRef" class="px-2.5 py-1.5">
                <template v-if="isLoggedIn">
                    <div v-on:click="accountClicked">{{user?.userName}}</div>
                </template>
                <template v-else>
                    <div v-on:click="loginClicked">Login</div>
                </template>
            </div>
        </template>
    </UNavigationMenu>

    <LoginPopover :reference="loginArchorRef" :open="open" />
	<!-- <UPopover
		v-model:open="open"
		:content="{
			reference: loginArchorRef,
			align: 'end'
		}"
        arrow
	>
		<template #content>
			
		</template>
	</UPopover> -->

    <!-- <UDropdownMenu
        :items="accountItems"
        v-model:open="openAccountMenu"
        :content="{
            align: 'start',
            side: 'bottom',
            sideOffset: 8,
            reference: loginArchorRef
        }"
        :ui="{
            content: 'w-48'
        }"
    >
    </UDropdownMenu> -->
</template>