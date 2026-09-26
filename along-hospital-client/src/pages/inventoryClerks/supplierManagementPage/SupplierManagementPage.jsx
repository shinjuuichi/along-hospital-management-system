import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import ActionMenu from '@/components/generals/ActionMenu'
import ConfirmationButton from '@/components/generals/ConfirmationButton'
import { GenericTablePagination } from '@/components/generals/GenericPagination'
import GenericTable from '@/components/tables/GenericTable'
import { ApiUrls } from '@/configs/apiUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useConfirm from '@/hooks/useConfirm'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import { maxLen } from '@/utils/validateUtil'
import { DeleteRounded, EditRounded } from '@mui/icons-material'
import { Button, Paper, Stack, Typography } from '@mui/material'
import { useCallback, useMemo, useState } from 'react'

const SupplierManagementPage = () => {
	const { t } = useTranslation()
	const confirm = useConfirm()

	const [sort, setSort] = useState({ key: 'id', direction: 'asc' })
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)
	const [selectedIds, setSelectedIds] = useState([])
	const [selectedRow, setSelectedRow] = useState({})
	const [openCreate, setOpenCreate] = useState(false)
	const [openUpdate, setOpenUpdate] = useState(false)

	const sortParam = useMemo(() => `${sort.key ?? 'id'} ${sort.direction ?? 'asc'}`, [sort])

	const {
		loading,
		data,
		fetch: refetch,
	} = useFetch(ApiUrls.SUPPLIER.MANAGEMENT.INDEX, { page, pageSize, sort: sortParam }, [
		page,
		pageSize,
		sortParam,
	])

	const totalPage = data?.totalPage ?? 1

	const postSupplier = useAxiosSubmit({
		url: ApiUrls.SUPPLIER.MANAGEMENT.INDEX,
		method: 'POST',
	})

	const putSupplier = useAxiosSubmit({
		url: ApiUrls.SUPPLIER.MANAGEMENT.DETAIL(selectedRow?.id),
		method: 'PUT',
	})
	const deleteSupplier = useAxiosSubmit({
		method: 'DELETE',
	})

	const handleDeleteSelected = useCallback(async () => {
		if (!selectedIds.length) return
		await deleteSupplier.submit({
			overrideUrl: `${ApiUrls.SUPPLIER.MANAGEMENT.INDEX}/selected`,
			overrideParam: { ids: selectedIds },
		})
		setSelectedIds([])
		await refetch()
	}, [deleteSupplier, selectedIds, refetch])

	const formFields = useMemo(
		() => [
			{ key: 'name', title: t('supplier.field.name'), validate: [maxLen(255)] },
			{ key: 'phone', title: t('supplier.field.phone'), validate: [maxLen(20)] },
			{ key: 'email', title: t('supplier.field.email'), validate: [maxLen(255)] },
			{
				key: 'address',
				title: t('supplier.field.address'),
				validate: [maxLen(255)],
				multiline: 2,
				rows: 2,
			},
			{
				key: 'note',
				title: t('supplier.field.note'),
				validate: [maxLen(500)],
				multiline: 3,
				rows: 3,
			},
		],
		[t]
	)

	const tableFields = useMemo(
		() => [
			{ key: 'id', title: t('supplier.field.id'), width: 10, sortable: true, fixedColumn: true },
			{ key: 'name', title: t('supplier.field.name'), width: 20, sortable: false },
			{ key: 'phone', title: t('supplier.field.phone'), width: 15, sortable: false },
			{ key: 'email', title: t('supplier.field.email'), width: 20, sortable: false },
			{ key: 'address', title: t('supplier.field.address'), width: 20, sortable: false },
			{ key: 'note', title: t('supplier.field.note'), width: 20, sortable: false },
			{
				key: '',
				title: '',
				width: 5,
				render: (value, row) => (
					<ActionMenu
						actions={[
							{
								title: t('button.update'),
								icon: <EditRounded fontSize='small' />,
								onClick: () => {
									setSelectedRow(row)
									setOpenUpdate(true)
								},
							},
							{
								title: t('button.delete'),
								icon: <DeleteRounded fontSize='small' />,
								onClick: async () => {
									const isConfirmed = await confirm({
										confirmText: t('button.delete'),
										confirmColor: 'error',
										title: t('supplier.dialog.confirm_delete_title'),
										description: t('supplier.dialog.confirm_delete_description', {
											name: row?.name ?? '',
										}),
									})

									if (isConfirmed) {
										await deleteSupplier.submit({
											overrideUrl: ApiUrls.SUPPLIER.MANAGEMENT.DETAIL(row.id),
										})
										setSelectedIds((prev) => prev.filter((id) => id !== row.id))
										await refetch()
									}
								},
							},
						]}
					/>
				),
			},
		],
		[confirm, deleteSupplier, refetch, t]
	)

	return (
		<Paper sx={{ p: 2 }}>
			<Stack spacing={2}>
				<Typography variant='h5'>{t('supplier.title.supplier_management')}</Typography>
				<Stack direction='row' justifyContent='flex-end' alignItems='center'>
					<Stack spacing={2} direction='row' alignItems='center'>
						<Button variant='contained' color='primary' onClick={() => setOpenCreate(true)}>
							{t('button.create')}
						</Button>
						<ConfirmationButton
							confirmButtonColor='error'
							confirmButtonText={t('button.delete')}
							confirmationTitle={t('supplier.dialog.confirm_delete_title')}
							confirmationDescription={t('text.confirm_delete')}
							onConfirm={handleDeleteSelected}
							color='error'
							variant='outlined'
							disabled={!selectedIds.length}
						>
							{t('button.delete_selected')}
						</ConfirmationButton>
					</Stack>
				</Stack>
				<GenericTable
					data={data?.collection || data?.items || data || []}
					fields={tableFields}
					sort={sort}
					setSort={setSort}
					rowKey='id'
					canSelectRows={true}
					selectedRows={selectedIds}
					setSelectedRows={setSelectedIds}
					loading={loading}
				/>
				<GenericTablePagination
					totalPage={totalPage}
					page={page}
					setPage={setPage}
					pageSize={pageSize}
					setPageSize={setPageSize}
					loading={loading}
				/>
			</Stack>
			<GenericFormDialog
				open={openCreate}
				onClose={() => setOpenCreate(false)}
				fields={formFields}
				submitLabel={t('button.create')}
				submitButtonColor='success'
				title={t('supplier.dialog.create_title')}
				onSubmit={async ({ values, closeDialog }) => {
					const ok = await postSupplier.submit({ overrideData: values })
					if (ok) {
						closeDialog()
						await refetch()
					}
				}}
			/>
			<GenericFormDialog
				open={openUpdate}
				onClose={() => setOpenUpdate(false)}
				fields={formFields}
				initialValues={selectedRow}
				submitLabel={t('button.update')}
				submitButtonColor='success'
				title={t('supplier.dialog.update_title')}
				onSubmit={async ({ values, closeDialog }) => {
					if (!selectedRow?.id) return
					const ok = await putSupplier.submit({ overrideData: values })
					if (ok) {
						closeDialog()
						setSelectedRow({})
						await refetch()
					}
				}}
			/>
		</Paper>
	)
}

export default SupplierManagementPage
