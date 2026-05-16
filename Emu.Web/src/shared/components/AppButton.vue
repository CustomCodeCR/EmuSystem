<template>
  <button :class="['app-button', variant, { compact }]" :type="type" :disabled="disabled">
    <slot />
  </button>
</template>

<script setup lang="ts">
withDefaults(
  defineProps<{
    variant?: 'primary' | 'ghost' | 'danger'
    compact?: boolean
    type?: 'button' | 'submit' | 'reset'
    disabled?: boolean
  }>(),
  {
    variant: 'primary',
    compact: false,
    type: 'button',
    disabled: false,
  },
)
</script>
