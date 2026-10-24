<script setup>
import { ref } from 'vue'
import { items } from '../ticketsStore'

const newTicket = ref({
  title: '',
  siteId: null,
  description: ''
})

function handleCreate() {
  const nextId = items.value.length + 1
  items.value.push({
    id: nextId,
    number: `КБ-${100 + nextId}`,
    title: newTicket.value.title,
    siteId: newTicket.value.siteId,
    status: 'New'
  })
  newTicket.value = { title: '', siteId: null, description: '' }
  alert('Заявка добавлена в общий список')
}
</script>

<template>
  <section>
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