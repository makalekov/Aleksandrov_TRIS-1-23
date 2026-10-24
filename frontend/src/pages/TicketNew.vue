<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { addItem } from '../ticketsStore'

const router = useRouter()

const form = ref({
  title: '',
  siteId: null,
  description: ''
})

const error = ref('')

function onCreate() {
  const title = form.value.title.trim()
  const siteId = form.value.siteId
  const description = form.value.description.trim()

  // Валидация
  if (title.length < 5 || title.length > 80) {
    error.value = 'Тема: от 5 до 80 символов'
    return
  }
  if (!siteId || siteId <= 0) {
    error.value = 'Укажите корректный номер участка (больше нуля)'
    return
  }
  if (description.length < 10 || description.length > 500) {
    error.value = 'Описание: от 10 до 500 символов'
    return
  }

  error.value = ''
  addItem({
    title,
    siteId: Number(siteId),
    description
  })

  form.value = { title: '', siteId: null, description: '' }
  router.push('/')
}
</script>

<template>
  <section>
    <h2>Новая заявка</h2>
    
    <form @submit.prevent="onCreate">
      <div class="field">
        <label for="title">Тема (5–80 символов)</label>
        <input
          id="title"
          v-model="form.title"
          type="text"
          autofocus
        />
      </div>

      <div class="field">
        <label for="siteId">Номер участка (число)</label>
        <input
          id="siteId"
          v-model.number="form.siteId"
          type="number"
          min="1"
        />
      </div>

      <div class="field">
        <label for="description">Описание (10–500 символов)</label>
        <textarea
          id="description"
          v-model="form.description"
        ></textarea>
      </div>

      <p v-if="error" class="error">{{ error }}</p>

      <button type="submit">Создать</button>
    </form>
  </section>
</template>

<style scoped>
form {
  display: flex;
  flex-direction: column;
  gap: 1rem;
  max-width: 400px;
}

.field {
  display: flex;
  flex-direction: column;
  gap: 0.25rem;
}

label {
  font-weight: 600;
}

input, textarea {
  padding: 0.5rem;
  border: 1px solid #ccc;
  border-radius: 4px;
}

button {
  padding: 0.5rem;
  cursor: pointer;
}

.error {
  color: red;
  font-size: 0.9rem;
  margin: 0;
}
</style>