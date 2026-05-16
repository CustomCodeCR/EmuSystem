<template>
  <DashboardLayout title="Control de acceso" eyebrow="Policies">
    <LiquidCard class-name="panel reveal-up">
      <div class="panel-header">
        <div>
          <p class="eyebrow">Policies</p>
          <h3>Permisos por API Key</h3>
        </div>

        <AppButton compact @click="openCreate = true">Nueva policy</AppButton>
      </div>

      <div class="toolbar">
        <select v-model="selectedApiKeyId" @change="loadData">
          <option value="">Seleccione una API Key</option>

          <option v-for="key in apiKeys" :key="key.id" :value="key.id">
            {{ key.name }} - {{ key.keyPrefix }}
          </option>
        </select>

        <AppButton variant="ghost" compact @click="loadData"> Recargar </AppButton>
      </div>

      <p v-if="loading">Cargando policies...</p>

      <p v-else-if="error" class="form-error">
        {{ error }}
      </p>

      <div v-else class="data-table">
        <div class="table-row table-head four">
          <span>Path prefix</span>
          <span>Read</span>
          <span>Write</span>
          <span>Delete</span>
        </div>

        <div v-for="policy in policies" :key="policy.id" class="table-row four">
          <span class="mono">
            {{ policy.pathPrefix }}
          </span>

          <span>
            {{ policy.canRead ? 'Sí' : 'No' }}
          </span>

          <span>
            {{ policy.canWrite ? 'Sí' : 'No' }}
          </span>

          <span>
            {{ policy.canDelete ? 'Sí' : 'No' }}
          </span>
        </div>
      </div>
    </LiquidCard>

    <ModalShell v-if="openCreate" @close="openCreate = false">
      <form class="modal-form" @submit.prevent="create">
        <div class="panel-header">
          <div>
            <p class="eyebrow">Nueva policy</p>
            <h3>Crear permiso</h3>
          </div>

          <button type="button" class="icon-button" @click="openCreate = false">×</button>
        </div>

        <label class="field">
          <span>API Key</span>

          <select v-model="form.apiKeyId" required>
            <option value="">Seleccione una API Key</option>

            <option v-for="key in apiKeys" :key="key.id" :value="key.id">
              {{ key.name }} - {{ key.keyPrefix }}
            </option>
          </select>
        </label>

        <AppInput v-model="form.pathPrefix" label="Path prefix" placeholder="database/" />

        <div class="check-grid">
          <label>
            <input v-model="form.canRead" type="checkbox" />

            Read
          </label>

          <label>
            <input v-model="form.canWrite" type="checkbox" />

            Write
          </label>

          <label>
            <input v-model="form.canDelete" type="checkbox" />

            Delete
          </label>
        </div>

        <AppButton type="submit"> Guardar policy </AppButton>
      </form>
    </ModalShell>
  </DashboardLayout>
</template>

<script setup lang="ts">
import { onMounted, reactive, ref } from 'vue'

import DashboardLayout from '@/shared/layouts/DashboardLayout.vue'
import LiquidCard from '@/shared/components/LiquidCard.vue'
import AppButton from '@/shared/components/AppButton.vue'
import AppInput from '@/shared/components/AppInput.vue'
import ModalShell from '@/shared/components/ModalShell.vue'

import { useVaultContextStore } from '@/core/context/vaultContext.store'

import {
  createAccessPolicy,
  listPoliciesByApiKey,
  type AccessPolicy,
} from '@/features/policies/policy.service'

import { listApiKeys, type ApiKey } from '@/features/api-keys/apiKey.service'

const context = useVaultContextStore()

const loading = ref(false)
const error = ref('')
const openCreate = ref(false)

const apiKeys = ref<ApiKey[]>([])
const policies = ref<AccessPolicy[]>([])

const selectedApiKeyId = ref('')

const form = reactive({
  apiKeyId: '',
  pathPrefix: '',
  canRead: true,
  canWrite: false,
  canDelete: false,
})

async function loadApiKeys() {
  if (!context.selectedTenantId) return

  apiKeys.value = await listApiKeys(context.selectedTenantId)

  const firstApiKey = apiKeys.value[0]

  if (!selectedApiKeyId.value && firstApiKey) {
    selectedApiKeyId.value = firstApiKey.id
    form.apiKeyId = firstApiKey.id
  }
}

async function loadData() {
  if (!selectedApiKeyId.value) {
    policies.value = []

    return
  }

  loading.value = true
  error.value = ''

  try {
    policies.value = await listPoliciesByApiKey(selectedApiKeyId.value)
  } catch {
    error.value = 'No se pudieron cargar las policies.'
  } finally {
    loading.value = false
  }
}

async function create() {
  await createAccessPolicy({
    apiKeyId: form.apiKeyId,
    tenantId: context.selectedTenantId,
    projectId: context.selectedProjectId || null,
    environmentId: context.selectedEnvironmentId || null,
    pathPrefix: form.pathPrefix,
    canRead: form.canRead,
    canWrite: form.canWrite,
    canDelete: form.canDelete,
  })

  selectedApiKeyId.value = form.apiKeyId

  openCreate.value = false

  form.pathPrefix = ''
  form.canRead = true
  form.canWrite = false
  form.canDelete = false

  await loadData()
}

onMounted(async () => {
  await loadApiKeys()
  await loadData()
})
</script>
