<template>
  <nav class="navbar">
    <div class="navbar-content">
      <div class="navbar-brand">
        <span class="logo">Poké<span class="accent">App</span></span>
      </div>
      <div class="navbar-links">
        <router-link v-if="isAuth" to="/">Home</router-link>
        <router-link v-if="isAuth" to="/search">Search</router-link>
        <router-link v-if="!isAuth" to="/login">Login</router-link>
        <router-link v-if="!isAuth" to="/register">Register</router-link>
        <button v-if="isAuth" @click="handleLogout" class="logout-btn">Logout</button>
      </div>
    </div>
  </nav>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { isAuthenticated, logout } from '../store/auth'
import { useRouter } from 'vue-router'

const router = useRouter()
const isAuth = ref(isAuthenticated())

function updateAuth() {
  isAuth.value = isAuthenticated()
}
router.afterEach(updateAuth)
onMounted(updateAuth)

function handleLogout() {
  logout()
  updateAuth()
  router.push('/login')
}
</script>

<style scoped>
.navbar {
  background: var(--card-bg);
  box-shadow: var(--shadow);
  padding: 0.5rem 0;
  margin-bottom: 2rem;
  position: sticky;
  top: 0;
  z-index: 100;
}
.navbar-content {
  max-width: 900px;
  margin: 0 auto;
  display: flex;
  align-items: center;
  justify-content: space-between;
}
.navbar-brand .logo {
  font-weight: 700;
  font-size: 1.5rem;
  color: var(--primary);
  letter-spacing: 1px;
}
.navbar-brand .accent {
  color: var(--accent);
}
.navbar-links {
  display: flex;
  gap: 1.2rem;
  align-items: center;
}
.logout-btn {
  background: var(--primary);
  color: #fff;
  border: none;
  border-radius: var(--radius);
  padding: 0.4rem 1.1rem;
  font-weight: 500;
  cursor: pointer;
  transition: background var(--transition), box-shadow var(--transition);
  box-shadow: 0 1px 4px rgba(37,99,235,0.08);
}
.logout-btn:hover {
  background: var(--primary-hover);
}
</style> 