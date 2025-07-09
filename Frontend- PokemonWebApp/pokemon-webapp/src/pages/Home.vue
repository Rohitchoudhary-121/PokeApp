<template>
  <div class="home-container">
    <div class="search-bar">
      <input v-model="searchQuery" @keyup.enter="handleSearch" placeholder="Search by name or ID" />
      <button @click="handleSearch" :disabled="loading">
        <span v-if="loading" class="spinner"></span>
        Search
      </button>
      <button v-if="searchQuery" @click="clearSearch" :disabled="loading">Clear</button>
    </div>
    <div v-if="error" class="alert error">{{ error }}</div>
    <div class="pokemon-list">
      <PokemonCard
        v-for="pokemon in pokemons"
        :key="pokemon.id || pokemon.name"
        :pokemon="pokemon"
      />
    </div>
    <button v-if="!searchQuery" @click="loadMore" :disabled="loading" class="load-more-btn">
      <span v-if="loading" class="spinner"></span>
      Load More
    </button>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import api from '../services/api'
import PokemonCard from '../components/PokemonCard.vue'

const pokemons = ref<any[]>([])
const error = ref('')
const loading = ref(false)
const limit = 10
let offset = 0
const searchQuery = ref('')

async function fetchPokemons(reset = false) {
  error.value = ''
  loading.value = true
  try {
    if (reset) {
      pokemons.value = []
      offset = 0
    }
    const res = await api.get(`/api/Pokemon/list-with-images?limit=${limit}&offset=${offset}`)
    pokemons.value.push(...res.data.result)
    offset += limit
  } catch (err: any) {
    error.value = err.response?.data?.message || 'Failed to load Pokémon'
  } finally {
    loading.value = false
  }
}

async function handleSearch() {
  if (!searchQuery.value) {
    clearSearch()
    return
  }
  error.value = ''
  loading.value = true
  try {
    const res = await api.get(`/api/Pokemon/${searchQuery.value}`)
    const p = res.data.result
    pokemons.value = [p]
  } catch (err: any) {
    pokemons.value = []
    error.value = err.response?.data?.message || 'Not found'
  } finally {
    loading.value = false
  }
}

function clearSearch() {
  searchQuery.value = ''
  pokemons.value = []
  fetchPokemons(true)
}

function loadMore() {
  fetchPokemons()
}

onMounted(() => fetchPokemons(true))
</script>

<style scoped>
.home-container {
  max-width: 900px;
  margin: 0 auto;
  padding: 1rem;
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
.pokemon-list {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(220px, 1fr));
  gap: 1.5rem;
  margin-bottom: 2rem;
}
.load-more-btn {
  display: block;
  margin: 0 auto;
  background: var(--accent);
  color: var(--text-main);
  border: none;
  border-radius: var(--radius);
  padding: 0.7rem 2.5rem;
  font-weight: 600;
  font-size: 1.1rem;
  cursor: pointer;
  transition: background var(--transition), box-shadow var(--transition);
  box-shadow: 0 1px 4px rgba(251,191,36,0.08);
}
.load-more-btn:hover {
  background: #f59e0b;
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
@media (max-width: 600px) {
  .home-container {
    padding: 0.5rem;
  }
  .pokemon-list {
    grid-template-columns: 1fr;
  }
}
</style> 