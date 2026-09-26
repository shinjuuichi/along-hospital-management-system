import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { formatDatetimeStringBasedOnCurrentLanguage } from '@/utils/formatDateUtil'
import { getEnumLabelByValue } from '@/utils/handleStringUtil'
import { PeopleAltOutlined } from '@mui/icons-material'
import { Box, Chip, Paper, Stack, Typography, alpha, useTheme } from '@mui/material'
import { formatQueueNumber } from '../../queueManagementUtil'

const QueueDetailManagementQueueListSection = ({ queues = [], currentQueueId = null }) => {
	const theme = useTheme()
	const { t } = useTranslation()
	const _enum = useEnum()

	const safeQueues = Array.isArray(queues) ? queues : []

	return (
		<Paper
			elevation={0}
			sx={{
				height: '100%',
				width: '100%',
				border: `1px solid ${alpha(theme.palette.primary.main, 0.2)}`,
				overflow: 'hidden',
				display: 'flex',
				flexDirection: 'column',
				backgroundColor: alpha(theme.palette.primary.main, 0.02),
			}}
		>
			<Stack
				direction='row'
				alignItems='center'
				spacing={0.8}
				sx={{
					px: 1.5,
					py: 1.2,
					borderBottom: `1px solid ${alpha(theme.palette.divider, 0.85)}`,
					bgcolor: alpha(theme.palette.common.black, 0.03),
				}}
			>
				<PeopleAltOutlined sx={{ color: alpha(theme.palette.primary.main, 0.86) }} />
				<Typography
					variant='h6'
					sx={{
						fontSize: { xs: '1rem', md: '1.08rem' },
						fontWeight: 800,
						letterSpacing: '0.01em',
					}}
				>
					{t('queue.title.queue_list')}
				</Typography>
			</Stack>

			<Stack sx={{ overflow: 'auto', flex: 1, bgcolor: theme.palette.background.paper }}>
				{safeQueues.length === 0 && (
					<Stack alignItems='center' justifyContent='center' sx={{ height: '100%' }}>
						<Typography color='text.secondary'>{t('queue.placeholder.no_queue_in_room')}</Typography>
					</Stack>
				)}

				{safeQueues.map((queue) => {
					const isCurrent = queue.id === currentQueueId
					const queueLabel = `#${formatQueueNumber(queue.queueNumber)}`
					const queueTime = formatDatetimeStringBasedOnCurrentLanguage(queue.queueDate)
					const patientName =
						queue?.medicalHistorySnapshot?.patient?.name || t('queue.placeholder.patient_name_pending')
					const statusLabel =
						getEnumLabelByValue(_enum.queueStatusOptions, queue.queueStatus) || queue.queueStatus

					return (
						<Box
							key={queue.id}
							sx={{
								px: 1.2,
								py: 1.1,
								position: 'relative',
								borderBottom: `1px solid ${alpha(theme.palette.divider, 0.9)}`,
								backgroundColor: isCurrent ? alpha(theme.palette.primary.main, 0.12) : 'transparent',
								'&::before': isCurrent
									? {
											content: '""',
											position: 'absolute',
											left: 0,
											top: 0,
											bottom: 0,
											width: 3,
											backgroundColor: theme.palette.primary.main,
										}
									: undefined,
							}}
						>
							<Stack spacing={0.55}>
								<Stack direction='row' justifyContent='space-between' alignItems='flex-start' spacing={1}>
									<Stack spacing={0.15} sx={{ minWidth: 0 }}>
										<Typography
											sx={{
												fontWeight: isCurrent ? 800 : 700,
												fontSize: { xs: '0.98rem', md: '1.06rem' },
												lineHeight: 1.2,
												color: isCurrent ? theme.palette.primary.main : theme.palette.text.primary,
												overflow: 'hidden',
												textOverflow: 'ellipsis',
												whiteSpace: 'nowrap',
											}}
										>
											{queueLabel} - {patientName}
										</Typography>
									</Stack>

									{Number(queue.priority) > 0 && (
										<Chip
											label={`${t('queue.text.priority')} ${queue.priority}`}
											size='small'
											color='error'
											sx={{
												fontWeight: 800,
												height: 20,
												fontSize: '0.66rem',
											}}
										/>
									)}
								</Stack>

								<Stack direction='row' spacing={0.6} alignItems='center'>
									<Typography
										variant='caption'
										sx={{
											fontWeight: 600,
											fontSize: '0.72rem',
											color: alpha(theme.palette.text.secondary, 0.9),
										}}
									>
										{statusLabel}
									</Typography>
									<Typography variant='caption' color='text.disabled'>
										|
									</Typography>
									<Typography
										variant='caption'
										sx={{
											fontWeight: 600,
											fontSize: '0.72rem',
											color: alpha(theme.palette.text.secondary, 0.9),
										}}
									>
										{queueTime}
									</Typography>
								</Stack>
							</Stack>
						</Box>
					)
				})}
			</Stack>
		</Paper>
	)
}

export default QueueDetailManagementQueueListSection
