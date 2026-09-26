import DOMPurify from 'dompurify'

export const sanitizeHtml = (html) => {
	if (!html) return ''

	return DOMPurify.sanitize(html, {
		ALLOWED_TAGS: [
			'p', 'br', 'b', 'i', 'u', 'strong', 'em', 's', 'strike',
			'h1', 'h2', 'h3', 'h4', 'h5', 'h6',
			'ul', 'ol', 'li',
			'a', 'img',
			'table', 'thead', 'tbody', 'tr', 'th', 'td',
			'blockquote', 'pre', 'code', 'span', 'div'
		],
		ALLOWED_ATTR: ['href', 'src', 'alt', 'title', 'target', 'rel', 'width', 'height', 'style', 'class'],
		ALLOW_DATA_ATTR: false,
		ADD_ATTR: ['target'],
	})
}
