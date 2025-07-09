<template>
  <div class="search-container">
    <div class="search-bar">
      <input v-model="query" @keyup.enter="searchPokemon" placeholder="Enter name or ID" />
      <button @click="searchPokemon" :disabled="loading">
        <span v-if="loading" class="spinner"></span>
        Search
      </button>
      <button v-if="query" @click="clearSearch" :disabled="loading">Clear</button>
    </div>
    <div v-if="error" class="alert error">{{ error }}</div>
    <div v-if="pokemon" class="result-list">
      <PokemonCard :pokemon="pokemon" />
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import api from '../services/api'
import PokemonCard from '../components/PokemonCard.vue'

const query = ref('')
const pokemon = ref<any>(null)
const error = ref('')
const loading = ref(false)

async function searchPokemon() {
  error.value = ''
  pokemon.value = null
  loading.value = true
  try {
    const res = await api.get(`/api/Pokemon/${query.value}`)
    pokemon.value = res.data.result
  } catch (err: any) {
    error.value = err.response?.data?.message || 'Not found'
  } finally {
    loading.value = false
  }
}

function clearSearch() {
  query.value = ''
  pokemon.value = null
  error.value = ''
}
</script>

<style scoped>
.search-container {
  max-width: 500px;
  margin: 3rem auto 0 auto;
  padding: 2rem 1rem 1rem 1rem;
  background: var(--card-bg);
  border-radius: var(--radius);
  box-shadow: var(--shadow);
}
.search-bar {
  display: flex;
  gap: 0.7rem;
  margin-bottom: 1.5rem;
}
.search-bar input {
  flex: 1;
  padding: 0.7rem 1rem;
  border: 1px solid #e5e7eb;
  border-radius: var(--radius);
  background: var(--secondary);
  transition: border var(--transition);
}
.search-bar input:focus {
  border-color: var(--primary);
  outline: none;
}
.search-bar button {
  background: var(--primary);
  color: #fff;
  border: none;
  border-radius: var(--radius);
  padding: 0.7rem 1.2rem;
  font-weight: 600;
  cursor: pointer;
  transition: background var(--transition), box-shadow var(--transition);
  box-shadow: 0 1px 4px rgba(37,99,235,0.08);
  display: flex;
  align-items: center;
  gap: 0.5rem;
}
.search-bar button:hover {
  background: var(--primary-hover);
}
.alert.error {
  background: var(--error);
  color: #fff;
  padding: 0.5rem 1rem;
  border-radius: var(--radius);
  margin-bottom: 1rem;
  font-size: 1rem;
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
.result-list {
  margin-top: 1.5rem;
  display: flex;
  justify-content: center;
}
</style> 