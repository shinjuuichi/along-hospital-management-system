import { GenericTablePagination } from '@/components/generals/GenericPagination'
import { ApiUrls } from '@/configs/apiUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import SalaryAdvanceManagementFilterSection from '@/pages/accountants/salaryAdvanceManagementPage/sections/SalaryAdvanceManagementFilterSection'
import SalaryAdvanceManagementTableSection from '@/pages/accountants/salaryAdvanceManagementPage/sections/SalaryAdvanceManagementTableSection'
import { Paper, Stack, Typography } from '@mui/material'
import { useState } from 'react'

const SalaryAdvanceManagementPage = () => {
	const { t } = useTranslation()

	const [filters, setFilters] = useState({ status: '' })
	const [sort, setSort] = useState({ key: 'id', direction: 'desc' })
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)
	const [selectedIds, setSelectedIds] = useState([])

	const getSalaryAdvanceRequests = useFetch(
		ApiUrls.SALARY_ADVANCE.MANAGEMENT.INDEX,
		{
			sort: `${sort.key} ${sort.direction}`,
			...filters,
			pageNumber: page,
			pageSize,
		},
		[sort, filters, page, pageSize]
	)

	const approveSalaryAdvance = useAxiosSubmit({ method: 'PUT' })
	const rejectSalaryAdvance = useAxiosSubmit({ method: 'PUT' })

	const handleApproveSelected = async () => {
		if (!selectedIds.length) return

		const response = await approveSalaryAdvance.submit({
			overrideUrl: ApiUrls.SALARY_ADVANCE.MANAGEMENT.APPROVE,
			overrideParam: { ids: selectedIds },
		})
		if (!response) return

		setSelectedIds([])
		await getSalaryAdvanceRequests.fetch()
	}

	const handleRejectSelected = async () => {
		if (!selectedIds.length) return

		const response = await rejectSalaryAdvance.submit({
			overrideUrl: ApiUrls.SALARY_ADVANCE.MANAGEMENT.REJECT,
			overrideParam: { ids: selectedIds },
		})
		if (!response) return

		setSelectedIds([])
		await getSalaryAdvanceRequests.fetch()
	}

	return (
		<>
			<Paper sx={{ p: 2 }}>
				<Stack spacing={2}>
					<Typography variant='h5'>{t('salary_advance.title.salary_advance_management')}</Typography>
					<SalaryAdvanceManagementFilterSection
						filters={filters}
						setFilters={(nextFilters) => {
							setFilters(nextFilters)
							setPage(1)
						}}
						loading={getSalaryAdvanceRequests.loading}
					/>
					<SalaryAdvanceManagementTableSection
						salaryAdvances={getSalaryAdvanceRequests.data?.collection || []}
						loading={getSalaryAdvanceRequests.loading}
						selectedIds={selectedIds}
						setSelectedIds={setSelectedIds}
						sort={sort}
						setSort={setSort}
						onApproveSelected={handleApproveSelected}
						onRejectSelected={handleRejectSelected}
						actionLoading={approveSalaryAdvance.loading || rejectSalaryAdvance.loading}
					/>
					<GenericTablePagination
						totalPage={getSalaryAdvanceRequests.data?.totalPage}
						page={page}
						setPage={setPage}
						pageSize={pageSize}
						setPageSize={setPageSize}
						loading={getSalaryAdvanceRequests.loading}
					/>
				</Stack>
			</Paper>
		</>
	)
}

export default SalaryAdvanceManagementPage
