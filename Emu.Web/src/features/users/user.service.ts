import apiClient from '@/core/api/apiClient'

export interface User {
  id: string
  tenantId: string
  email: string
  fullName: string
  isActive: boolean
  createdAt: string
  lastLoginAt?: string | null
}

export interface CreateUserRequest {
  tenantId: string
  email: string
  fullName: string
  password: string
}

export interface CreateUserResponse {
  id: string
  tenantId: string
  email: string
  fullName: string
}

export async function listUsers(tenantId: string): Promise<User[]> {
  const { data } = await apiClient.get<User[]>('/api/users', {
    params: { tenantId },
  })

  return data
}

export async function createUser(request: CreateUserRequest): Promise<CreateUserResponse> {
  const { data } = await apiClient.post<CreateUserResponse>('/api/users', request)

  return data
}
