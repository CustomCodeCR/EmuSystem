<template>
  <label v-if="label" class="field">
    <span>{{ label }}</span>

    <select
      :value="modelValue"
      :required="required"
      @change="$emit('update:modelValue', ($event.target as HTMLSelectElement).value)"
    >
      <option v-if="placeholder" value="">
        {{ placeholder }}
      </option>

      <option v-for="option in options" :key="option.id" :value="option.id">
        {{ option.name }}
      </option>
    </select>
  </label>

  <select
    v-else
    :value="modelValue"
    :required="required"
    @change="$emit('update:modelValue', ($event.target as HTMLSelectElement).value)"
  >
    <option v-if="placeholder" value="">
      {{ placeholder }}
    </option>

    <option v-for="option in options" :key="option.id" :value="option.id">
      {{ option.name }}
    </option>
  </select>
</template>

<script setup lang="ts">
withDefaults(
  defineProps<{
    modelValue: string
    options: Array<{ id: string; name: string }>
    label?: string
    placeholder?: string
    required?: boolean
  }>(),
  {
    label: '',
    placeholder: '',
    required: false,
  },
)

defineEmits<{
  'update:modelValue': [value: string]
}>()
</script>
