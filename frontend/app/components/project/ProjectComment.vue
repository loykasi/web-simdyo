<script setup lang="ts">
import type { ProjectComment } from '~/types/projectComment.type';
import { useAuthStore } from '~/stores/auth.store';
import type { AddProjectCommentRequest } from '../../types/projectComment.type';
import CommentItem from './CommentItem.vue';

const { isLoggedIn } = useAuthStore();
const route = useRoute();
const projectId = route.params.id as string;
const commentCount = ref(0);

const { data: comments, status } = await useAsyncData(
	`project.${projectId}.comments`,
	() => useAPI<ProjectComment[]>(`projects/${projectId}/comments`, {
		method: "GET",
        query: {
            limit: 2,
        }

	}),
    {
        deep: true
    }
);

watchEffect(() => {
    if (status.value == 'success') {
        if (comments.value != undefined) {
            commentCount.value = comments.value.length;
        }
    }
});

async function addComment(payload: AddProjectCommentRequest) {
    useAPI<ProjectComment>(`projects/${projectId}/comments`, {
		method: "POST",
        body: payload
	})
    .then(res => {
        if (comments.value != undefined) {
            console.log(res);

            if (res.parentId == null) {
                comments.value.unshift(res);
            } else {
                const comment = comments.value.find(c => c.id == res.parentId);
                if (comment!.replies == undefined) {
                    comment!.replies = [];
                }
                comment!.replies.push(res);
                comment!.totalReplies++;
            }
        }
    })
    .catch(err => {
        console.log(err);
    });
}

async function deleteComment(commendId: number, parentId: number | null) {
    useAPI(`projects/${projectId}/comments`, {
		method: "DELETE",
        query: {
            commentId: commendId,
        }
	})
    .then(res => {
        if (comments.value != undefined) {

            console.log(commendId + " | " + parentId);
            if (parentId == null) {
                comments.value = comments.value.filter(c => c.id != commendId);
                commentCount.value--;
            } else {
                const comment = comments.value.find(c => c.id == parentId);
                if (comment!.replies != undefined) {
                    comment!.replies = comment!.replies.filter(c => c.id != commendId);
                    comment!.totalReplies--;
                    comment!.replyCount--;
                }
            }
        }
    })
    .catch(err => {
        console.log(err);
    });
}

async function loadComments() {
    if (comments.value == undefined) return;

    const lastId = comments.value[commentCount.value - 1]?.id;
    
    useAPI<ProjectComment[]>(`projects/${projectId}/comments`, {
		method: "GET",
        query: {
            limit: 2,
            lastId: lastId,
        }
	})
    .then(res => {
        if (comments.value == undefined) return;

        comments.value = comments.value.slice(0, commentCount.value);
        
        comments.value = [...comments.value, ...res];
        commentCount.value = comments.value.length;
    })
    .catch(err => {
        console.log(err);
    });
}

function loadReplies(id: number) {
    const limit = 2;
    const parentId = id;
    let lastId = null;

    const comment = comments.value?.find(c => c.id == id);
    if (comment == undefined) return;

    if (comment.replyCount > 0) {
        lastId = comment.replies[comment.replyCount - 1]?.id;
    }
    
    useAPI<ProjectComment[]>(`projects/${projectId}/comments`, {
		method: "GET",
        query: {
            limit: limit,
            parentId: parentId,
            lastId: lastId,
        }
	})
    .then(res => {
        console.log(res);

        if (comment.replies == undefined || comment.replyCount == 0) {
            comment.replies = [];
        } else {
            comment.replies = comment.replies.slice(0, comment.replyCount);
        }
        comment.replies = [...comment.replies, ...res];
        comment.replyCount = comment.replies.length;

        console.log(comment.replyCount);
    })
    .catch(err => {
        console.log(err);
    });
}
</script>
<template>
    <div class="grid grid-cols-3 mt-8">
        <div class="col-span-2">
            <h1 class="text-2xl font-semibold">Comments</h1>
            <ProjectCommentForm v-if="isLoggedIn" v-on:add-comment="addComment" />
            <div class="mt-8">
                <div
                    v-for="comment in comments"
                >
                    <CommentItem
                        :comment="comment"
                        :parent-id="comment.id"
                        v-on:add-comment="addComment"
                        v-on:delete-comment="deleteComment"
                    />
                    <UButton
                        v-if="comment.totalReplies > 0 && (comment.replies == undefined || comment.replies.length == 0)"
                        variant="ghost"
                        class="px-6 mt-2 font-semibold"
                        @click="loadReplies(comment.id)"
                    >
                        {{ comment.totalReplies }} replies
                    </UButton>
                    <div
                        v-if="comment.replies != undefined"
                        class="ps-20"
                    >
                        <CommentItem
                            v-for="reply in comment.replies"
                            :comment="reply"
                            :parent-id="comment.id"
                            v-on:add-comment="addComment"
                            v-on:delete-comment="deleteComment"
                        />
                        <UButton
                            v-if="comment.totalReplies != comment.replies.length"
                            variant="ghost"
                            class="px-6 mt-2 font-semibold"
                            @click="loadReplies(comment.id)"
                        >
                            Show more replies
                        </UButton>
                    </div>
                </div>
            </div>
            <UButton
                class="w-full justify-center px-6 mt-4 font-semibold"
                @click="loadComments"
            >
                Show more comments
            </UButton>
        </div>
    </div>
</template>