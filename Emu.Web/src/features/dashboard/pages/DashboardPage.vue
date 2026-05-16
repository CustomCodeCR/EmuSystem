<template>
  <DashboardLayout title="Centro de control" eyebrow="Dashboard">
    <section class="metric-grid">
      <MetricCard label="Secrets activos" :value="secretCount" hint="ambiente actual" />
      <MetricCard label="API Keys" :value="apiKeyCount" hint="tenant actual" />
      <MetricCard label="Usuarios" :value="userCount" hint="tenant actual" />
      <MetricCard label="Eventos" :value="auditCount" hint="últimas consultas" />
    </section>

    <LiquidCard class-name="panel reveal-up">
      <div class="panel-header">
        <div>
          <p class="eyebrow">Actividad</p>
          <h3>Últimos eventos auditados</h3>
        </div>
      </div>

      <div class="timeline">
        <div v-for="log in auditLogs" :key="log.id" class="timeline-item">
          <div class="timeline-dot" />
          <div>
            <strong>{{ log.action }}</strong>
            <p>{{ log.path }}</p>
            <small>{{ log.createdAt }}</small>
          </div>
        </div>
      </div>
    </LiquidCard>
  </DashboardLayout>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue'
import DashboardLayout from '@/shared/layouts/DashboardLayout.vue'
import LiquidCard from '@/shared/components/LiquidCard.vue'
import MetricCard from '@/shared/components/MetricCard.vue'
import { useVaultContextStore } from '@/core/context/vaultContext.store'
import { listSecrets } from '@/features/secrets/secret.service'
import { listApiKeys } from '@/features/api-keys/apiKey.service'
import { listUsers } from '@/features/users/user.service'
import { listAuditLogs, type AuditLog } from '@/features/audit-logs/auditLog.service'

const context = useVaultContextStore()

const secretCount = ref(0)
const apiKeyCount = ref(0)
const userCount = ref(0)
const auditCount = ref(0)
const auditLogs = ref<AuditLog[]>([])

async function loadData() {
  if (context.selectedEnvironmentId) {
    const secrets = await listSecrets(context.selectedEnvironmentId)
    secretCount.value = secrets.length
  }

  if (context.selectedTenantId) {
    const apiKeys = await listApiKeys(context.selectedTenantId)
    const users = await listUsers(context.selectedTenantId)
    const logs = await listAuditLogs(context.selectedTenantId, 1, 5)

    apiKeyCount.value = apiKeys.length
    userCount.value = users.length
    auditCount.value = logs.length
    auditLogs.value = logs
  }
}

onMounted(loadData)
</script>
