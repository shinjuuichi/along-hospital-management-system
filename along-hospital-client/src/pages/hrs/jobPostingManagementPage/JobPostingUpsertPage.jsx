import { ApiUrls } from '@/configs/apiUrls'
import { routeUrls } from '@/configs/routeUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useFetch from '@/hooks/useFetch'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import { uploadImage } from '@/utils/imageUpload'
import { ArrowBack } from '@mui/icons-material'
import { Button, Paper, Stack, Typography } from '@mui/material'
import { useEffect, useRef, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import JobPostingUpsertFormSection from './sections/JobPostingUpsertFormSection'

const JobPostingUpsertPage = () => {
	const { t } = useTranslation()
	const navigate = useNavigate()
	const { id: jobPostingIdParam } = useParams()
	const jobPostingId = jobPostingIdParam
	const isEditMode = Boolean(jobPostingId)

	const { values: formData, setField, reset } = useForm({
		title: '',
		role: '',
		description: '',
		requirement: '',
		benefit: '',
		employmentType: '',
		salaryMin: '',
		closeDate: '',
	})

	const [errors, setErrors] = useState({})
	const dataLoadedRef = useRef(false)

	const getJobPosting = useFetch(
		isEditMode && jobPostingId ? ApiUrls.JOB_POSTING.MANAGEMENT.DETAIL(jobPostingId) : null,
		{},
		isEditMode && jobPostingId ? [jobPostingId] : []
	)

	const createJobPosting = useAxiosSubmit({
		url: ApiUrls.JOB_POSTING.MANAGEMENT.INDEX,
		method: 'POST',
		onSuccess: async () => {
			navigate(routeUrls.BASE_ROUTE.HR(routeUrls.HR.JOB_POSTING.INDEX))
		},
	})

	const updateJobPosting = useAxiosSubmit({
		method: 'PUT',
		onSuccess: async () => {
			navigate(routeUrls.BASE_ROUTE.HR(routeUrls.HR.JOB_POSTING.INDEX))
		},
	})

	const handleInputChange = (field, value) => {
		setField(field, value)
		if (errors[field]) {
			setErrors((prev) => ({ ...prev, [field]: undefined }))
		}
	}

	const handleContentImageUpload = async (file) => {
		return await uploadImage(file, 'JobPostingContent')
	}

	useEffect(() => {
		dataLoadedRef.current = false
	}, [jobPostingId])

	useEffect(() => {
		if (!isEditMode || !jobPostingId || !getJobPosting.data || dataLoadedRef.current) return
		const job = getJobPosting.data
		dataLoadedRef.current = true
		reset({
			title: job.title ?? '',
			role: job.role ?? '',
			description: job.description ?? '',
			requirement: job.requirement ?? '',
			benefit: job.benefit ?? '',
			employmentType: job.employmentType ?? '',
			salaryMin: job.salaryMin ?? '',
			closeDate: job.closeDate ?? '',
		})
	}, [getJobPosting.data, isEditMode, jobPostingId, reset])

	const headerTitle = `${t(`button.${isEditMode ? 'update' : 'create'}`)} ${t('job_posting.title.management').toLowerCase()}`
	const jobPostingSubmit = isEditMode ? updateJobPosting : createJobPosting
	const submitButtonLabel = jobPostingSubmit.loading
		? isEditMode
			? t('button.submitting')
			: t('button.creating')
		: t(`button.${isEditMode ? 'update' : 'create'}`)
	const isFormDisabled = getJobPosting.loading || jobPostingSubmit.loading

	const handleSubmit = async (event) => {
		event.preventDefault()
		if (isFormDisabled) return

		const newErrors = {}
		if (!formData.title?.trim()) {
			newErrors.title = t('error.required')
		} else if (formData.title.length > 255) {
			newErrors.title = t('error.max_length', { max: 255 })
		}

		if (!formData.role?.trim()) {
			newErrors.role = t('error.required')
		}

		if (!formData.description?.trim()) {
			newErrors.description = t('error.required')
		}

		if (Object.keys(newErrors).length > 0) {
			setErrors(newErrors)
			return
		}

		const submitData = {
			Title: formData.title,
			Role: formData.role,
			Description: formData.description,
			Requirement: formData.requirement,
			Benefit: formData.benefit,
			EmploymentType: formData.employmentType,
			SalaryMin: formData.salaryMin ? parseFloat(formData.salaryMin) : null,
			CloseDate: formData.closeDate || null,
		}

		await jobPostingSubmit.submit({
			overrideData: submitData,
			...(isEditMode ? { overrideUrl: ApiUrls.JOB_POSTING.MANAGEMENT.DETAIL(jobPostingId) } : {}),
		})
	}

	const handleBack = () => {
		navigate(routeUrls.BASE_ROUTE.HR(routeUrls.HR.JOB_POSTING.INDEX))
	}

	return (
		<Paper sx={{ py: 2, px: 3, mt: 2 }}>
			<Stack direction='row' alignItems='center' spacing={2} mb={3}>
				<Button
					startIcon={<ArrowBack />}
					onClick={handleBack}
					variant='outlined'
					disabled={jobPostingSubmit.loading}
				>
					{t('button.back')}
				</Button>
				<Typography variant='h5' fontWeight='bold'>
					{headerTitle}
				</Typography>
			</Stack>
			<JobPostingUpsertFormSection
				detailLoading={getJobPosting.loading}
				onSubmit={handleSubmit}
				formData={formData}
				errors={errors}
				onInputChange={handleInputChange}
				onDescriptionChange={(html) => handleInputChange('description', html)}
				onContentImageUpload={handleContentImageUpload}
				submitButtonLabel={submitButtonLabel}
				isFormDisabled={isFormDisabled}
				onBack={handleBack}
				isSubmitting={jobPostingSubmit.loading}
			/>
		</Paper>
	)
}

export default JobPostingUpsertPage
