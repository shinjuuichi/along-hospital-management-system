import ActionMenu from '@/components/generals/ActionMenu'
import ConfirmationButton from '@/components/generals/ConfirmationButton'
import GenericTable from '@/components/tables/GenericTable'
import { ApiUrls } from '@/configs/apiUrls'
import { defaultRoomStatusStyle } from '@/configs/defaultStylesConfig'
import { routeUrls } from '@/configs/routeUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useConfirm from '@/hooks/useConfirm'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import RoomFormDialog from '@/pages/managers/roomManagementPage/sections/RoomFormDialog'
import { getEnumLabelByValue } from '@/utils/handleStringUtil'
import { Box, Button, Chip, Stack, Typography } from '@mui/material'
import { useState } from 'react'
import { useNavigate } from 'react-router-dom'

const RoomManagementTableSection = ({
	rooms,
	loading,
	refetch,
	selectedIds,
	setSelectedIds,
	sort,
	setSort,
}) => {
	const { t } = useTranslation()
	const _enum = useEnum()
	const confirm = useConfirm()
	const navigate = useNavigate()

	const [selectedRoom, setSelectedRoom] = useState(null)
	const [openCreateDialog, setOpenCreateDialog] = useState(false)
	const [openUpdateDialog, setOpenUpdateDialog] = useState(false)

	const createRoom = useAxiosSubmit({
		url: ApiUrls.ROOM.MANAGEMENT.INDEX,
		method: 'POST',
		onSuccess: async () => {
			setOpenCreateDialog(false)
			await refetch()
		},
	})

	const updateRoom = useAxiosSubmit({
		url: ApiUrls.ROOM.MANAGEMENT.DETAIL(selectedRoom?.id),
		method: 'PUT',
		onSuccess: async () => {
			setOpenUpdateDialog(false)
			setSelectedRoom(null)
			await refetch()
		},
	})

	const deleteRoom = useAxiosSubmit({
		method: 'DELETE',
		onSuccess: async () => {
			setSelectedRoom(null)
			await refetch()
		},
	})

	const deleteSelectedRooms = useAxiosSubmit({
		url: ApiUrls.ROOM.MANAGEMENT.DELETE_SELECTED,
		method: 'DELETE',
		onSuccess: async () => {
			setSelectedIds([])
			await refetch()
		},
	})

	const tableFields = [
		{
			key: 'code',
			title: t('room.field.room_code'),
			width: 25,
			sortable: true,
			render: (_, row) => `${row.code} - ${row.roomCategoryName}`,
		},
		{
			key: 'roles',
			title: t('room_category.field.roles'),
			width: 14,
			render: (roles) =>
				roles && roles.length > 0 ? (
					<Stack direction='row' spacing={0.5} flexWrap='wrap' useFlexGap>
						{roles.map((role) => (
							<Chip
								key={role}
								label={getEnumLabelByValue(_enum.roleOptions, role)}
								size='small'
								variant='outlined'
							/>
						))}
					</Stack>
				) : null,
		},
		{
			key: 'floor.building',
			title: `${t('room.field.building')} - ${t('room.field.floor')}`,
			width: 15,
			render: (_, row) => (
				<Stack>
					<Typography variant='body2'>{row.buildingName}</Typography>
					<Typography variant='body2'>{`${t('room.field.floor')}: ${row.floorNumber}`}</Typography>
				</Stack>
			),
		},
		{
			key: 'specialtyName',
			title: t('room.field.specialty'),
			width: 18,
			sortable: false,
		},
		{
			key: 'bedCount',
			title: t('room.field.bed_count'),
			width: 14,
			sortable: false,
		},
		{
			key: 'status',
			title: t('room.field.status'),
			width: 8,
			render: (value) => (
				<Chip
					label={t(`enum.room_status.${value?.toLowerCase()}`)}
					color={defaultRoomStatusStyle(value)}
					size='small'
				/>
			),
		},
		{
			key: '',
			title: '',
			render: (_, row) => (
				<ActionMenu
					actions={[
						{
							title: t('button.detail'),
							onClick: () => {
								navigate(routeUrls.BASE_ROUTE.MANAGER(routeUrls.MANAGER.ROOM_MANAGEMENT.DETAIL(row.id)))
							},
						},
						{
							title: t('button.update'),
							onClick: () => {
								setSelectedRoom(row)
								setOpenUpdateDialog(true)
							},
						},
						{
							title: t('button.delete'),
							onClick: async () => {
								const isConfirmed = await confirm({
									confirmText: t('button.delete'),
									confirmColor: 'error',
									title: t('room.dialog.confirm_delete_title'),
									description: t('room.dialog.confirm_delete_description', { code: row.code }),
								})
								if (isConfirmed) {
									await deleteRoom.submit({
										overrideUrl: ApiUrls.ROOM.MANAGEMENT.DETAIL(row.id),
									})
								}
							},
						},
					]}
				/>
			),
		},
	]

	return (
		<>
			<Stack spacing={2}>
				<Stack direction='row' alignItems='center' justifyContent='flex-end' spacing={1}>
					<ConfirmationButton
						confirmationTitle={t('room.dialog.confirm_delete_selected_title')}
						confirmationDescription={t('room.dialog.confirm_delete_selected_description', {
							count: selectedIds.length,
						})}
						confirmButtonText={t('button.delete')}
						confirmButtonColor='error'
						onConfirm={async () => {
							await deleteSelectedRooms.submit({
								overrideParam: { ids: selectedIds },
							})
						}}
						variant='contained'
						color='error'
						disabled={!selectedIds.length}
					>
						{t('button.delete_selected')}
					</ConfirmationButton>
					<Button variant='contained' color='primary' onClick={() => setOpenCreateDialog(true)}>
						{t('button.create')}
					</Button>
				</Stack>

				<Box overflow={'auto'}>
					<GenericTable
						data={rooms}
						fields={tableFields}
						rowKey='id'
						canSelectRows={true}
						selectedRows={selectedIds}
						setSelectedRows={setSelectedIds}
						loading={loading}
						sort={sort}
						setSort={setSort}
						stickyHeader
					/>
				</Box>
			</Stack>

			<RoomFormDialog
				title={t('room.dialog.create_title')}
				open={openCreateDialog}
				onClose={() => setOpenCreateDialog(false)}
				submitLabel={t('button.create')}
				submitButtonColor='success'
				onSubmit={async ({ values, closeDialog }) => {
					const response = await createRoom.submit({ overrideData: values })
					if (response) closeDialog()
				}}
			/>

			<RoomFormDialog
				title={t('room.dialog.update_title')}
				open={openUpdateDialog}
				onClose={() => setOpenUpdateDialog(false)}
				initialValues={selectedRoom || {}}
				submitLabel={t('button.update')}
				submitButtonColor='info'
				onSubmit={async ({ values, closeDialog }) => {
					const response = await updateRoom.submit({ overrideData: values })
					if (response) closeDialog()
				}}
			/>
		</>
	)
}

export default RoomManagementTableSection
