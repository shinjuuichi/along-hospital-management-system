import useTranslation from '@/hooks/useTranslation'
import { AccessTimeRounded } from '@mui/icons-material'
import { Box, CircularProgress, Paper, Stack, Typography, useTheme } from '@mui/material'
import { alpha } from '@mui/material/styles'

const CreateQueueWaitingSection = ({ countdownProgress, remainingSeconds }) => {
	const { t } = useTranslation()
	const theme = useTheme()

	return (
		<Stack justifyContent='center' alignItems='center' sx={{ width: '100%', height: '100%' }}>
			<Paper
				elevation={0}
				sx={{
					width: '100%',
					maxWidth: 760,
					mx: 'auto',
					borderRadius: { xs: 4, md: 5 },
					p: { xs: 3, md: 'clamp(1.8rem, 3.4vh, 3.6rem)' },
					border: `1px solid ${alpha(theme.palette.primary.main, 0.18)}`,
					boxShadow: `0 20px 48px ${alpha(theme.palette.primary.main, 0.12)}`,
				}}
			>
				<Stack alignItems='center' spacing={2.5}>
					<Box sx={{ position: 'relative', display: 'inline-flex' }}>
						<CircularProgress
							variant='determinate'
							value={100}
							size={136}
							thickness={3}
							sx={{ color: alpha(theme.palette.primary.main, 0.2) }}
						/>
						<CircularProgress
							variant='determinate'
							value={countdownProgress}
							size={136}
							thickness={3}
							sx={{
								position: 'absolute',
								left: 0,
								color: theme.palette.primary.main,
							}}
						/>
						<Box
							sx={{
								top: 0,
								left: 0,
								right: 0,
								bottom: 0,
								position: 'absolute',
								display: 'flex',
								alignItems: 'center',
								justifyContent: 'center',
								color: theme.palette.primary.main,
							}}
						>
							<AccessTimeRounded sx={{ fontSize: 52 }} />
						</Box>
					</Box>

					<Typography
						component='h1'
						variant='h4'
						align='center'
						sx={{
							fontWeight: 800,
							fontSize: { xs: '1.8rem', md: 'clamp(2rem, 3.8vh, 2.3rem)' },
							lineHeight: 1.2,
						}}
					>
						{t('queue.guest.title.waiting')}
					</Typography>

					<Typography
						align='center'
						sx={{
							fontSize: { xs: '1rem', md: 'clamp(1rem, 2vh, 1.15rem)' },
							lineHeight: 1.6,
							color: theme.palette.text.secondary,
						}}
					>
						{t('queue.guest.text.waiting_description')}
					</Typography>

					<Typography
						align='center'
						sx={{
							fontWeight: 700,
							fontSize: { xs: '1.6rem', md: 'clamp(1.7rem, 3.2vh, 2rem)' },
							color: theme.palette.primary.main,
							letterSpacing: '0.04em',
						}}
					>
						{t('queue.guest.text.waiting_countdown', {
							time: remainingSeconds,
						})}
					</Typography>
				</Stack>
			</Paper>
		</Stack>
	)
}

export default CreateQueueWaitingSection
