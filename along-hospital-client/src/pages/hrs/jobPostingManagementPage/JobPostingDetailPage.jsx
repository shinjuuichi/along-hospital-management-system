import { ApiUrls } from '@/configs/apiUrls'
import { routeUrls } from '@/configs/routeUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useConfirm from '@/hooks/useConfirm'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import JobApplicationTableSection from '@/pages/hrs/jobPostingManagementPage/sections/JobApplicationTableSection'
import JobPostingInfoSection from '@/pages/hrs/jobPostingManagementPage/sections/JobPostingInfoSection'
import { Paper } from '@mui/material'
import { useEffect, useMemo, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'

const JobPostingDetailPage = () => {
	const { t } = useTranslation()
	const confirm = useConfirm()
	const navigate = useNavigate()
	const { id } = useParams()
	const postingId = id ? Number(id) : null
	const isValidPostingId = Number.isFinite(postingId) && postingId > 0

	const [sort, setSort] = useState({ key: 'id', direction: 'desc' })
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)
	const [filters, setFilters] = useState({ email: '', applicationStatus: '' })

	useEffect(() => {
		setPage(1)
	}, [sort, filters])

	const fetchPosting = useFetch(
		isValidPostingId ? ApiUrls.JOB_POSTING.MANAGEMENT.DETAIL(postingId) : null,
		{},
		[postingId],
		isValidPostingId
	)

	const jobApplicationsParams = useMemo(() => {
		return {
			jobPostingId: postingId,
			email: filters.email,
			applicationStatus: filters.applicationStatus,
			page,
			pageSize,
			sort: `${sort.key} ${sort.direction}`,
		}
	}, [postingId, filters, page, pageSize, sort])

	const fetchApplications = useFetch(
		ApiUrls.JOB_APPLICATION.MANAGEMENT.INDEX,
		jobApplicationsParams,
		[jobApplicationsParams]
	)

	const jobApplications = useMemo(
		() => fetchApplications.data?.collection || [],
		[fetchApplications.data]
	)
	const totalPage = fetchApplications.data?.totalPage || 1

	const rejectAllApplications = useAxiosSubmit({
		method: 'PUT',
		onSuccess: async () => {
			await fetchPosting.fetch()
			await fetchApplications.fetch()
		},
	})

	const stats = useMemo(() => {
		const total = jobApplications.length
		return { total }
	}, [jobApplications])

	const handleRejectAll = async () => {
		const isConfirmed = await confirm({
			title: t('job_application.dialog.confirm_reject_all_title'),
			description: t('job_application.dialog.confirm_reject_all_description'),
			confirmText: t('job_application.button.reject_all'),
			confirmColor: 'error',
		})
		if (isConfirmed) {
			await rejectAllApplications.submit({
				overrideUrl: ApiUrls.JOB_APPLICATION.MANAGEMENT.REJECT_ALL(postingId),
			})
		}
	}

	return (
		<Paper sx={{ p: { xs: 2, md: 3 }, display: 'flex', flexDirection: 'column', gap: 2 }}>
			<JobPostingInfoSection
				t={t}
				selectedPosting={fetchPosting.data}
				stats={stats}
				onBack={() => navigate(routeUrls.BASE_ROUTE.HR(routeUrls.HR.JOB_POSTING.INDEX))}
			/>

			<JobApplicationTableSection
				applications={jobApplications}
				loading={fetchApplications.loading}
				sort={sort}
				setSort={setSort}
				filters={filters}
				setFilters={setFilters}
				onDetail={(application) => {
					const appId = application?.id ?? application?.applicationId
					if (!appId) return
					navigate(
						routeUrls.BASE_ROUTE.HR(routeUrls.HR.JOB_POSTING.APPLICATION_DETAIL(postingId, appId))
					)
				}}
				onRejectAll={handleRejectAll}
				rejectAllLoading={rejectAllApplications.loading}
				totalPage={totalPage}
				page={page}
				setPage={setPage}
				pageSize={pageSize}
				setPageSize={setPageSize}
			/>
		</Paper>
	)
}

export default JobPostingDetailPage
