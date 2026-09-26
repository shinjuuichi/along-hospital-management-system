import { GenericTablePagination } from '@/components/generals/GenericPagination'
import { ApiUrls } from '@/configs/apiUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useConfirm from '@/hooks/useConfirm'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import SalaryAdvanceFilterSection from '@/pages/staffs/salaryAdvancePage/sections/SalaryAdvanceFilterSection'
import SalaryAdvanceFormSection from '@/pages/staffs/salaryAdvancePage/sections/SalaryAdvanceFormSection'
import SalaryAdvanceTableSection from '@/pages/staffs/salaryAdvancePage/sections/SalaryAdvanceTableSection'
import { Paper, Stack, Typography } from '@mui/material'
import { useState } from 'react'

const initialFilters = {
	status: '',
}

const initialSort = {
	key: 'id',
	direction: 'desc',
}

const SalaryAdvancePage = () => {
	const { t } = useTranslation()
	const confirm = useConfirm()

	const [filters, setFilters] = useState(initialFilters)
	const [sort, setSort] = useState(initialSort)
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)
	const [openCreate, setOpenCreate] = useState(false)
	const [openUpdate, setOpenUpdate] = useState(false)
	const [selectedRow, setSelectedRow] = useState({})

	const getMyRequests = useFetch(
		ApiUrls.SALARY_ADVANCE.MY_REQUESTS,
		{
			sort: `${sort.key} ${sort.direction}`,
			...filters,
			pageNumber: page,
			pageSize,
		},
		[sort, filters, page, pageSize]
	)

	const createSalaryAdvance = useAxiosSubmit({
		url: ApiUrls.SALARY_ADVANCE.INDEX,
		method: 'POST',
	})
	const updateSalaryAdvance = useAxiosSubmit({ method: 'PUT' })
	const cancelSalaryAdvance = useAxiosSubmit({ method: 'PUT' })

	const actionLoading =
		createSalaryAdvance.loading || updateSalaryAdvance.loading || cancelSalaryAdvance.loading

	const handleCreate = async ({ values, closeDialog }) => {
		const response = await createSalaryAdvance.submit({ overrideData: values })
		if (!response) return

		closeDialog()
		setPage(1)
		await getMyRequests.fetch()
	}

	const handleUpdate = async ({ values, closeDialog }) => {
		const response = await updateSalaryAdvance.submit({
			overrideUrl: ApiUrls.SALARY_ADVANCE.DETAIL(selectedRow?.id),
			overrideData: values,
		})
		if (!response) return

		setSelectedRow({})
		closeDialog()
		await getMyRequests.fetch()
	}

	const handleCancel = async (row) => {
		const confirmed = await confirm({
			title: t('salary_advance.confirm.cancel_title'),
			description: t('salary_advance.confirm.cancel_description'),
			confirmColor: 'error',
			confirmText: t('salary_advance.button.cancel_request'),
		})
		if (!confirmed) return

		const response = await cancelSalaryAdvance.submit({
			overrideUrl: ApiUrls.SALARY_ADVANCE.CANCEL(row.id),
			overrideData: null,
		})
		if (!response) return

		if (String(selectedRow?.id ?? '') === String(row?.id ?? '')) {
			setSelectedRow({})
			setOpenUpdate(false)
		}

		await getMyRequests.fetch()
	}

	return (
		<Paper sx={{ p: 2 }}>
			<Stack spacing={2}>
				<Typography variant='h5'>{t('salary_advance.title.salary_advance')}</Typography>
				<SalaryAdvanceFilterSection
					filters={filters}
					setFilters={(nextFilters) => {
						setFilters(nextFilters)
						setPage(1)
					}}
					loading={getMyRequests.loading}
				/>
				<SalaryAdvanceTableSection
					salaryAdvances={getMyRequests.data?.collection || []}
					loading={getMyRequests.loading}
					sort={sort}
					setSort={setSort}
					onCreate={() => setOpenCreate(true)}
					onEdit={(row) => {
						setSelectedRow(row)
						setOpenUpdate(true)
					}}
					onCancel={handleCancel}
					actionLoading={actionLoading}
				/>
				<GenericTablePagination
					totalPage={getMyRequests.data?.totalPage}
					page={page}
					setPage={setPage}
					pageSize={pageSize}
					setPageSize={setPageSize}
					loading={getMyRequests.loading}
				/>
			</Stack>
			<SalaryAdvanceFormSection
				openCreate={openCreate}
				setOpenCreate={setOpenCreate}
				openUpdate={openUpdate}
				setOpenUpdate={setOpenUpdate}
				selectedRow={selectedRow}
				onCreate={handleCreate}
				onUpdate={handleUpdate}
			/>
		</Paper>
	)
}

export default SalaryAdvancePage
