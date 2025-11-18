<script setup lang="ts">
import { h, resolveComponent } from 'vue';
import type { TableColumn, TableRow } from '@nuxt/ui';
import type { Row } from '@tanstack/vue-table';
import type { Pagination } from '~/types/pagination.type';
import type { ProjectReportResponse } from '~/types/projectReport.type';

const toast = useToast();
const UButton = resolveComponent('UButton');
const UCheckbox = resolveComponent('UCheckbox');
const UDropdownMenu = resolveComponent('UDropdownMenu');

const pageSize = 10;
let abortController = new AbortController();
const currentPage = ref(1);

const { data: userPagination, pending, refresh } = await useLazyAsyncData(
	"projectReports",
	() => useAPI<Pagination<ProjectReportResponse>>("projects/reports", {
		method: "GET",
        query: {
            page: currentPage.value,
            limit: pageSize,
        },
        signal: abortController.signal
	}), {
        server: false,
    }
);

const columns: TableColumn<ProjectReportResponse>[] = [
    {
        accessorKey: 'id',
        header: 'ID'
    },
    {
        accessorKey: 'reason',
        header: 'Reason'
    },
    {
        accessorKey: 'description',
        header: 'Description'
    },
    {
        accessorKey: 'projectPublicId',
        header: 'Project Public ID'
    },
    {
        accessorKey: 'username',
        header: 'Username'
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
    // {
    //     id: 'actions',
    //     cell: ({ row }) => {
    //     return h(
    //         'div',
    //         { class: 'text-right' },
    //         h(
    //         UDropdownMenu,
    //         {
    //             content: {
    //             align: 'end'
    //             },
    //             items: getRowItems(row),
    //             'aria-label': 'Actions dropdown'
    //         },
    //         () =>
    //             h(UButton, {
    //             icon: 'i-lucide-ellipsis-vertical',
    //             color: 'neutral',
    //             variant: 'ghost',
    //             class: 'ml-auto',
    //             'aria-label': 'Actions dropdown'
    //             })
    //         )
    //     )
    //     }
    // }
]

// function getRowItems(row: Row<ProjectReportResponse>) {
//     return [
//         {
//             type: 'label',
//             label: 'Actions'
//         },
//         {
//             label: 'Ban',
//             onSelect() {
//                 toast.add({
//                     title: 'Success!',
//                     color: 'success',
//                     icon: 'i-lucide-circle-check'
//                 })
//             }
//         }
//     ]
// }

const table = useTemplateRef('table')

function onSelect(e: Event, row: TableRow<ProjectReportResponse>) {
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
            </div>

            <UTable
                ref="table"
                :data="userPagination?.items"
                :columns="columns"
                :ui="{
                    thead: 'border-t border-(--ui-border-accented)'
                }"
            />
                
            <div class="flex items-center justify-end border-t border-accented py-3.5">
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
