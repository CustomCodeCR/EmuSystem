import apiClient from '@/core/api/apiClient'

export interface AccessPolicy {
  id: string
  apiKeyId: string
  tenantId: string
  projectId?: string | null
  environmentId?: string | null
  pathPrefix: string
  canRead: boolean
  canWrite: boolean
  canDelete: boolean
  createdAt: string
}

export interface CreateAccessPolicyRequest {
  apiKeyId: string
  tenantId: string
  projectId?: string | null
  environmentId?: string | null
  pathPrefix: string
  canRead: boolean
  canWrite: boolean
  canDelete: boolean
}

export interface CreateAccessPolicyResponse {
  id: string
  apiKeyId: string
  pathPrefix: string
}

export async function listPoliciesByApiKey(apiKeyId: string): Promise<AccessPolicy[]> {
  const { data } = await apiClient.get<AccessPolicy[]>(`/api/policies/by-api-key/${apiKeyId}`)

  return data
}

export async function createAccessPolicy(
  request: CreateAccessPolicyRequest,
): Promise<CreateAccessPolicyResponse> {
  const { data } = await apiClient.post<CreateAccessPolicyResponse>('/api/policies', request)

  return data
}
