import apiClient from '@/core/api/apiClient'

export interface AuditLog {
  id: string
  tenantId: string
  actorType: string
  actorId?: string | null
  action: string
  resourceType: string
  resourceId?: string | null
  path?: string | null
  ipAddress?: string | null
  userAgent?: string | null
  createdAt: string
}

export async function listAuditLogs(
  tenantId: string,
  page = 1,
  pageSize = 50,
): Promise<AuditLog[]> {
  const { data } = await apiClient.get<AuditLog[]>('/api/audit-logs', {
    params: { tenantId, page, pageSize },
  })

  return data
}
