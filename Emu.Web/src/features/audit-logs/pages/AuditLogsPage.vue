<template>
  <DashboardLayout title="Auditoría del sistema" eyebrow="Audit Logs">
    <LiquidCard class-name="panel reveal-up">
      <div class="panel-header">
        <div>
          <p class="eyebrow">Audit Logs</p>
          <h3>Trazabilidad</h3>
        </div>

        <div class="row-actions">
          <AppButton variant="ghost" compact @click="previousPage"> Anterior </AppButton>

          <AppButton variant="ghost" compact @click="nextPage"> Siguiente </AppButton>
        </div>
      </div>

      <p v-if="loading">Cargando auditoría...</p>
      <p v-else-if="error" class="form-error">{{ error }}</p>

      <div v-else class="timeline full">
        <div v-for="log in auditLogs" :key="log.id" class="timeline-item">
          <div class="timeline-dot" />

          <div>
            <strong>{{ log.action }}</strong>

            <p>
              {{ log.resourceType }}
              <span v-if="log.path"> · {{ log.path }}</span>
            </p>

            <small>
              {{ formatDate(log.createdAt) }}
              · {{ log.actorType }}
              <span v-if="log.ipAddress"> · {{ log.ipAddress }}</span>
            </small>
          </div>
        </div>
      </div>

      <p class="pagination-label">Página {{ page }}</p>
    </LiquidCard>
  </DashboardLayout>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue'
import DashboardLayout from '@/shared/layouts/DashboardLayout.vue'
import LiquidCard from '@/shared/components/LiquidCard.vue'
import AppButton from '@/shared/components/AppButton.vue'
import { useVaultContextStore } from '@/core/context/vaultContext.store'
import { listAuditLogs, type AuditLog } from '@/features/audit-logs/auditLog.service'

const context = useVaultContextStore()

const auditLogs = ref<AuditLog[]>([])
const loading = ref(false)
const error = ref('')
const page = ref(1)
const pageSize = ref(50)

async function loadData() {
  if (!context.selectedTenantId) {
    auditLogs.value = []
    return
  }

  loading.value = true
  error.value = ''

  try {
    auditLogs.value = await listAuditLogs(context.selectedTenantId, page.value, pageSize.value)
  } catch {
    error.value = 'No se pudieron cargar los audit logs.'
  } finally {
    loading.value = false
  }
}

async function nextPage() {
  page.value += 1
  await loadData()
}

async function previousPage() {
  if (page.value <= 1) return

  page.value -= 1
  await loadData()
}

function formatDate(value: string) {
  return new Date(value).toLocaleString()
}

onMounted(loadData)
</script>
