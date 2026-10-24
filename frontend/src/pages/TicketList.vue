<script setup>
import { ref, onMounted } from 'vue'
import { getTickets } from '../api/tickets'
import TicketTable from '../components/TicketTable.vue'

//loading, empty, data, error
const screen = ref('loading')
const items = ref([])
const errorMessage = ref('')

async function loadTickets() {
  screen.value = 'loading'
  try {
    const data = await getTickets()
    items.value = data
    if (data.length === 0) {
      screen.value = 'empty'
    } else {
      screen.value = 'data'
    }
  } catch (error) {
    screen.value = 'error'
    if (error.message.includes('500')) {
      errorMessage.value = 'Сервер не смог обработать запрос'
    } else {
      errorMessage.value = 'Нет связи с сервером'
    }
  }
}

onMounted(() => {
  loadTickets()
})
</script>

<template>
  <section>
    <h2>Заявки на повреждение кабеля</h2>

    <div v-if="screen === 'loading'" class="status-msg">Загрузка...</div>

    <div v-else-if="screen === 'error'" class="status-msg error">{{ errorMessage }}</div>

    <div v-else-if="screen === 'empty'" class="status-msg">Записей пока нет</div>

    <div v-else-if="screen === 'data'">
      <TicketTable :items="items" />
    </div>
  </section>
</template>

<style scoped>
.status-msg {
  padding: 1rem;
  text-align: center;
  border: 1px dashed #ccc;
  border-radius: 4px;
}
.error {
  color: red;
  border-color: red;
}
</style>