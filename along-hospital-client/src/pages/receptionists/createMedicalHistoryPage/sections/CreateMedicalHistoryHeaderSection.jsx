import useTranslation from '@/hooks/useTranslation'
import { Box, Paper, Stack, Typography } from '@mui/material'

const CreateMedicalHistoryHeaderSection = () => {
	const { t } = useTranslation()

	return (
		<Paper sx={{ p: 2.5, mb: 2, borderRadius: 2, bgcolor: 'background.lightBlue' }}>
			<Stack
				direction='row'
				alignItems='center'
				justifyContent='space-between'
				flexWrap='wrap'
				spacing={1.5}
			>
				<Box>
					<Typography variant='h5'>{t('medical_history.title.create_medical_history')}</Typography>
					<Typography variant='body2' color='text.secondary'>
						{t('text.step')} 1/1 — {t('medical_history.title.select_patient')}
					</Typography>
				</Box>
			</Stack>
		</Paper>
	)
}

export default CreateMedicalHistoryHeaderSection
