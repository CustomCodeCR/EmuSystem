import { defineStore } from 'pinia'
import apiClient from '@/core/api/apiClient'

export interface LoginRequest {
  tenantId: string
  email: string
  password: string
}

export interface LoginResponse {
  accessToken: string
  userId: string
  tenantId: string
  email: string
}

export const useAuthStore = defineStore('auth', {
  state: () => ({
    accessToken: localStorage.getItem('emu_access_token') || '',
    tenantId: localStorage.getItem('emu_tenant_id') || '',
    email: localStorage.getItem('emu_email') || '',
  }),

  getters: {
    isAuthenticated: (state) => Boolean(state.accessToken),
  },

  actions: {
    async login(payload: LoginRequest) {
      const { data } = await apiClient.post<LoginResponse>('/api/auth/login', payload)

      this.accessToken = data.accessToken
      this.tenantId = data.tenantId
      this.email = data.email

      localStorage.setItem('emu_access_token', data.accessToken)
      localStorage.setItem('emu_tenant_id', data.tenantId)
      localStorage.setItem('emu_email', data.email)
    },

    logout() {
      this.accessToken = ''
      this.tenantId = ''
      this.email = ''

      localStorage.removeItem('emu_access_token')
      localStorage.removeItem('emu_tenant_id')
      localStorage.removeItem('emu_email')
    },
  },
})
