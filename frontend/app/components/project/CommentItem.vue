<script setup lang="ts">
import { useAuthStore } from '~/stores/auth.store';
import type { AddProjectCommentRequest, ProjectComment } from '~/types/projectComment.type';

const emit = defineEmits<{
    addComment: [payload: AddProjectCommentRequest],
    deleteComment: [commendId: number, parentId: number | null]
}>();

const props = defineProps<{
    comment: ProjectComment,
    parentId: number | null
}>();

const { isLoggedIn, user } = useAuthStore();

const showReplyForm = ref(false);

function toggleCommentForm() {
    showReplyForm.value = !showReplyForm.value;
}

function onFormCancle() {
    showReplyForm.value = false;
}

function onFormSubmit(payload: AddProjectCommentRequest) {
    payload.parentId = props.parentId;
    payload.repliedUsername = props.comment.username;
    emit("addComment", payload);

    showReplyForm.value = false;
}

async function deleteComment() {
    emit("deleteComment", props.comment.id, props.comment.parentId);
}

function toDuration(dateString: string) {
    const date = new Date(dateString);
    return Date.now() - date.getTime();
}
</script>
<template>
    <UCard
        class="mt-4"
        :ui="{
            header: 'flex justify-between items-center',
            body: '!py-2'
        }"
    >
        <template #header>
            <div>
                <NuxtLink :to="`/profile/${comment.username}`" class="text-default font-semibold">{{ comment.username }}</NuxtLink>
                <NuxtTime
                    :datetime="Date.now() - toDuration(comment.createdAt)"
                    relative                
                    class="ms-3 text-dimmed"
                />
            </div>
            <UButton
                v-if="comment.username == user?.username"
                color="neutral"
                variant="link"
                class="px-0"
                @click="deleteComment()"
            >
                Delete
            </UButton>
        </template>
        
        <template #default>
            <p>
                {{ comment.content }}
            </p>
            <UButton
                v-if="isLoggedIn"
                color="neutral"
                variant="link"
                class="px-0 mt-2"
                @click="toggleCommentForm"
            >
                Reply
            </UButton>
        </template>
    </UCard>
    <ProjectCommentForm
        v-if="showReplyForm"
        v-on:cancle="onFormCancle"
        v-on:add-comment="onFormSubmit"
    />
</template>