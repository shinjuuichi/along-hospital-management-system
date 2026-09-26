import { defaultFeedbackReplyStatusStyle } from '@/configs/defaultStylesConfig'
import useTranslation from '@/hooks/useTranslation'
import { Box, Card, CardContent, Chip, Divider, Rating, Stack, Typography } from '@mui/material'

const FeedbackInfoSection = ({ feedback }) => {
	const { t } = useTranslation()

	return (
		<Card variant='outlined'>
			<CardContent>
				<Stack spacing={2}>
					<Stack direction='row' justifyContent='space-between' alignItems='center'>
						<Typography variant='subtitle1' fontWeight='bold'>
							{t('feedback.field.id')}: #{feedback.id}
						</Typography>
						<Chip
							label={feedback.feedbackReplyStatus}
							color={defaultFeedbackReplyStatusStyle(feedback.feedbackReplyStatus)}
							size='small'
						/>
					</Stack>
					<Divider />
					<Box>
						<Typography variant='caption' color='text.secondary' gutterBottom>
							{t('feedback.field.rating')}
						</Typography>
						<Stack direction='row' spacing={1} alignItems='center'>
							<Rating value={feedback.rating} readOnly />
							<Typography variant='body2'>({feedback.rating}/5)</Typography>
						</Stack>
					</Box>
					<Box>
						<Typography variant='caption' color='text.secondary' gutterBottom>
							{t('feedback.field.content')}
						</Typography>
						<Typography variant='body1'>{feedback.content}</Typography>
					</Box>
				</Stack>
			</CardContent>
		</Card>
	)
}

export default FeedbackInfoSection
