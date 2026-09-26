import {
	defaultAppointmentPaymentStatusStyle,
	defaultAppointmentStatusStyle,
	defaultLineClampStyle,
} from '@/configs/defaultStylesConfig'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { formatDateAndTimeBasedOnCurrentLanguage } from '@/utils/formatDateUtil'
import { getEnumLabelByValue } from '@/utils/handleStringUtil'
import { Box, Card, CardActionArea, CardContent, Chip, Stack, Typography } from '@mui/material'
import { useTheme } from '@mui/material/styles'
import React, { Fragment } from 'react'

const ManageAppointmentListItemSection = ({ appointment, onClick }) => {
	const theme = useTheme()
	const { t } = useTranslation()
	const _enum = useEnum()
	const statusStyle = defaultAppointmentStatusStyle(theme, appointment?.appointmentStatus)

	const generalDisplayFields = [
		{
			label: t('appointment.field.meeting_type'),
			value: getEnumLabelByValue(
				_enum.appointmentMeetingTypeOptions,
				appointment?.appointmentMeetingType
			),
		},
		{
			label: t('appointment.field.specialty'),
			value: appointment?.specialty?.name,
		},
		{
			label: t('appointment.field.patient'),
			value: appointment?.patient?.name,
		},
	]

	const statusFields = [
		{
			label: t('appointment.field.status'),
			value: (
				<Chip
					label={getEnumLabelByValue(_enum.appointmentStatusOptions, appointment?.appointmentStatus)}
					size='small'
					sx={{
						bgcolor: statusStyle.bg,
						color: statusStyle.color,
						border: `1px solid ${statusStyle.border}`,
					}}
				/>
			),
		},
		{
			label: t('appointment.field.payment_status'),
			value: (
				<Chip
					label={getEnumLabelByValue(
						_enum.appointmentPaymentStatusOptions,
						appointment?.appointmentPaymentStatus
					)}
					size='small'
					color={defaultAppointmentPaymentStatusStyle(appointment?.appointmentPaymentStatus)}
				/>
			),
		},
	]

	return (
		<Card
			variant='outlined'
			sx={{
				bgcolor: 'background.paper',
				borderColor: 'divider',
				height: '100%',
				'&:hover': { borderColor: 'primary.light' },
			}}
		>
			<CardActionArea onClick={onClick} sx={{ height: '100%' }}>
				<CardContent sx={{ height: '100%' }}>
					<Stack direction='row' spacing={2} alignItems='flex-start'>
						<Box sx={{ minWidth: 90 }}>
							<Typography variant='subtitle2'>
								{formatDateAndTimeBasedOnCurrentLanguage(
									appointment?.date,
									appointment?.timeSlotSnapshot?.time
								)
									.split(' ')
									.map((x, i) => (
										<Fragment key={i}>
											{x}
											<br />
										</Fragment>
									))}
							</Typography>
						</Box>
						<Stack spacing={0.5} sx={{ flex: 1, minWidth: 0 }}>
							<Stack direction={'column'} gap={{ xs: 0.2, md: 1 }} flexWrap={'wrap'}>
								{generalDisplayFields.map((item) => (
									<Typography variant='body2' key={`${appointment?.id}-${item.label}`}>
										{item.label}: {item.value}
									</Typography>
								))}
							</Stack>
							<Typography variant='body2' sx={{ color: 'text.secondary', ...defaultLineClampStyle(2) }}>
								{appointment?.purpose}
							</Typography>
						</Stack>
						<Stack spacing={1} alignItems='end'>
							{statusFields.map((item) => (
								<Stack direction={'row'} gap={1} key={`${appointment?.id}-${item.label}`}>
									<Typography variant='subtitle2'>{item.label}:</Typography>
									{React.isValidElement(item.value) ? (
										item.value
									) : (
										<Typography variant='body2'>{item.value}</Typography>
									)}
								</Stack>
							))}
						</Stack>
					</Stack>
				</CardContent>
			</CardActionArea>
		</Card>
	)
}

export default ManageAppointmentListItemSection
