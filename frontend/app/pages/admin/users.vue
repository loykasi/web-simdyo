<script setup lang="ts">
import { h, resolveComponent } from 'vue'
import type { TableColumn, TableRow } from '@nuxt/ui'
import type { Row } from '@tanstack/vue-table'

const toast = useToast()
const UButton = resolveComponent('UButton')
const UCheckbox = resolveComponent('UCheckbox')
const UDropdownMenu = resolveComponent('UDropdownMenu')

type User = {
  id: string
  joinedDate: string
  email: string
}

const data = ref<User[]>([
  {
    id: '4600',
    joinedDate: '2024-03-11T15:30:00',
    email: 'james.anderson@example.com',
  },
  {
    id: '4599',
    joinedDate: '2024-03-11T10:10:00',
    email: 'mia.white@example.com',
  }
])

const columns: TableColumn<User>[] = [
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
        accessorKey: 'joinedDate',
        header: 'Joined Date',
        cell: ({ row }) => {
        return new Date(row.getValue('joinedDate')).toLocaleString('en-US', {
            day: 'numeric',
            month: 'short',
            hour: '2-digit',
            minute: '2-digit',
            hour12: false
        })
        }
    },
    {
        accessorKey: 'email',
        header: 'Email'
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

function getRowItems(row: Row<User>) {
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

const rowSelection = ref<Record<string, boolean>>({})

function onSelect(e: Event, row: TableRow<User>) {
  /* If you decide to also select the column you can do this  */
  row.toggleSelected(!row.getIsSelected())
}

const globalFilter = ref('')
const pagination = ref({
    pageIndex: 0,
    pageSize: 5
})
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
                v-model:row-selection="rowSelection"
                v-model:global-filter="globalFilter" 
                :data="data"
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
                    :default-page="(table?.tableApi?.getState().pagination.pageIndex || 0) + 1"
                    :items-per-page="table?.tableApi?.getState().pagination.pageSize"
                    :total="table?.tableApi?.getFilteredRowModel().rows.length"
                    @update:page="(p) => table?.tableApi?.setPageIndex(p - 1)"
                />
            </div>
        </template>
    </UDashboardPanel>
</template>
