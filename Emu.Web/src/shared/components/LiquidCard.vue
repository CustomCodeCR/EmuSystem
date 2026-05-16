<template>
  <section :class="['liquid-card', className]">
    <slot />
  </section>
</template>

<script setup lang="ts">
defineProps<{
  className?: string
}>()
</script>
