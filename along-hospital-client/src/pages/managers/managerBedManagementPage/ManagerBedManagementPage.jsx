import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import { GenericTablePagination } from '@/components/generals/GenericPagination'
import { ApiUrls } from '@/configs/apiUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useConfirm from '@/hooks/useConfirm'
import useEnum from '@/hooks/useEnum'
import useFetch from '@/hooks/useFetch'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import { setBedCategoriesStore, setRoomsStore } from '@/redux/reducers/managementReducer'
import { Paper, Stack, Typography } from '@mui/material'
import { useEffect, useMemo, useState } from 'react'
import BedFilterBarSection from './sections/BedFilterBarSection'
import BedManagementHeaderSection from './sections/BedManagementHeaderSection'
import BedManagementTableSection from './sections/BedManagementTableSection'

const ManagerBedManagementPage = () => {
	const { t } = useTranslation()
	const confirm = useConfirm()
	const { bedStatusOptions } = useEnum()

	const [sort, setSort] = useState({ key: 'id', direction: 'desc' })
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)
	const [selectedIds, setSelectedIds] = useState([])
	const [filters, setFilters] = useState({ status: '', roomId: '', bedCategoryId: '' })
	const [selectedBed, setSelectedBed] = useState(null)
	const [openCreateDialog, setOpenCreateDialog] = useState(false)
	const [openUpdateDialog, setOpenUpdateDialog] = useState(false)

	const { data: bedCategories } = useReduxStore({
		selector: (state) => state.management.bedCategories,
		setStore: setBedCategoriesStore,
	})

	const { data: rooms } = useReduxStore({
		selector: (state) => state.management.rooms,
		setStore: setRoomsStore,
	})

	const bedCategoryOptions = useMemo(() => {
		if (!Array.isArray(bedCategories)) return []
		return bedCategories.map((cat) => ({ value: cat.id, label: cat.name }))
	}, [bedCategories])

	const roomOptions = useMemo(() => {
		if (!Array.isArray(rooms)) return []
		return rooms.map((room) => ({ value: room.id, label: room.code }))
	}, [rooms])

	useEffect(() => {
		setPage(1)
	}, [filters])

	const { loading, data, fetch } = useFetch(
		ApiUrls.BED.MANAGEMENT.INDEX,
		{ sort: `${sort.key} ${sort.direction}`, ...filters, page, pageSize },
		[sort, filters, page, pageSize]
	)

	const beds = useMemo(() => (Array.isArray(data?.collection) ? data.collection : []), [data])

	const createBed = useAxiosSubmit({
		url: ApiUrls.BED.MANAGEMENT.INDEX,
		method: 'POST',
		onSuccess: async () => {
			setOpenCreateDialog(false)
			await fetch()
		},
	})

	const updateBed = useAxiosSubmit({
		method: 'PUT',
		onSuccess: async () => {
			setOpenUpdateDialog(false)
			setSelectedBed(null)
			await fetch()
		},
	})

	const deleteBed = useAxiosSubmit({ method: 'DELETE' })

	const handleDelete = async (row) => {
		const isConfirmed = await confirm({
			title: t('bed.dialog.delete_title'),
			description: t('bed.dialog.delete_description', { code: row.code }),
			confirmColor: 'error',
			confirmText: t('button.delete'),
		})
		if (isConfirmed) {
			await deleteBed.submit({
				overrideUrl: ApiUrls.BED.MANAGEMENT.DETAIL(row.id),
			})
			await fetch()
		}
	}

	const handleDeleteMany = async () => {
		if (!selectedIds.length) return
		const isConfirmed = await confirm({
			title: t('bed.dialog.delete_many_title'),
			description: t('bed.dialog.delete_many_description', { number: selectedIds.length }),
			confirmColor: 'error',
			confirmText: t('button.delete'),
		})
		if (isConfirmed) {
			await deleteBed.submit({
				overrideUrl: ApiUrls.BED.MANAGEMENT.DELETE_SELECTED,
				overrideParam: { ids: selectedIds },
			})
			setSelectedIds([])
			await fetch()
		}
	}

	const handleFilterClick = (newFilters) => {
		setFilters(newFilters)
	}

	const handleResetFilter = () => {
		const resetFilters = { status: '', roomId: '', bedCategoryId: '' }
		setFilters(resetFilters)
	}

	const createFormFields = useMemo(
		() => [
			{
				key: 'bedCategoryId',
				title: t('bed.field.bed_category'),
				type: 'select',
				options: bedCategoryOptions,
				required: true,
			},
			{
				key: 'roomId',
				title: t('bed.field.room'),
				type: 'select',
				options: roomOptions,
				required: true,
			},
		],
		[t, bedCategoryOptions, roomOptions]
	)

	const updateFormFields = useMemo(
		() => [
			{
				key: 'code',
				title: t('bed.field.code'),
				type: 'text',
				required: false,
				props: { disabled: true, readOnly: true },
			},
			{
				key: 'bedCategoryId',
				title: t('bed.field.bed_category'),
				type: 'select',
				options: bedCategoryOptions,
				required: true,
			},
			{
				key: 'status',
				title: t('bed.field.status'),
				type: 'select',
				options: bedStatusOptions,
				required: false,
			},
			{
				key: 'roomId',
				title: t('bed.field.room'),
				type: 'select',
				options: roomOptions,
				required: true,
			},
		],
		[t, bedCategoryOptions, bedStatusOptions, roomOptions]
	)

	return (
		<Paper sx={{ py: 1, px: 2, mt: 2 }}>
			<Stack spacing={2}>
				<Typography variant='h5'>{t('bed.title.management')}</Typography>

				<BedFilterBarSection
					filters={filters}
					bedCategories={bedCategoryOptions}
					roomOptions={roomOptions}
					loading={loading}
					onFilterClick={handleFilterClick}
					onResetFilterClick={handleResetFilter}
				/>

				<BedManagementHeaderSection
					t={t}
					selectedIds={selectedIds}
					handleDeleteMany={handleDeleteMany}
					handleCreate={() => setOpenCreateDialog(true)}
				/>

				<BedManagementTableSection
					beds={beds}
					bedCategories={bedCategories}
					rooms={rooms}
					bedStatusOptions={bedStatusOptions}
					t={t}
					sort={sort}
					setSort={setSort}
					selectedIds={selectedIds}
					setSelectedIds={setSelectedIds}
					loading={loading}
					handleDelete={handleDelete}
					handleUpdate={(row) => {
						setSelectedBed(row)
						setOpenUpdateDialog(true)
					}}
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

			<GenericFormDialog
				open={openCreateDialog}
				onClose={() => setOpenCreateDialog(false)}
				fields={createFormFields}
				submitLabel={t('button.create')}
				submitButtonColor='success'
				title={t('bed.dialog.create_title')}
				onSubmit={async ({ values, closeDialog }) => {
					const submitData = {
						RoomId: parseInt(values.roomId),
						BedCategoryId: parseInt(values.bedCategoryId),
					}
					const response = await createBed.submit({ overrideData: submitData })
					if (response) closeDialog()
				}}
			/>

			<GenericFormDialog
				open={openUpdateDialog}
				onClose={() => {
					setOpenUpdateDialog(false)
					setSelectedBed(null)
				}}
				fields={updateFormFields}
				initialValues={selectedBed}
				submitLabel={t('button.update')}
				submitButtonColor='info'
				title={t('bed.dialog.update_title')}
				onSubmit={async ({ values, closeDialog }) => {
					const submitData = {
						Code: values.code || null,
						Status: values.status || null,
						RoomId: parseInt(values.roomId),
						BedCategoryId: parseInt(values.bedCategoryId),
					}
					const response = await updateBed.submit({
						overrideUrl: ApiUrls.BED.MANAGEMENT.DETAIL(selectedBed.id),
						overrideData: submitData,
					})
					if (response) closeDialog()
				}}
			/>
		</Paper>
	)
}

export default ManagerBedManagementPage
