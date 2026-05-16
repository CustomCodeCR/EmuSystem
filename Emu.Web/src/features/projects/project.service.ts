import apiClient from '@/core/api/apiClient'

export interface Project {
  id: string
  tenantId: string
  name: string
  slug: string
  isActive: boolean
  createdAt: string
}

export interface CreateProjectRequest {
  tenantId: string
  name: string
  slug?: string | null
}

export async function listProjects(tenantId: string): Promise<Project[]> {
  const { data } = await apiClient.get<Project[]>('/api/projects', {
    params: { tenantId },
  })

  return data
}

export async function createProject(request: CreateProjectRequest): Promise<Project> {
  const { data } = await apiClient.post<Project>('/api/projects', request)
  return data
}
