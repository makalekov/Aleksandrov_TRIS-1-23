const delay = (ms) => new Promise((resolve) => setTimeout(resolve, ms))

let mockDb = [
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

let nextId = 3

// Флаг для принудительной ошибки 500
let force500 = false
export function setForce500(value) {
  force500 = value
}

export async function mockGetTickets() {
  await delay(500) // Имитация задержки сети
  if (force500) return { ok: false, status: 500, data: null }
  return { ok: true, status: 200, data: [...mockDb] }
}

export async function mockCreateTicket(body) {
  await delay(500)
  
  if (force500) return { ok: false, status: 500, data: null }

  // Валидация
  if (!body.title || body.title.length < 5) {
    return { ok: false, status: 400, data: { error: 'Слишком короткая тема' } }
  }

  const newTicket = {
    id: nextId,
    number: `КБ-${100 + nextId}`,
    title: body.title,
    siteId: body.siteId,
    status: 'New'
  }
  
  mockDb.push(newTicket)
  nextId++

  return { ok: true, status: 201, data: newTicket }
}