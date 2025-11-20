<script setup lang="ts">
import { h, resolveComponent } from 'vue';
import type { TableColumn, TableRow } from '@nuxt/ui';
import type { Row } from '@tanstack/vue-table';
import { UBadge } from '#components';
import { useAdminProjectCategoriesStore } from '~/stores/admin/adminProjectCategories.store';
import type { ProjectCategory } from '~/types/projectCategory.type';
import ProjectCategoryEditModal from '~/components/dashboard/ProjectCategoryEditModal.vue';

const overlay = useOverlay();
const modal = overlay.create(ProjectCategoryEditModal);

const toast = useToast();
const UButton = resolveComponent('UButton');
const UCheckbox = resolveComponent('UCheckbox');
const UDropdownMenu = resolveComponent('UDropdownMenu');

const {
    categories,
    pending,
    fetch: fetchCategories,
} = useAdminProjectCategoriesStore();

let abortController = new AbortController();

const columns: TableColumn<ProjectCategory>[] = [
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
        accessorKey: 'name',
        header: 'Name'
    },
    {
        accessorKey: 'createdAt',
        header: 'Created Date',
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
        accessorKey: 'updatedAt',
        header: 'Updated Date',
        cell: ({ row }) => {
        return new Date(row.getValue('updatedAt')).toLocaleString('en-US', {
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
                () => h(UButton, {
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

function getRowItems(row: Row<ProjectCategory>) {
    return [
        {
            type: 'label',
            label: 'Actions'
        },
        {
            label: 'Edit',
            onSelect() {
                updateCategory(row.original);
            }
        },
        {
            label: 'Delete',
            onSelect() {
                deleteCategory(row.original);
            }
        }
    ]
}

async function deleteCategory(category: ProjectCategory) {
    useAPI(`projects/categories/${category.id}`, {
        method: "DELETE"
    }).then(() => {
        categories.value = categories.value.filter(c => c.id != category.id);
    }).catch(() => {
        
    })
}

async function updateCategory(category: ProjectCategory) {
    const instance = modal.open({
        category: category
    });
}

const table = useTemplateRef('table')

function onSelect(e: Event, row: TableRow<ProjectCategory>) {
    row.toggleSelected(!row.getIsSelected())
}

const globalFilter = ref('')

callOnce(async () => {
    fetchCategories(abortController.signal);
}, {
    mode: 'navigation'
})

async function updatePage(page: number) {
    if (pending.value) {
        abortController.abort();
    }

    abortController = new AbortController();
    fetchCategories(abortController.signal);
}
</script>

<template>
    <UDashboardPanel id="users" resizable >
        <template #header>
            <UDashboardNavbar title="Categories">
                <template #right>
                    <DashboardProjectCategoryAddModal />
                </template>
            </UDashboardNavbar>
        </template>

        <template v-if="!pending" #body>
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
                :data="categories"
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
            </div>
        </template>
    </UDashboardPanel>
</template>
