<script setup lang="ts">
import { h, resolveComponent } from 'vue';
import type { TableColumn, TableRow } from '@nuxt/ui';
import type { Row } from '@tanstack/vue-table';
import type { Pagination } from '~/types/pagination.type';
import type { ProjectReportResponse } from '~/types/projectReport.type';
import { debounce } from '~/utilities/debounce';

const toast = useToast();
const UButton = resolveComponent('UButton');
const UCheckbox = resolveComponent('UCheckbox');
const UDropdownMenu = resolveComponent('UDropdownMenu');

const pageSize = 10;
let abortController = new AbortController();
const currentPage = ref(1);

const { data: reports, pending, refresh } = await useLazyAsyncData(
	"projectReports",
	() => useAPI<Pagination<ProjectReportResponse>>("projects/reports", {
		method: "GET",
        query: {
            filter: globalFilter.value,
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


function applySearchFilter(value: string) {
    console.log(value);
    
    currentPage.value = 1;
    refresh();
}

const updateDebouceSearch = debounce(applySearchFilter, 500);
</script>

<template>
    <UDashboardPanel id="users" resizable >
        <template #header>
            <UDashboardNavbar :title="$t('dashboard.reports')" />
        </template>

        <template #body>
            <div class="flex w-full items-center justify-between">
                <UInput
                    v-model="globalFilter"
                    class="max-w-sm"
                    placeholder="Filter..."
                    v-on:update:model-value="updateDebouceSearch"
                />
            </div>

            <UTable
                v-if="reports"
                ref="table"
                :data="reports?.items"
                :columns="columns"
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
                    {{ reports?.size || 0 }} of
                    {{ reports?.total || 0 }} row(s) selected.
                </div>
                <UPagination
                    :default-page="currentPage"
                    :items-per-page="pageSize"
                    :total="reports?.total"
                    @update:page="updatePage"
                />
            </div>
        </template>
    </UDashboardPanel>
</template>
