<script setup lang="ts">
import ConfirmationDialog from "~/components/ConfirmationDialog.vue";
import RoleAddModal from "~/components/dashboard/RoleAddModal.vue";
import type { Role } from "~/types/role.type";

const toast = useToast();
const overlay = useOverlay();
const confirmationDialog = overlay.create(ConfirmationDialog);

const {
  data: roles,
  pending,
} = await useLazyAsyncData(
  "roles",
  () =>
    useAPI<Role[]>("roles", {
      method: "GET",
    }),
  {
    server: false,
  },
);

const editModalOpen = ref(false);
const selectedRole = ref<Role>();

function openEditRole(role: Role) {
  editModalOpen.value = true;
  selectedRole.value = role;
}

function closeEditModel() {
  editModalOpen.value = false;
}

async function deleteRole(role: Role) {
  const instance = confirmationDialog.open({
    title: `Delete role ${role.name}`,
    description: `Do you want to delete role ${role.name}`,
  });

  const result = await instance.result;

  if (result) {
    useAPI<Role>(`roles/${role.id}`, {
      method: "DELETE",
    }).catch(() => {
      updateRole((roles) => {
        roles.push(role);
        return roles;
      });
      toast.add({
        title: "Error",
        description: `Something goes wrong! Try again.`,
        color: "error",
      });
    });

    console.log(role.id);
    updateRole((values) => {
      return values.filter((r) => r.id != role.id);
    });
  }
}

function updateRole(update: (value: Role[]) => Role[]) {
  if (!roles.value) return;
  roles.value = update([...roles.value]);
}
</script>

<template>
  <UDashboardPanel id="users" resizable>
    <template #header>
      <UDashboardNavbar :title="$t('dashboard.roles')">
        <template #right>
          <RoleAddModal @update="updateRole" />
        </template>
      </UDashboardNavbar>
    </template>

    <template v-if="!pending" #body>
      <div>
        <div
          class="px-4 flex w-full items-center font-bold py-2 bg-elevated/50 rounded-lg border border-default"
        >
          <div>ROLES</div>
        </div>
        <div>
          <div
            v-for="role in roles"
            :key="role.id"
            class="px-4 py-3 flex justify-between border-b border-default hover:rounded-md hover:bg-elevated/50"
          >
            <div class="text-lg">
              {{ role.name }}
            </div>
            <div class="flex gap-x-2">
              <UButton
                icon="material-symbols:edit-outline"
                color="neutral"
                variant="subtle"
                @click="openEditRole(role)"
              />
              <UButton
                icon="material-symbols:delete-outline"
                color="error"
                variant="subtle"
                @click="deleteRole(role)"
              />
            </div>
          </div>
        </div>
      </div>
      <DashboardRoleEditModal
        :open="editModalOpen"
        :role="selectedRole"
        @update="updateRole"
        @close="closeEditModel"
      />
    </template>
  </UDashboardPanel>
</template>
