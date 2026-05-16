<template>
  <DashboardLayout title="Tenants" eyebrow="Administración">
    <LiquidCard class-name="panel reveal-up">
      <div class="panel-header">
        <div>
          <p class="eyebrow">Tenants</p>
          <h3>Clientes / organizaciones</h3>
        </div>

        <AppButton compact @click="openCreate = true">Nuevo tenant</AppButton>
      </div>

      <div class="card-list">
        <article v-for="tenant in tenants" :key="tenant.id" class="mini-card">
          <div>
            <strong>{{ tenant.name }}</strong>
            <p class="mono">{{ tenant.slug }}</p>
          </div>

          <StatusPill :variant="tenant.isActive ? 'active' : 'disabled'">
            {{ tenant.isActive ? 'Activo' : 'Inactivo' }}
          </StatusPill>
        </article>
      </div>
    </LiquidCard>

    <ModalShell v-if="openCreate" @close="openCreate = false">
      <form class="modal-form" @submit.prevent="create">
        <div class="panel-header">
          <div>
            <p class="eyebrow">Nuevo tenant</p>
            <h3>Crear organización</h3>
          </div>

          <button type="button" class="icon-button" @click="openCreate = false">×</button>
        </div>

        <AppInput v-model="form.name" label="Nombre" placeholder="CustomCodeCR" />
        <AppInput v-model="form.slug" label="Slug" placeholder="customcodecr" />

        <AppButton type="submit">Crear tenant</AppButton>
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
import { createTenant, listTenants, type Tenant } from '@/features/tenants/tenant.service'

const tenants = ref<Tenant[]>([])
const openCreate = ref(false)

const form = reactive({
  name: '',
  slug: '',
})

async function loadData() {
  tenants.value = await listTenants()
}

async function create() {
  await createTenant({
    name: form.name,
    slug: form.slug,
  })

  form.name = ''
  form.slug = ''
  openCreate.value = false

  await loadData()
}

onMounted(loadData)
</script>
