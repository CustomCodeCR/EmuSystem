import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '@/core/auth/auth.store'
import LoginPage from '@/features/auth/pages/LoginPage.vue'
import DashboardPage from '@/features/dashboard/pages/DashboardPage.vue'
import TenantsPage from '@/features/tenants/pages/TenantsPage.vue'
import ProjectsPage from '@/features/projects/pages/ProjectsPage.vue'
import EnvironmentsPage from '@/features/environments/pages/EnvironmentsPage.vue'
import SecretsPage from '@/features/secrets/pages/SecretsPage.vue'
import ApiKeysPage from '@/features/api-keys/pages/ApiKeysPage.vue'
import PoliciesPage from '@/features/policies/pages/PoliciesPage.vue'
import UsersPage from '@/features/users/pages/UsersPage.vue'
import AuditLogsPage from '@/features/audit-logs/pages/AuditLogsPage.vue'

const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: '/login',
      name: 'login',
      component: LoginPage,
    },
    {
      path: '/',
      name: 'dashboard',
      component: DashboardPage,
      meta: { requiresAuth: true },
    },
    {
      path: '/tenants',
      name: 'tenants',
      component: TenantsPage,
      meta: { requiresAuth: true },
    },
    {
      path: '/projects',
      name: 'projects',
      component: ProjectsPage,
      meta: { requiresAuth: true },
    },
    {
      path: '/environments',
      name: 'environments',
      component: EnvironmentsPage,
      meta: { requiresAuth: true },
    },
    {
      path: '/secrets',
      name: 'secrets',
      component: SecretsPage,
      meta: { requiresAuth: true },
    },
    {
      path: '/api-keys',
      name: 'apiKeys',
      component: ApiKeysPage,
      meta: { requiresAuth: true },
    },
    {
      path: '/policies',
      name: 'policies',
      component: PoliciesPage,
      meta: { requiresAuth: true },
    },
    {
      path: '/users',
      name: 'users',
      component: UsersPage,
      meta: { requiresAuth: true },
    },
    {
      path: '/audit-logs',
      name: 'auditLogs',
      component: AuditLogsPage,
      meta: { requiresAuth: true },
    },
  ],
})

router.beforeEach((to) => {
  const auth = useAuthStore()

  if (to.meta.requiresAuth && !auth.isAuthenticated) {
    return '/login'
  }

  if (to.path === '/login' && auth.isAuthenticated) {
    return '/'
  }
})

export default router
