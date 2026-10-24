<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { createTicket } from '../api/tickets'

const router = useRouter()

const form = ref({
  title: '',
  siteId: null,
  description: ''
})

const error = ref('')
const isSubmitting = ref(false)

async function onCreate() {
  if (isSubmitting.value) return // Защита от двойного клика
  isSubmitting.value = true
  error.value = ''

  const title = form.value.title.trim()
  const siteId = form.value.siteId
  const description = form.value.description.trim()

  // Локальная валидация перед отправкой
  if (title.length < 5 || title.length > 80) {
    error.value = 'Тема: от 5 до 80 символов'
    isSubmitting.value = false
    return
  }
  if (!siteId || siteId <= 0) {
    error.value = 'Укажите корректный номер участка (больше нуля)'
    isSubmitting.value = false
    return
  }
  if (description.length < 10 || description.length > 500) {
    error.value = 'Описание: от 10 до 500 символов'
    isSubmitting.value = false
    return
  }

  try {
    await createTicket({
      title,
      siteId: Number(siteId),
      description
    })
    
    router.push('/')
  } catch (err) {
    if (err.message.includes('400')) {
      error.value = 'Проверьте поля'
    } else if (err.message.includes('500')) {
      error.value = 'Сервер не смог обработать запрос'
    } else {
      error.value = 'Нет связи с сервером'
    }
  } finally {
    isSubmitting.value = false
  }
}
</script>

<template>
  <section>
    <h2>Новая заявка</h2>
    
    <form @submit.prevent="onCreate">
      <div class="field">
        <label for="title">Тема (5–80 символов)</label>
        <input id="title" v-model="form.title" type="text" autofocus :disabled="isSubmitting" />
      </div>

      <div class="field">
        <label for="siteId">Номер участка</label>
        <input id="siteId" v-model.number="form.siteId" type="number" min="1" :disabled="isSubmitting" />
      </div>

      <div class="field">
        <label for="description">Описание (10–500 символов)</label>
        <textarea id="description" v-model="form.description" :disabled="isSubmitting"></textarea>
      </div>

      <p v-if="error" class="error">{{ error }}</p>

      <button type="submit" :disabled="isSubmitting">
        {{ isSubmitting ? 'Создаем...' : 'Создать' }}
      </button>
    </form>
  </section>
</template>

<style scoped>
form { display: flex; flex-direction: column; gap: 1rem; max-width: 400px; }
.field { display: flex; flex-direction: column; gap: 0.25rem; }
label { font-weight: 600; }
input, textarea { padding: 0.5rem; border: 1px solid #ccc; border-radius: 4px; }
button { padding: 0.5rem; cursor: pointer; }
button:disabled { background: #eee; cursor: not-allowed; }
.error { color: red; font-size: 0.9rem; margin: 0; }
</style>