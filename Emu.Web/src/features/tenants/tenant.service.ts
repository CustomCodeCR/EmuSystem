import apiClient from '@/core/api/apiClient'

export interface Tenant {
  id: string
  name: string
  slug: string
  isActive: boolean
  createdAt: string
}

export interface CreateTenantRequest {
  name: string
  slug: string
}

export async function listTenants(): Promise<Tenant[]> {
  const { data } = await apiClient.get<Tenant[]>('/api/tenants')
  return data
}

export async function getTenant(id: string): Promise<Tenant> {
  const { data } = await apiClient.get<Tenant>(`/api/tenants/${id}`)
  return data
}

export async function createTenant(request: CreateTenantRequest): Promise<Tenant> {
  const { data } = await apiClient.post<Tenant>('/api/tenants', request)
  return data
}
