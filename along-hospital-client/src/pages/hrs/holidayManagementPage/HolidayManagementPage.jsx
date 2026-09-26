import { ApiUrls } from '@/configs/apiUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useConfirm from '@/hooks/useConfirm'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import { Paper, Typography } from '@mui/material'
import { useMemo, useState } from 'react'
import HolidayManagementFormSection from './sections/HolidayManagementFormSection'
import HolidayManagementTableSection from './sections/HolidayManagementTableSection'

const HolidayManagementPage = () => {
	const { t } = useTranslation()
	const confirm = useConfirm()

	const [sort, setSort] = useState({ key: 'id', direction: 'desc' })
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)

	const [openCreateForm, setOpenCreateForm] = useState(false)
	const [openUpdateForm, setOpenUpdateForm] = useState(false)
	const [selectedItem, setSelectedItem] = useState(null)
	const [selectedIds, setSelectedIds] = useState([])

	const sortParam = useMemo(
		() => `${sort.key ?? 'id'} ${sort.direction ?? 'desc'}`,
		[sort]
	)

	const {
		loading,
		data,
		fetch: refetch,
	} = useFetch(
		ApiUrls.HOLIDAY.MANAGEMENT.INDEX,
		{
			sort: sortParam,
			page,
			pageSize,
		},
		[sortParam, page, pageSize]
	)

	const createHoliday = useAxiosSubmit({
		url: ApiUrls.HOLIDAY.MANAGEMENT.INDEX,
		method: 'POST',
		onSuccess: () => {
			refetch()
		},
	})

	const updateHoliday = useAxiosSubmit({
		url: selectedItem?.id ? ApiUrls.HOLIDAY.MANAGEMENT.DETAIL(selectedItem.id) : undefined,
		method: 'PUT',
		onSuccess: () => {
			refetch()
		},
	})

	const deleteHoliday = useAxiosSubmit({
		method: 'DELETE',
		onSuccess: () => {
			refetch()
		},
	})

	const handleDelete = async (row) => {
		const isConfirm = await confirm({
			title: t('holiday.dialog.confirm_delete_title'),
			description: t('holiday.dialog.confirm_delete_description', {
				name: row?.name ?? '',
			}),
			confirmText: t('button.delete'),
			confirmColor: 'error',
		})

		if (isConfirm) {
			deleteHoliday.submit({
				overrideUrl: ApiUrls.HOLIDAY.MANAGEMENT.DETAIL(row.id),
			})
		}
	}

	const handleDeleteSelected = async () => {
		if (!selectedIds.length) return

		const isConfirm = await confirm({
			title: t('holiday.dialog.confirm_delete_title'),
			description: t('text.confirm_delete'),
			confirmText: t('button.delete'),
			confirmColor: 'error',
		})

		if (isConfirm) {
			await deleteHoliday.submit({
				overrideUrl: ApiUrls.HOLIDAY.MANAGEMENT.DELETE_SELECTED,
				overrideParam: { ids: selectedIds },
			})
			setSelectedIds([])
			await refetch()
		}
	}

	return (
		<Paper sx={{ p: 2, display: 'flex', flexDirection: 'column', gap: 2 }}>
			<Typography variant='h5'>{t('holiday.title.holiday_management')}</Typography>
			<HolidayManagementTableSection
				data={data?.collection || data?.items || data || []}
				loading={loading}
				totalPage={data?.totalPage}
				sort={sort}
				setSort={setSort}
				page={page}
				setPage={setPage}
				pageSize={pageSize}
				setPageSize={setPageSize}
				selectedRows={selectedIds}
				setSelectedRows={setSelectedIds}
				onCreateClick={() => setOpenCreateForm(true)}
				onUpdateClick={(row) => {
					setSelectedItem(row)
					setOpenUpdateForm(true)
				}}
				onDeleteClick={handleDelete}
				onDeleteSelectedClick={handleDeleteSelected}
			/>
			<HolidayManagementFormSection
				openCreateForm={openCreateForm}
				setOpenCreateForm={setOpenCreateForm}
				onCreateSubmit={(params) => {
					const { type, closeDialog } = params
					if (type === 'DATERANGE') {
						createHoliday.submit({
							overrideUrl: ApiUrls.HOLIDAY.MANAGEMENT.RANGE,
							overrideData: params,
						})
					} else {
						createHoliday.submit({ overrideData: params })
					}
					closeDialog()
				}}
				openUpdateForm={openUpdateForm}
				setOpenUpdateForm={setOpenUpdateForm}
				selectedItem={selectedItem}
				onUpdateSubmit={(params) => {
					const { closeDialog } = params
					if (!selectedItem?.id) return
					updateHoliday.submit({
						overrideUrl: ApiUrls.HOLIDAY.MANAGEMENT.DETAIL(selectedItem.id),
						overrideData: params,
					})
					closeDialog()
				}}
			/>
		</Paper>
	)
}

export default HolidayManagementPage
