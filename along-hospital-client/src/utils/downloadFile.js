import axios from 'axios'
import { getEnv } from './commons'

const XLSX_MIME_TYPE = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'

const deriveFilename = (url, extension = '.xlsx') => {
	const today = new Date().toISOString().split('T')[0].replace(/-/g, '')
	const match = url.match(/\/report\/([^/]+)\/export/)
	if (match) {
		const roleSlug = match[1]

		const roleMap = {
			'manager-dashboard': 'manager-statistics',
			'accountant-dashboard': 'accountant-statistics',
			'hr-dashboard': 'hr-statistics',
			'inventory-clerk-dashboard': 'inventory-clerk-statistics',
			'pharmacist-dashboard': 'pharmacist-statistics',
		}
		const name = roleMap[roleSlug] || `${roleSlug}-statistics`
		return `${name}-${today}${extension}`
	}
	return `export-${today}${extension}`
}

const getExtensionFromContentType = (contentType = '') => {
	if (contentType.includes('text/csv')) {
		return '.csv'
	}

	if (contentType.includes('spreadsheetml') || contentType.includes('sheet')) {
		return '.xlsx'
	}

	return '.xlsx'
}

const getFilenameFromContentDisposition = (contentDisposition = '') => {
	const utf8Match = contentDisposition.match(/filename\*=UTF-8''([^;]+)/i)
	if (utf8Match?.[1]) {
		return decodeURIComponent(utf8Match[1])
	}

	const plainMatch = contentDisposition.match(/filename="?([^"]+)"?/i)
	if (plainMatch?.[1]) {
		return plainMatch[1]
	}

	return null
}

export const downloadFile = async (url, params = {}) => {
	const token = localStorage.getItem('accessToken')
	const baseURL = getEnv('VITE_BASE_API_URL', 'https://localhost:5000/api/v1')
	const response = await axios.get(url, {
		baseURL,
		params,
		paramsSerializer: (p) =>
			Object.entries(p)
				.map(([key, value]) => `${encodeURIComponent(key)}=${encodeURIComponent(value)}`)
				.join('&'),
		headers: {
			Authorization: token ? `Bearer ${JSON.parse(token)}` : undefined,
		},
		responseType: 'blob',
	})

	const contentType = response.headers['content-type'] || XLSX_MIME_TYPE
	const contentDisposition = response.headers['content-disposition'] || ''
	const extension = getExtensionFromContentType(contentType)
	const filename = getFilenameFromContentDisposition(contentDisposition) || deriveFilename(url, extension)

	const blob = new Blob([response.data], { type: contentType })
	const downloadUrl = URL.createObjectURL(blob)
	const link = document.createElement('a')
	link.href = downloadUrl
	link.download = filename
	document.body.appendChild(link)
	link.click()
	document.body.removeChild(link)
	URL.revokeObjectURL(downloadUrl)
}

export const downloadCsv = downloadFile
