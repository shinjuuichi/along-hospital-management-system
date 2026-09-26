import { SmartToy } from '@mui/icons-material'
import { Box, Stack, Typography } from '@mui/material'
import { useTheme } from '@mui/material/styles'

const ChatBotMessageBubble = ({ message }) => {
	const theme = useTheme()

	if (message.role === 'user') {
		return (
			<Box
				sx={{
					display: 'flex',
					justifyContent: 'flex-end',
					mb: 1,
				}}
			>
				<Box
					sx={{
						maxWidth: '80%',
						backgroundColor: theme.palette.primary.main,
						color: theme.palette.primary.contrastText,
						borderRadius: 1,
						px: 1.5,
						py: 1,
						boxShadow: 1,
						wordBreak: 'break-word',
					}}
				>
					<Typography variant='body2'>{message.content}</Typography>
				</Box>
			</Box>
		)
	}
	return (
		<Box
			sx={{
				display: 'flex',
				justifyContent: 'flex-start',
				mb: 1,
			}}
		>
			<Stack direction='row' spacing={1} alignItems='flex-start'>
				<Box
					sx={{
						width: 28,
						height: 28,
						borderRadius: '50%',
						display: 'flex',
						alignItems: 'center',
						justifyContent: 'center',
						backgroundColor: theme.palette.background.lightBlue || theme.palette.background.paper,
						border: `1px solid ${theme.palette.primary.softBorder || theme.palette.divider}`,
					}}
				>
					<SmartToy
						sx={{
							fontSize: 18,
							color: theme.palette.text.blue?.main || theme.palette.primary.main,
						}}
					/>
				</Box>
				<Box
					sx={{
						maxWidth: '80%',
					}}
				>
					<Typography
						variant='caption'
						sx={{
							display: 'block',
							mb: 0.25,
							color: theme.palette.text.secondary,
						}}
					>
						Bot
					</Typography>
					<Box
						sx={{
							backgroundColor: theme.palette.background.lightBlue || theme.palette.background.paper,
							border: `1px solid ${theme.palette.primary.softBorder || theme.palette.divider}`,
							borderRadius: 1,
							px: 1.5,
							py: 1,
							wordBreak: 'break-word',
						}}
					>
						<Typography
							variant='body2'
							sx={{ color: theme.palette.text.primary, whiteSpace: 'pre-wrap' }}
						>
							{message.content}
						</Typography>
					</Box>
				</Box>
			</Stack>
		</Box>
	)
}

export default ChatBotMessageBubble
