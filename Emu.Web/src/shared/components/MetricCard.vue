<template>
  <LiquidCard class-name="metric-card reveal-up">
    <span>{{ label }}</span>
    <strong>{{ value }}</strong>
    <small>{{ hint }}</small>
  </LiquidCard>
</template>

<script setup lang="ts">
import LiquidCard from '@/shared/components/LiquidCard.vue'

defineProps<{
  label: string
  value: string | number
  hint: string
}>()
</script>
