<template>
  <DashboardLayout title="Projects" eyebrow="Proyectos">
    <LiquidCard class-name="panel reveal-up">
      <div class="panel-header">
        <div>
          <p class="eyebrow">Projects</p>
          <h3>Proyectos del tenant</h3>
        </div>

        <AppButton compact @click="openCreate = true">Nuevo proyecto</AppButton>
      </div>

      <div class="card-list">
        <article v-for="project in projects" :key="project.id" class="mini-card">
          <div>
            <strong>{{ project.name }}</strong>
            <p class="mono">{{ project.slug }}</p>
          </div>

          <StatusPill :variant="project.isActive ? 'active' : 'disabled'">
            {{ project.isActive ? 'Activo' : 'Inactivo' }}
          </StatusPill>
        </article>
      </div>
    </LiquidCard>

    <ModalShell v-if="openCreate" @close="openCreate = false">
      <form class="modal-form" @submit.prevent="create">
        <div class="panel-header">
          <div>
            <p class="eyebrow">Nuevo proyecto</p>
            <h3>Crear proyecto</h3>
          </div>

          <button type="button" class="icon-button" @click="openCreate = false">×</button>
        </div>

        <AppInput v-model="form.name" label="Nombre" placeholder="Dhole" />
        <AppInput v-model="form.slug" label="Slug" placeholder="dhole" />

        <AppButton type="submit">Crear proyecto</AppButton>
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
import { createProject, listProjects, type Project } from '@/features/projects/project.service'

const context = useVaultContextStore()

const projects = ref<Project[]>([])
const openCreate = ref(false)

const form = reactive({
  name: '',
  slug: '',
})

async function loadData() {
  if (!context.selectedTenantId) return

  projects.value = await listProjects(context.selectedTenantId)
}

async function create() {
  await createProject({
    tenantId: context.selectedTenantId,
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
