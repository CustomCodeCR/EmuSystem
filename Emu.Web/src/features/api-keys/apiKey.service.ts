import apiClient from '@/core/api/apiClient'

export interface ApiKey {
  id: string
  tenantId: string
  name: string
  keyPrefix: string
  isActive: boolean
  createdAt: string
  expiresAt?: string | null
  lastUsedAt?: string | null
}

export interface CreateApiKeyRequest {
  tenantId: string
  name: string
  description?: string | null
  expiresAt?: string | null
}

export interface CreateApiKeyResponse {
  id: string
  name: string
  keyPrefix: string
  apiKey: string
}

export interface DisableApiKeyResponse {
  id: string
  isActive: boolean
}

export async function listApiKeys(tenantId: string): Promise<ApiKey[]> {
  const { data } = await apiClient.get<ApiKey[]>('/api/api-keys', {
    params: { tenantId },
  })

  return data
}

export async function createApiKey(request: CreateApiKeyRequest): Promise<CreateApiKeyResponse> {
  const { data } = await apiClient.post<CreateApiKeyResponse>('/api/api-keys', request)

  return data
}

export async function disableApiKey(id: string): Promise<DisableApiKeyResponse> {
  const { data } = await apiClient.post<DisableApiKeyResponse>(`/api/api-keys/${id}/disable`, {})

  return data
}
