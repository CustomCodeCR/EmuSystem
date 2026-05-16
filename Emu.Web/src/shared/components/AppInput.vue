<template>
  <label class="field">
    <span>{{ label }}</span>

    <input
      :value="modelValue"
      :type="type"
      :placeholder="placeholder"
      :required="required"
      @input="$emit('update:modelValue', ($event.target as HTMLInputElement).value)"
    />
  </label>
</template>

<script setup lang="ts">
withDefaults(
  defineProps<{
    modelValue: string
    label: string
    placeholder?: string
    type?: string
    required?: boolean
  }>(),
  {
    type: 'text',
    placeholder: '',
    required: false,
  },
)

defineEmits<{
  'update:modelValue': [value: string]
}>()
</script>
