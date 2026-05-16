import apiClient from '@/core/api/apiClient'

export interface Secret {
  id: string
  environmentId: string
  name: string
  path: string
  currentVersionNumber: number
  status: string
  createdAt: string
  updatedAt?: string | null
}

export interface CreateSecretRequest {
  environmentId: string
  name: string
  path: string
  value: string
}

export interface CreateSecretResponse {
  id: string
  path: string
  versionNumber: number
}

export interface GetSecretResponse {
  id: string
  name: string
  path: string
  value: string
  versionNumber: number
}

export interface RotateSecretRequest {
  value: string
}

export interface RotateSecretResponse {
  id: string
  path: string
  versionNumber: number
}

export interface DeleteSecretResponse {
  id: string
  status: string
}

export async function listSecrets(environmentId: string): Promise<Secret[]> {
  const { data } = await apiClient.get<Secret[]>('/api/secrets', {
    params: { environmentId },
  })

  return data
}

export async function createSecret(request: CreateSecretRequest): Promise<CreateSecretResponse> {
  const { data } = await apiClient.post<CreateSecretResponse>('/api/secrets', request)

  return data
}

export async function getSecretByPath(
  environmentId: string,
  path: string,
): Promise<GetSecretResponse> {
  const { data } = await apiClient.get<GetSecretResponse>('/api/secrets/by-path', {
    params: { environmentId, path },
  })

  return data
}

export async function rotateSecret(
  id: string,
  request: RotateSecretRequest,
): Promise<RotateSecretResponse> {
  const { data } = await apiClient.post<RotateSecretResponse>(`/api/secrets/${id}/rotate`, request)

  return data
}

export async function deleteSecret(id: string): Promise<DeleteSecretResponse> {
  const { data } = await apiClient.delete<DeleteSecretResponse>(`/api/secrets/${id}`)

  return data
}
