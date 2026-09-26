import { MeetingRoomRounded } from '@mui/icons-material'
import { Stack, Typography, useTheme } from '@mui/material'
import { alpha } from '@mui/material/styles'

const CreateQueueFromQRHeaderSection = ({ roomCode, doctorName, roomSpecialtyName }) => {
	const theme = useTheme()

	return (
		<Stack alignItems='center' spacing={{ xs: 1, md: 1.4 }} sx={{ width: '100%' }}>
			<Stack
				direction='row'
				alignItems='center'
				spacing={0.75}
				sx={{
					px: 2,
					py: 0.9,
					borderRadius: '999px',
					bgcolor: alpha(theme.palette.primary.main, 0.1),
					color: theme.palette.primary.main,
				}}
			>
				<MeetingRoomRounded sx={{ fontSize: 18 }} />
				<Typography
					sx={{ fontWeight: 700, letterSpacing: '0.01em', fontSize: { xs: '1.05rem', md: '1.45rem' } }}
				>
					{roomCode}
				</Typography>
			</Stack>

			{doctorName && (
				<Typography
					component='h1'
					align='center'
					sx={{
						fontWeight: 800,
						fontSize: { xs: '2rem', md: 'clamp(2.2rem, 5vh, 4rem)' },
						lineHeight: 1.08,
						color: theme.palette.text.primary,
						letterSpacing: '-0.01em',
						textTransform: 'uppercase',
					}}
				>
					{doctorName}
				</Typography>
			)}

			{roomSpecialtyName && (
				<Typography
					align='center'
					sx={{
						fontWeight: 700,
						fontSize: { xs: '1.3rem', md: 'clamp(1.4rem, 2.9vh, 2.1rem)' },
						lineHeight: 1.25,
						color: theme.palette.primary.main,
					}}
				>
					{roomSpecialtyName}
				</Typography>
			)}
		</Stack>
	)
}

export default CreateQueueFromQRHeaderSection
