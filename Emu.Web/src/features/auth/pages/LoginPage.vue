<template>
  <AuthLayout>
    <LiquidCard class-name="login-card reveal-up">
      <div class="brand-mark">
        <div class="brand-icon">E</div>
        <div>
          <p class="eyebrow">CustomCodeCR</p>
          <h1>Emu VaultSecret</h1>
        </div>
      </div>

      <p class="login-copy">Centraliza, cifra y audita secretos por tenant, proyecto y ambiente.</p>

      <form class="login-form" @submit.prevent="login">
        <AppInput v-model="form.tenantId" label="Tenant ID" placeholder="ce6f1ee5-..." />

        <AppInput v-model="form.email" label="Email" placeholder="admin@customcodecr.com" />

        <AppInput v-model="form.password" label="Password" type="password" placeholder="••••••••" />

        <p v-if="error" class="form-error">{{ error }}</p>

        <AppButton type="submit">
          {{ loading ? 'Ingresando...' : 'Ingresar' }}
          <span>→</span>
        </AppButton>
      </form>
    </LiquidCard>
  </AuthLayout>
</template>

<script setup lang="ts">
import { reactive, ref } from 'vue'
import { useRouter } from 'vue-router'
import AuthLayout from '@/shared/layouts/AuthLayout.vue'
import LiquidCard from '@/shared/components/LiquidCard.vue'
import AppInput from '@/shared/components/AppInput.vue'
import AppButton from '@/shared/components/AppButton.vue'
import { useAuthStore } from '@/core/auth/auth.store'

const router = useRouter()
const auth = useAuthStore()

const loading = ref(false)
const error = ref('')

const form = reactive({
  tenantId: 'ce6f1ee5-5186-4674-8496-d5f6b497def6',
  email: 'admin@customcodecr.com',
  password: 'Admin123!',
})

async function login() {
  loading.value = true
  error.value = ''

  try {
    await auth.login(form)
    await router.push('/')
  } catch {
    error.value = 'Credenciales inválidas o API no disponible.'
  } finally {
    loading.value = false
  }
}
</script>
