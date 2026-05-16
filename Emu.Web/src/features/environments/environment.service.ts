import apiClient from '@/core/api/apiClient'

export interface ProjectEnvironment {
  id: string
  projectId: string
  name: string
  slug: string
  isActive: boolean
  createdAt: string
}

export interface CreateEnvironmentRequest {
  projectId: string
  name: string
  slug?: string | null
}

export async function listEnvironments(projectId: string): Promise<ProjectEnvironment[]> {
  const { data } = await apiClient.get<ProjectEnvironment[]>('/api/environments', {
    params: { projectId },
  })

  return data
}

export async function createEnvironment(
  request: CreateEnvironmentRequest,
): Promise<ProjectEnvironment> {
  const { data } = await apiClient.post<ProjectEnvironment>('/api/environments', request)

  return data
}
