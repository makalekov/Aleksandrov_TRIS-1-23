import { mockGetTickets, mockCreateTicket } from './mockTickets'

const USE_MOCK = true 

const baseUrl = import.meta.env.VITE_API_BASE_URL

export async function getTickets() {
  if (USE_MOCK) {
    const response = await mockGetTickets()
    if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`)
    return response.data
  }

  // Реальный запрос
  const response = await fetch(`${baseUrl}/api/cable-tickets`)
  if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`)
  return await response.json()
}

export async function createTicket(body) {
  if (USE_MOCK) {
    const response = await mockCreateTicket(body)
    if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`)
    return response.data
  }

  const response = await fetch(`${baseUrl}/api/cable-tickets`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(body)
  })
  if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`)
  return await response.json()
}