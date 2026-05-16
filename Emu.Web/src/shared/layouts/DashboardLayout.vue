<template>
  <div class="app-shell">
    <div class="orb orb-one" />
    <div class="orb orb-two" />
    <div class="orb orb-three" />

    <main class="workspace">
      <aside class="sidebar liquid-card">
        <div class="sidebar-header">
          <div class="brand-icon small">E</div>

          <div>
            <strong>Emu Vault</strong>
            <span>Secret Platform</span>
          </div>
        </div>

        <nav class="nav-list">
          <RouterLink v-for="item in navigation" :key="item.path" :to="item.path" class="nav-item">
            <span class="nav-icon">{{ item.icon }}</span>
            {{ item.label }}
          </RouterLink>
        </nav>

        <div class="sidebar-footer">
          <button class="logout-button" @click="logout">Cerrar sesión</button>

          <div class="user-chip">
            <div class="avatar">{{ initials }}</div>

            <div>
              <strong>{{ auth.email || 'Admin' }}</strong>
              <span>Admin</span>
            </div>
          </div>
        </div>
      </aside>

      <section class="content-area">
        <header class="topbar liquid-card">
          <div>
            <p class="eyebrow">{{ eyebrow }}</p>
            <h2>{{ title }}</h2>
          </div>

          <div class="context-selectors">
            <select :value="context.selectedTenantId" @change="handleTenantChange">
              <option value="">Tenant</option>

              <option v-for="tenant in context.tenants" :key="tenant.id" :value="tenant.id">
                {{ tenant.name }}
              </option>
            </select>

            <select :value="context.selectedProjectId" @change="handleProjectChange">
              <option value="">Project</option>

              <option v-for="project in context.projects" :key="project.id" :value="project.id">
                {{ project.name }}
              </option>
            </select>

            <select :value="context.selectedEnvironmentId" @change="handleEnvironmentChange">
              <option value="">Environment</option>

              <option
                v-for="environment in context.environments"
                :key="environment.id"
                :value="environment.id"
              >
                {{ environment.name }}
              </option>
            </select>
          </div>
        </header>

        <slot />
      </section>
    </main>
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted } from 'vue'
import { RouterLink, useRouter } from 'vue-router'

import { useAuthStore } from '@/core/auth/auth.store'
import { useVaultContextStore } from '@/core/context/vaultContext.store'

withDefaults(
  defineProps<{
    title: string
    eyebrow?: string
  }>(),
  {
    eyebrow: 'CustomCodeCR',
  },
)

const router = useRouter()

const auth = useAuthStore()
const context = useVaultContextStore()

const navigation = [
  { path: '/', label: 'Dashboard', icon: '✦' },
  { path: '/tenants', label: 'Tenants', icon: '◆' },
  { path: '/projects', label: 'Projects', icon: '▤' },
  { path: '/environments', label: 'Environments', icon: '▥' },
  { path: '/secrets', label: 'Secrets', icon: '●' },
  { path: '/api-keys', label: 'API Keys', icon: '◇' },
  { path: '/policies', label: 'Policies', icon: '▣' },
  { path: '/users', label: 'Users', icon: '◉' },
  { path: '/audit-logs', label: 'Audit Logs', icon: '◌' },
]

const initials = computed(() => {
  const email = auth.email || 'Admin'

  return email.slice(0, 2).toUpperCase()
})

async function handleTenantChange(event: Event) {
  const tenantId = (event.target as HTMLSelectElement).value

  if (!tenantId) {
    return
  }

  await context.setTenant(tenantId)
}

async function handleProjectChange(event: Event) {
  const projectId = (event.target as HTMLSelectElement).value

  if (!projectId) {
    return
  }

  await context.setProject(projectId)
}

function handleEnvironmentChange(event: Event) {
  const environmentId = (event.target as HTMLSelectElement).value

  context.setEnvironment(environmentId)
}

async function logout() {
  auth.logout()

  await router.push('/login')
}

onMounted(async () => {
  await context.initialize()
})
</script>
