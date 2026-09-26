import ActionMenu from '@/components/generals/ActionMenu'
import ConfirmationButton from '@/components/generals/ConfirmationButton'
import { GenericTablePagination } from '@/components/generals/GenericPagination'
import GenericTable from '@/components/tables/GenericTable'
import useTranslation from '@/hooks/useTranslation'
import { Box, Button, Stack } from '@mui/material'

const StaffCertificateTypeManagementTableSection = ({
	data,
	loading,
	sort,
	setSort,
	totalPage,
	page,
	setPage,
	pageSize,
	setPageSize,
	selectedRows = [],
	setSelectedRows = () => {},
	onCreateClick = () => {},
	onUpdateClick = (row) => Promise.resolve(row),
	onDeleteClick = (row) => Promise.resolve(row),
	onDeleteSelectedClick = () => {},
}) => {
	const { t } = useTranslation()

	const tableFields = [
		{ key: 'id', title: t('staff_certificate_type.table.id'), width: 10, sortable: true, fixedColumn: true },
		{ key: 'name', title: t('staff_certificate_type.table.name'), width: 40, sortable: true },
		{ key: 'scopeOfPractice', title: t('staff_certificate_type.table.scope_of_practice'), width: 40, sortable: false },
		{
			key: '',
			title: t('staff_certificate_type.table.actions'),
			width: 10,
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
				<Stack direction='row' justifyContent='flex-end' alignItems='center' spacing={2}>
					<Button variant='contained' color='primary' onClick={() => onCreateClick()}>
						{t('button.create')}
					</Button>
					<ConfirmationButton
						confirmButtonColor='error'
						confirmButtonText={t('button.delete')}
						confirmationTitle={t('staff_certificate_type.dialog.confirm_delete_title')}
						confirmationDescription={t('text.confirm_delete')}
						onConfirm={onDeleteSelectedClick}
						color='error'
						variant='outlined'
						disabled={!selectedRows.length}
					>
						{t('button.delete_selected')}
					</ConfirmationButton>
				</Stack>
			</Box>
			<GenericTable
				fields={tableFields}
				data={data}
				rowKey='id'
				loading={loading}
				sort={sort}
				setSort={setSort}
				canSelectRows={true}
				selectedRows={selectedRows}
				setSelectedRows={setSelectedRows}
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

export default StaffCertificateTypeManagementTableSection