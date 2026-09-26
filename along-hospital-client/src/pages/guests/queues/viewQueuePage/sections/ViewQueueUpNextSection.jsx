import { EnumConfig } from '@/configs/enumConfig'
import useTranslation from '@/hooks/useTranslation'
import { formatQueueNumber } from '@/pages/staffs/queues/queueManagementUtil'
import { getEnumLabelByValue } from '@/utils/handleStringUtil'
import { FormatListNumberedRounded } from '@mui/icons-material'
import { Box, Chip, Paper, Stack, Typography, useTheme } from '@mui/material'
import { alpha } from '@mui/material/styles'

const ViewQueueUpNextSection = ({ queues = [], queueStatusOptions = [] }) => {
	const theme = useTheme()
	const { t } = useTranslation()

	return (
		<Paper
			elevation={0}
			sx={{
				height: '100%',
				display: 'flex',
				flexDirection: 'column',
				borderRadius: 3,
				overflow: 'hidden',
				border: `1px solid ${alpha(theme.palette.primary.main, 0.25)}`,
			}}
		>
			<Stack
				direction='row'
				alignItems='center'
				spacing={0.9}
				sx={{
					px: { xs: 1.5, md: 2 },
					py: { xs: 1.2, md: 1.6 },
					borderBottom: `1px solid ${alpha(theme.palette.divider, 0.85)}`,
				}}
			>
				<FormatListNumberedRounded color='primary' />
				<Typography sx={{ fontSize: { xs: '1.06rem', md: '1.55rem' }, fontWeight: 800 }}>
					{t('queue.guest.title.up_next')}
				</Typography>
			</Stack>

			<Stack sx={{ flex: 1, overflow: 'auto' }}>
				{queues.length === 0 && (
					<Stack sx={{ flex: 1 }} alignItems='center' justifyContent='center' spacing={0.5}>
						<Typography color='text.secondary'>
							{t('queue.guest.placeholder.no_queue_available')}
						</Typography>
					</Stack>
				)}

				{queues.map((queue, index) => {
					const isInProgress = queue.queueStatus === EnumConfig.QueueStatus.InProgress
					const isCalled = queue.queueStatus === EnumConfig.QueueStatus.Called
					const patientName =
						queue?.medicalHistorySnapshot?.patient?.name ||
						t('queue.guest.placeholder.patient_name_pending')
					const statusLabel =
						getEnumLabelByValue(queueStatusOptions, queue.queueStatus) || queue.queueStatus

					return (
						<Box
							key={queue.id || `${queue.queueNumber}-${index}`}
							sx={{
								display: 'flex',
								alignItems: 'center',
								justifyContent: 'space-between',
								gap: 1,
								px: { xs: 1.5, md: 2 },
								py: { xs: 1, md: 1.2 },
								bgcolor: isInProgress
									? alpha(theme.palette.primary.main, 0.12)
									: isCalled
										? alpha(theme.palette.warning.main, 0.1)
										: 'transparent',
								borderBottom:
									index < queues.length - 1 ? `1px solid ${alpha(theme.palette.divider, 0.7)}` : 'none',
							}}
						>
							<Stack direction='row' alignItems='center' spacing={1.4} sx={{ minWidth: 0, flex: 1 }}>
								<Typography
									sx={{
										fontWeight: 800,
										fontSize: { xs: '1.3rem', md: '2.2rem' },
										lineHeight: 1,
										letterSpacing: '-0.02em',
										color: isInProgress
											? theme.palette.primary.main
											: isCalled
												? theme.palette.warning.dark
												: alpha(theme.palette.text.primary, 0.4),
									}}
								>
									{formatQueueNumber(queue.queueNumber)}
								</Typography>

								<Typography
									sx={{
										fontWeight: isInProgress ? 800 : 700,
										fontSize: { xs: '1.02rem', md: '1.9rem' },
										lineHeight: 1.2,
										color: isInProgress ? theme.palette.primary.main : theme.palette.text.primary,
										overflow: 'hidden',
										textOverflow: 'ellipsis',
										whiteSpace: 'nowrap',
									}}
								>
									{patientName}
								</Typography>
							</Stack>

							<Stack alignItems='flex-end' justifyContent='center' sx={{ minWidth: { xs: 72, md: 96 } }}>
								{Number(queue.priority) > 0 ? (
									<Chip
										label={t('queue.guest.text.priority_badge', { value: queue.priority })}
										size='small'
										sx={{
											fontWeight: 800,
											bgcolor: alpha(theme.palette.warning.main, 0.16),
											color: theme.palette.warning.dark,
										}}
									/>
								) : !isInProgress ? (
									<Typography
										sx={{
											fontWeight: 700,
											fontSize: { xs: '0.8rem', md: '1.08rem' },
											textTransform: 'uppercase',
											color: alpha(theme.palette.text.secondary, 0.85),
										}}
									>
										{statusLabel}
									</Typography>
								) : null}
							</Stack>
						</Box>
					)
				})}
			</Stack>
		</Paper>
	)
}

export default ViewQueueUpNextSection
