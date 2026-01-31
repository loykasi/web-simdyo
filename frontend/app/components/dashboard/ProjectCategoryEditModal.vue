<script setup lang="ts">
import * as z from "zod";
import type { FormSubmitEvent } from "@nuxt/ui";
import type { ProjectCategory } from "~/types/projectCategory.type";
import { useAdminProjectCategoriesStore } from "~/stores/admin/adminProjectCategories.store";

const prop = defineProps<{
  category: ProjectCategory;
}>();

const emit = defineEmits<{
  close: [boolean];
}>();

const { categories } = useAdminProjectCategoriesStore();

const schema = z.object({
  name: z.string("required").nonempty("required"),
});
const open = ref(false);

type Schema = z.output<typeof schema>;

const state = reactive<Partial<Schema>>({
  name: prop.category.name,
});

const toast = useToast();
async function onSubmit(event: FormSubmitEvent<Schema>) {
  emit("close", true);
  try {
    const res = await useAPI<ProjectCategory>(
      `projects/categories/${prop.category.id}`,
      {
        method: "PUT",
        body: {
          name: event.data.name,
        },
      },
    );

    const index = categories.value.findIndex((c) => c.id == prop.category.id);
    if (index != -1) {
      categories.value[index]!.name = res.name;
      categories.value[index]!.updatedAt = res.updatedAt;
    }

    toast.add({
      title: "Success",
      description: `Category ${event.data.name} updated`,
      color: "success",
    });
  } catch {
    toast.add({
      title: "Error",
      description: `Something goes wrong! Try again.`,
      color: "error",
    });
  } finally {
    open.value = false;
  }
}
</script>

<template>
  <UModal
    v-model:open="open"
    :title="$t('create_category.title')"
    :description="$t('create_category.description')"
    :close="{ onClick: () => emit('close', false) }"
  >
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
            :label="$t('cancle')"
            color="neutral"
            variant="subtle"
            @click="() => emit('close', false)"
          />
          <UButton
            :label="$t('create')"
            color="primary"
            variant="solid"
            type="submit"
          />
        </div>
      </UForm>
    </template>
  </UModal>
</template>
