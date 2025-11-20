<script setup lang="ts">
import * as z from 'zod';
import type { FormSubmitEvent } from '@nuxt/ui';
import type { ProjectCategory } from '~/types/projectCategory.type';
import { useAdminProjectCategoriesStore } from '~/stores/admin/adminProjectCategories.store';

const { categories } = useAdminProjectCategoriesStore();

const schema = z.object({
    name: z.string("required").nonempty("required"),
})
const open = ref(false)

type Schema = z.output<typeof schema>

const state = reactive<Partial<Schema>>({
    name: "",
})

const toast = useToast()
async function onSubmit(event: FormSubmitEvent<Schema>) {
    // toast.add({ title: 'Success', description: `New customer ${event.data.name} added`, color: 'success' })

    useAPI<ProjectCategory>(`projects/categories`, {
        method: "POST",
        body: {
            name: event.data.name
        }
    }).then((res) => {
        categories.value.push(res);
        toast.add({ title: 'Success', description: `New category ${event.data.name} added`, color: 'success' });
    }).catch(() => {
        toast.add({ title: 'Error', description: `Something goes wrong! Try again.`, color: 'error' });
    }).finally(() => {
        open.value = false;
    })
}
</script>

<template>
    <UModal v-model:open="open" title="New category" description="Add a new category">
        <UButton label="New category" icon="i-lucide-plus" />

        <template #body>
        <UForm
            :schema="schema"
            :state="state"
            class="space-y-4"
            @submit="onSubmit"
        >
            <UFormField label="Category" placeholder="Name" name="name">
                <UInput v-model="state.name" class="w-full" />
            </UFormField>
            <div class="flex justify-end gap-2">
                <UButton
                    label="Cancel"
                    color="neutral"
                    variant="subtle"
                    @click="open = false"
                />
                <UButton
                    label="Create"
                    color="primary"
                    variant="solid"
                    type="submit"
                />
            </div>
        </UForm>
        </template>
    </UModal>
</template>