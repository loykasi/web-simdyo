<script setup lang="ts">
import type { UserResponse } from "~/types/user.type";
import { useAdminUsersStore } from "~/stores/admin/adminUsers.store";

const prop = defineProps<{
  open: boolean;
  user: UserResponse | undefined;
}>();

const emit = defineEmits<{
  close: [];
}>();

const { update: updateUsers } = useAdminUsersStore();
const values = ref<string[]>([]);

watchEffect(() => {
  if (!prop.user) return;

  values.value = prop.user.roles;
});

const { data: roles } = await useLazyAsyncData(
  "roleNames",
  () =>
    useAPI<string[]>("roles", {
      method: "GET",
      query: {
        nameOnly: true,
      },
    }),
  {
    server: false,
  },
);

async function onSubmit() {
  if (!prop.user) return;

  try {
    await useAPI(`admin/users/${prop.user.id}/roles`, {
      method: "PUT",
      body: {
        roles: values.value,
      },
    });
    updateRole(prop.user!.id, values.value);
  } finally {
    emit("close");
  }
}

function updateRole(id: string, roles: string[]) {
  updateUsers((p) => {
    const target = p.items?.find((u) => u.id == id);
    if (target !== undefined) target.roles = roles;

    return p;
  });
}
</script>

<template>
  <UModal
    :open="open"
    :title="$t('role')"
    :close="{ onClick: () => emit('close') }"
  >
    <template #body>
      <UForm class="space-y-4" @submit="onSubmit">
        <UFormField name="roles">
          <UCheckboxGroup v-model="values" :items="roles" size="xl" />
        </UFormField>

        <UButton type="submit">
          {{ $t("confirm") }}
        </UButton>
      </UForm>
    </template>
  </UModal>
</template>
