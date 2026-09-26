import ActionMenu from '@/components/generals/ActionMenu'
import { GenericTablePagination } from '@/components/generals/GenericPagination'
import GenericTable from '@/components/tables/GenericTable'
import { ApiUrls } from '@/configs/apiUrls'
import { defaultJobPostingStatusStyle } from '@/configs/defaultStylesConfig'
import { EnumConfig } from '@/configs/enumConfig'
import { routeUrls } from '@/configs/routeUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useConfirm from '@/hooks/useConfirm'
import useEnum from '@/hooks/useEnum'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import { formatDateBasedOnCurrentLanguage } from '@/utils/formatDateUtil'
import { formatCurrencyBasedOnCurrentLanguage } from '@/utils/formatNumberUtil'
import { getEnumLabelByValue, renderEmptyFallback } from '@/utils/handleStringUtil'
import { Delete, Edit, Lock, LockOpen, Visibility } from '@mui/icons-material'
import { Button, Chip, Paper, Stack, Typography } from '@mui/material'
import { useEffect, useMemo, useState } from 'react'
import { useNavigate } from 'react-router-dom'

import JobPostingManagementFilterSection from './sections/JobPostingManagementFilterSection'

const JobPostingManagementPage = () => {
	const navigate = useNavigate()
	const { t } = useTranslation()
	const confirm = useConfirm()
	const _enum = useEnum()

	const [sort, setSort] = useState({ key: 'id', direction: 'desc' })
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)
	const [selectedIds, setSelectedIds] = useState([])
	const [filters, setFilters] = useState({ status: '', employmentType: '', role: '' })

	useEffect(() => {
		setPage(1)
	}, [sort, filters])

	const { loading, data, fetch } = useFetch(
		ApiUrls.JOB_POSTING.MANAGEMENT.INDEX,
		{
			Page: page,
			PageSize: pageSize,
			Sort: `${sort.key} ${sort.direction}`,
			...filters,
		},
		[page, pageSize, sort.key, sort.direction, filters]
	)

	const jobPostings = useMemo(() => (Array.isArray(data?.collection) ? data.collection : []), [data])

	const deleteJobPosting = useAxiosSubmit({ method: 'DELETE' })
	const openJobPosting = useAxiosSubmit({ method: 'PUT' })
	const closeJobPosting = useAxiosSubmit({ method: 'PUT' })

	const handleDelete = async (row) => {
		const isConfirmed = await confirm({
			title: t('job_posting.dialog.delete_title'),
			description: t('job_posting.dialog.delete_description', { title: row.title }),
			confirmColor: 'error',
			confirmText: t('button.delete'),
		})
		if (isConfirmed) {
			await deleteJobPosting.submit({
				overrideUrl: ApiUrls.JOB_POSTING.MANAGEMENT.DETAIL(row.id),
			})
			await fetch()
		}
	}

	const handleDeleteMany = async () => {
		if (!selectedIds.length) return
		const isConfirmed = await confirm({
			title: t('job_posting.dialog.delete_many_title'),
			description: t('job_posting.dialog.delete_many_description', { number: selectedIds.length }),
			confirmColor: 'error',
			confirmText: t('button.delete'),
		})
		if (isConfirmed) {
			await deleteJobPosting.submit({
				overrideUrl: ApiUrls.JOB_POSTING.MANAGEMENT.DELETE_SELECTED,
				overrideParam: { ids: selectedIds },
			})
			setSelectedIds([])
			await fetch()
		}
	}

	const handleOpen = async (row) => {
		const isConfirmed = await confirm({
			title: t('job_posting.action.open'),
			description: t('job_posting.action.open_description'),
			confirmColor: 'success',
			confirmText: t('job_posting.action.open'),
		})
		if (isConfirmed) {
			await openJobPosting.submit({
				overrideUrl: ApiUrls.JOB_POSTING.MANAGEMENT.OPEN(row.id),
			})
			await fetch()
		}
	}

	const handleClose = async (row) => {
		const isConfirmed = await confirm({
			title: t('job_posting.action.close'),
			description: t('job_posting.action.close_description'),
			confirmColor: 'warning',
			confirmText: t('job_posting.action.close'),
		})
		if (isConfirmed) {
			await closeJobPosting.submit({
				overrideUrl: ApiUrls.JOB_POSTING.MANAGEMENT.CLOSE(row.id),
			})
			await fetch()
		}
	}

	const fields = useMemo(
		() => [
			{ key: 'title', title: t('job_posting.field.title'), width: 20, sortable: true },
			{ key: 'role', title: t('job_posting.field.role'), width: 10, sortable: true },
			{
				key: 'employmentType',
				title: t('job_posting.field.employment_type'),
				width: 15,
				sortable: false,
			},
			{
				key: 'salaryMin',
				title: t('job_posting.field.salary_min'),
				width: 12,
				sortable: true,
				render: (value) =>
					renderEmptyFallback(value ? formatCurrencyBasedOnCurrentLanguage(value) : null),
			},
			{
				key: 'closeDate',
				title: t('job_posting.field.close_date'),
				width: 12,
				sortable: true,
				render: (value) => renderEmptyFallback(formatDateBasedOnCurrentLanguage(value)),
			},
			{
				key: 'status',
				title: t('job_posting.field.status'),
				width: 10,
				sortable: true,
				render: (value) => (
					<Chip
						label={getEnumLabelByValue(_enum.jobPostingManagementStatusOptions, value) || value}
						color={defaultJobPostingStatusStyle(value)}
						size='small'
					/>
				),
			},
			{
				key: 'action',
				title: t('job_posting.field.action'),
				width: 10,
				render: (value, row) => (
					<ActionMenu
						actions={[
							row.status === EnumConfig.JobPostingStatus.Draft && {
								title: t('button.update'),
								icon: <Edit fontSize='small' />,
								onClick: () =>
									navigate(routeUrls.BASE_ROUTE.HR(routeUrls.HR.JOB_POSTING.UPDATE(row.id))),
							},
							row.status === EnumConfig.JobPostingStatus.Draft && {
								title: t('job_posting.action.open'),
								icon: <LockOpen fontSize='small' />,
								onClick: () => handleOpen(row),
							},
							row.status === EnumConfig.JobPostingStatus.Open && {
								title: t('job_posting.action.close'),
								icon: <Lock fontSize='small' />,
								onClick: () => handleClose(row),
							},
							{
								title: t('job_posting.action.view_applications'),
								icon: <Visibility fontSize='small' />,
								onClick: () => navigate(routeUrls.BASE_ROUTE.HR(routeUrls.HR.JOB_POSTING.DETAIL(row.id))),
							},
							{
								title: t('button.delete'),
								icon: <Delete color='error' fontSize='small' />,
								onClick: () => handleDelete(row),
							},
						].filter(Boolean)}
					/>
				),
			},
		],
		[navigate, t, handleDelete, handleOpen, handleClose]
	)

	return (
		<Paper sx={{ py: 1, px: 2, mt: 2 }}>
			<Stack spacing={2}>
				<Typography variant='h5'>{t('job_posting.title.management')}</Typography>

				<JobPostingManagementFilterSection
					filters={filters}
					setFilters={setFilters}
					loading={loading}
				/>

				<Stack direction='row' justifyContent='flex-end' spacing={2}>
					<Button
						variant='contained'
						color='primary'
						onClick={() => navigate(routeUrls.BASE_ROUTE.HR(routeUrls.HR.JOB_POSTING.CREATE))}
					>
						{t('button.create')}
					</Button>
					<Button
						variant='outlined'
						color='error'
						disabled={!selectedIds.length}
						onClick={handleDeleteMany}
					>
						{t('button.delete_selected')}
					</Button>
				</Stack>
				<GenericTable
					data={jobPostings}
					fields={fields}
					rowKey='id'
					sort={sort}
					setSort={setSort}
					canSelectRows={true}
					selectedRows={selectedIds}
					setSelectedRows={setSelectedIds}
					loading={loading}
				/>
				<GenericTablePagination
					totalPage={data?.totalPage || 1}
					page={page}
					setPage={setPage}
					pageSize={pageSize}
					setPageSize={setPageSize}
					loading={loading}
				/>
			</Stack>
		</Paper>
	)
}

export default JobPostingManagementPage
