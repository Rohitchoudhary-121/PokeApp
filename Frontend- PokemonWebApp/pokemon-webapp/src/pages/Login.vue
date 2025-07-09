<template>
  <div class="auth-panel">
    <h2>Login</h2>
    <form @submit.prevent="login">
      <input v-model="email" type="email" placeholder="Email" required />
      <input v-model="password" type="password" placeholder="Password" required />
      <button type="submit" :disabled="loading">
        <span v-if="loading" class="spinner"></span>
        Login
      </button>
      <div v-if="error" class="alert error">{{ error }}</div>
    </form>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import api from '../services/api'
import { setToken } from '../store/auth'
import { useRouter } from 'vue-router'

const email = ref('')
const password = ref('')
const error = ref('')
const loading = ref(false)
const router = useRouter()

async function login() {
  error.value = ''
  loading.value = true
  try {
    const res = await api.post('/api/Account/login', { email: email.value, password: password.value })
    if (res.data.result && res.data.result.accessToken) {
      setToken(res.data.result.accessToken)
      router.push('/')
    } else {
      error.value = res.data.message || 'Login failed'
    }
  } catch (err: any) {
    error.value = err.response?.data?.message || 'Login failed'
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
.auth-panel {
  background: var(--card-bg);
  max-width: 350px;
  margin: 3rem auto;
  padding: 2.5rem 2rem 2rem 2rem;
  border-radius: var(--radius);
  box-shadow: var(--shadow);
  display: flex;
  flex-direction: column;
  align-items: center;
}
.auth-panel h2 {
  margin-bottom: 1.5rem;
  color: var(--primary);
  font-weight: 600;
}
form {
  width: 100%;
  display: flex;
  flex-direction: column;
  gap: 1.1rem;
}
input {
  padding: 0.7rem 1rem;
  border: 1px solid #e5e7eb;
  border-radius: var(--radius);
  background: var(--secondary);
  transition: border var(--transition);
}
input:focus {
  border-color: var(--primary);
  outline: none;
}
button[type="submit"] {
  background: var(--primary);
  color: #fff;
  border: none;
  border-radius: var(--radius);
  padding: 0.7rem 0;
  font-weight: 600;
  font-size: 1rem;
  cursor: pointer;
  transition: background var(--transition), box-shadow var(--transition);
  box-shadow: 0 1px 4px rgba(37,99,235,0.08);
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
}
button[disabled] {
  opacity: 0.7;
  cursor: not-allowed;
}
.alert.error {
  background: var(--error);
  color: #fff;
  padding: 0.5rem 1rem;
  border-radius: var(--radius);
  margin-top: 0.5rem;
  font-size: 0.97rem;
  text-align: center;
}
.spinner {
  border: 2px solid #fff;
  border-top: 2px solid var(--accent);
  border-radius: 50%;
  width: 1em;
  height: 1em;
  animation: spin 0.7s linear infinite;
  display: inline-block;
}
@keyframes spin {
  to { transform: rotate(360deg); }
}
</style> 