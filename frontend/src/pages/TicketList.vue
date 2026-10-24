<script setup>
import { items } from '../ticketsStore'
import TicketTable from '../components/TicketTable.vue'
</script>

<template>
  <section>
    <h2>Заявки на повреждение кабеля</h2>
    <TicketTable :items="items" />
  </section>
</template>