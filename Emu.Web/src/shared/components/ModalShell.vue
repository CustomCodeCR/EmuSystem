<template>
  <div class="modal-backdrop" @click.self="$emit('close')">
    <LiquidCard class-name="modal-card">
      <slot />
    </LiquidCard>
  </div>
</template>

<script setup lang="ts">
import LiquidCard from '@/shared/components/LiquidCard.vue'

defineEmits<{
  close: []
}>()
</script>
