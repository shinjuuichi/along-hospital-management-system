import useTranslation from '@/hooks/useTranslation'
import { Grid, Paper, Typography, alpha, useTheme } from '@mui/material'
import { useMemo } from 'react'
import { renderEmptyFallback } from '@/utils/handleStringUtil'

const QueueDetailManagementMainInfoSection = ({ currentQueue }) => {
	const theme = useTheme()
	const { t } = useTranslation()

	const patient = currentQueue?.medicalHistorySnapshot?.patient

	const heightWeightText = useMemo(() => {
		const height = patient?.height
		const weight = patient?.weight

		if (height == null && weight == null) {
			return t('queue.placeholder.height_weight_pending')
		}

		return `${renderEmptyFallback(height)} cm / ${renderEmptyFallback(weight)} kg`
	}, [currentQueue, t])

	const demographicText = useMemo(() => {
		const segments = [patient?.dateOfBirth, patient?.gender].filter(Boolean)
		return segments.length ? segments.join(' / ') : t('queue.placeholder.demographic_pending')
	}, [currentQueue, t])

	const infoCards = useMemo(
		() => [
			{
				key: 'appointment_purpose',
				title: t('queue.field.appointment_snapshot.purpose'),
				value: currentQueue?.appointmentSnapshot?.purpose || t('queue.placeholder.purpose_pending'),
			},
			{
				key: 'specialty_snapshot_name',
				title: t('queue.field.specialty_snapshot.name'),
				value: currentQueue?.specialtySnapshot?.name || t('text.none'),
			},
			{
				key: 'patient_height_weight',
				title: t('queue.field.medical_history_snapshot.patient.height_weight'),
				value: heightWeightText,
			},
			{
				key: 'patient_demographic',
				title: t('queue.field.medical_history_snapshot.patient.demographic'),
				value: demographicText,
			},
		],
		[currentQueue, demographicText, heightWeightText, t]
	)

	return (
		<Grid container spacing={1}>
			{infoCards.map((item) => (
				<Grid key={item.key} size={{ xs: 12, sm: 6, lg: 3 }}>
					<Paper
						variant='outlined'
						sx={{
							p: 1,
							height: '100%',
							display: 'flex',
							flexDirection: 'column',
							justifyContent: 'center',
						}}
					>
						<Typography
							variant='caption'
							sx={{
								fontWeight: 800,
								textTransform: 'uppercase',
								letterSpacing: '0.05em',
								color: alpha(theme.palette.text.primary, 0.52),
							}}
						>
							{item.title}
						</Typography>
						<Typography
							sx={{
								fontWeight: 700,
								fontSize: { xs: '0.95rem', md: '1rem' },
								lineHeight: 1.35,
								mt: 0.35,
							}}
						>
							{item.value}
						</Typography>
					</Paper>
				</Grid>
			))}
		</Grid>
	)
}

export default QueueDetailManagementMainInfoSection
