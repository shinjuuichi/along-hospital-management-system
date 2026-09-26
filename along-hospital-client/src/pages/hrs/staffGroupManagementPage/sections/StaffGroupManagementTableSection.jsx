import ActionMenu from '@/components/generals/ActionMenu'
import { GenericTablePagination } from '@/components/generals/GenericPagination'
import GenericTable from '@/components/tables/GenericTable'
import useTranslation from '@/hooks/useTranslation'
import { formatDatetimeStringBasedOnCurrentLanguage } from '@/utils/formatDateUtil'
import { Box, Button, Chip } from '@mui/material'

const StaffGroupManagementTableSection = ({
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
		{ key: 'name', title: t('staff_group.field.name'), width: 20, sortable: true },
		{
			key: 'staffGroupMembers',
			title: t('staff_group.field.number_of_members'),
			width: 15,
			render: (value) => (
				<Chip
					label={`${value.length} ${t('staff_group.field.members')}`}
					color='primary'
					size='small'
				/>
			),
		},
		{
			key: 'staffGroupMembers',
			title: t('staff_group.field.staff_group_members'),
			width: 40,
			render: (value) => value.map((m) => m.staff.name).join(', '),
		},
		{
			key: 'creationDate',
			title: t('staff_group.field.creation_date'),
			sortable: true,
			width: 20,
			render: (value) => formatDatetimeStringBasedOnCurrentLanguage(value),
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

export default StaffGroupManagementTableSection
