import { GenericTablePagination } from '@/components/generals/GenericPagination'
import { ApiUrls } from '@/configs/apiUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useConfirm from '@/hooks/useConfirm'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import ShiftFilterSection from '@/pages/hrs/shiftManagementPage/sections/ShiftFilterSection'
import ShiftManagementFormDialog from '@/pages/hrs/shiftManagementPage/sections/ShiftManagementFormDialog'
import ShiftManagementTableSection from '@/pages/hrs/shiftManagementPage/sections/ShiftManagementTableSection'
import { formatTimeToHourMinute } from '@/utils/formatDateUtil'
import { Paper, Stack, Typography } from '@mui/material'
import { useState } from 'react'

const ShiftManagementPage = () => {
	const { t } = useTranslation()
	const confirm = useConfirm()

	const [filters, setFilters] = useState()
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)
	const [sort, setSort] = useState({ key: 'id', direction: 'desc' })
	const [selectedShift, setSelectedShift] = useState(null)
	const [selectedIds, setSelectedIds] = useState([])
	const [openCreateDialog, setOpenCreateDialog] = useState(false)
	const [openUpdateDialog, setOpenUpdateDialog] = useState(false)

	const getShifts = useFetch(
		ApiUrls.SHIFT.MANAGEMENT.INDEX,
		{ ...filters, page, pageSize, sort: `${sort.key} ${sort.direction}` },
		[filters, page, pageSize, sort]
	)

	const createShift = useAxiosSubmit({
		url: ApiUrls.SHIFT.MANAGEMENT.INDEX,
		method: 'POST',
	})

	const updateShift = useAxiosSubmit({
		method: 'PUT',
	})

	const deleteShift = useAxiosSubmit({
		method: 'DELETE',
	})

	const handleCreateShift = async ({ values, closeDialog }) => {
		const response = await createShift.submit({ overrideData: values })
		if (!response) return

		closeDialog()
		await getShifts.fetch()
	}

	const handleUpdateShift = async ({ values, closeDialog }) => {
		if (!selectedShift?.id) return

		const response = await updateShift.submit({
			overrideUrl: ApiUrls.SHIFT.MANAGEMENT.DETAIL(selectedShift.id),
			overrideData: values,
		})
		if (!response) return

		closeDialog()
		setSelectedShift(null)
		await getShifts.fetch()
	}

	const handleDeleteShift = async (shift) => {
		const isConfirmed = await confirm({
			confirmText: t('button.delete'),
			confirmColor: 'error',
			title: t('shift.dialog.delete_title'),
			description: t('shift.dialog.delete_description', { name: shift.name }),
		})

		if (!isConfirmed) return

		const response = await deleteShift.submit({
			overrideUrl: ApiUrls.SHIFT.MANAGEMENT.DETAIL(shift.id),
		})
		if (response) {
			await getShifts.fetch()
		}
	}

	const handleDeleteSelected = async () => {
		if (!selectedIds.length) return

		const isConfirmed = await confirm({
			confirmText: t('button.delete'),
			confirmColor: 'error',
			title: t('shift.dialog.delete_title'),
			description: t('text.confirm_delete'),
		})

		if (!isConfirmed) return

		const response = await deleteShift.submit({
			overrideUrl: ApiUrls.SHIFT.MANAGEMENT.DELETE_SELECTED,
			overrideParam: { ids: selectedIds },
		})
		if (response) {
			setSelectedIds([])
			await getShifts.fetch()
		}
	}

	return (
		<Paper sx={{ p: 2 }}>
			<Stack spacing={2}>
				<Typography variant='h5'>{t('shift.title.management')}</Typography>

				<ShiftFilterSection filters={filters} setFilters={setFilters} loading={getShifts.loading} />

				<ShiftManagementTableSection
					shifts={getShifts.data?.collection || []}
					loading={getShifts.loading}
					sort={sort}
					setSort={setSort}
					selectedRows={selectedIds}
					setSelectedRows={setSelectedIds}
					onCreate={() => setOpenCreateDialog(true)}
					onEdit={(shift) => {
						setSelectedShift({
							...shift,
							startTime: formatTimeToHourMinute(shift.startTime),
							endTime: formatTimeToHourMinute(shift.endTime),
						})
						setOpenUpdateDialog(true)
					}}
					onDelete={handleDeleteShift}
					onDeleteSelected={handleDeleteSelected}
				/>

				<Stack justifyContent='center' px={2}>
					<GenericTablePagination
						totalPage={getShifts.data?.totalPage}
						page={page}
						setPage={setPage}
						pageSize={pageSize}
						setPageSize={setPageSize}
						pageSizeOptions={[5, 10, 20]}
						loading={getShifts.loading}
					/>
				</Stack>
			</Stack>

			<ShiftManagementFormDialog
				title={t('shift.title.create')}
				open={openCreateDialog}
				onClose={() => setOpenCreateDialog(false)}
				submitLabel={t('button.create')}
				submitButtonColor='success'
				onSubmit={handleCreateShift}
			/>

			<ShiftManagementFormDialog
				title={t('shift.title.edit')}
				open={openUpdateDialog}
				onClose={() => {
					setOpenUpdateDialog(false)
					setSelectedShift(null)
				}}
				initialValues={selectedShift || {}}
				submitLabel={t('button.save')}
				submitButtonColor='info'
				onSubmit={handleUpdateShift}
			/>
		</Paper>
	)
}

export default ShiftManagementPage
