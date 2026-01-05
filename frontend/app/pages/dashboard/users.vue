<script setup lang="ts">
import { h, resolveComponent } from 'vue';
import type { TableColumn, TableRow } from '@nuxt/ui';
import type { Row } from '@tanstack/vue-table';
import type { UserResponse } from '~/types/user.type';
import { UBadge } from '#components';
import UserBanModal from '~/components/dashboard/UserBanModal.vue';
import { useAuthStore } from '~/stores/auth.store';
import { useAdminUsersStore } from '~/stores/admin/adminUsers.store';
import { debounce } from '~/utilities/debounce';

const overlay = useOverlay();
const modal = overlay.create(UserBanModal);

const { user: authUser } = useAuthStore();

const toast = useToast();
const UButton = resolveComponent('UButton');
const UCheckbox = resolveComponent('UCheckbox');
const UDropdownMenu = resolveComponent('UDropdownMenu');

const globalFilter = ref('');

const {
    users,
    pending,
    pageSize,
    fetch: fetchUsers,
    update: updateUsers
} = useAdminUsersStore();

let abortController = new AbortController();
const currentPage = ref(1);

const columns: TableColumn<UserResponse>[] = [
    {
        id: 'select',
        header: ({ table }) =>
        h(UCheckbox, {
            modelValue: table.getIsSomePageRowsSelected()
            ? 'indeterminate'
            : table.getIsAllPageRowsSelected(),
            'onUpdate:modelValue': (value: boolean | 'indeterminate') =>
            table.toggleAllPageRowsSelected(!!value),
            'aria-label': 'Select all'
        }),
        cell: ({ row }) =>
        h(UCheckbox, {
            modelValue: row.getIsSelected(),
            'onUpdate:modelValue': (value: boolean | 'indeterminate') => row.toggleSelected(!!value),
            'aria-label': 'Select row'
        })
    },
    {
        accessorKey: 'id',
        header: 'ID'
    },
    {
        accessorKey: 'username',
        header: $t('username')
    },
    {
        accessorKey: 'email',
        header: $t('email')
    },
    {
        accessorKey: 'roles',
        header: $t('role'),
        cell: ({ row }) => {
            const roles = row.getValue('roles') as string[];

            return h(UBadge, { class: 'capitalize', variant: 'subtle', color: 'neutral' }, () => roles[0])
        }
    },
    {
        accessorKey: 'createdAt',
        header: $t('joined_date'),
        cell: ({ row }) => {
        return new Date(row.getValue('createdAt')).toLocaleString('en-US', {
            day: 'numeric',
            month: 'short',
            hour: '2-digit',
            minute: '2-digit',
            hour12: false
        })
        }
    },
    {
        accessorKey: 'isBanned',
        header: $t('ban_status'),
        cell: ({ row }) => {
            const isBanned = row.getValue('isBanned') as boolean;
            const color = isBanned ? 'error' : 'success';
            const label = $t(isBanned ? 'banned' : 'active');

            return h(UBadge, { class: 'capitalize', variant: 'subtle', color }, () => label)
        }
    },
    {
        id: 'actions',
        cell: ({ row }) => {
        return h(
            'div',
            { class: 'text-right' },
            h(
            UDropdownMenu,
            {
                content: {
                align: 'end'
                },
                items: getRowItems(row),
                'aria-label': 'Actions dropdown'
            },
            () =>
                h(UButton, {
                icon: 'i-lucide-ellipsis-vertical',
                color: 'neutral',
                variant: 'ghost',
                class: 'ml-auto',
                'aria-label': 'Actions dropdown'
                })
            )
        )
        }
    }
]

function getRowItems(row: Row<UserResponse>) {
    return [
        {
            type: 'label',
            label: 'Actions'
        },
        row.original.isBanned ? {
            label: 'Unban',
            onSelect() {
                unBan(row.original);
            }
        } : {
            label: 'Ban',
            onSelect() {
                openBanModal(row.original);
            }
        },
        {
            label: 'Set role',
            onSelect() {
                openRoleModal(row.original);
            }
        }
    ]
}

