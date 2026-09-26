import { EnumConfig } from '@/configs/enumConfig'
import { getImageFromCloud } from '@/utils/commons'
import { formatDatetimeToDDMMYYYY } from '@/utils/formatDateUtil'
import { Avatar, Box, Button, Chip, Rating, Stack, Typography } from '@mui/material'
import FeedbackRespondItem from './FeedbackRespondItem'

const FeedbackItem = ({
	review,
	canModify = false,
	onEdit = () => {},
	onDelete = () => {},
	onReport = () => {},
	onReply = () => {},
	onEditRespond = () => {},
	onDeleteRespond = () => {},
	canEditRespond = () => false,
	deleteRespondLoading = false,
	t,
}) => {
	const { id, content, rating, creationDate, feedbackStatus, feedbackResponds } = review

	const patientName = review?.getUserDTO?.patientName
	const patientImage = review?.getUserDTO?.patientImage
	const patientRole = review?.getUserDTO?.patientRole

	const isHiddenFeedback = feedbackStatus === EnumConfig.FeedbackStatus.Hidden

	return (
		<Box py={2} borderBottom={(theme) => `1px solid ${theme.palette.divider}`}>
			<Stack direction='row' spacing={2}>
				<Avatar src={getImageFromCloud(patientImage)} />
				<Box flex={1}>
					<Stack direction='row' alignItems='center' spacing={1}>
						<Typography fontWeight={600}>{patientName}</Typography>
						<Chip label={patientRole} color='primary' size='small' sx={{ height: 20, fontSize: 10 }} />
						<Rating size='small' value={rating} readOnly />
						<Box flex={1} />
						{!isHiddenFeedback && canModify && (
							<Stack direction='row' spacing={1}>
								<Button size='small' variant='text' onClick={() => onEdit(review)}>
									{t('button.edit')}
								</Button>
								<Button size='small' variant='text' color='error' onClick={() => onDelete(review)}>
									{t('button.delete')}
								</Button>
							</Stack>
						)}

						{!canModify && !isHiddenFeedback && (
							<Button size='small' variant='text' color='warning' onClick={() => onReport(review)}>
								{t('button.report')}
							</Button>
						)}
					</Stack>
					{isHiddenFeedback ? (
						<Typography sx={{ mt: 0.5, fontStyle: 'italic' }} color='text.secondary'>
							{t('feedback.text.hidden_feedback')}
						</Typography>
					) : (
						content && <Typography sx={{ mt: 0.5 }}>{content}</Typography>
					)}
					<Stack direction='row' alignItems='center' spacing={1} sx={{ mt: 0.5 }}>
						<Typography variant='caption' color='text.secondary'>
							{formatDatetimeToDDMMYYYY(creationDate)}
						</Typography>
						{!isHiddenFeedback && (
							<Button
								size='small'
								variant='text'
								color='warning'
								sx={{ minWidth: 'auto', p: 0.5 }}
								onClick={() => onReply(id)}
							>
								{t('feedback.button.reply')}
							</Button>
						)}
					</Stack>
				</Box>
			</Stack>

			{feedbackResponds.length > 0 && (
				<Box sx={{ position: 'relative', mt: 1.5 }}>
					<Box
						sx={{
							position: 'absolute',
							left: 20,
							top: -12,
							bottom: 67,
							width: 2,
							bgcolor: 'divider',
							opacity: 0.4,
						}}
					/>
					{feedbackResponds.map((respond) => (
						<FeedbackRespondItem
							key={respond.id}
							respond={respond}
							feedbackId={id}
							canEdit={canEditRespond(respond)}
							onEdit={onEditRespond}
							onDelete={onDeleteRespond}
							onReply={onReply}
							deleteLoading={deleteRespondLoading}
							t={t}
						/>
					))}
				</Box>
			)}
		</Box>
	)
}

export default FeedbackItem
