import { GenericTablePagination } from '@/components/generals/GenericPagination'
import { ApiUrls } from '@/configs/apiUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useConfirm from '@/hooks/useConfirm'
import useFetch from '@/hooks/useFetch'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import { setSpecialtiesStore } from '@/redux/reducers/managementReducer'
import { Paper, Stack, Typography } from '@mui/material'
import { useState } from 'react'
import TeleRoomManagementFilterSection from './sections/TeleRoomManagementFilterSection'
import TeleRoomManagementFormSection from './sections/TeleRoomManagementFormSection'
import TeleRoomManagementTableSection from './sections/TeleRoomManagementTableSection'

const TeleRoomManagementPage = () => {
	const [filters, setFilters] = useState({
		roomCode: '',
		roomDisplayName: '',
		specialtyId: '',
	})
	const [sort, setSort] = useState({ key: 'id', direction: 'desc' })
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)
	const [openCreate, setOpenCreate] = useState(false)
	const [openUpdate, setOpenUpdate] = useState(false)
	const [selectedRow, setSelectedRow] = useState({})

	const { t } = useTranslation()
	const confirm = useConfirm()

	const specialtiesStore = useReduxStore({
		selector: (state) => state.management.specialties,
		setStore: setSpecialtiesStore,
	})

	const getTeleRooms = useFetch(
		ApiUrls.TELE_ROOM.MANAGEMENT.INDEX,
		{ sort: `${sort.key} ${sort.direction}`, ...filters, page, pageSize },
		[sort, filters, page, pageSize]
	)

	const teleRoomPost = useAxiosSubmit({
		url: ApiUrls.TELE_ROOM.MANAGEMENT.INDEX,
		method: 'POST',
	})

	const teleRoomPut = useAxiosSubmit({
		url: ApiUrls.TELE_ROOM.MANAGEMENT.DETAIL(selectedRow?.id),
		method: 'PUT',
	})

	const teleRoomDelete = useAxiosSubmit({
		method: 'DELETE',
	})

	const handleCreate = async ({ values, closeDialog }) => {
		const response = await teleRoomPost.submit({ overrideData: values })

		if (response) {
			closeDialog()
			getTeleRooms.fetch()
		}
	}

	const handleUpdate = async ({ values, closeDialog }) => {
		const response = await teleRoomPut.submit({ overrideData: values })

		if (response) {
			closeDialog()
			getTeleRooms.fetch()
		}
	}

	const handleDelete = async (row) => {
		const isConfirmed = await confirm({
			confirmText: t('button.delete'),
			confirmColor: 'error',
			title: t('tele_room.title.delete'),
			description: `${t('tele_room.title.delete_confirm')} ${row.roomDisplayName}?`,
		})

		if (isConfirmed) {
			await teleRoomDelete.submit({
				overrideUrl: ApiUrls.TELE_ROOM.MANAGEMENT.DETAIL(row.id),
			})
			getTeleRooms.fetch()
		}
	}

	return (
		<Paper sx={{ p: 2 }}>
			<Stack spacing={2}>
				<Typography variant='h5'>{t('tele_room.title.tele_room_management')}</Typography>
				<TeleRoomManagementFilterSection
					filters={filters}
					setFilters={setFilters}
					specialties={specialtiesStore.data}
					loading={getTeleRooms.loading}
				/>
				<TeleRoomManagementTableSection
					teleRooms={getTeleRooms.data?.collection}
					specialties={specialtiesStore.data}
					loading={getTeleRooms.loading}
					sort={sort}
					setSort={setSort}
					setOpenCreate={setOpenCreate}
					setOpenUpdate={setOpenUpdate}
					setSelectedRow={setSelectedRow}
					onDelete={handleDelete}
				/>
				<GenericTablePagination
					totalPage={getTeleRooms.data?.totalPage}
					page={page}
					setPage={setPage}
					pageSize={pageSize}
					setPageSize={setPageSize}
					loading={getTeleRooms.loading}
				/>
			</Stack>
			<TeleRoomManagementFormSection
				openCreate={openCreate}
				setOpenCreate={setOpenCreate}
				openUpdate={openUpdate}
				setOpenUpdate={setOpenUpdate}
				selectedRow={selectedRow}
				specialties={specialtiesStore.data}
				onCreate={handleCreate}
				onUpdate={handleUpdate}
			/>
		</Paper>
	)
}

export default TeleRoomManagementPage