async function openBanModal(user: UserResponse) {
    if (user.username == authUser.value?.username) {
        toast.add({
            title: 'Fail!',
            description: 'Cannot ban yourself!',
            color: 'warning',
        })
        return;
    }
    const instance = modal.open({
        user: user
    });
}

async function unBan(user: UserResponse) {
    updateBanStatus(user.id, false);

    useAPI(`admin/users/${user.id}/ban`, {
        method: "DELETE"
    }).then(() => {
        
    }).catch(() => {
        updateBanStatus(user.id, true);
    })
}

function updateBanStatus(id: string, status: boolean) {
    updateUsers((p) => {
        const target = p.items?.find(u => u.id == id);
        if (target !== undefined)
            target.isBanned = status;

        return p;
    });
}

const table = useTemplateRef('table')

const rowSelection = ref<Record<string, boolean>>({})

function onSelect(e: Event, row: TableRow<UserResponse>) {
    row.toggleSelected(!row.getIsSelected())
}

callOnce(async () => {
    fetchUsers(globalFilter.value, currentPage.value, abortController.signal);
}, {
    mode: 'navigation'
})

async function updatePage(page: number) {
    if (pending.value) {
        abortController.abort();
    }
    
    currentPage.value = page;
    abortController = new AbortController();
    fetchUsers(globalFilter.value, currentPage.value, abortController.signal);
}

function applySearchFilter(value: string) {
    console.log(value);
    
    currentPage.value = 1;
    fetchUsers(globalFilter.value, currentPage.value, abortController.signal);
}

const updateDebouceSearch = debounce(applySearchFilter, 500);

// roles

const roleModalOpen = ref(false);
const selectedUser = ref<UserResponse>();

async function openRoleModal(user: UserResponse) {
    roleModalOpen.value = true;
    selectedUser.value = user;
}

async function closeRoleModal() {
    roleModalOpen.value = false;
}
</script>

<template>
    <UDashboardPanel id="users" resizable >
        <template #header>
            <UDashboardNavbar :title="$t('dashboard.users')" />
        </template>

        <template #body>
            <div class="flex w-full items-center justify-between">
                <UInput
                    v-model="globalFilter"
                    class="max-w-sm"
                    placeholder="Filter..."
                    v-on:update:model-value="updateDebouceSearch"
                />

                <UButton
                    v-if="table?.tableApi?.getFilteredSelectedRowModel().rows.length"
                    label="Delete"
                    color="error"
                    variant="subtle"
                    icon="i-lucide-trash"
                >
                    <template #trailing>
                        <UKbd>
                            {{ table?.tableApi?.getFilteredSelectedRowModel().rows.length }}
                        </UKbd>
                    </template>
                </UButton>
            </div>

            <UTable
                v-if="users"
                ref="table"
                :data="users.items"
                :columns="columns"
                @select="onSelect"
                :loading="pending"
                :ui="{
                    base: 'table-fixed border-separate border-spacing-0',
                    thead: '[&>tr]:bg-elevated/50 [&>tr]:after:content-none',
                    tbody: '[&>tr]:last:[&>td]:border-b-0',
                    th: 'py-2 first:rounded-l-lg last:rounded-r-lg border-y border-default first:border-l last:border-r',
                    td: 'border-b border-default',
                    separator: 'h-0'
                }"
            />
                
            <div class="flex items-center justify-between border-t border-accented py-3.5">
                <div class="text-sm text-muted">
                    {{ users?.size || 0 }} of
                    {{ users?.total || 0 }} row(s).
                </div>
                <UPagination
                    :default-page="currentPage"
                    :items-per-page="pageSize"
                    :total="users?.total"
                    @update:page="updatePage"
                />
            </div>

            <DashboardUserRoleModal
                :open="roleModalOpen"
                :user="selectedUser"
                v-on:close="closeRoleModal"
            />
        </template>
    </UDashboardPanel>
</template>
