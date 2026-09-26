import ActionMenu from '@/components/generals/ActionMenu'
import GenericTable from '@/components/tables/GenericTable'
import { defaultBedStatusStyle } from '@/configs/defaultStylesConfig'
import { renderEmptyFallback } from '@/utils/handleStringUtil'
import { Delete, Edit } from '@mui/icons-material'
import { Chip } from '@mui/material'
import { useMemo } from 'react'

const BedManagementTableSection = ({
	beds,
	bedCategories,
	rooms,
	bedStatusOptions,
	t,
	sort,
	setSort,
	selectedIds,
	setSelectedIds,
	loading,
	handleDelete,
	handleUpdate,
}) => {
	const getBedCategoryName = (bedCategoryId) => {
		const category = bedCategories?.find((cat) => cat.id === bedCategoryId)
		return renderEmptyFallback(category?.name)
	}

	const getRoomCode = (roomId) => {
		const room = rooms?.find((r) => r.id === roomId)
		return room?.code || roomId
	}

	const getStatusLabel = (status) => {
		const option = bedStatusOptions.find((opt) => opt.value === status)
		return option?.label || status
	}

	const fields = useMemo(
		() => [
			{ key: 'id', title: t('bed.field.id'), width: 10, sortable: true, fixedColumn: true },
			{ key: 'code', title: t('bed.field.code'), width: 20, sortable: true },
			{
				key: 'status',
				title: t('bed.field.status'),
				width: 20,
				sortable: true,
				render: (value) => (
					<Chip label={getStatusLabel(value)} color={defaultBedStatusStyle(value)} size='small' />
				),
			},
			{
				key: 'bedCategoryId',
				title: t('bed.field.bed_category'),
				width: 20,
				sortable: false,
				render: (value) => getBedCategoryName(value),
			},
			{
				key: 'roomId',
				title: t('bed.field.room'),
				width: 15,
				sortable: true,
				render: (value) => getRoomCode(value),
			},
			{
				key: '',
				title: '',
				width: 5,
				render: (value, row) => (
					<ActionMenu
						actions={[
							{
								title: t('button.update'),
								icon: <Edit fontSize='small' />,
								onClick: () => handleUpdate(row),
							},
							{
								title: t('button.delete'),
								icon: <Delete color='error' fontSize='small' />,
								onClick: () => handleDelete(row),
							},
						]}
					/>
				),
			},
		],
		[
			t,
			handleDelete,
			handleUpdate,
			getStatusLabel,
			getBedCategoryName,
			getRoomCode,
			bedCategories,
			rooms,
			bedStatusOptions,
		]
	)

	return (
		<GenericTable
			data={beds}
			fields={fields}
			rowKey='id'
			sort={sort}
			setSort={setSort}
			canSelectRows={true}
			selectedRows={selectedIds}
			setSelectedRows={setSelectedIds}
			loading={loading}
		/>
	)
}

export default BedManagementTableSection
