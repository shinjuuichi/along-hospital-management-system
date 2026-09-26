import useAuth from '@/hooks/useAuth'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import { getImageFromCloud } from '@/utils/commons'
import EditIcon from '@mui/icons-material/Edit'
import ExpandMoreIcon from '@mui/icons-material/ExpandMore'
import ReplyIcon from '@mui/icons-material/Reply'
import SendIcon from '@mui/icons-material/Send'
import {
	Accordion,
	AccordionDetails,
	AccordionSummary,
	Avatar,
	Box,
	Button,
	Card,
	CardContent,
	Chip,
	IconButton,
	Stack,
	Typography,
} from '@mui/material'
import { useState } from 'react'
import EditRespondDialog from './EditRespondDialog'

const FeedbackRespondsSection = ({
	feedback,
	onCreateRespond,
	onEditRespond,
	createRespondLoading = false,
	editRespondLoading = false,
}) => {
	const { t } = useTranslation()
	const { auth } = useAuth()
	const [editRespondDialog, setEditRespondDialog] = useState({ open: false, respond: null })

	const { values, handleChange, setField, registerRef, reset } = useForm({ content: '' })

	const { renderField } = useFieldRenderer(
		values,
		setField,
		handleChange,
		registerRef,
		false,
		'outlined',
		'small'
	)

	const handleSendReply = async () => {
		if (!values.content?.trim() || !feedback?.id) return

		const data = {
			content: values.content,
			feedbackId: feedback.id,
		}

		const res = await onCreateRespond?.(data)
		if (res) {
			reset({ content: '' })
		}
	}

	const handleEditRespondSubmit = async (values) => {
		if (!editRespondDialog.respond?.id) return

		const result = await onEditRespond?.(editRespondDialog.respond.id, {
			content: values.content.trim(),
		})

		if (result) {
			setEditRespondDialog({ open: false, respond: null })
		}
	}

	const renderFeedbackResponds = (responds) => {
		if (!responds || responds.length === 0) {
			return (
				<Typography color='text.secondary' sx={{ fontStyle: 'italic' }}>
					{t('feedback.text.no_responds')}
				</Typography>
			)
		}

		return (
			<Stack spacing={1.5}>
				{responds.map((respond, index) => {
					const canEdit = String(respond.responderId ?? '') === String(auth?.userId ?? '')
					const responder = respond?.getResponderDTO
					return (
						<Card
							key={respond.id || index}
							variant='outlined'
							sx={{
								bgcolor: 'action.hover',
								borderLeft: 3,
								borderLeftColor: 'primary.main',
							}}
						>
							<CardContent sx={{ py: 1.5, '&:last-child': { pb: 1.5 } }}>
								<Stack direction='row' spacing={1.5} alignItems='flex-start'>
									<Avatar src={getImageFromCloud(responder?.image)}>
										<ReplyIcon color='primary' fontSize='small' />
									</Avatar>
									<Box sx={{ flex: 1 }}>
										<Stack direction='row' justifyContent='space-between' alignItems='center' mb={1}>
											<Stack direction='row' spacing={1} alignItems='center' flexWrap='wrap'>
												<Typography variant='subtitle2' color='primary'>
													{responder?.name}
												</Typography>
												{responder?.role && (
													<Chip label={responder.role} size='small' color='primary' variant='outlined' />
												)}
												{responder?.email && (
													<Typography variant='caption' color='text.secondary'>
														{responder.email}
													</Typography>
												)}
											</Stack>
											<Stack direction='row' spacing={1} alignItems='center'>
												{respond.creationDate && (
													<Typography variant='caption' color='text.secondary'>
														{new Date(respond.creationDate).toLocaleDateString()}
													</Typography>
												)}
												{canEdit && (
													<IconButton size='small' onClick={() => setEditRespondDialog({ open: true, respond })}>
														<EditIcon fontSize='small' />
													</IconButton>
												)}
											</Stack>
										</Stack>
										<Typography variant='body2'>{respond.content}</Typography>
									</Box>
								</Stack>
							</CardContent>
						</Card>
					)
				})}
			</Stack>
		)
	}

	return (
		<>
			<Accordion defaultExpanded>
				<AccordionSummary expandIcon={<ExpandMoreIcon />}>
					<Stack direction='row' spacing={1} alignItems='center'>
						<Typography variant='subtitle1' fontWeight='bold' color='primary'>
							{t('feedback.title.feedback_responds')}
						</Typography>
						{feedback.feedbackResponds && (
							<Chip label={feedback.feedbackResponds.length} size='small' color='primary' />
						)}
					</Stack>
				</AccordionSummary>
				<AccordionDetails>
					<Stack spacing={2}>
						{renderFeedbackResponds(feedback.feedbackResponds)}

						{/* Reply Form */}
						<Card variant='outlined' sx={{ bgcolor: 'background.paper' }}>
							<CardContent>
								<Stack direction='row' spacing={1} alignItems='flex-start'>
									{renderField({
										key: 'content',
										multiple: 4,
										placeholder: t('feedback.placeholder.content'),
									})}
									<Button
										variant='contained'
										color='primary'
										onClick={handleSendReply}
										disabled={!values.content?.trim() || createRespondLoading}
										sx={{ minWidth: 100 }}
										startIcon={<SendIcon />}
									>
										{t('feedback.button.send_reply')}
									</Button>
								</Stack>
							</CardContent>
						</Card>
					</Stack>
				</AccordionDetails>
			</Accordion>

			<EditRespondDialog
				open={editRespondDialog.open}
				respond={editRespondDialog.respond}
				onClose={() => setEditRespondDialog({ open: false, respond: null })}
				onSubmit={handleEditRespondSubmit}
				loading={editRespondLoading}
			/>
		</>
	)
}

export default FeedbackRespondsSection
