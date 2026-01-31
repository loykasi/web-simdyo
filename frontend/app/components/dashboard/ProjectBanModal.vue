<script setup lang="ts">
import * as z from "zod";
import type { FormSubmitEvent } from "@nuxt/ui";
import type { ProjectResponse } from "~/types/project.type";
import { useAdminProjectsStore } from "~/stores/admin/adminProjects.store";

const prop = defineProps<{
  project: ProjectResponse;
}>();

const emit = defineEmits<{
  close: [boolean];
}>();

const { update: updateProjects } = useAdminProjectsStore();

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

  try {
    await useAPI(`admin/projects/${prop.project.publicId}/ban`, {
      method: "POST",
      body: {
        reason: event.data.reason,
        description: event.data.description,
      },
    });

    updateBanStatus(prop.project.publicId, true);
    emit("close", true);
  } catch {
    updateBanStatus(prop.project.publicId, false);
  }
}

function updateBanStatus(id: string, status: boolean) {
  updateProjects((p) => {
    const target = p.items?.find((u) => u.publicId == id);
    if (target !== undefined) target.isBanned = status;

    return p;
  });
}
</script>

<template>
  <UModal :title="$t('ban')" :close="{ onClick: () => emit('close', false) }">
    <template #body>
      <UForm
        :schema="schema"
        :state="state"
        class="space-y-4"
        @submit="onSubmit"
      >
        <UFormField :label="$t('reason')" name="reason">
          <UInput v-model="state.reason" class="w-full mt-1" />
        </UFormField>

        <UFormField :label="$t('description')" name="description">
          <UInput v-model="state.description" class="w-full mt-1" />
        </UFormField>

        <UButton type="submit">
          {{ $t("confirm") }}
        </UButton>
      </UForm>
    </template>
  </UModal>
</template>
