import useTranslation from '@/hooks/useTranslation'
import CloseIcon from '@mui/icons-material/Close'
import { Dialog, DialogContent, DialogTitle, IconButton, Stack, Typography } from '@mui/material'
import FeedbackInfoSection from './feedbackDetailSections/FeedbackInfoSection'
import FeedbackRespondsSection from './feedbackDetailSections/FeedbackRespondsSection'
import MedicineInfoSection from './feedbackDetailSections/MedicineInfoSection'
import PatientInfoSection from './feedbackDetailSections/PatientInfoSection'

const FeedbackDetailDialog = ({
	open,
	onClose,
	feedback,
	onReplySuccess,
	onCreateRespond,
	onEditRespond,
	createRespondLoading,
	editRespondLoading,
}) => {
	const { t } = useTranslation()

	return (
		<Dialog open={open} onClose={onClose} maxWidth='md' fullWidth>
			<DialogTitle>
				<Stack direction='row' justifyContent='space-between' alignItems='center'>
					<Typography variant='h6'>{t('feedback.title.feedback_detail')}</Typography>
					<IconButton onClick={onClose} size='small'>
						<CloseIcon />
					</IconButton>
				</Stack>
			</DialogTitle>
			<DialogContent dividers>
				{feedback ? (
					<Stack spacing={3}>
						<FeedbackInfoSection feedback={feedback} />
						<PatientInfoSection feedback={feedback} />
						<MedicineInfoSection feedback={feedback} />
						<FeedbackRespondsSection
							feedback={feedback}
							onReplySuccess={onReplySuccess}
							onCreateRespond={onCreateRespond}
							onEditRespond={onEditRespond}
							createRespondLoading={createRespondLoading}
							editRespondLoading={editRespondLoading}
						/>
					</Stack>
				) : (
					<Typography color='text.secondary' textAlign='center'>
						{t('feedback.text.no_feedback')}
					</Typography>
				)}
			</DialogContent>
		</Dialog>
	)
}

export default FeedbackDetailDialog
