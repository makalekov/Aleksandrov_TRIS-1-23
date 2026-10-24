<script setup>
import AppHeader from './components/AppHeader.vue'
</script>

<template>
  <AppHeader />
  <main class="app-main">
    <router-view />
  </main>
</template>

<style scoped>
.app-main {
  max-width: 900px;
  margin: 0 auto;
  padding: 0 2rem 2rem 2rem;
  font-family: system-ui, sans-serif;
}
</style>