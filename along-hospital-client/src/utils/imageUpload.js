import { ApiUrls } from '@/configs/apiUrls'
import axiosConfig from '@/configs/axiosConfig'
import { toast } from 'react-toastify'

const MAX_IMAGE_SIZE_BYTES = 5 * 1024 * 1024

export const uploadImage = async (file, folder = 'Default') => {
	try {
		if (!file) {
			throw new Error('No file provided')
		}

		if (!file.type || !file.type.startsWith('image/')) {
			throw new Error('Invalid file type. Only image files are allowed')
		}

		if (typeof file.size === 'number' && file.size > MAX_IMAGE_SIZE_BYTES) {
			throw new Error('File size exceeds the maximum allowed limit')
		}

		const formData = new FormData()
		formData.append('file', file)

		const response = await axiosConfig.post(ApiUrls.UPLOAD.SINGLE_IMAGE(folder), formData, {
			headers: { 'Content-Type': 'multipart/form-data' },
		})

		const fileName = response.data.fileName
		if (!fileName) {
			throw new Error('No filename returned from server')
		}

		return fileName
	} catch (error) {
		toast.error(error.message)
		return null
	}
}
