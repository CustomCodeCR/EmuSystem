<template>
  <DashboardLayout title="Usuarios y administración" eyebrow="Users">
    <LiquidCard class-name="panel reveal-up">
      <div class="panel-header">
        <div>
          <p class="eyebrow">Users</p>
          <h3>Usuarios del tenant</h3>
        </div>

        <AppButton compact @click="openCreate = true">Nuevo usuario</AppButton>
      </div>

      <p v-if="loading">Cargando usuarios...</p>
      <p v-else-if="error" class="form-error">{{ error }}</p>

      <div v-else class="card-list">
        <article v-for="user in users" :key="user.id" class="mini-card">
          <div class="user-line">
            <div class="avatar">{{ getInitials(user.fullName) }}</div>

            <div>
              <strong>{{ user.fullName }}</strong>
              <p>{{ user.email }}</p>
              <small v-if="user.lastLoginAt">
                Último login: {{ formatDate(user.lastLoginAt) }}
              </small>
            </div>
          </div>

          <StatusPill :variant="user.isActive ? 'active' : 'disabled'">
            {{ user.isActive ? 'Activo' : 'Inactivo' }}
          </StatusPill>
        </article>
      </div>
    </LiquidCard>

    <ModalShell v-if="openCreate" @close="openCreate = false">
      <form class="modal-form" @submit.prevent="create">
        <div class="panel-header">
          <div>
            <p class="eyebrow">Nuevo usuario</p>
            <h3>Crear usuario admin</h3>
          </div>

          <button type="button" class="icon-button" @click="openCreate = false">×</button>
        </div>

        <AppInput v-model="form.fullName" label="Nombre completo" placeholder="Maurice Lang" />

        <AppInput v-model="form.email" label="Email" placeholder="user@customcodecr.com" />

        <AppInput
          v-model="form.password"
          label="Password"
          type="password"
          placeholder="StrongPassword123!"
        />

        <AppButton type="submit">Crear usuario</AppButton>
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
import { createUser, listUsers, type User } from '@/features/users/user.service'

const context = useVaultContextStore()

const users = ref<User[]>([])
const loading = ref(false)
const error = ref('')
const openCreate = ref(false)

const form = reactive({
  fullName: '',
  email: '',
  password: '',
})

async function loadData() {
  if (!context.selectedTenantId) return

  loading.value = true
  error.value = ''

  try {
    users.value = await listUsers(context.selectedTenantId)
  } catch {
    error.value = 'No se pudieron cargar los usuarios.'
  } finally {
    loading.value = false
  }
}

async function create() {
  await createUser({
    tenantId: context.selectedTenantId,
    fullName: form.fullName,
    email: form.email,
    password: form.password,
  })

  form.fullName = ''
  form.email = ''
  form.password = ''
  openCreate.value = false

  await loadData()
}

function getInitials(value: string) {
  return value
    .split(' ')
    .filter(Boolean)
    .slice(0, 2)
    .map((part) => part[0]?.toUpperCase())
    .join('')
}

function formatDate(value: string) {
  return new Date(value).toLocaleString()
}

onMounted(loadData)
</script>
