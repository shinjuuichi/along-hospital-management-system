import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import ActionMenu from '@/components/generals/ActionMenu'
import GenericTable from '@/components/tables/GenericTable'
import { ApiUrls } from '@/configs/apiUrls'
import axiosConfig from '@/configs/axiosConfig'
import { defaultInterviewResultStyle } from '@/configs/defaultStylesConfig'
import { EnumConfig } from '@/configs/enumConfig'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useConfirm from '@/hooks/useConfirm'
import useEnum from '@/hooks/useEnum'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import { setInterviewTypesStore } from '@/redux/reducers/managementReducer'
import { formatDateBasedOnCurrentLanguage } from '@/utils/formatDateUtil'
import { getEnumLabelByValue, renderEmptyFallback } from '@/utils/handleStringUtil'
import { Add } from '@mui/icons-material'
import { Button, Chip, Paper, Stack, Typography } from '@mui/material'
import { useCallback, useEffect, useMemo, useState } from 'react'

const JobApplicationInterviewSection = ({
	applicationId,
	applicationStatus,
	onChanged = async () => {},
}) => {
	const { t } = useTranslation()
	const confirm = useConfirm()
	const _enum = useEnum()

	const isValidApplicationId = Number.isFinite(applicationId) && applicationId > 0
	const isApplicationPassed = applicationStatus === EnumConfig.JobApplicationStatus.Passed

	const minDateTime = useMemo(() => new Date().toISOString().slice(0, 16), [])

	const [openCreate, setOpenCreate] = useState(false)
	const [openUpdate, setOpenUpdate] = useState(false)
	const [selectedInterview, setSelectedInterview] = useState(null)
	const [interviews, setInterviews] = useState([])
	const [loading, setLoading] = useState(false)

	const allInterviewsFinal =
		interviews.length > 0 &&
		interviews.some(
			(i) =>
				i.result === EnumConfig.InterviewResult.Failed ||
				i.result === EnumConfig.InterviewResult.Cancelled
		)
	const isApplicationFinal = isApplicationPassed || allInterviewsFinal

	const handleSuccess = async () => {
		await refreshInterviews()
		await onChanged?.()
	}

	const refreshInterviews = async () => {
		const response = await axiosConfig.get(
			ApiUrls.INTERVIEW.MANAGEMENT.BY_JOB_APPLICATION(applicationId)
		)
		const data = response.data !== undefined ? response.data : response
		setInterviews(Array.isArray(data) ? data : [])
	}

	const fetchInterviews = async () => {
		setLoading(true)
		await refreshInterviews()
		setLoading(false)
	}

	useEffect(() => {
		if (!isValidApplicationId) return
		fetchInterviews()
	}, [applicationId, isValidApplicationId])

	const { data: interviewTypeOptions = [] } = useReduxStore({
		selector: (state) => state.management.interviewTypes,
		setStore: setInterviewTypesStore,
		dataToGet: (storeData) =>
			(storeData || []).map((item) => ({
				value: item.id,
				label: item.name,
			})),
	})

	const createInterview = useAxiosSubmit({
		url: ApiUrls.INTERVIEW.MANAGEMENT.INDEX,
		method: 'POST',
		onSuccess: handleSuccess,
	})

	const handleCreateSubmit = async ({ values, closeDialog }) => {
		await createInterview.submit({
			overrideData: {
				...values,
				jobApplicationId: applicationId,
				result: EnumConfig.InterviewResult.Pending,
			},
		})
		closeDialog()
	}

	const updateInterview = useAxiosSubmit({
		method: 'PUT',
		onSuccess: handleSuccess,
	})

	const canSetResult = (result) =>
		!isApplicationPassed && result === EnumConfig.InterviewResult.Pending

	const handleUpdateResult = useCallback(
		async (interview, result) => {
			const isCancelled = result === EnumConfig.InterviewResult.Cancelled
			const isConfirmed = await confirm({
				title: t('interview.dialog.confirm_result_title'),
				description: t('interview.dialog.confirm_result_description', {
					result: getEnumLabelByValue(_enum.interviewResultOptions, result) || result,
				}),
				confirmText: t('button.confirm'),
				confirmColor: isCancelled
					? 'warning'
					: result === EnumConfig.InterviewResult.Passed
						? 'success'
						: 'error',
			})
			if (!isConfirmed) return

			await updateInterview.submit({
				overrideUrl: ApiUrls.INTERVIEW.MANAGEMENT.DETAIL(interview.id),
				overrideData: { result, interviewTypeId: interview.interviewTypeId },
			})
		},
		[t, _enum.interviewResultOptions, confirm, updateInterview]
	)

	const createFields = useMemo(
		() => [
			{
				key: 'interviewDate',
				title: t('interview.field.interview_date'),
				type: 'datetime-local',
				minValue: minDateTime,
			},
			{
				key: 'interviewTypeId',
				title: t('interview.field.interview_type'),
				type: 'select',
				options: interviewTypeOptions,
			},
			{
				key: 'note',
				title: t('interview.field.note'),
				required: false,
				multiple: 3,
			},
		],
		[t, interviewTypeOptions, minDateTime]
	)

	const updateFields = useMemo(
		() => [
			{
				key: 'interviewDate',
				title: t('interview.field.interview_date'),
				type: 'datetime-local',
				minValue: minDateTime,
			},
			{
				key: 'interviewTypeId',
				title: t('interview.field.interview_type'),
				type: 'select',
				options: interviewTypeOptions,
			},
			{
				key: 'note',
				title: t('interview.field.note'),
				required: false,
				multiple: 3,
			},
		],
		[t, interviewTypeOptions, minDateTime]
	)

	const dialogFields = useMemo(() => {
		let fields = [...updateFields]
		if (isApplicationFinal) {
			fields = fields.map((field) => ({
				...field,
				props: { ...field.props, disabled: true },
			}))
		}
		return fields
	}, [updateFields, isApplicationFinal])

	const tableFields = useMemo(
		() => [
			{ key: 'id', title: t('interview.field.id'), width: 8 },
			{
				key: 'interviewDate',
				title: t('interview.field.interview_date'),
				width: 20,
				render: (value) => renderEmptyFallback(formatDateBasedOnCurrentLanguage(value)),
			},
			{
				key: 'interviewTypeName',
				title: t('interview.field.interview_type'),
				width: 20,
			},
			{
				key: 'result',
				title: t('interview.field.result'),
				width: 14,
				render: (value) => (
					<Chip
						size='small'
						color={defaultInterviewResultStyle(value)}
						label={renderEmptyFallback(getEnumLabelByValue(_enum.interviewResultOptions, value) || value)}
					/>
				),
			},
			{
				key: 'note',
				title: t('interview.field.note'),
				width: 28,
				sortable: false,
				render: (value) => renderEmptyFallback(value),
			},
			{
				key: 'actions',
				title: t('interview.text.actions'),
				width: 10,
				sortable: false,
				render: (_, row) => (
					<ActionMenu
						actions={[
							{
								title: t('button.detail'),
								onClick: () => {
									setSelectedInterview(row)
									setOpenUpdate(true)
								},
							},
							{
								title: t('interview.button.set_passed'),
								onClick: () => handleUpdateResult(row, EnumConfig.InterviewResult.Passed),
								disabled: !canSetResult(row.result),
							},
							{
								title: t('interview.button.set_failed'),
								onClick: () => handleUpdateResult(row, EnumConfig.InterviewResult.Failed),
								disabled: !canSetResult(row.result),
							},
							{
								title: t('interview.button.set_cancelled'),
								onClick: () => handleUpdateResult(row, EnumConfig.InterviewResult.Cancelled),
								disabled: !canSetResult(row.result),
							},
						]}
					/>
				),
			},
		],
		[t, _enum.interviewResultOptions, isApplicationPassed]
	)

	const handleUpdateSubmit = async ({ values, closeDialog }) => {
		if (!selectedInterview?.id) return
		await updateInterview.submit({
			overrideUrl: ApiUrls.INTERVIEW.MANAGEMENT.DETAIL(selectedInterview.id),
			overrideData: values,
		})
		closeDialog()
	}

	const initialUpdateValues = selectedInterview
		? {
				interviewDate: selectedInterview.interviewDate || '',
				interviewTypeId: selectedInterview.interviewTypeId,
				note: selectedInterview.note || '',
			}
		: {}

	return (
		<Paper variant='outlined' sx={{ p: 2 }}>
			<Stack spacing={2}>
				<Stack direction='row' justifyContent='space-between' alignItems='center'>
					<Typography variant='h6'>{t('interview.title.management')}</Typography>
					<Button
						variant='contained'
						startIcon={<Add />}
						onClick={() => setOpenCreate(true)}
						disabled={isApplicationFinal}
					>
						{t('interview.button.create')}
					</Button>
				</Stack>

				<GenericTable data={interviews} fields={tableFields} rowKey='id' loading={loading} />
			</Stack>

			<GenericFormDialog
				open={openCreate}
				onClose={() => setOpenCreate(false)}
				title={t('interview.dialog.create_title')}
				fields={createFields}
				submitLabel={t('button.create')}
				submitButtonColor='success'
				onSubmit={handleCreateSubmit}
			/>

			<GenericFormDialog
				open={openUpdate}
				onClose={() => {
					setOpenUpdate(false)
					setSelectedInterview(null)
				}}
				title={isApplicationFinal ? t('button.detail') : t('interview.dialog.update_title')}
				fields={dialogFields}
				initialValues={initialUpdateValues}
				submitLabel={t('button.update')}
				submitButtonColor='info'
				showSubmit={!isApplicationFinal}
				onSubmit={handleUpdateSubmit}
			/>
		</Paper>
	)
}

export default JobApplicationInterviewSection
