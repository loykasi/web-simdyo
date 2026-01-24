<script setup lang="ts">
import { h, resolveComponent } from 'vue';
import type { TableColumn, TableRow } from '@nuxt/ui';
import type { Row } from '@tanstack/vue-table';
import type { Pagination } from '~/types/pagination.type';
import type { ProjectResponse } from '~/types/project.type';
import ProjectBanModal from '~/components/dashboard/ProjectBanModal.vue';
import { UBadge } from '#components';
import { useAdminProjectsStore } from '~/stores/admin/adminProjects.store';
import { debounce } from '~/utilities/debounce';

const overlay = useOverlay();
const modal = overlay.create(ProjectBanModal);

const toast = useToast();
const UButton = resolveComponent('UButton');
const UCheckbox = resolveComponent('UCheckbox');
const UDropdownMenu = resolveComponent('UDropdownMenu');

const {
    projects,
    pending,
    pageSize,
    fetch: fetchProjects,
    update: updateProjects
} = useAdminProjectsStore();

let abortController = new AbortController();
const currentPage = ref(1);

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
        header: $t('title')
    },
    {
        accessorKey: 'category',
        header: $t('category')
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
        accessorKey: 'deletedAt',
        header: $t('public_status'),
        cell: ({ row }) => {
            const isPublic = row.getValue('deletedAt') == null;
            const color = isPublic ? 'success' : 'error';
            const label = isPublic ? 'Public' : 'Trash';

            return h(UBadge, { class: 'capitalize', variant: 'subtle', color }, () => label)
        }
    },
    {
        accessorKey: 'isBanned',
        header: $t('ban_status'),
        cell: ({ row }) => {
            const isBanned = row.getValue('isBanned') as boolean;
            const color = isBanned ? 'error' : 'success';
            const label = $t(isBanned ? 'banned' : 'active')

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

function getRowItems(row: Row<ProjectResponse>) {
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
        }
    ]
}

async function openBanModal(project: ProjectResponse) {
    const instance = modal.open({
        project: project
    });
}

async function unBan(project: ProjectResponse) {
    useAPI(`admin/projects/${project.publicId}/ban`, {
        method: "DELETE"
    }).then(() => {
        
    }).catch(() => {
        updateBanStatus(project.publicId, true);
    })
}

function updateBanStatus(id: string, status: boolean) {
    updateProjects((p) => {
        const target = p.items?.find(u => u.publicId == id);
        if (target !== undefined)
            target.isBanned = status;

        return p;
    });
}

const table = useTemplateRef('table')

function onSelect(e: Event, row: TableRow<ProjectResponse>) {
    row.toggleSelected(!row.getIsSelected())
}

const globalFilter = ref('')

callOnce(async () => {
    fetchProjects(globalFilter.value, currentPage.value, abortController.signal);
}, {
    mode: 'navigation'
})

async function updatePage(page: number) {
    if (pending.value) {
        abortController.abort();
    }
    
    currentPage.value = page;
    abortController = new AbortController();
    fetchProjects(globalFilter.value, currentPage.value, abortController.signal);
}

function applySearchFilter(value: string) {
    console.log(value);
    
    currentPage.value = 1;
    fetchProjects(globalFilter.value, currentPage.value, abortController.signal);
}

const updateDebouceSearch = debounce(applySearchFilter, 500);
</script>

<template>
    <UDashboardPanel id="users" resizable >
        <template #header>
            <UDashboardNavbar :title="$t('dashboard.projects')" />
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
                v-if="projects"
                ref="table"
                :data="projects?.items"
                :columns="columns"
                :loading="pending"
                @select="onSelect"
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
                    {{ projects?.size || 0 }} of
                    {{ projects?.total || 0 }} row(s).
                </div>
                <UPagination
                    :default-page="currentPage"
                    :items-per-page="pageSize"
                    :total="projects?.total"
                    @update:page="updatePage"
                />
            </div>
        </template>
    </UDashboardPanel>
</template>
