<script setup lang="ts">
import { h, resolveComponent } from 'vue';
import type { TableColumn, TableRow } from '@nuxt/ui';
import type { Row } from '@tanstack/vue-table';
import type { Pagination } from '~/types/pagination.type';
import type { ProjectResponse } from '~/types/project.type';

const toast = useToast();
const UButton = resolveComponent('UButton');
const UCheckbox = resolveComponent('UCheckbox');
const UDropdownMenu = resolveComponent('UDropdownMenu');

const pageSize = 10;
let abortController = new AbortController();
const currentPage = ref(1);

const { data: userPagination, pending, refresh } = await useLazyAsyncData(
	"projects",
	() => useAPI<Pagination<ProjectResponse>>(`projects`, {
		method: "GET",
        query: {
            pageNumber: currentPage.value,
            limit: pageSize,
        },
        signal: abortController.signal
	}), {
        server: false,
    }
);

const columns: TableColumn<ProjectResponse>[] = [
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
        accessorKey: 'publicId',
        header: 'Public ID'
    },
    {
        accessorKey: 'title',
        header: 'Title'
    },
    {
        accessorKey: 'category',
        header: 'Category'
    },
    {
        accessorKey: 'createdAt',
        header: 'Joined Date',
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

function getRowItems(row: Row<ProjectResponse>) {
    return [
        {
            type: 'label',
            label: 'Actions'
        },
        {
            label: 'Ban',
            onSelect() {
                toast.add({
                    title: 'Success!',
                    color: 'success',
                    icon: 'i-lucide-circle-check'
                })
            }
        }
    ]
}

const table = useTemplateRef('table')

function onSelect(e: Event, row: TableRow<ProjectResponse>) {
    row.toggleSelected(!row.getIsSelected())
}

const globalFilter = ref('')

async function updatePage(page: number) {
    if (pending.value) {
        abortController.abort();
    }
    abortController = new AbortController();
    currentPage.value = page;
    refresh();
}
</script>

<template>
    <UDashboardPanel id="users" resizable >
        <template #header>
            <UDashboardNavbar title="Users" />
        </template>

        <template #body>
            <div class="flex w-full items-center justify-between">
                <UInput v-model="globalFilter" class="max-w-sm" placeholder="Filter..." />

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
                ref="table"
                :data="userPagination?.items"
                :columns="columns"
                @select="onSelect"
                :ui="{
                    thead: 'border-t border-(--ui-border-accented)'
                }"
            />
                
            <div class="flex items-center justify-between border-t border-accented py-3.5">
                <div class="text-sm text-muted">
                    {{ table?.tableApi?.getFilteredSelectedRowModel().rows.length || 0 }} of
                    {{ table?.tableApi?.getFilteredRowModel().rows.length || 0 }} row(s) selected.
                </div>
                <UPagination
                    :default-page="currentPage"
                    :items-per-page="pageSize"
                    :total="userPagination?.total"
                    @update:page="updatePage"
                />
            </div>
        </template>
    </UDashboardPanel>
</template>
