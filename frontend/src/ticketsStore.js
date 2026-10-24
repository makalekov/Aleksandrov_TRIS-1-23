import { ref } from 'vue'

export const items = ref([
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
])