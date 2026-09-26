import { getEnv } from '@/utils/commons'
import axios from 'axios'
import { toast } from 'react-toastify'
import { ApiUrls } from './apiUrls'
import { routeUrls } from './routeUrls'

const axiosConfig = axios.create({
	baseURL: getEnv('VITE_BASE_API_URL', 'https://localhost:5000/api/v1'),
	withCredentials: true,
	headers: {
		'Content-Type': 'multipart/form-data',
		'Access-Control-Allow-Origin': '*',
		'Access-Control-Allow-Headers': 'X-Requested-With',
	},
	paramsSerializer: {
		serialize: (params) => {
			return Object.entries(params)
				.map(([key, value]) => {
					if (Array.isArray(value)) {
						return value.map((v) => `${encodeURIComponent(key)}=${encodeURIComponent(v)}`).join('&')
					}
					return `${encodeURIComponent(key)}=${encodeURIComponent(value)}`
				})
				.join('&')
		},
	},
})

axiosConfig.interceptors.request.use(
	(request) => {
		const token = localStorage.getItem('accessToken')
		if (token) {
			request.headers.Authorization = `Bearer ${JSON.parse(token)}`
		}
		return request
	},
	(error) => Promise.reject(error)
)

axiosConfig.interceptors.response.use(
	(response) => {
		const { message } = response.data
		if (message) {
			showMessageToast(message, 'success')
		}

		return response.data
	},
	async (error) => {
		const { status, response } = error || {}
		const originalRequest = error.config || {}

		const sessionExpiredMessage = 'Your session has expired. Please log in again'

		if (
			status === 401 &&
			!originalRequest._retry &&
			response?.data?.message === sessionExpiredMessage
		) {
			originalRequest._retry = true

			try {
				const resp = await axios.post(
					ApiUrls.AUTH.REFRESH_TOKEN,
					{},
					{ baseURL: axiosConfig.defaults.baseURL, withCredentials: true }
				)
				const accessToken = resp.data?.data?.accessToken
				if (!accessToken) {
					throw new Error('No access token in refresh response')
				}

				localStorage.setItem('accessToken', JSON.stringify(accessToken))
				originalRequest.headers = {
					...originalRequest.headers,
					Authorization: `Bearer ${accessToken}`,
				}

				return axiosConfig(originalRequest)
			} catch (ex) {
				localStorage.removeItem('accessToken')
				setTimeout(() => {
					window.location.href = routeUrls.BASE_ROUTE.AUTH(routeUrls.AUTH.LOGIN)
				}, 1500)

				return Promise.reject(ex)
			}
		}

		let errorMessages = response?.data?.error

		if (typeof errorMessages === 'string') {
			errorMessages = [errorMessages]
		} else if (errorMessages && typeof errorMessages === 'object' && !Array.isArray(errorMessages)) {
			errorMessages = Object.entries(errorMessages).map(([key, value]) => `[${key}] ${value}`)
		}

		switch (status) {
			case 400:
			case 401:
			case 403:
			case 404:
			case 409:
			case 422:
			case 429:
			case 503:
				errorMessages?.forEach((msg) => showMessageToast(msg))
				break
			case 500:
			default:
				showMessageToast(
					`Occurred a server error, please try again later: ${response?.data?.message || error.message}`
				)
				break
		}
		return Promise.reject(error)
	}
)

export default axiosConfig

const toastCache = new Map()
const TOAST_TTL = 5000

function showMessageToast(message, type = 'error') {
	const now = Date.now()
	const lastShownAt = toastCache.get(message)

	if (lastShownAt && now - lastShownAt < TOAST_TTL) return

	toastCache.set(message, now)

	if (type === 'success') {
		toast.success(message)
	} else {
		toast.error(message)
	}
}
