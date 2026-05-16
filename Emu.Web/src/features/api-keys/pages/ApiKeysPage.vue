<template>
  <DashboardLayout title="Llaves de integración" eyebrow="API Keys">
    <LiquidCard class-name="panel reveal-up">
      <div class="panel-header">
        <div>
          <p class="eyebrow">API Keys</p>
          <h3>Llaves de integración</h3>
        </div>

        <AppButton compact @click="create">Crear API Key</AppButton>
      </div>

      <p v-if="createdKey" class="generated-key">
        Guardá esta key ahora: <strong>{{ createdKey }}</strong>
      </p>

      <div class="card-list">
        <article v-for="key in apiKeys" :key="key.id" class="mini-card">
          <div>
            <strong>{{ key.name }}</strong>
            <p class="mono">{{ key.keyPrefix }}</p>
          </div>

          <div class="row-actions">
            <StatusPill :variant="key.isActive ? 'active' : 'disabled'">
              {{ key.isActive ? 'Activa' : 'Deshabilitada' }}
            </StatusPill>

            <button v-if="key.isActive" @click="disable(key.id)">Deshabilitar</button>
          </div>
        </article>
      </div>
    </LiquidCard>
  </DashboardLayout>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue'
import DashboardLayout from '@/shared/layouts/DashboardLayout.vue'
import LiquidCard from '@/shared/components/LiquidCard.vue'
import AppButton from '@/shared/components/AppButton.vue'
import StatusPill from '@/shared/components/StatusPill.vue'
import { useVaultContextStore } from '@/core/context/vaultContext.store'
import {
  createApiKey,
  disableApiKey,
  listApiKeys,
  type ApiKey,
} from '@/features/api-keys/apiKey.service'

const context = useVaultContextStore()

const apiKeys = ref<ApiKey[]>([])
const createdKey = ref('')

async function loadData() {
  if (!context.selectedTenantId) return

  apiKeys.value = await listApiKeys(context.selectedTenantId)
}

async function create() {
  const name = window.prompt('Nombre de la API Key:')

  if (!name) return

  const response = await createApiKey({
    tenantId: context.selectedTenantId,
    name,
    description: null,
    expiresAt: null,
  })

  createdKey.value = response.apiKey
  await loadData()
}

async function disable(id: string) {
  await disableApiKey(id)
  await loadData()
}

onMounted(loadData)
</script>
