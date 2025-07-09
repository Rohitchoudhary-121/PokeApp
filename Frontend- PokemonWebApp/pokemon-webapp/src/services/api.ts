import axios from 'axios'
import { getToken, logout } from '../store/auth'

const api = axios.create({
  baseURL: 'https://localhost:44306',
})

api.interceptors.request.use(config => {
  const token = getToken()
  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }
  return config
})

api.interceptors.response.use(
  response => response,
  error => {
    if (error.response && error.response.status === 401) {
      logout()
      window.location.href = '/login'
    }
    return Promise.reject(error)
  }
)

export default api 