import { InboxOutlined } from '@mui/icons-material'
import { alpha, Paper, Stack, Typography } from '@mui/material'

const HomeSectionFallback = ({ title, subtitle, icon }) => {
	return (
		<Paper
			variant='outlined'
			sx={(theme) => ({
				p: { xs: 3, md: 4 },
				borderRadius: 4,
				borderStyle: 'dashed',
				borderColor: alpha(theme.palette.primary.main, 0.22),
				bgcolor:
					theme.palette.mode === 'dark'
						? alpha(theme.palette.primary.softBg, 0.7)
						: alpha(theme.palette.primary.softBg, 0.55),
			})}
		>
			<Stack spacing={1.25} alignItems='center' textAlign='center'>
				<Paper
					elevation={0}
					sx={(theme) => ({
						width: 64,
						height: 64,
						borderRadius: '50%',
						display: 'flex',
						alignItems: 'center',
						justifyContent: 'center',
						bgcolor: alpha(theme.palette.primary.main, 0.12),
						color: 'primary.main',
					})}
				>
					{icon || <InboxOutlined sx={{ fontSize: 30 }} />}
				</Paper>
				<Typography variant='h6' fontWeight={700}>
					{title}
				</Typography>
				{subtitle ? (
					<Typography variant='body2' color='text.secondary' sx={{ maxWidth: 520 }}>
						{subtitle}
					</Typography>
				) : null}
			</Stack>
		</Paper>
	)
}

export default HomeSectionFallback
