import { defineStore } from 'pinia'
import { listTenants, type Tenant } from '@/features/tenants/tenant.service'
import { listProjects, type Project } from '@/features/projects/project.service'
import {
  listEnvironments,
  type ProjectEnvironment,
} from '@/features/environments/environment.service'

interface VaultContextState {
  tenants: Tenant[]
  projects: Project[]
  environments: ProjectEnvironment[]

  selectedTenantId: string
  selectedProjectId: string
  selectedEnvironmentId: string

  initialized: boolean
}

export const useVaultContextStore = defineStore('vault-context', {
  state: (): VaultContextState => ({
    tenants: [],
    projects: [],
    environments: [],

    selectedTenantId: '',
    selectedProjectId: '',
    selectedEnvironmentId: '',

    initialized: false,
  }),

  actions: {
    async initialize() {
      if (this.initialized) return

      await this.loadTenants()

      this.initialized = true
    },

    async loadTenants() {
      const tenants = await listTenants()

      this.tenants = tenants

      const firstTenant = tenants[0]

      if (firstTenant) {
        this.selectedTenantId = firstTenant.id

        await this.loadProjects(firstTenant.id)
      }
    },

    async loadProjects(tenantId: string) {
      const projects = await listProjects(tenantId)

      this.projects = projects

      const firstProject = projects[0]

      if (firstProject) {
        this.selectedProjectId = firstProject.id

        await this.loadEnvironments(firstProject.id)
      } else {
        this.selectedProjectId = ''
        this.environments = []
        this.selectedEnvironmentId = ''
      }
    },

    async loadEnvironments(projectId: string) {
      const environments = await listEnvironments(projectId)

      this.environments = environments

      const firstEnvironment = environments[0]

      if (firstEnvironment) {
        this.selectedEnvironmentId = firstEnvironment.id
      } else {
        this.selectedEnvironmentId = ''
      }
    },

    async setTenant(tenantId: string) {
      this.selectedTenantId = tenantId

      this.selectedProjectId = ''
      this.selectedEnvironmentId = ''

      await this.loadProjects(tenantId)
    },

    async setProject(projectId: string) {
      this.selectedProjectId = projectId

      this.selectedEnvironmentId = ''

      await this.loadEnvironments(projectId)
    },

    setEnvironment(environmentId: string) {
      this.selectedEnvironmentId = environmentId
    },
  },
})
