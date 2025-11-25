<script setup lang="ts">
import * as z from 'zod';
import type { FormSubmitEvent } from '@nuxt/ui';
import type { Role } from '~/types/role.type';

const emit = defineEmits<{
    update: [(value: Role[]) => Role[]]
}>();

const open = ref(false);

const schema = z.object({
    name: z.string("required").nonempty("required"),
})

type Schema = z.output<typeof schema>

const state = reactive<Partial<Schema>>({
    name: "",
})

const toast = useToast()
async function onSubmit(event: FormSubmitEvent<Schema>) {
    useAPI<Role>(`roles`, {
        method: "POST",
        body: {
            name: event.data.name
        }
    }).then((res) => {
        emit("update", (roles: Role[]) => {
            roles.push({
                id: res.id,
                name: res.name,
                permissions: res.permissions
            });
            return roles;
        });
    }).catch(() => {
        toast.add({ title: 'Error', description: `Something goes wrong! Try again.`, color: 'error' });
    }).finally(() => {
        open.value = false;
    })
}
</script>

<template>
    <UModal
        v-model:open="open"
        title="New role"
        description="Add a new role"
    >
        <UButton label="New role" icon="i-lucide-plus" />

        <template #body>
            <UForm
                :schema="schema"
                :state="state"
                class="space-y-4"
                @submit="onSubmit"
            >
                <UFormField label="Name" placeholder="Name" name="name">
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