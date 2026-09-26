import { getImageFromCloud } from '@/utils/commons'
import { formatDatetimeToDDMMYYYY } from '@/utils/formatDateUtil'
import { Avatar, Box, Button, Chip, Stack, Typography } from '@mui/material'

const FeedbackRespondItem = ({
	respond,
	feedbackId,
	canEdit = false,
	onEdit = () => {},
	onDelete = () => {},
	onReply = () => {},
	deleteLoading = false,
	t,
}) => {
	const { content, creationDate } = respond

	const responderName = respond?.getResponderDTO?.name
	const responderImage = respond?.getResponderDTO?.image
	const responderRole = respond?.getResponderDTO?.role

	return (
		<Box sx={{ position: 'relative', pl: 6, py: 1 }}>
			<Box
				sx={{
					position: 'absolute',
					left: 20,
					top: 26,
					width: 21,
					height: 2,
					bgcolor: 'divider',
					opacity: 0.4,
				}}
			/>
			<Stack direction='row' spacing={1.5}>
				<Avatar src={getImageFromCloud(responderImage)} />
				<Box flex={1}>
					<Stack direction='row' alignItems='center' spacing={1}>
						<Typography fontWeight={600} variant='body2'>
							{responderName}
						</Typography>
						<Chip label={responderRole} color='primary' size='small' sx={{ height: 20, fontSize: 10 }} />
					</Stack>
					<Typography variant='body2' sx={{ mt: 0.5 }}>
						{content}
					</Typography>
					<Stack direction='row' alignItems='center' spacing={1} sx={{ mt: 0.5 }}>
						<Typography variant='caption' color='text.secondary'>
							{formatDatetimeToDDMMYYYY(creationDate)}
						</Typography>
						{canEdit && (
							<Button
								size='small'
								variant='text'
								color='primary'
								sx={{ minWidth: 'auto', p: 0.5 }}
								onClick={() => onEdit(respond)}
							>
								{t('button.edit')}
							</Button>
						)}
						{canEdit && (
							<Button
								size='small'
								variant='text'
								color='error'
								sx={{ minWidth: 'auto', p: 0.5 }}
								loading={deleteLoading}
								onClick={() => onDelete(respond)}
							>
								{t('button.delete')}
							</Button>
						)}
						<Button
							size='small'
							variant='text'
							color='warning'
							sx={{ minWidth: 'auto', p: 0.5 }}
							onClick={() => onReply(feedbackId)}
						>
							{t('feedback.button.reply')}
						</Button>
					</Stack>
				</Box>
			</Stack>
		</Box>
	)
}

export default FeedbackRespondItem
