<template>
  <ModalShell @close="$emit('close')">
    <form class="modal-form" @submit.prevent="submit">
      <div class="panel-header">
        <div>
          <p class="eyebrow">Nuevo secreto</p>
          <h3>Crear secreto cifrado</h3>
        </div>

        <button type="button" class="icon-button" @click="$emit('close')">×</button>
      </div>

      <AppInput v-model="form.name" label="Nombre" placeholder="postgres-password" required />

      <AppInput
        v-model="form.path"
        label="Path"
        placeholder="database/postgres/password"
        required
      />

      <AppInput
        v-model="form.value"
        label="Valor"
        type="password"
        placeholder="••••••••"
        required
      />

      <AppButton type="submit"> Guardar secreto </AppButton>
    </form>
  </ModalShell>
</template>

<script setup lang="ts">
import { reactive } from 'vue'
import ModalShell from '@/shared/components/ModalShell.vue'
import AppInput from '@/shared/components/AppInput.vue'
import AppButton from '@/shared/components/AppButton.vue'

const emit = defineEmits<{
  close: []
  create: [payload: { name: string; path: string; value: string }]
}>()

const form = reactive({
  name: '',
  path: '',
  value: '',
})

function submit() {
  emit('create', {
    name: form.name,
    path: form.path,
    value: form.value,
  })

  form.name = ''
  form.path = ''
  form.value = ''
}
</script>
