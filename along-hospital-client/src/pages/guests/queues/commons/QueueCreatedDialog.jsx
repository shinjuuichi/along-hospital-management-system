import useTranslation from '@/hooks/useTranslation'
import { formatQueueNumber } from '@/pages/staffs/queues/queueManagementUtil'
import {
	Button,
	Dialog,
	DialogActions,
	DialogContent,
	DialogTitle,
	Typography,
} from '@mui/material'

const QueueCreatedDialog = ({ open, queueNumber, onClose }) => {
	const { t } = useTranslation()

	return (
		<Dialog open={open} onClose={onClose} maxWidth='xs' fullWidth>
			<DialogTitle>{t('queue.guest.title.queue_created')}</DialogTitle>
			<DialogContent>
				<Typography variant='body2' color='text.secondary'>
					{t('queue.guest.text.queue_created_success')}
				</Typography>
				<Typography variant='h3' fontWeight={700} sx={{ mt: 1.5 }}>
					{formatQueueNumber(queueNumber)}
				</Typography>
				<Typography variant='caption' color='text.secondary' sx={{ mt: 0.5, display: 'block' }}>
					{t('queue.guest.title.ticket_number')}
				</Typography>
			</DialogContent>
			<DialogActions>
				<Button onClick={onClose}>{t('button.close')}</Button>
			</DialogActions>
		</Dialog>
	)
}

export default QueueCreatedDialog
