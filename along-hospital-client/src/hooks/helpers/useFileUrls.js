import { useCallback, useEffect, useRef } from 'react'

export default function useFileUrls() {
	const fileUrlMapRef = useRef(new Map())

	useEffect(() => {
		return () => {
			for (const u of fileUrlMapRef.current.values()) URL.revokeObjectURL(u)
			fileUrlMapRef.current.clear()
		}
	}, [])

	const getUrlForFile = useCallback((file) => {
		const map = fileUrlMapRef.current
		if (map.has(file)) return map.get(file)
		const u = URL.createObjectURL(file)
		map.set(file, u)
		return u
	}, [])

	const revokeUrlForFile = useCallback((file) => {
		const map = fileUrlMapRef.current
		const u = map.get(file)
		if (u) {
			URL.revokeObjectURL(u)
			map.delete(file)
		}
	}, [])
	return { getUrlForFile, revokeUrlForFile }
}
