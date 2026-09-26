import { EnumConfig } from '@/configs/enumConfig'
import useTranslation from '@/hooks/useTranslation'
import {
	Button,
	Dialog,
	DialogActions,
	DialogContent,
	DialogTitle,
	Link,
	Stack,
	Typography,
} from '@mui/material'

const CreateAppointmentSuccessfullyDialog = ({
	open,
	onClose,
	paymentUrl,
	appointmentMeetingType,
}) => {
	const { t } = useTranslation()
	const isTelehealth = appointmentMeetingType === EnumConfig.AppointmentMeetingType.Telehealth

	return (
		<Dialog open={open} onClose={onClose} maxWidth='sm' fullWidth>
			<DialogTitle>{t('appointment.dialog.create_success_title')}</DialogTitle>
			<DialogContent>
				<Stack spacing={1.5}>
					<Typography variant='body2' color='text.secondary'>
						{t('appointment.dialog.create_success_payment_instruction')}
					</Typography>
					{paymentUrl ? (
						<Link href={paymentUrl} target='_blank' rel='noopener noreferrer' sx={{ fontWeight: 700 }}>
							{t('appointment.dialog.create_success_payment_link_label')}
						</Link>
					) : null}
					<Typography variant='body2' color='text.secondary'>
						{t('appointment.dialog.create_success_skip_payment_instruction')}
					</Typography>
					{isTelehealth ? (
						<Typography variant='body2' color='text.secondary'>
							{t('appointment.dialog.create_success_telehealth_join_instruction')}
						</Typography>
					) : null}
				</Stack>
			</DialogContent>
			<DialogActions>
				<Button onClick={onClose}>{t('button.close')}</Button>
			</DialogActions>
		</Dialog>
	)
}

export default CreateAppointmentSuccessfullyDialog
