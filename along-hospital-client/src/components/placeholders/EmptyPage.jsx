import useTranslation from '@/hooks/useTranslation'
import { Inbox } from '@mui/icons-material'
import { Box, Button, Typography } from '@mui/material'
import { useNavigate } from 'react-router-dom'

const EmptyPage = ({ title, subtitle, showButton = false, buttonText, onButtonClick }) => {
	const { t } = useTranslation()
	const navigate = useNavigate()

	const finalTitle = title || t('text.placeholder.no_data')
	const finalSubtitle = subtitle || t('text.placeholder.no_data_subtitle')
	const finalButtonText = buttonText || t('button.back')

	return (
		<Box
			sx={{
				minHeight: 'calc(100vh - 120px)',
				display: 'flex',
				flexDirection: 'column',
				alignItems: 'center',
				justifyContent: 'center',
				textAlign: 'center',
				gap: 2,
				px: 2,
			}}
		>
			<Box
				sx={{
					p: 2.5,
					borderRadius: '50%',
					bgcolor: 'secondary.softBg',
					border: (theme) => `1px dashed ${theme.palette.divider}`,
					display: 'flex',
					alignItems: 'center',
					justifyContent: 'center',
				}}
			>
				<Inbox sx={{ fontSize: 48, color: 'text.secondary' }} />
			</Box>

			<Typography variant='h5' sx={{ fontWeight: 600 }}>
				{finalTitle}
			</Typography>

			<Typography variant='body1' sx={{ color: 'text.secondary', maxWidth: 360 }}>
				{finalSubtitle}
			</Typography>

			{showButton && (
				<Button
					variant='contained'
					onClick={() => onButtonClick?.() ?? navigate(-1)}
					sx={{ mt: 1, px: 4, borderRadius: 3 }}
				>
					{finalButtonText}
				</Button>
			)}
		</Box>
	)
}

export default EmptyPage
