<template>
  <DashboardLayout title="Environments" eyebrow="Ambientes">
    <LiquidCard class-name="panel reveal-up">
      <div class="panel-header">
        <div>
          <p class="eyebrow">Environments</p>
          <h3>Ambientes del proyecto</h3>
        </div>

        <AppButton compact @click="openCreate = true">Nuevo ambiente</AppButton>
      </div>

      <div class="card-list">
        <article v-for="environment in environments" :key="environment.id" class="mini-card">
          <div>
            <strong>{{ environment.name }}</strong>
            <p class="mono">{{ environment.slug }}</p>
          </div>

          <StatusPill :variant="environment.isActive ? 'active' : 'disabled'">
            {{ environment.isActive ? 'Activo' : 'Inactivo' }}
          </StatusPill>
        </article>
      </div>
    </LiquidCard>

    <ModalShell v-if="openCreate" @close="openCreate = false">
      <form class="modal-form" @submit.prevent="create">
        <div class="panel-header">
          <div>
            <p class="eyebrow">Nuevo ambiente</p>
            <h3>Crear environment</h3>
          </div>

          <button type="button" class="icon-button" @click="openCreate = false">×</button>
        </div>

        <AppInput v-model="form.name" label="Nombre" placeholder="Production" />
        <AppInput v-model="form.slug" label="Slug" placeholder="prod" />

        <AppButton type="submit">Crear ambiente</AppButton>
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
import StatusPill from '@/shared/components/StatusPill.vue'
import { useVaultContextStore } from '@/core/context/vaultContext.store'
import {
  createEnvironment,
  listEnvironments,
  type ProjectEnvironment,
} from '@/features/environments/environment.service'

const context = useVaultContextStore()

const environments = ref<ProjectEnvironment[]>([])
const openCreate = ref(false)

const form = reactive({
  name: '',
  slug: '',
})

async function loadData() {
  if (!context.selectedProjectId) return

  environments.value = await listEnvironments(context.selectedProjectId)
}

async function create() {
  await createEnvironment({
    projectId: context.selectedProjectId,
    name: form.name,
    slug: form.slug || null,
  })

  form.name = ''
  form.slug = ''
  openCreate.value = false

  await loadData()
}

onMounted(loadData)
</script>
