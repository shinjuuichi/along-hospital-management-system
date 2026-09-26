import { getEnv } from '@/utils/commons'
import { Box, ButtonBase, Typography } from '@mui/material'

export default function SystemLogoAndName({ onClick, onlyShowIcon = false }) {
	return (
		<>
			<ButtonBase
				onClick={onClick}
				sx={{
					minWidth: 40,
					height: 40,
					borderRadius: '50%',
					overflow: 'hidden',
					bgcolor: 'background.paper',
					border: (theme) => `2px solid ${theme.palette.text.blue.light}`,
					boxShadow: 1,
				}}
			>
				<Box
					component='img'
					src={'/system-logo.png'}
					sx={{ width: '100%', height: '100%', objectFit: 'cover' }}
				/>
			</ButtonBase>
			{!onlyShowIcon && (
				<Typography
					variant='h6'
					component='div'
					noWrap
					textOverflow={'ellipsis'}
					sx={{ ml: 1, userSelect: 'none', color: 'text.blue.dark' }}
				>
					{getEnv('VITE_SYSTEM_NAME')}
				</Typography>
			)}
		</>
	)
}
