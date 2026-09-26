import useTranslation from '@/hooks/useTranslation'
import { Stack, Typography, useTheme } from '@mui/material'

const CreateQueueFromQRInstructionSection = () => {
	const { t } = useTranslation()
	const theme = useTheme()

	return (
		<Stack alignItems='center' spacing={{ xs: 1, md: 1.6 }} sx={{ width: '100%' }}>
			<Typography
				align='center'
				sx={{
					maxWidth: 860,
					fontWeight: 800,
					fontSize: { xs: '2rem', md: 'clamp(2rem, 4.8vh, 4.1rem)' },
					lineHeight: 1.12,
					color: theme.palette.text.primary,
					textTransform: 'uppercase',
					whiteSpace: 'pre-line',
				}}
			>
				{t('queue.guest.title.scan_instruction')}
			</Typography>

			<Typography
				align='center'
				sx={{
					fontWeight: 500,
					fontSize: { xs: '1.15rem', md: 'clamp(1.1rem, 2.2vh, 1.7rem)' },
					lineHeight: 1.5,
					color: theme.palette.text.secondary,
				}}
			>
				{t('queue.guest.text.scan_hint')}
			</Typography>
		</Stack>
	)
}

export default CreateQueueFromQRInstructionSection
