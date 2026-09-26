import { ApiUrls } from '@/configs/apiUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import { Box, Container, Stack, useTheme } from '@mui/material'
import { alpha } from '@mui/material/styles'
import { useCallback, useMemo, useRef, useState } from 'react'
import { toast } from 'react-toastify'
import QueueCreatedDialog from '../commons/QueueCreatedDialog'
import QueueRoomSelectionDrawer from '../commons/QueueRoomSelectionDrawer'
import CreateQueueFromQRHeaderSection from './sections/CreateQueueFromQRHeaderSection'
import CreateQueueFromQRInstructionSection from './sections/CreateQueueFromQRInstructionSection'
import CreateQueueFromQRScannerSection from './sections/CreateQueueFromQRScannerSection'

const SCAN_LOCK_MS = 5000

const CreateQueueFromQRPage = () => {
	const { t } = useTranslation()
	const theme = useTheme()
	const [selectedRoomId, setSelectedRoomId] = useState(null)
	const [createdQueueNumber, setCreatedQueueNumber] = useState(null)
	const scanLockUntilRef = useRef(0)

	const getAllQueueSnapshots = useFetch(ApiUrls.QUEUE.GET_ALL)

	const createQueueFromQRAPI = useAxiosSubmit({
		url: ApiUrls.QUEUE.QR,
		method: 'POST',
		onSuccess: (response) => {
			const queue = response?.data
			setCreatedQueueNumber(queue?.queueNumber ?? null)
		},
	})

	const roomOptions = useMemo(() => {
		return (getAllQueueSnapshots.data || [])
			.filter((item) => Number(item?.roomId) > 0)
			.map((item) => ({
				id: Number(item.roomId),
				code: item?.room?.code || '',
			}))
	}, [getAllQueueSnapshots.data])

	const selectedSnapshot = useMemo(() => {
		return (
			(getAllQueueSnapshots.data || []).find(
				(item) => String(item.roomId ?? '') === String(selectedRoomId ?? '')
			) || null
		)
	}, [getAllQueueSnapshots.data, selectedRoomId])

	const roomCode = selectedSnapshot?.room?.code || t('queue.text.reception_room')
	const roomSpecialtyName = selectedSnapshot?.room?.specialtyName
	const doctorName = selectedSnapshot?.doctor?.name

	const handleScanCode = useCallback(
		async (scannedValue = '') => {
			if (Date.now() < scanLockUntilRef.current || createQueueFromQRAPI.loading) return

			let parsedData = null
			try {
				parsedData = JSON.parse(scannedValue)
			} catch {
				toast.error(t('queue.guest.error.invalid_qr_format'), {
					toastId: 'queue-invalid-qr-format',
				})
				return
			}

			const medicalHistoryId = Number(parsedData?.medicalHistory)
			if (!Number.isInteger(medicalHistoryId) || medicalHistoryId <= 0) {
				toast.error(t('queue.guest.error.invalid_qr_format'), {
					toastId: 'queue-invalid-qr-format',
				})
				return
			}

			const parsedRoomId = Number(selectedRoomId)
			const roomId = Number.isInteger(parsedRoomId) && parsedRoomId > 0 ? parsedRoomId : null

			scanLockUntilRef.current = Date.now() + SCAN_LOCK_MS

			await createQueueFromQRAPI.submit({
				overrideData: {
					medicalHistoryId,
					roomId,
				},
			})
		},
		[createQueueFromQRAPI, selectedRoomId, t]
	)

	const handleCloseQueueDialog = () => {
		setCreatedQueueNumber(null)
	}

	return (
		<Box
			sx={{
				height: '100dvh',
				minHeight: '100vh',
				overflow: 'hidden',
				position: 'relative',
				py: { xs: 1.2, md: 1.8 },
				px: { xs: 1.5, md: 2.5 },
				display: 'flex',
				background: `linear-gradient(180deg, ${alpha(theme.palette.primary.light, 0.08)} 0%, ${theme.palette.background.default} 44%, ${theme.palette.background.paper} 100%)`,
			}}
		>
			<QueueRoomSelectionDrawer
				roomOptions={roomOptions}
				selectedRoomId={selectedRoomId}
				onApplyRoom={setSelectedRoomId}
				currentRoomCode={roomCode}
			/>

			<Container
				maxWidth='lg'
				sx={{
					height: '100%',
					display: 'flex',
					alignItems: 'stretch',
				}}
			>
				<Stack direction={'row'} alignItems={'center'}>
					<Stack
						alignItems='center'
						justifyContent='space-evenly'
						spacing={1}
						sx={{
							width: '100%',
							height: '100%',
							py: { xs: 1, md: 1.4 },
						}}
					>
						<CreateQueueFromQRHeaderSection
							roomCode={roomCode}
							doctorName={doctorName}
							roomSpecialtyName={roomSpecialtyName}
						/>

						<CreateQueueFromQRScannerSection onScanCode={handleScanCode} />
					</Stack>
					<CreateQueueFromQRInstructionSection />
				</Stack>
			</Container>

			<QueueCreatedDialog
				open={createdQueueNumber != null}
				queueNumber={createdQueueNumber}
				onClose={handleCloseQueueDialog}
			/>
		</Box>
	)
}

export default CreateQueueFromQRPage
