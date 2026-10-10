<script setup>
import { ref } from 'vue'

// Текущий экран: 'list' | 'new' | 'login'
const screen = ref('list')

// Тестовые данные
const items = [
  {
    id: 1,
    number: 'КБ-101',
    title: 'Обрыв кабеля на участке Москва-Замоскворечье',
    siteId: 3,
    status: 'New'
  },
  {
    id: 2,
    number: 'КБ-102',
    title: 'Повреждение магистрали Север-3 после шторма',
    siteId: 5,
    status: 'InProgress'
  }
]

// Поля формы создания
const newTicket = ref({
  title: '',
  siteId: null,
  description: ''
})

function handleCreate() {
  // Пока просто очищаем форму и возвращаемся к списку
  alert('Заявка создана: ' + newTicket.value.title)
  newTicket.value = { title: '', siteId: null, description: '' }
  screen.value = 'list'
}

function handleLogin() {
  alert('Вход')
  screen.value = 'list'
}
</script>

<template>
  <main>
    <h1>Учёт повреждений кабеля · ТТК</h1>

    <!-- Переключатель экранов -->
    <nav>
      <button type="button" @click="screen = 'list'">Список</button>
      <button type="button" @click="screen = 'new'">Создать</button>
      <button type="button" @click="screen = 'login'">Вход</button>
    </nav>

    <!-- ЭКРАН 1: Список -->
    <section v-if="screen === 'list'">
      <h2>Заявки на повреждение кабеля</h2>
      <table>
        <thead>
          <tr>
            <th>Номер</th>
            <th>Тема</th>
            <th>Участок (siteId)</th>
            <th>Статус</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="item in items" :key="item.id">
            <td>{{ item.number }}</td>
            <td>{{ item.title }}</td>
            <td>{{ item.siteId }}</td>
            <td>{{ item.status }}</td>
          </tr>
        </tbody>
      </table>
    </section>

    <!-- ЭКРАН 2: Создание -->
    <section v-else-if="screen === 'new'">
      <h2>Новая заявка</h2>
      <form @submit.prevent="handleCreate">
        <label for="title">Тема (5–80 символов)</label>
        <input
          id="title"
          v-model="newTicket.title"
          type="text"
          required
          minlength="5"
          maxlength="80"
          autofocus
        />

        <label for="siteId">Номер участка (siteId)</label>
        <input
          id="siteId"
          v-model.number="newTicket.siteId"
          type="number"
          required
          min="1"
        />

        <label for="description">Описание (10–500 символов)</label>
        <textarea
          id="description"
          v-model="newTicket.description"
          required
          minlength="10"
          maxlength="500"
        ></textarea>

        <button type="submit">Создать</button>
      </form>
    </section>

    <!-- ЭКРАН 3: Вход -->
    <section v-else-if="screen === 'login'">
      <h2>Вход в систему</h2>
      <form @submit.prevent="handleLogin">
        <label for="login">Логин</label>
        <input id="login" type="text" required />

        <label for="password">Пароль</label>
        <input id="password" type="password" required />

        <button type="submit">Войти</button>
      </form>
    </section>
  </main>
</template>

<style scoped>
main {
  max-width: 900px;
  margin: 2rem auto;
  font-family: system-ui, sans-serif;
}

nav {
  display: flex;
  gap: 0.5rem;
  margin-bottom: 1rem;
}

nav button {
  padding: 0.5rem 1rem;
  cursor: pointer;
}

table {
  width: 100%;
  border-collapse: collapse;
}

th, td {
  border: 1px solid #ccc;
  padding: 0.5rem;
  text-align: left;
}

form {
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
  max-width: 400px;
}

label {
  font-weight: 600;
}

input, textarea, button {
  padding: 0.5rem;
  font-size: 1rem;
}

button[type="submit"] {
  margin-top: 0.5rem;
  cursor: pointer;
}
</style>