import { EnumConfig } from '@/configs/enumConfig'
import { routeUrls } from '@/configs/routeUrls'
import useTranslation from '@/hooks/useTranslation'
import { LocalHospital, MeetingRoom, Science, SupportAgent } from '@mui/icons-material'
import { Box, Paper, Stack, Typography, alpha, useTheme } from '@mui/material'
import { useMemo } from 'react'
import { useNavigate } from 'react-router-dom'
import {
	formatQueueNumber,
	getRoomCardState,
	getRoomDisplayCode,
	getVisibleQueues,
} from '../../queueManagementUtil'

const QueueManagementRoomCardSection = ({ snapshot }) => {
	const theme = useTheme()
	const { t } = useTranslation()
	const navigate = useNavigate()

	const roomState = getRoomCardState(snapshot)
	const roomCode = getRoomDisplayCode(snapshot, t)
	const roomSpecialtyName =
		snapshot?.room?.specialtyName || t('queue.placeholder.room_specialty_pending')
	const doctorName = snapshot?.doctor?.name

	const visibleQueues = useMemo(() => {
		const sourceQueues = Array.isArray(snapshot?.queues) ? snapshot.queues : []
		return getVisibleQueues(sourceQueues)
	}, [snapshot])

	const currentQueue = useMemo(
		() =>
			visibleQueues.find((queue) => queue.queueStatus === EnumConfig.QueueStatus.InProgress) || null,
		[visibleQueues]
	)

	const upNextQueues = useMemo(
		() => visibleQueues.filter((queue) => queue.id !== currentQueue?.id).slice(0, 3),
		[visibleQueues, currentQueue]
	)

	const roomStateIcon = (() => {
		switch (roomState) {
			case 'reception':
				return <SupportAgent fontSize='small' />
			case 'non_doctor_room':
				return <Science fontSize='small' />
			default:
				return <LocalHospital fontSize='small' />
		}
	})()

	const handleOpenRoomDetail = () => {
		navigate(routeUrls.BASE_ROUTE.STAFF(routeUrls.STAFF.QUEUE_MANAGEMENT.DETAIL(snapshot?.roomId)))
	}

	return (
		<Paper
			className='queue-room-card'
			elevation={0}
			role='button'
			tabIndex={0}
			onClick={handleOpenRoomDetail}
			onKeyDown={(event) => {
				if (event.key === 'Enter' || event.key === ' ') {
					event.preventDefault()
					handleOpenRoomDetail()
				}
			}}
			sx={{
				height: '100%',
				overflow: 'hidden',
				borderRadius: 3,
				cursor: 'pointer',
				display: 'flex',
				flexDirection: 'column',
				border: `1px solid ${alpha(theme.palette.primary.main, 0.16)}`,
				backgroundColor: theme.palette.background.paper,
				transition: 'all 0.2s ease',
				'& .queue-room-card-header': {
					backgroundColor: theme.palette.primary.dark,
				},
				'&:hover': {
					transform: 'translateY(-2px)',
					backgroundColor: alpha(theme.palette.primary.main, 0.14),
					borderColor: theme.palette.primary.main,
					boxShadow: `0 14px 24px -20px ${alpha(theme.palette.primary.main, 0.72)}`,
				},
				'&:hover .queue-room-card-header': {
					backgroundColor: theme.palette.primary.main,
				},
			}}
		>
			<Box
				className='queue-room-card-header'
				sx={{
					px: 2,
					py: 1.5,
					color: theme.palette.common.white,
					display: 'flex',
					alignItems: 'flex-start',
					justifyContent: 'space-between',
					gap: 1,
				}}
			>
				<Stack spacing={0.15} minWidth={0}>
					<Typography
						variant='h6'
						sx={{
							fontWeight: 800,
							lineHeight: 1.1,
							textTransform: 'uppercase',
							wordBreak: 'break-word',
						}}
					>
						{roomCode}
					</Typography>

					<Typography
						variant='body2'
						sx={{
							lineHeight: 1.2,
							fontWeight: 600,
							color: alpha(theme.palette.common.white, 0.84),
							wordBreak: 'break-word',
						}}
					>
						{roomSpecialtyName}
					</Typography>
				</Stack>

				<Box sx={{ display: 'flex', alignItems: 'center', pt: 0.25 }}>
					{roomState === 'reception' ? <MeetingRoom fontSize='small' /> : roomStateIcon}
				</Box>
			</Box>

			<Stack sx={{ p: 2, flex: 1 }} spacing={1.4}>
				<Stack alignItems='center' spacing={0.4} sx={{ py: 0.5, flexGrow: 1 }}>
					<Typography
						variant='caption'
						sx={{
							fontWeight: 800,
							textTransform: 'uppercase',
							letterSpacing: 1.1,
							color: alpha(theme.palette.text.primary, 0.44),
						}}
					>
						{t('queue.title.now_serving')}
					</Typography>

					<Typography variant='h2' sx={{ fontWeight: 800, lineHeight: 1 }}>
						{formatQueueNumber(currentQueue?.queueNumber)}
					</Typography>

					{doctorName && (
						<Typography
							variant='body2'
							sx={{
								fontWeight: 700,
								color: alpha(theme.palette.text.primary, 0.7),
								textAlign: 'center',
							}}
						>
							{doctorName}
						</Typography>
					)}
				</Stack>

				<Box
					sx={{
						mt: 'auto',
						p: 1.2,
						borderRadius: 2,
						backgroundColor: alpha(theme.palette.text.primary, 0.04),
					}}
				>
					<Typography
						variant='caption'
						sx={{
							fontWeight: 800,
							textTransform: 'uppercase',
							color: alpha(theme.palette.text.primary, 0.54),
						}}
					>
						{t('queue.title.up_next')}
					</Typography>

					{upNextQueues.length > 0 ? (
						<Stack direction='row' spacing={0.8} flexWrap='wrap' useFlexGap sx={{ mt: 0.8 }}>
							{upNextQueues.map((queue) => (
								<Box
									key={queue.id}
									sx={{
										py: 0.45,
										px: 1.15,
										borderRadius: 0.75,
										fontWeight: 700,
										lineHeight: 1,
										border: `1px solid ${alpha(theme.palette.text.primary, 0.12)}`,
										backgroundColor: theme.palette.background.paper,
									}}
								>
									{formatQueueNumber(queue.queueNumber)}
								</Box>
							))}
						</Stack>
					) : (
						<Typography variant='body2' color='text.secondary' sx={{ mt: 0.8 }}>
							{t('queue.placeholder.no_visible_queue')}
						</Typography>
					)}
				</Box>
			</Stack>
		</Paper>
	)
}

export default QueueManagementRoomCardSection
