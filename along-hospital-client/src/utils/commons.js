export const getEnv = (key, defaultValue = '') => {
	if (typeof window !== 'undefined' && window.__ENV__ && window.__ENV__[key]) {
		return window.__ENV__[key]
	}

	return import.meta.env[key] || defaultValue
}

export const getImageFromCloud = (imagePath) => {
	const cloudUrl = getEnv('VITE_IMAGE_CLOUD_URL')
	if (!cloudUrl || !imagePath) return '/placeholder-image.png'
	if (imagePath.startsWith('http://') || imagePath.startsWith('https://')) {
		return imagePath
	}
	return `${cloudUrl}/${imagePath}`
}

export const processHtmlImages = (html) => {
	if (!html) return ''

	const parser = new DOMParser()
	const doc = parser.parseFromString(html, 'text/html')
	const images = doc.querySelectorAll('img')

	images.forEach((img) => {
		const src = img.getAttribute('src')
		if (src) {
			img.setAttribute('src', getImageFromCloud(src))
		}
	})

	return doc.body.innerHTML
}

export const resolveDateTime = (date, time) => {
	if (!date || !time) return null
	const normalizedTime = String(time).length === 5 ? `${time}:00` : String(time)
	return `${date}T${normalizedTime}`
}

export const resolveEventPalette = (theme, statusColor) => {
	const paletteColor = theme.palette[statusColor] || theme.palette.primary
	return {
		backgroundColor: paletteColor.main,
		borderColor: paletteColor.dark || paletteColor.main,
		textColor: paletteColor.contrastText || '#ffffff',
	}
}
