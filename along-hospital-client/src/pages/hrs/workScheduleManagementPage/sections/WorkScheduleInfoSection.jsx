import { defaultWorkScheduleStatusStyle } from '@/configs/defaultStylesConfig'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { getEnumLabelByValue } from '@/utils/handleStringUtil'
import { Chip, Grid, Paper, Stack, Typography } from '@mui/material'

const WorkScheduleInfoSection = ({ schedule }) => {
	const { t } = useTranslation()
	const _enum = useEnum()

	const status = schedule?.workScheduleStatus

	return (
		<Paper variant='outlined' sx={{ p: 2 }}>
			<Stack spacing={2}>
				<Typography variant='h6'>{t('work_schedule.title.info')}</Typography>

				<Grid container spacing={2}>
					<Grid size={{ xs: 12, sm: 6, md: 3 }}>
						<Typography variant='caption' color='text.secondary'>
							{t('work_schedule.field.work_date')}
						</Typography>
						<Typography variant='body1' fontWeight={600}>
							{schedule?.workDate || '—'}
						</Typography>
					</Grid>

					<Grid size={{ xs: 12, sm: 6, md: 3 }}>
						<Typography variant='caption' color='text.secondary'>
							{t('work_schedule.field.shift')}
						</Typography>
						<Typography variant='body1' fontWeight={600}>
							{schedule?.shift?.name || '—'}
						</Typography>
						{schedule?.shift && (
							<Typography variant='body2' color='text.secondary'>
								{schedule.shift.startTime} – {schedule.shift.endTime}
							</Typography>
						)}
					</Grid>

					<Grid size={{ xs: 12, sm: 6, md: 3 }}>
						<Typography variant='caption' color='text.secondary'>
							{t('work_schedule.field.status')}
						</Typography>
						<Stack direction='row' alignItems='center' sx={{ mt: 0.5 }}>
							<Chip
								label={getEnumLabelByValue(_enum.workScheduleStatusOptions, status) || status}
								color={defaultWorkScheduleStatusStyle(status)}
								size='small'
							/>
						</Stack>
					</Grid>

					<Grid size={{ xs: 12, sm: 6, md: 3 }}>
						<Typography variant='caption' color='text.secondary'>
							{t('work_schedule.field.template_id')}
						</Typography>
						<Typography variant='body1' fontWeight={600}>
							{schedule?.workScheduleTemplateId != null ? `#${schedule.workScheduleTemplateId}` : '—'}
						</Typography>
					</Grid>
				</Grid>
			</Stack>
		</Paper>
	)
}

export default WorkScheduleInfoSection
