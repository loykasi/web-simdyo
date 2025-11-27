<script setup lang="ts">
import * as z from 'zod';
import type { FormSubmitEvent } from '@nuxt/ui';
import type { Role } from '~/types/role.type';
import { permissionConfig } from '~/config/permissionConfig';

const toast = useToast();
const seletedPermissions = new Map<string, boolean>([]);

const props = defineProps<{
    open: boolean,
    role: Role | undefined
}>();

const emit = defineEmits<{
    close: [],
    update: [(value: Role[]) => Role[]]
}>();

watchEffect(() => {
    if (props.role) {
        state.name = props.role.name;
        seletedPermissions.clear();
    }
})

const { data: permissions, pending, refresh } = await useLazyAsyncData(
	"permissions",
	() => useAPI<string[]>("roles/permissions", {
		method: "GET"
	}), {
        server: false,
    }
);

const schema = z.object({
    name: z.string("required").nonempty("required"),
})

type Schema = z.output<typeof schema>

const state = reactive<Partial<Schema>>({
    name: "",
})

function togglePermission(permission: string, value: boolean) {    
    if (props.role === undefined) return;

    const initState = props.role.permissions.includes(permission);

    if (initState == value && seletedPermissions.has(permission)) {
        seletedPermissions.delete(permission);
        return;
    }

    seletedPermissions.set(permission, value);
}

async function onSubmit(event: FormSubmitEvent<Schema>) {
    const enables: string[] = [];
    const disables: string[] = [];
    seletedPermissions.forEach((value, key) => {
        if (value) {
            enables.push(key);
        } else {
            disables.push(key);
        }
    });

    useAPI<Role>(`roles/${props.role?.id}`, {
        method: "PUT",
        body: {
            name: event.data.name,
            enables,
            disables
        }
    }).then(() => {
        emit("update", (roles: Role[]) => {
            const role = roles.find(r => r.id == props.role?.id);
            if (!role) return roles;

            role.name = event.data.name;
            enables.forEach(e => {
                role.permissions.push(e);
            })
            disables.forEach(d => {
                role.permissions = role.permissions.filter(p => p !== d);
            })
            return roles;
        });
    }).catch(() => {
        toast.add({ title: 'Error', description: `Something goes wrong! Try again.`, color: 'error' });
    }).finally(() => {
        emit('close')
    });
}
</script>

<template>
    <UModal
        :open="open"
        :title="`${role?.name}`"
        description="Edit role"
        :close="{ onClick: () => emit('close') }"
    >
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
                
                <div>
                    <label class="block font-medium text-default">Permissions</label>
                    <div class="mt-1 space-y-4">
                        <USwitch
                            v-for="permission in permissions"
                            :default-value="role?.permissions.includes(permission)"
                            :label="permissionConfig.get(permission)"
                            v-on:update:model-value="(value) => togglePermission(permission, value)"
                        />
                    </div>
                </div>
                
                <div class="flex justify-end gap-2">
                    <UButton
                        label="Cancel"
                        color="neutral"
                        variant="subtle"
                        @click="() => emit('close')"
                    />
                    <UButton
                        label="Save"
                        color="primary"
                        variant="solid"
                        type="submit"
                    />
                </div>
            </UForm>
        </template>
    </UModal>
</template>