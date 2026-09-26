import ActionMenu from '@/components/generals/ActionMenu'
import GenericTable from '@/components/tables/GenericTable'
import useTranslation from '@/hooks/useTranslation'
import { renderEmptyFallback } from '@/utils/handleStringUtil'
import { Button, Stack } from '@mui/material'
import { useMemo, useState } from 'react'

const TeleRoomManagementTableSection = ({
	teleRooms,
	specialties = [],
	loading,
	sort,
	setSort,
	setOpenCreate,
	setOpenUpdate,
	setSelectedRow,
	onDelete,
}) => {
	const [selectedIds, setSelectedIds] = useState([])
	const { t } = useTranslation()

	const specialtyMap = useMemo(
		() =>
			new Map((Array.isArray(specialties) ? specialties : []).map((item) => [item?.id, item?.name])),
		[specialties]
	)

	const fields = useMemo(
		() => [
			{ key: 'id', title: t('tele_room.field.id'), width: 10, sortable: true, fixedColumn: true },
			{ key: 'roomCode', title: t('tele_room.field.room_code'), width: 20, sortable: true },
			{
				key: 'roomDisplayName',
				title: t('tele_room.field.room_display_name'),
				width: 30,
				sortable: true,
			},
			{
				key: 'specialtyId',
				title: t('tele_room.field.specialty_id'),
				width: 20,
				sortable: true,
				render: (value) => renderEmptyFallback(specialtyMap.get(value) || value),
			},
			{
				key: '',
				title: '',
				width: 5,
				render: (value, row) => (
					<ActionMenu
						actions={[
							{
								title: t('button.edit'),
								onClick: () => {
									setSelectedRow(row)
									setOpenUpdate(true)
								},
							},
							{
								title: t('button.delete'),
								onClick: () => onDelete(row),
							},
						]}
					/>
				),
			},
		],
		[t, specialtyMap, setSelectedRow, setOpenUpdate, onDelete]
	)

	return (
		<>
			<Stack spacing={2} direction='row' alignItems='center' justifyContent='flex-end' ml={2} mb={1.5}>
				<Button variant='contained' color='primary' onClick={() => setOpenCreate(true)}>
					{t('button.create')}
				</Button>
			</Stack>
			<GenericTable
				data={teleRooms}
				fields={fields}
				sort={sort}
				setSort={setSort}
				rowKey='id'
				selectedRows={selectedIds}
				setSelectedRows={setSelectedIds}
				loading={loading}
			/>
		</>
	)
}

export default TeleRoomManagementTableSection
