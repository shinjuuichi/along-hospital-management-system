import ActionMenu from '@/components/generals/ActionMenu'
import { GenericTablePagination } from '@/components/generals/GenericPagination'
import GenericTable from '@/components/tables/GenericTable'
import { defaultJobApplicationStatusStyle } from '@/configs/defaultStylesConfig'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import JobApplicationFilterSection from '@/pages/hrs/jobPostingManagementPage/sections/JobApplicationFilterSection'
import { formatDateBasedOnCurrentLanguage } from '@/utils/formatDateUtil'
import { getEnumLabelByValue } from '@/utils/handleStringUtil'
import { Button, Chip, Stack } from '@mui/material'

const JobApplicationTableSection = ({
	applications = [],
	loading = false,
	sort,
	setSort,
	filters = {},
	setFilters = () => {},
	onDetail = () => {},
	onRejectAll = () => {},
	rejectAllLoading = false,
	totalPage = 1,
	page = 1,
	setPage = () => {},
	pageSize = 10,
	setPageSize = () => {},
}) => {
	const { t } = useTranslation()
	const _enum = useEnum()

	const fields = [
		{ key: 'id', title: 'ID', width: 8, sortable: true },
		{
			key: 'name',
			title: t('job_application.field.full_name'),
			width: 20,
			sortable: true,
		},
		{
			key: 'email',
			title: t('job_application.field.email'),
			width: 20,
			sortable: true,
		},
		{
			key: 'phone',
			title: t('job_application.field.phone'),
			width: 15,
			sortable: true,
		},
		{
			key: 'applyDate',
			title: t('job_application.field.applied_date'),
			width: 15,
			sortable: true,
			render: (value) => formatDateBasedOnCurrentLanguage(value),
		},
		{
			key: 'applicationStatus',
			title: t('job_application.field.status'),
			width: 12,
			sortable: true,
			render: (value) => (
				<Chip
					size='small'
					color={defaultJobApplicationStatusStyle(value)}
					label={getEnumLabelByValue(_enum.jobApplicationStatusOptions, value) || value}
				/>
			),
		},
		{
			key: 'actions',
			title: t('job_posting.field.action'),
			width: 10,
			sortable: false,
			render: (_, row) => (
				<ActionMenu
					actions={[
						{
							title: t('button.detail'),
							onClick: () => onDetail(row),
						},
					]}
				/>
			),
		},
	]

	return (
		<Stack spacing={2}>
			<JobApplicationFilterSection filters={filters} setFilters={setFilters} loading={loading} />

			<Stack direction='row' justifyContent='flex-end'>
				<Button
					variant='contained'
					color='error'
					onClick={onRejectAll}
					disabled={rejectAllLoading || applications.length === 0}
				>
					{t('job_application.button.reject_all')}
				</Button>
			</Stack>

			<GenericTable
				data={applications}
				fields={fields}
				loading={loading}
				sort={sort}
				setSort={setSort}
				rowKey='id'
			/>

			<GenericTablePagination
				totalPage={totalPage}
				page={page}
				setPage={setPage}
				pageSize={pageSize}
				setPageSize={setPageSize}
				pageSizeOptions={[5, 10, 20]}
				loading={loading}
			/>
		</Stack>
	)
}

export default JobApplicationTableSection
