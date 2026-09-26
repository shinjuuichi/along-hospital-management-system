import ActionMenu from '@/components/generals/ActionMenu'
import { GenericTablePagination } from '@/components/generals/GenericPagination'
import GenericTable from '@/components/tables/GenericTable'
import useTranslation from '@/hooks/useTranslation'
import { Box, Button } from '@mui/material'

const TimeSlotManagementTableSection = ({
	data,
	loading,
	sort,
	setSort,
	totalPage,
	page,
	setPage,
	pageSize,
	setPageSize,
	onCreateClick = () => {},
	onUpdateClick = (row) => Promise.resolve(row),
	onDeleteClick = (row) => Promise.resolve(row),
}) => {
	const { t } = useTranslation()

	const tableFields = [
		{ key: 'time', title: t('time_slot.field.time'), width: 50, sortable: true },
		{
			key: 'capacityPerDoctor',
			title: t('time_slot.field.capacity_per_doctor'),
			width: 45,
			sortable: true,
		},
		{
			key: '',
			title: '',
			width: 5,
			render: (_, row) => (
				<ActionMenu
					actions={[
						{
							title: t('button.update'),
							onClick: () => onUpdateClick(row),
						},
						{
							title: t('button.delete'),
							onClick: () => onDeleteClick(row),
						},
					]}
				/>
			),
		},
	]

	return (
		<>
			<Box>
				<Button variant='contained' sx={{ float: 'right' }} onClick={() => onCreateClick()}>
					{t('button.create')}
				</Button>
			</Box>
			<GenericTable
				fields={tableFields}
				data={data}
				rowKey='id'
				loading={loading}
				sort={sort}
				setSort={setSort}
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
		</>
	)
}

export default TimeSlotManagementTableSection
