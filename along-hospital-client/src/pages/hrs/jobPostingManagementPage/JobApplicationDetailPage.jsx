import DetailCard from '@/components/generals/DetailCard'
import EmptyPage from '@/components/placeholders/EmptyPage'
import { ApiUrls } from '@/configs/apiUrls'
import { defaultJobApplicationStatusStyle } from '@/configs/defaultStylesConfig'
import { EnumConfig } from '@/configs/enumConfig'
import { routeUrls } from '@/configs/routeUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useEnum from '@/hooks/useEnum'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import JobApplicationInterviewSection from '@/pages/hrs/jobPostingManagementPage/sections/JobApplicationInterviewSection'
import { getImageFromCloud } from '@/utils/commons'
import { formatDateBasedOnCurrentLanguage } from '@/utils/formatDateUtil'
import { getEnumLabelByValue, renderEmptyFallback } from '@/utils/handleStringUtil'
import { ArrowBack, PictureAsPdf } from '@mui/icons-material'
import { Button, Chip, Paper, Stack, Typography } from '@mui/material'
import { useMemo } from 'react'
import { useNavigate, useParams } from 'react-router-dom'

const JobApplicationDetailPage = () => {
	const { t } = useTranslation()
	const _enum = useEnum()
	const navigate = useNavigate()
	const { jobPostingId: postingIdParam, applicationId: applicationIdParam } = useParams()

	const postingId = postingIdParam ? Number(postingIdParam) : null
	const applicationId = applicationIdParam ? Number(applicationIdParam) : null
	const isValidPostingId = Number.isFinite(postingId) && postingId > 0
	const isValidApplicationId = Number.isFinite(applicationId) && applicationId > 0

	const getApplicationDetail = useFetch(
		isValidApplicationId ? ApiUrls.JOB_APPLICATION.MANAGEMENT.DETAIL(applicationId) : null,
		{},
		[applicationId],
		isValidApplicationId
	)

	const setApplicationPassed = useAxiosSubmit({
		method: 'PUT',
		onSuccess: async () => {
			await getApplicationDetail.fetch()
		},
	})

	const handleSetPassed = async () => {
		await setApplicationPassed.submit({
			overrideUrl: ApiUrls.JOB_APPLICATION.MANAGEMENT.PASS(applicationId),
		})
	}

	const application = getApplicationDetail.data
	const isApplicationPassed =
		application?.applicationStatus === EnumConfig.JobApplicationStatus.Passed
	const isApplicationFailed =
		application?.applicationStatus === EnumConfig.JobApplicationStatus.Failed
	const fullName = application?.name || application?.fullName || ''

	const applicationDetailFields = useMemo(
		() => [
			{ label: t('job_application.field.full_name'), value: fullName },
			{ label: t('job_application.field.email'), value: application?.email },
			{ label: t('job_application.field.phone'), value: application?.phone },
			{
				label: t('job_application.field.date_of_birth'),
				value: application?.dateOfBirth,
				render: (value) => (
					<Typography variant='body1'>{renderEmptyFallback(formatDateBasedOnCurrentLanguage(value))}</Typography>
				),
			},
			{ label: t('job_application.field.address'), value: application?.address },
			{
				label: t('job_application.field.gender'),
				value: application?.gender,
				render: (value) => (
					<Typography variant='body1'>
						{renderEmptyFallback(getEnumLabelByValue(_enum.genderOptions, value) || value)}
					</Typography>
				),
			},
			{
				label: t('job_application.field.applied_date'),
				value: application?.applyDate,
				render: (value) => (
					<Typography variant='body1'>{renderEmptyFallback(formatDateBasedOnCurrentLanguage(value))}</Typography>
				),
			},
			{
				label: t('job_application.field.status'),
				value: application?.applicationStatus,
				render: (value) => (
					<Chip
						color={defaultJobApplicationStatusStyle(value)}
						label={getEnumLabelByValue(_enum.jobApplicationStatusOptions, value)}
						sx={{ width: 'fit-content' }}
					/>
				),
			},
			{
				label: t('job_application.field.cv_url'),
				value: application?.cvUrl,
				render: (value) =>
					value ? (
						<Button
							variant='outlined'
							startIcon={<PictureAsPdf />}
							onClick={() => window.open(getImageFromCloud(value), '_blank')}
							sx={{ textTransform: 'none' }}
						>
							{t('job_application.button.download_cv')}
						</Button>
					) : (
						<Typography variant='body1'>-</Typography>
					),
				md: 12,
			},
		],
		[t, fullName, application, _enum.jobApplicationStatusOptions]
	)

	if (!isValidPostingId || !isValidApplicationId) {
		return <EmptyPage title={t('text.placeholder.no_data')} />
	}

	if (!application && !getApplicationDetail.loading) {
		return <EmptyPage title={t('text.placeholder.no_data')} />
	}

	return (
		<Paper sx={{ p: { xs: 2, md: 3 } }}>
			<Stack spacing={2}>
				<Stack direction='row' alignItems='center' justifyContent='space-between'>
					<Button
						startIcon={<ArrowBack />}
						onClick={() => navigate(routeUrls.BASE_ROUTE.HR(routeUrls.HR.JOB_POSTING.DETAIL(postingId)))}
					>
						{t('button.back')}
					</Button>
					{!isApplicationPassed && !isApplicationFailed && (
						<Button
							variant='contained'
							color='success'
							onClick={handleSetPassed}
							disabled={setApplicationPassed.loading}
						>
							{t('interview.button.set_passed')}
						</Button>
					)}
				</Stack>

				<Typography variant='h5'>{t('job_application.dialog.detail_title')}</Typography>

				<DetailCard fields={applicationDetailFields} />

				<JobApplicationInterviewSection
					applicationId={applicationId}
					applicationStatus={application?.applicationStatus}
					onChanged={async () => {
						await getApplicationDetail.fetch()
					}}
				/>
			</Stack>
		</Paper>
	)
}

export default JobApplicationDetailPage
