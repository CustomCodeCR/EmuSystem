<template>
  <DashboardLayout title="Administración de secretos" eyebrow="Secrets">
    <LiquidCard class-name="panel reveal-up">
      <div class="panel-header">
        <div>
          <p class="eyebrow">Secrets</p>
          <h3>Secretos del ambiente</h3>
        </div>

        <AppButton compact @click="openModal = true">Nuevo secreto</AppButton>
      </div>

      <div class="toolbar">
        <input v-model="search" placeholder="Buscar por path, nombre o estado..." />
        <AppButton variant="ghost" compact @click="loadData">Recargar</AppButton>
      </div>

      <p v-if="loading">Cargando secretos...</p>
      <p v-else-if="error" class="form-error">{{ error }}</p>

      <div v-else class="data-table">
        <div class="table-row table-head">
          <span>Nombre</span>
          <span>Path</span>
          <span>Versión</span>
          <span>Estado</span>
          <span>Acciones</span>
        </div>

        <div v-for="secret in filteredSecrets" :key="secret.id" class="table-row">
          <span>{{ secret.name }}</span>
          <span class="mono">{{ secret.path }}</span>
          <span>v{{ secret.currentVersionNumber }}</span>
          <span
            ><StatusPill>{{ secret.status }}</StatusPill></span
          >

          <span class="row-actions">
            <button @click="showSecret(secret.path)">Ver</button>
            <button @click="rotate(secret.id)">Rotar</button>
            <button class="danger" @click="remove(secret.id)">Eliminar</button>
          </span>
        </div>
      </div>
    </LiquidCard>

    <SecretFormModal v-if="openModal" @close="openModal = false" @create="create" />

    <ModalShell v-if="revealedSecret" @close="revealedSecret = ''">
      <div class="panel-header">
        <div>
          <p class="eyebrow">Secret value</p>
          <h3>Valor descifrado</h3>
        </div>
        <button class="icon-button" @click="revealedSecret = ''">×</button>
      </div>

      <pre class="secret-value">{{ revealedSecret }}</pre>
    </ModalShell>
  </DashboardLayout>
</template>

<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import DashboardLayout from '@/shared/layouts/DashboardLayout.vue'
import LiquidCard from '@/shared/components/LiquidCard.vue'
import AppButton from '@/shared/components/AppButton.vue'
import StatusPill from '@/shared/components/StatusPill.vue'
import ModalShell from '@/shared/components/ModalShell.vue'
import SecretFormModal from '@/features/secrets/components/SecretFormModal.vue'
import { useVaultContextStore } from '@/core/context/vaultContext.store'
import {
  createSecret,
  deleteSecret,
  getSecretByPath,
  listSecrets,
  rotateSecret,
  type Secret,
} from '@/features/secrets/secret.service'

const context = useVaultContextStore()

const secrets = ref<Secret[]>([])
const loading = ref(false)
const error = ref('')
const search = ref('')
const openModal = ref(false)
const revealedSecret = ref('')

const filteredSecrets = computed(() => {
  const query = search.value.trim().toLowerCase()

  if (!query) return secrets.value

  return secrets.value.filter((secret) =>
    [secret.name, secret.path, secret.status].some((value) => value.toLowerCase().includes(query)),
  )
})

async function loadData() {
  if (!context.selectedEnvironmentId) return

  loading.value = true
  error.value = ''

  try {
    secrets.value = await listSecrets(context.selectedEnvironmentId)
  } catch {
    error.value = 'No se pudieron cargar los secretos.'
  } finally {
    loading.value = false
  }
}

async function create(payload: { name: string; path: string; value: string }) {
  await createSecret({
    environmentId: context.selectedEnvironmentId,
    name: payload.name,
    path: payload.path,
    value: payload.value,
  })

  openModal.value = false
  await loadData()
}

async function showSecret(path: string) {
  const response = await getSecretByPath(context.selectedEnvironmentId, path)
  revealedSecret.value = response.value
}

async function rotate(id: string) {
  const value = window.prompt('Nuevo valor del secreto:')

  if (!value) return

  await rotateSecret(id, { value })
  await loadData()
}

async function remove(id: string) {
  if (!window.confirm('¿Eliminar este secreto?')) return

  await deleteSecret(id)
  await loadData()
}

onMounted(loadData)
</script>
