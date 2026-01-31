<script setup lang="ts">
import type { ProjectComment } from "~/types/projectComment.type";
import type { Pagination } from "~/types/pagination.type";
import { useAuthStore } from "~/stores/auth.store";
import type { AddProjectCommentRequest } from "../../types/projectComment.type";
import CommentItem from "./CommentItem.vue";

const pageSize = 10;

const { isLoggedIn } = useAuthStore();
const route = useRoute();
const projectId = route.params.id as string;
// const commentCount = ref(0);

const { data: pagination, status } = await useAsyncData(
  `project.${projectId}.comments`,
  () =>
    useAPI<Pagination<ProjectComment>>(`projects/${projectId}/comments`, {
      method: "GET",
      query: {
        limit: pageSize,
      },
    }),
  {
    deep: true,
  },
);

// watchEffect(() => {
//     if (status.value == 'success') {
//         console.log(pagination.value);
//         // if (comments.value != undefined) {
//         //     commentCount.value = comments.value.length;
//         // }
//     }
// });

async function addComment(payload: AddProjectCommentRequest) {
  try {
    const res = await useAPI<ProjectComment>(`projects/${projectId}/comments`, {
      method: "POST",
      body: payload,
    });

    if (pagination.value != undefined) {
      console.log(res);

      if (res.parentId == null) {
        pagination.value.items.unshift(res);
      } else {
        const comment = pagination.value.items.find(
          (c) => c.id == res.parentId,
        );
        if (comment!.replies == undefined) {
          comment!.replies = [];
        }
        comment!.replies.push(res);
        comment!.totalReplies++;
      }
    }
  } catch (error) {
    console.log(error);
  }
}

async function deleteComment(commendId: number, parentId: number | null) {
  try {
    await useAPI(`projects/${projectId}/comments`, {
      method: "DELETE",
      query: {
        commentId: commendId,
      },
    });

    if (pagination.value != undefined) {
      console.log(commendId + " | " + parentId);
      if (parentId == null) {
        pagination.value.items = pagination.value.items.filter(
          (c) => c.id != commendId,
        );
        pagination.value.size--;
      } else {
        const comment = pagination.value.items.find((c) => c.id == parentId);
        if (comment!.replies != undefined) {
          comment!.replies = comment!.replies.filter((c) => c.id != commendId);
          comment!.totalReplies--;
          comment!.replyCount--;
        }
      }
    }
  } catch (error) {
    console.log(error);
  }
}

async function loadComments() {
  if (pagination.value == undefined) return;

  const lastId = pagination.value.items[pagination.value.size - 1]?.id;

  useAPI<Pagination<ProjectComment>>(`projects/${projectId}/comments`, {
    method: "GET",
    query: {
      limit: pageSize,
      lastId: lastId,
    },
  })
    .then((res) => {
      if (pagination.value == undefined) return;

      pagination.value.items = pagination.value.items.slice(
        0,
        pagination.value.size,
      );

      pagination.value.items = [...pagination.value.items, ...res.items];
      pagination.value.size = pagination.value.items.length;
    })
    .catch((err) => {
      console.log(err);
    });
}

function loadReplies(id: number) {
  const limit = 2;
  const parentId = id;
  let lastId = null;

  const comment = pagination.value?.items.find((c) => c.id == id);
  if (comment == undefined) return;

  if (comment.replyCount > 0) {
    lastId = comment.replies[comment.replyCount - 1]?.id;
  }

  useAPI<Pagination<ProjectComment>>(`projects/${projectId}/comments`, {
    method: "GET",
    query: {
      limit: limit,
      parentId: parentId,
      lastId: lastId,
    },
  })
    .then((res) => {
      console.log(res);

      if (comment.replies == undefined || comment.replyCount == 0) {
        comment.replies = [];
      } else {
        comment.replies = comment.replies.slice(0, comment.replyCount);
      }
      comment.replies = [...comment.replies, ...res.items];
      comment.replyCount = comment.replies.length;

      console.log(comment.replyCount);
    })
    .catch((err) => {
      console.log(err);
    });
}
</script>
<template>
  <div class="grid grid-cols-3 mt-8">
    <div class="col-span-2">
      <h1 class="text-2xl font-semibold">{{ $t("project.comments") }}</h1>
      <ProjectCommentForm v-if="isLoggedIn" @add-comment="addComment" />
      <div class="mt-8">
        <div v-for="comment in pagination?.items" :key="comment.id">
          <CommentItem
            :comment="comment"
            :parent-id="comment.id"
            @add-comment="addComment"
            @delete-comment="deleteComment"
          />
          <UButton
            v-if="
              comment.totalReplies > 0 &&
              (comment.replies == undefined || comment.replies.length == 0)
            "
            variant="ghost"
            class="px-6 mt-2 font-semibold"
            @click="loadReplies(comment.id)"
          >
            {{ comment.totalReplies }} replies
          </UButton>
          <div v-if="comment.replies != undefined" class="ps-20">
            <CommentItem
              v-for="reply in comment.replies"
              :key="reply.id"
              :comment="reply"
              :parent-id="comment.id"
              @add-comment="addComment"
              @delete-comment="deleteComment"
            />
            <UButton
              v-if="comment.totalReplies != comment.replies.length"
              variant="ghost"
              class="px-6 mt-2 font-semibold"
              @click="loadReplies(comment.id)"
            >
              {{ $t("project.show_replies") }}
            </UButton>
          </div>
        </div>
      </div>
      <UButton
        v-if="pagination?.size != pagination?.total"
        class="w-full justify-center px-6 mt-4 font-semibold"
        @click="loadComments"
      >
        {{ $t("project.show_comments") }}
      </UButton>
    </div>
  </div>
</template>
