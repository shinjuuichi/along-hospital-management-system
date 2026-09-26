import { useSessionStorage } from '@/hooks/useStorage'
import { useCallback, useEffect, useState } from 'react'

export default function useResendTimer(storageKey) {
	const [expiry, setExpiry] = useSessionStorage(storageKey, null)
	const [timer, setTimer] = useState(0)

	useEffect(() => {
		if (!expiry) {
			setTimer(0)
			return
		}

		const updateTimer = () => {
			const remaining = Math.max(0, Math.ceil((expiry - Date.now()) / 1000))
			setTimer(remaining)

			if (remaining === 0) {
				setExpiry(null)
			}
		}

		updateTimer()
		const interval = setInterval(updateTimer, 1000)
		return () => clearInterval(interval)
	}, [expiry, setExpiry])

	const start = useCallback(
		(durationMs) => {
			setExpiry(Date.now() + durationMs)
		},
		[setExpiry]
	)

	return {
		timer,
		start,
		clear: () => setExpiry(null),
	}
}
