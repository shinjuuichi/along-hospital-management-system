import useAuth from '@/hooks/useAuth'
import useTranslation from '@/hooks/useTranslation'
import FeedbackEditRespondDialog from '@/pages/patients/feedbackPage/dialogs/FeedbackEditRespondDialog'
import FeedbackReplyDialog from '@/pages/patients/feedbackPage/dialogs/FeedbackReplyDialog'
import ReportFeedbackDialog from '@/pages/patients/feedbackPage/dialogs/ReportFeedbackDialog'
import FeedbackItem from '@/pages/patients/feedbackPage/sections/FeedbackItem'
import { Paper, Typography } from '@mui/material'
import { useState } from 'react'

const FeedbackListSection = ({
	items,
	onEdit = () => {},
	onDelete = () => {},
	canModify = () => false,
	currentUserId = null,
	onReplySubmit = null,
	onEditRespondSubmit = null,
	onDeleteRespondSubmit = null,
	onReportSubmit = null,
	replyLoading = false,
	editRespondLoading = false,
	deleteRespondLoading = false,
	reportLoading = false,
}) => {
	const { t } = useTranslation()
	const { auth } = useAuth()

	const [reportDialog, setReportDialog] = useState({ open: false, feedbackId: null })
	const [replyDialog, setReplyDialog] = useState({ open: false, feedbackId: null })
	const [editRespondDialog, setEditRespondDialog] = useState({ open: false, respond: null })

	const handleReport = (review) => {
		setReportDialog({ open: true, feedbackId: review?.id })
	}

	const handleReply = (feedbackId) => {
		setReplyDialog({ open: true, feedbackId })
	}

	const handleEditRespond = (respond) => {
		setEditRespondDialog({ open: true, respond })
	}

	const canEditRespond = (respond) => {
		return String(respond?.responderId ?? '') === String(auth?.userId ?? '')
	}

	const handleReportSubmit = async (reason) => {
		if (!reportDialog.feedbackId) return
		const target = items?.find((x) => x.id === reportDialog.feedbackId)
		const isSelf = String(target?.patientId ?? '') === String(currentUserId ?? '')
		if (isSelf) return

		const result = await onReportSubmit?.(reportDialog.feedbackId, reason)
		if (result) {
			setReportDialog({ open: false, feedbackId: null })
		}
		return result
	}

	return (
		<>
			<Paper sx={{ p: 2, borderRadius: 2 }}>
				{items?.length === 0 && (
					<Typography color='text.secondary' py={3} textAlign='center'>
						{t('feedback.text.empty')}
					</Typography>
				)}

				{items?.map((review) => (
					<FeedbackItem
						key={review.id}
						review={review}
						canModify={!!canModify(review)}
						onEdit={onEdit}
						onDelete={onDelete}
						onReport={handleReport}
						onReply={handleReply}
						onEditRespond={handleEditRespond}
						onDeleteRespond={onDeleteRespondSubmit}
						canEditRespond={canEditRespond}
						deleteRespondLoading={deleteRespondLoading}
						t={t}
					/>
				))}
			</Paper>

			<ReportFeedbackDialog
				open={!!reportDialog.open}
				onClose={() => setReportDialog({ open: false, feedbackId: null })}
				onSubmit={handleReportSubmit}
				loading={reportLoading}
			/>

			<FeedbackReplyDialog
				open={replyDialog.open}
				onClose={() => setReplyDialog({ open: false, feedbackId: null })}
				feedbackId={replyDialog.feedbackId}
				onSubmit={onReplySubmit}
				loading={replyLoading}
			/>

			<FeedbackEditRespondDialog
				open={editRespondDialog.open}
				onClose={() => setEditRespondDialog({ open: false, respond: null })}
				respond={editRespondDialog.respond}
				onSubmit={async (values) => {
					if (!editRespondDialog.respond?.id) return
					const payload = { ...values, id: editRespondDialog.respond.id }
					await onEditRespondSubmit?.(payload)
				}}
				loading={editRespondLoading}
			/>
		</>
	)
}

export default FeedbackListSection
