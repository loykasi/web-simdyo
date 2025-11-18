<script setup lang="ts">
import * as z from 'zod';
import type { FormSubmitEvent } from '@nuxt/ui';
import type { ProjectResponse } from '~/types/project.type';
import type { ReportProjectRequest } from '~/types/projectReport.type';

const prop = defineProps<{
    project: ProjectResponse
}>();

const items = ref([
    'Select a reason',
    'Inappropriate content',
    'Other'
]);

const open = ref(false);
const loading = ref(false);

const toast = useToast();

const schema = z.object({
	reason: z.string().nonempty("Required"),
    description: z.string().optional(),
}).refine((data) => data.reason !== "Select a reason", {
    message: "Please select a reason",
    path: ["reason"],
}).refine((data) => {
    if (data.reason !== "Other") return true;

    return data.description !== "";
}, {
    message: "Required",
    path: ["description"],
});

type Schema = z.output<typeof schema>

const state = reactive<Partial<Schema>>({
    reason: "Select a reason",
    description: "",
})

function delay(time: number) {
  return new Promise(resolve => setTimeout(resolve, time));
}
async function onSubmit(event: FormSubmitEvent<Schema>) {
    loading.value = true;

    const payload: ReportProjectRequest = {
        reason: event.data.reason,
        description: event.data.description!
    };

    useAPI(`projects/${prop.project.publicId}/reports`, {
        method: "POST",
        body: payload
    }).then(() => {
        toast.add({
            title: 'Success!',
            description: 'Your report will be review by moderators',
            color: 'success',
            icon: 'i-lucide-circle-check'
        })
    }).catch(() => {
        toast.add({
            title: 'Error!',
            description: 'Something wrong! Try again.',
            color: 'error',
            icon: 'i-lucide-circle-check'
        })
    }).finally(() => {
        loading.value = false;
        open.value = false;
        state.reason = "Select a reason";
        state.description = "";
    });
}
</script>
<template>
    <UModal
        title="Report project"
        description="What's going on?"
        v-model:open="open"
        :dismissible="!loading"
    >
        <UButton color="warning">Report</UButton>

        <template #body>
            <UForm :schema="schema" :state="state" class="space-y-4" @submit="onSubmit">
                <UFormField label="Reason" name="reason" >
                    <USelect
                        v-model="state.reason"
                        :items="items"
                        class="w-full mt-1"
                    />
                </UFormField>

                <UFormField label="Tell use more specific" name="description" >
                    <UTextarea v-model="state.description" placeholder="Description" class="w-full mt-1" />
                </UFormField>

                <UButton type="submit" :loading="loading">
                    Submit
                </UButton>
            </UForm>
        </template>
    </UModal>
</template>
