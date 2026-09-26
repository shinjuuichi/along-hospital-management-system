import FeedbackFormDialog from '@/components/dialogs/FeedbackFormDialog'
import { ApiUrls } from '@/configs/apiUrls'
import useAuth from '@/hooks/useAuth'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useConfirm from '@/hooks/useConfirm'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import { Grid, Typography } from '@mui/material'
import { useEffect, useMemo, useState } from 'react'
import FeedbackFilterSection from './FeedbackFilterSection'
import FeedbackListSection from './FeedbackListSection'
import FeedbackSummarySection from './FeedbackSummarySection'

const FeedbackSection = ({ medicineId }) => {
	const { t } = useTranslation()
	const { auth } = useAuth()
	const confirm = useConfirm()

	const [selectedStar, setSelectedStar] = useState(null)
	const [reviews, setReviews] = useState([])
	const [openCreate, setOpenCreate] = useState(false)
	const [openEdit, setOpenEdit] = useState(false)
	const [editingItem, setEditingItem] = useState(null)

	const createFeedback = useAxiosSubmit({ url: ApiUrls.FEEDBACK.INDEX, method: 'POST' })
	const updateFeedback = useAxiosSubmit({
		url: ApiUrls.FEEDBACK.DETAIL(editingItem?.id),
		method: 'PUT',
	})
	const deleteFeedback = useAxiosSubmit({ method: 'DELETE' })

	const createRespond = useAxiosSubmit({ url: ApiUrls.FEEDBACK_RESPOND.INDEX, method: 'POST' })
	const editRespond = useAxiosSubmit({ method: 'PUT' })
	const deleteRespond = useAxiosSubmit({ method: 'DELETE' })

	const reportFeedback = useAxiosSubmit({ url: ApiUrls.FEEDBACK_REPORT.INDEX, method: 'POST' })

	const getFeedbacksByMedicine = useFetch(
		ApiUrls.FEEDBACK.GET_FEEDBACK_BY_MEDICINE(medicineId),
		{},
		[medicineId]
	)

	useEffect(() => {
		const res = getFeedbacksByMedicine.data
		if (!res) return

		const items = Array.isArray(res) ? res : Array.isArray(res?.collection) ? res.collection : []
		setReviews(items)
	}, [getFeedbacksByMedicine.data])

	const distribution = useMemo(() => {
		const d = { 5: 0, 4: 0, 3: 0, 2: 0, 1: 0 }
		reviews.forEach((r) => {
			const key = Number(r?.rating) || 0
			if (key >= 1 && key <= 5) d[key] = (d[key] || 0) + 1
		})
		return d
	}, [reviews])

	const total = reviews.length
	const average = useMemo(() => {
		if (!total) return 0
		const sum = reviews.reduce((acc, r) => acc + (Number(r?.rating) || 0), 0)
		return sum / total
	}, [total, reviews])

	const filtered = useMemo(() => {
		return selectedStar ? reviews.filter((r) => r.rating === selectedStar) : reviews
	}, [selectedStar, reviews])

	const visibleItems = filtered

	const summary = { total, average, distribution }

	const handleReplySubmit = async (payload) => {
		const result = await createRespond.submit({ overrideData: payload })
		if (result) {
			await getFeedbacksByMedicine.fetch()
		}
		return result
	}

	const handleEditRespondSubmit = async (payload) => {
		if (!payload?.id) return
		const result = await editRespond.submit({
			overrideUrl: ApiUrls.FEEDBACK_RESPOND.DETAIL(payload.id),
			overrideData: { content: payload.content.trim() },
		})
		if (result) {
			await getFeedbacksByMedicine.fetch()
		}
		return result
	}

	const handleReportSubmit = async (feedbackId, reason) => {
		const payload = {
			feedbackId,
			reason,
		}
		const result = await reportFeedback.submit({ overrideData: payload })
		return result
	}

	const handleDeleteRespondSubmit = async (respond) => {
		if (!respond?.id) return
		const ok = await confirm({
			title: t('feedback.title.page'),
			description: t('commons.text.confirm_delete'),
			confirmText: t('button.delete'),
			confirmColor: 'error',
		})
		if (!ok) return
		const result = await deleteRespond.submit({
			overrideUrl: ApiUrls.FEEDBACK_RESPOND.DETAIL(respond.id),
		})
		if (result) {
			await getFeedbacksByMedicine.fetch()
		}
		return result
	}

	if (!medicineId) return null

	return (
		<>
			<Typography variant='h5' fontWeight={700} my={2}>
				{t('feedback.title.page')}
			</Typography>
			<Grid container spacing={2} sx={{ width: '100%' }}>
				<Grid size={{ xs: 12 }}>
					<FeedbackSummarySection summary={summary} onOpenReview={() => setOpenCreate(true)} />
				</Grid>
				<Grid size={{ xs: 12 }}>
					<FeedbackFilterSection
						selected={selectedStar}
						counts={distribution}
						onChange={(v) => {
							setSelectedStar(v)
						}}
					/>
				</Grid>
				<Grid size={{ xs: 12 }}>
					<FeedbackListSection
						items={visibleItems}
						loading={!!getFeedbacksByMedicine.loading}
						currentUserId={auth?.userId}
						canModify={(rv) => String(rv?.patientId ?? '') === String(auth?.userId ?? '')}
						onEdit={(rv) => {
							setEditingItem(rv)
							setOpenEdit(true)
						}}
						onDelete={async (rv) => {
							const ok = await confirm({
								title: t('feedback.title.page'),
								description: t('commons.text.confirm_delete'),
								confirmText: t('button.delete'),
								confirmColor: 'error',
							})
							if (!ok) return
							const res = await deleteFeedback.submit({
								overrideUrl: ApiUrls.FEEDBACK.DETAIL(rv.id),
							})
							if (res != null) {
								await getFeedbacksByMedicine.fetch()
							}
						}}
						onRefresh={() => getFeedbacksByMedicine.fetch()}
						onReplySubmit={handleReplySubmit}
						onEditRespondSubmit={handleEditRespondSubmit}
						onDeleteRespondSubmit={handleDeleteRespondSubmit}
						onReportSubmit={handleReportSubmit}
						replyLoading={createRespond.loading}
						editRespondLoading={editRespond.loading}
						deleteRespondLoading={deleteRespond.loading}
						reportLoading={reportFeedback.loading}
					/>
				</Grid>
			</Grid>

			<FeedbackFormDialog
				open={openCreate}
				onClose={() => setOpenCreate(false)}
				initialValues={{ rating: 5, content: '' }}
				submitLabel={t('button.create')}
				submitButtonColor='success'
				title={t('feedback.button.submit_review')}
				onSubmit={async ({ values, closeDialog }) => {
					const payload = {
						content: values.content,
						rating: Number(values.rating),
						medicineId,
					}
					const res = await createFeedback.submit({ overrideData: payload })
					if (res) {
						await getFeedbacksByMedicine.fetch()
						closeDialog()
					}
				}}
			/>

			<FeedbackFormDialog
				open={openEdit}
				onClose={() => setOpenEdit(false)}
				initialValues={{ rating: editingItem?.rating ?? 5, content: editingItem?.content ?? '' }}
				submitLabel={t('button.update')}
				submitButtonColor='success'
				title={t('button.edit')}
				onSubmit={async ({ values, closeDialog }) => {
					if (!editingItem?.id) return
					const payload = {
						content: values.content,
						rating: Number(values.rating),
						medicineId,
					}
					const res = await updateFeedback.submit({ overrideData: payload })
					if (res) {
						await getFeedbacksByMedicine.fetch()
						closeDialog()
					}
				}}
			/>
		</>
	)
}

export default FeedbackSection
