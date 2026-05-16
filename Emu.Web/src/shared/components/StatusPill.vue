<template>
  <b :class="['status-pill', variant]">
    <slot />
  </b>
</template>

<script setup lang="ts">
withDefaults(
  defineProps<{
    variant?: 'active' | 'disabled' | 'warning' | 'danger'
  }>(),
  {
    variant: 'active',
  },
)
</script>
