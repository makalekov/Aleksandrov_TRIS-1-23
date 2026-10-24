<script setup>
</script>

<template>
  <header class="app-header">
    <h1>Учёт повреждений кабеля · ТТК</h1>
    <nav>
      <router-link to="/">Список</router-link>
      <router-link to="/new">Создать</router-link>
      <router-link to="/login">Вход</router-link>
    </nav>
  </header>
</template>

<style scoped>
.app-header {
  border-bottom: 1px solid #ccc;
  padding: 1rem 2rem;
  margin-bottom: 1rem;
}

.app-header h1 {
  margin: 0 0 0.5rem 0;
  font-size: 1.25rem;
}

.app-header nav {
  display: flex;
  gap: 1rem;
}

.app-header a {
  text-decoration: none;
  color: #0066cc;
}

.app-header a.router-link-active {
  font-weight: bold;
  color: #000;
}
</style>