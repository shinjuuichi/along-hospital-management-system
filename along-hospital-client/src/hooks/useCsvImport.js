import useTranslation from '@/hooks/useTranslation'
import { useCallback, useRef, useState } from 'react'

export function useCsvImport({ delimiter = ',', hasHeader = true, onError, onSuccess } = {}) {
	const [loading, setLoading] = useState(false)
	const [error, setError] = useState(null)
	const [data, setData] = useState(null)
	const [file, setFile] = useState(null)
	const fileInputRef = useRef(null)
	const { t } = useTranslation()

	const parseCsv = useCallback(
		(text) => {
			const lines = text.split(/\r?\n/).filter((line) => line.trim() !== '')
			if (lines.length === 0) {
				throw new Error(t('error.csv.empty'))
			}

			let headers = []
			let startIndex = 0

			if (hasHeader) {
				headers = lines[0].split(delimiter).map((h) => h.trim().replace(/^["']|["']$/g, ''))
				startIndex = 1
			} else {
				const firstLine = lines[0].split(delimiter)
				headers = firstLine.map((_, index) => `Column${index + 1}`)
				startIndex = 0
			}

			const result = []
			for (let i = startIndex; i < lines.length; i++) {
				const line = lines[i]
				const values = []
				let currentValue = ''
				let insideQuotes = false

				for (let j = 0; j < line.length; j++) {
					const char = line[j]
					const nextChar = line[j + 1]

					if (char === '"' || char === "'") {
						if (insideQuotes && nextChar === char) {
							currentValue += char
							j++
						} else {
							insideQuotes = !insideQuotes
						}
					} else if (char === delimiter && !insideQuotes) {
						values.push(currentValue.trim())
						currentValue = ''
					} else {
						currentValue += char
					}
				}
				values.push(currentValue.trim())
				const row = {}
				headers.forEach((header, index) => {
					row[header] = values[index] || ''
				})
				result.push(row)
			}

			return result
		},
		[delimiter, hasHeader]
	)

	const handleFileRead = useCallback(
		async (file) => {
			if (!file) return

			const fileName = file.name.toLowerCase()
			if (!fileName.endsWith('.csv')) {
				const err = new Error(t('error.csv.invalid_format'))
				setError(err)
				onError?.(err, file)
				return
			}

			setLoading(true)
			setError(null)
			setFile(file)

			try {
				const text = await new Promise((resolve, reject) => {
					const reader = new FileReader()
					reader.onload = (e) => resolve(e.target.result)
					reader.onerror = () => reject(new Error(t('error.csv.read_error')))
					reader.readAsText(file, 'UTF-8')
				})

				const parsedData = parseCsv(text)
				setData(parsedData)
				onSuccess?.(parsedData, file)
			} catch (err) {
				const error = err instanceof Error ? err : new Error(t('error.csv.parse_error'))
				setError(error)
				onError?.(error, file)
			} finally {
				setLoading(false)
			}
		},
		[parseCsv, onError, onSuccess]
	)

	const selectFile = useCallback(() => {
		if (fileInputRef.current) {
			fileInputRef.current.click()
		}
	}, [])

	const handleFileChange = useCallback(
		(e) => {
			const file = e.target.files?.[0]
			if (file) {
				handleFileRead(file)
			}
			if (e.target) {
				e.target.value = ''
			}
		},
		[handleFileRead]
	)

	const reset = useCallback(() => {
		setData(null)
		setError(null)
		setLoading(false)
		setFile(null)
	}, [])

	return {
		loading,
		error,
		data,
		file,
		selectFile,
		reset,
		fileInputRef,
		handleFileChange,
	}
}
