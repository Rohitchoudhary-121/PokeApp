<template>
  <div class="pokemon-card">
    <img :src="pokemon.spriteUrl" :alt="pokemon.name" />
    <h3>{{ pokemon.name }}</h3>
    <div class="info">
      <span v-if="pokemon.height">Height: <b>{{ pokemon.height }}</b></span>
      <span v-if="pokemon.weight">Weight: <b>{{ pokemon.weight }}</b></span>
      <span v-if="pokemon.abilities">Abilities: <b>{{ pokemon.abilities.join(', ') }}</b></span>
      <span v-if="pokemon.types">Types: <b>{{ pokemon.types.join(', ') }}</b></span>
    </div>
  </div>
</template>

<script setup lang="ts">
defineProps<{ pokemon: any }>()
</script>

<style scoped>
.pokemon-card {
  background: var(--card-bg);
  border-radius: var(--radius);
  box-shadow: var(--shadow);
  padding: 1.5rem 1rem 1rem 1rem;
  text-align: center;
  transition: transform var(--transition), box-shadow var(--transition);
  cursor: pointer;
  min-height: 270px;
  display: flex;
  flex-direction: column;
  align-items: center;
}
.pokemon-card:hover {
  transform: translateY(-4px) scale(1.03);
  box-shadow: 0 4px 16px rgba(37,99,235,0.13);
}
.pokemon-card img {
  width: 90px;
  height: 90px;
  object-fit: contain;
  margin-bottom: 0.7rem;
  background: var(--secondary);
  border-radius: 50%;
  box-shadow: 0 1px 4px rgba(37,99,235,0.08);
}
.pokemon-card h3 {
  margin: 0.5rem 0 0.7rem 0;
  color: var(--primary);
  font-size: 1.2rem;
  font-weight: 700;
  letter-spacing: 1px;
}
.info {
  display: flex;
  flex-direction: column;
  gap: 0.3rem;
  color: var(--text-secondary);
  font-size: 0.98rem;
}
.info b {
  color: var(--text-main);
  font-weight: 600;
}
</style> 