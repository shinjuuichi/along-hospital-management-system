import { ApiUrls } from '@/configs/apiUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import { useLocalStorage } from '@/hooks/useStorage'
import { Box, Container, useTheme } from '@mui/material'
import { alpha } from '@mui/material/styles'
import { useEffect, useMemo, useState } from 'react'
import QueueCreatedDialog from '../commons/QueueCreatedDialog'
import CreateQueueNormalSection from './sections/CreateQueueNormalSection'
import CreateQueueWaitingSection from './sections/CreateQueueWaitingSection'

const QUEUE_COOLDOWN_MS = 60 * 1000
const QUEUE_COOLDOWN_SECONDS = QUEUE_COOLDOWN_MS / 1000
const QUEUE_COOLDOWN_STORAGE_KEY = 'queue_guest_create_cooldown_until'

const getRemainingSeconds = (cooldownUntil) => {
	if (!cooldownUntil) return 0

	const parsedCooldownUntil = Number(cooldownUntil)
	if (Number.isNaN(parsedCooldownUntil)) return 0

	const diffMs = parsedCooldownUntil - Date.now()
	if (diffMs <= 0) return 0

	return Math.ceil(diffMs / 1000)
}

const CreateQueuePage = () => {
	const theme = useTheme()
	const [cooldownUntil, setCooldownUntil, removeCooldownUntil] = useLocalStorage(
		QUEUE_COOLDOWN_STORAGE_KEY,
		null
	)
	const [remainingSeconds, setRemainingSeconds] = useState(() => getRemainingSeconds(cooldownUntil))
	const [createdQueueNumber, setCreatedQueueNumber] = useState(null)

	const createQueueAPI = useAxiosSubmit({
		url: ApiUrls.QUEUE.INDEX,
		method: 'POST',
		onSuccess: (response) => {
			const queue = response?.data
			setCreatedQueueNumber(queue?.queueNumber ?? null)
		},
	})

	useEffect(() => {
		const syncCountdown = () => {
			const nextRemaining = getRemainingSeconds(cooldownUntil)
			setRemainingSeconds(nextRemaining)

			if (nextRemaining === 0 && cooldownUntil) {
				removeCooldownUntil()
			}
		}

		syncCountdown()
		if (!cooldownUntil) return

		const intervalId = window.setInterval(syncCountdown, 1000)

		return () => {
			window.clearInterval(intervalId)
		}
	}, [cooldownUntil, removeCooldownUntil])

	const isWaiting = remainingSeconds > 0

	const countdownProgress = useMemo(() => {
		const progress = ((QUEUE_COOLDOWN_SECONDS - remainingSeconds) / QUEUE_COOLDOWN_SECONDS) * 100
		return Math.min(100, Math.max(0, progress))
	}, [remainingSeconds])

	const handleCreateQueue = async () => {
		setCooldownUntil(Date.now() + QUEUE_COOLDOWN_MS)
		await createQueueAPI.submit()
	}

	const handleCloseQueueDialog = () => {
		setCreatedQueueNumber(null)
	}

	return (
		<Box
			sx={{
				height: '100dvh',
				minHeight: '100vh',
				overflow: 'hidden',
				py: { xs: 1.5, md: 2 },
				px: { xs: 1.5, md: 3 },
				display: 'flex',
				background: `linear-gradient(180deg, ${alpha(theme.palette.primary.light, 0.08)} 0%, ${theme.palette.background.default} 44%, ${theme.palette.background.paper} 100%)`,
			}}
		>
			<Container
				maxWidth='lg'
				sx={{
					height: '100%',
					display: 'flex',
					alignItems: 'stretch',
				}}
			>
				{isWaiting ? (
					<CreateQueueWaitingSection
						countdownProgress={countdownProgress}
						remainingSeconds={remainingSeconds}
					/>
				) : (
					<CreateQueueNormalSection onCreateQueue={handleCreateQueue} />
				)}
			</Container>

			<QueueCreatedDialog
				open={createdQueueNumber != null}
				queueNumber={createdQueueNumber}
				onClose={handleCloseQueueDialog}
			/>
		</Box>
	)
}

export default CreateQueuePage
