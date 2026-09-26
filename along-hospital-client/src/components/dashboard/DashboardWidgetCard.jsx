import useTranslation from '@/hooks/useTranslation'
import { Box, Paper, Skeleton, Stack, Typography } from '@mui/material'

const DashboardWidgetCard = ({
	title,
	subtitle,
	loading = false,
	error = null,
	empty = false,
	minHeight = 240,
	children,
}) => {
	const { t } = useTranslation()

	return (
		<Paper
			variant='outlined'
			sx={{
				p: 2,
				borderRadius: 3,
				minHeight,
				display: 'flex',
				flexDirection: 'column',
				background: (theme) =>
					`linear-gradient(180deg, ${theme.palette.background.paper} 0%, ${
						theme.palette.mode === 'dark'
							? theme.palette.background.default
							: theme.palette.grey[50]
					} 100%)`,
			}}
		>
			<Stack spacing={0.5} mb={2}>
				<Typography variant='h6' sx={{ fontWeight: 700 }}>
					{title}
				</Typography>
				{subtitle ? (
					<Typography variant='body2' color='text.secondary'>
						{subtitle}
					</Typography>
				) : null}
			</Stack>

			<Box sx={{ flex: 1, display: 'flex', alignItems: 'stretch' }}>
				{loading ? (
					<Stack spacing={1.25} sx={{ width: '100%' }}>
						<Skeleton variant='text' width='45%' height={36} />
						<Skeleton variant='rounded' height={Math.max(minHeight - 90, 120)} />
					</Stack>
				) : error ? (
					<Stack
						spacing={1}
						alignItems='center'
						justifyContent='center'
						sx={{ width: '100%', textAlign: 'center' }}
					>
						<Typography variant='body2' color='text.secondary'>
							{error?.message || t('text.placeholder.no_data')}
						</Typography>
					</Stack>
				) : empty ? (
					<Stack
						spacing={1}
						alignItems='center'
						justifyContent='center'
						sx={{ width: '100%', textAlign: 'center' }}
					>
						<Typography variant='body2' color='text.secondary'>
							{t('text.placeholder.no_data')}
						</Typography>
					</Stack>
				) : (
					<Box sx={{ width: '100%' }}>{children}</Box>
				)}
			</Box>
		</Paper>
	)
}

export default DashboardWidgetCard
