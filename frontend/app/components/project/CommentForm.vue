<script setup lang="ts">
import * as z from 'zod';
import type { FormSubmitEvent } from '@nuxt/ui';
import type { AddProjectCommentRequest } from '~/types/projectComment.type';

const emit = defineEmits<{
    addComment: [payload: AddProjectCommentRequest],
    cancle: []
}>();

const schema = z.object({
    content: z.string('Cannot be empty'),
})

type Schema = z.output<typeof schema>

const isContentInvalid = computed(() => state.content == "");

const state = reactive<Partial<Schema>>({
    content: "",
})

function cancleComment() {
    state.content = "";
    emit("cancle");
}

async function onSubmit(event: FormSubmitEvent<Schema>) {
    const payload: AddProjectCommentRequest = {
        content: event.data.content,
        parentId: null,
        repliedUsername: null
    }
    state.content = "";
    emit("addComment", payload);
}
</script>
<template>
    <div
        class="mt-4"
    >
        <UForm :schema="schema" :state="state" class="space-y-4" @submit="onSubmit">
            <UFormField name="content">
                <UTextarea
                    v-model="state.content"
                    class="w-full min-h-6 field-sizing-content"
                    :rows="1"
                    autoresize
                    placeholder="Add a comment"
                />
            </UFormField>

            <div class="mt-2 flex gap-x-2">
                <UButton type="submit" :disabled="isContentInvalid">Comment</UButton>
                <UButton color="neutral" variant="ghost" @click="cancleComment">Cancle</UButton>
            </div>
        </UForm>
    </div>
</template>