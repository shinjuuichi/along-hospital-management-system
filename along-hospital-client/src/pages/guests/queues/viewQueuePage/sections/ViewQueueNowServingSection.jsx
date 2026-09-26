import useTranslation from '@/hooks/useTranslation'
import { FiberManualRecordRounded } from '@mui/icons-material'
import { Box, Chip, Grid, Paper, Stack, Typography, useTheme } from '@mui/material'
import { alpha } from '@mui/material/styles'

const ViewQueueNowServingSection = ({
	currentQueueNumber = '--',
	currentPatientName = '',
	hasInProgress = false,
	summaryItems = [],
}) => {
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
				border: `1px solid ${alpha(theme.palette.primary.main, 0.25)}`,
				overflow: 'hidden',
			}}
		>
			<Stack
				direction='row'
				alignItems='center'
				justifyContent='space-between'
				sx={{
					px: { xs: 1.5, md: 2.2 },
					py: { xs: 1.2, md: 1.5 },
					bgcolor: alpha(theme.palette.primary.main, 0.5),
					color: theme.palette.primary.contrastText,
					borderBottom: `1px solid ${alpha(theme.palette.primary.main, 0.14)}`,
				}}
			>
				<Typography sx={{ fontSize: { xs: '1.1rem', md: '1.7rem' }, fontWeight: 800 }}>
					{t('queue.guest.title.now_serving')}
				</Typography>
				<Chip
					icon={<FiberManualRecordRounded sx={{ fontSize: '0.72rem !important' }} />}
					label={t('queue.guest.text.live')}
					size='small'
					sx={{
						bgcolor: theme.palette.primary.main,
						color: theme.palette.primary.contrastText,
						fontWeight: 700,
					}}
				/>
			</Stack>

			<Stack
				alignItems='center'
				justifyContent='center'
				spacing={{ xs: 0.9, md: 1.2 }}
				sx={{
					flex: 1,
					px: { xs: 1.5, md: 2.5 },
					py: { xs: 1.2, md: 1.6 },
				}}
			>
				<Typography
					align='center'
					sx={{
						fontWeight: 700,
						fontSize: { xs: '1.1rem', md: '2rem' },
						color: alpha(theme.palette.text.primary, 0.45),
						textTransform: 'uppercase',
						letterSpacing: '0.08em',
					}}
				>
					{t('queue.guest.title.ticket_number')}
				</Typography>

				<Typography
					align='center'
					sx={{
						fontWeight: 900,
						fontSize: { xs: '3.4rem', md: 'clamp(4.2rem, 12vh, 8.4rem)' },
						lineHeight: 1,
						letterSpacing: '-0.04em',
						color: theme.palette.text.primary,
					}}
				>
					{currentQueueNumber}
				</Typography>

				<Box
					sx={{
						width: { xs: 80, md: 120 },
						height: 7,
						borderRadius: 999,
						bgcolor: alpha(theme.palette.primary.main, 0.3),
					}}
				/>

				<Typography
					align='center'
					sx={{
						fontWeight: 700,
						fontSize: { xs: '1rem', md: '1.5rem' },
						color: alpha(theme.palette.text.primary, 0.58),
						textTransform: 'uppercase',
						letterSpacing: '0.08em',
					}}
				>
					{t('queue.guest.title.patient_name')}
				</Typography>

				<Typography
					align='center'
					sx={{
						fontWeight: 800,
						fontSize: { xs: '1.6rem', md: 'clamp(1.8rem, 4.8vh, 3.6rem)' },
						lineHeight: 1.15,
						maxWidth: '94%',
						textTransform: 'uppercase',
						color: theme.palette.text.primary,
					}}
				>
					{currentPatientName}
				</Typography>

				{!hasInProgress && (
					<Typography color='text.secondary' align='center'>
						{t('queue.guest.placeholder.no_queue_in_progress')}
					</Typography>
				)}
			</Stack>

			<Grid
				container
				spacing={0.8}
				sx={{
					p: { xs: 0.5, md: 1 },
					bgcolor: alpha(theme.palette.primary.main, 0.25),
				}}
			>
				{summaryItems.map((item) => (
					<Grid key={item.key} size={{ xs: 6, md: 3 }}>
						<Stack
							spacing={0.2}
							sx={{
								px: 1,
								py: 0.8,
								borderRadius: 1.2,
								bgcolor: alpha(theme.palette.background.paper, 0.75),
								border: `1px solid ${alpha(theme.palette.divider, 0.8)}`,
							}}
						>
							<Typography
								sx={{
									fontSize: { xs: '0.68rem', md: '0.8rem' },
									lineHeight: 1.2,
									fontWeight: 700,
									color: alpha(theme.palette.text.secondary, 0.95),
									textTransform: 'uppercase',
								}}
							>
								{item.label}
							</Typography>
							<Typography sx={{ fontWeight: 800, fontSize: { xs: '1.1rem', md: '1.35rem' } }}>
								{item.value}
							</Typography>
						</Stack>
					</Grid>
				))}
			</Grid>
		</Paper>
	)
}

export default ViewQueueNowServingSection
