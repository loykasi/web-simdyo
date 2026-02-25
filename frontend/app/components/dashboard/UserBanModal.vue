<script setup lang="ts">
import * as z from "zod";
import type { FormSubmitEvent } from "@nuxt/ui";
import type { UserResponse } from "~/types/user.type";
import { useAdminUsersStore } from "~/stores/admin/adminUsers.store";

const prop = defineProps<{
  user: UserResponse;
}>();

const emit = defineEmits<{
  close: [boolean];
}>();

const { update: updateUsers } = useAdminUsersStore();

const schema = z.object({
  reason: z.string().nonempty("Required"),
  description: z.string().optional(),
});

type Schema = z.output<typeof schema>;

const state = reactive<Partial<Schema>>({
  reason: "",
  description: "",
});

async function onSubmit(event: FormSubmitEvent<Schema>) {
  console.log(event.data);

  useAPI(`admin/users/${prop.user.id}/ban`, {
    method: "POST",
    body: {
      reason: event.data.reason,
      description: event.data.description,
    },
  }).catch(() => {
    updateBanStatus(prop.user.id, false);
  });

  updateBanStatus(prop.user.id, true);
  emit("close", true);
}

function updateBanStatus(id: string, status: boolean) {
  updateUsers((p) => {
    const target = p.items?.find((u) => u.id == id);
    if (target !== undefined) target.isBanned = status;

    return p;
  });
}
</script>

<template>
  <UModal
    :title="$t('dashboard.user_ban.title')"
    :description="`${$t('dashboard.user_ban.description')} ${user.username}`"
    :close="{ onClick: () => emit('close', false) }"
  >
    <template #body>
      <UForm
        :schema="schema"
        :state="state"
        class="space-y-4"
        @submit="onSubmit"
      >
        <UFormField :label="$t('common.fields.reason')" name="reason">
          <UInput v-model="state.reason" class="w-full mt-1" />
        </UFormField>

        <UFormField :label="$t('common.fields.description')" name="description">
          <UInput v-model="state.description" class="w-full mt-1" />
        </UFormField>

        <UButton type="submit">
          {{ $t("common.actions.confirm") }}
        </UButton>
      </UForm>
    </template>
  </UModal>
</template>
