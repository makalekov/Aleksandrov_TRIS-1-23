<script setup>
function handleLogin() {
  alert('Вход (пока без сервера)')
}
</script>

<template>
  <section>
    <h2>Вход в систему</h2>
    <form @submit.prevent="handleLogin">
      <label for="login">Логин</label>
      <input id="login" type="text" required />

      <label for="password">Пароль</label>
      <input id="password" type="password" required />

      <button type="submit">Войти</button>
    </form>
  </section>
</template>

<style scoped>
form {
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
  max-width: 400px;
}

label {
  font-weight: 600;
}
</style>