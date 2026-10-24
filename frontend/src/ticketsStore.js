import { ref } from 'vue'

export const items = ref([
  {
    id: 1,
    number: 'КБ-101',
    title: 'Обрыв кабеля на участке Москва-Замоскворечье',
    siteId: 3,
    description: 'Повреждение на магистрали',
    status: 'New'
  },
  {
    id: 2,
    number: 'КБ-102',
    title: 'Повреждение магистрали Север-3 после шторма',
    siteId: 5,
    description: 'Восстановительные работы',
    status: 'InProgress'
  }
])

let nextId = 3

export function addItem({ title, siteId, description }) {
  items.value.push({
    id: nextId,
    number: `TMP-${nextId}`,
    title,
    siteId,
    description,
    status: 'New'
  })
  nextId++
}