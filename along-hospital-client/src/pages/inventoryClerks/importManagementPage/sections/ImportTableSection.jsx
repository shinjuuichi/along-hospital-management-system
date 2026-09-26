import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import ActionMenu from '@/components/generals/ActionMenu'
import { GenericTablePagination } from '@/components/generals/GenericPagination'
import GenericTable from '@/components/tables/GenericTable'
import { formatDateBasedOnCurrentLanguage } from '@/utils/formatDateUtil'
import useTranslation from '@/hooks/useTranslation'
import { EditRounded, Preview } from '@mui/icons-material'
import { Stack } from '@mui/material'
import { useCallback, useMemo, useState } from 'react'
import ImportDetailDialog from './ImportDetailDialog'
import { renderEmptyFallback } from '@/utils/handleStringUtil'

const ImportTableSection = ({
	imports,
	loading,
	sort,
	setSort,
	page,
	setPage,
	pageSize,
	setPageSize,
	totalPage,
	onUpdateSubmit,
	onSuccess,
}) => {
	const { t } = useTranslation()

	const [openEdit, setOpenEdit] = useState(false)
	const [openDetail, setOpenDetail] = useState(false)
	const [selectedImport, setSelectedImport] = useState(null)

	const editFormFields = useMemo(
		() => [
			{
				key: 'note',
				title: t('import_management.field.note'),
				type: 'text',
				multiple: 3,
			},
		],
		[t]
	)

	const tableFields = useMemo(() => {
		const supplierIdPrefix = t('import_management.text.supplier_id_prefix')
		return [
			{
				key: 'id',
				title: t('import_management.table.id'),
				width: 10,
				sortable: true,
				fixedColumn: true,
			},
			{
				key: 'importDate',
				title: t('import_management.table.date'),
				width: 15,
				sortable: true,
				render: (value) => renderEmptyFallback(formatDateBasedOnCurrentLanguage(value)),
			},
			{
				key: 'supplierName',
				title: t('import_management.table.supplier'),
				width: 20,
				sortable: false,
				render: (value, row) =>
					renderEmptyFallback(value || (row.supplierId ? `${supplierIdPrefix}${row.supplierId}` : null)),
			},
			{
				key: 'note',
				title: t('import_management.table.note'),
				width: 25,
				sortable: false,
			},
			{
				key: 'importDetails',
				title: t('import_management.table.items'),
				width: 10,
				sortable: false,
				render: (value) => (Array.isArray(value) ? value.length : 0),
			},
			{
				key: '',
				title: '',
				width: 5,
				render: (_, row) => (
					<ActionMenu
						actions={[
							{
								title: t('import_request.button.view'),
								icon: <Preview fontSize='small' />,
								onClick: () => {
									setSelectedImport(row)
									setOpenDetail(true)
								},
							},
							{
								title: t('button.update'),
								icon: <EditRounded fontSize='small' />,
								onClick: () => {
									setSelectedImport(row)
									setOpenEdit(true)
								},
							},
						]}
					/>
				),
			},
		]
	}, [t])

	const handleEditSubmit = useCallback(
		async ({ values, closeDialog }) => {
			if (!selectedImport?.id) return
			const ok = await onUpdateSubmit(selectedImport.id, values)
			if (ok) {
				closeDialog()
				setSelectedImport(null)
				await onSuccess?.()
			}
		},
		[selectedImport, onUpdateSubmit, onSuccess]
	)

	const editInitialValues = useMemo(
		() => ({
			note: selectedImport?.note ?? '',
		}),
		[selectedImport]
	)

	return (
		<Stack spacing={2}>
			<GenericTable
				data={imports}
				fields={tableFields}
				sort={sort}
				setSort={setSort}
				rowKey='id'
				loading={loading}
				sx={{ minHeight: 400 }}
			/>

			<GenericTablePagination
				totalPage={totalPage}
				page={page}
				setPage={setPage}
				pageSize={pageSize}
				setPageSize={setPageSize}
				loading={loading}
			/>

			<GenericFormDialog
				open={openEdit}
				onClose={() => {
					setOpenEdit(false)
					setSelectedImport(null)
				}}
				fields={editFormFields}
				initialValues={editInitialValues}
				submitLabel={t('button.update')}
				title={t('import_management.dialog.update.title')}
				maxWidth='sm'
				onSubmit={handleEditSubmit}
			/>

			<ImportDetailDialog
				open={openDetail}
				onClose={() => {
					setOpenDetail(false)
					setSelectedImport(null)
				}}
				importData={selectedImport}
			/>
		</Stack>
	)
}

export default ImportTableSection
