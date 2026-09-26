import { ApiUrls } from '@/configs/apiUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useConfirm from '@/hooks/useConfirm'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import { Paper, Typography } from '@mui/material'
import { useState } from 'react'
import StaffCertificateTypeManagementFilterSection from './sections/StaffCertificateTypeManagementFilterSection'
import StaffCertificateTypeManagementFormSection from './sections/StaffCertificateTypeManagementFormSection'
import StaffCertificateTypeManagementTableSection from './sections/StaffCertificateTypeManagementTableSection'

const StaffCertificateTypeManagementPage = () => {
	const { t } = useTranslation()
	const confirm = useConfirm()

	const [sort, setSort] = useState({ key: 'id', direction: 'asc' })
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)
	const [filters, setFilters] = useState({})

	const [openCreateForm, setOpenCreateForm] = useState(false)
	const [openUpdateForm, setOpenUpdateForm] = useState(false)
	const [selectedItem, setSelectedItem] = useState(null)
	const [selectedIds, setSelectedIds] = useState([])

	const sortParam = `${sort.key ?? 'id'} ${sort.direction ?? 'asc'}`

	const {
		loading,
		data,
		fetch: refetch,
	} = useFetch(
		ApiUrls.STAFF_CERTIFICATE_TYPE.MANAGEMENT.INDEX,
		{
			sort: sortParam,
			page,
			pageSize,
			...filters,
		},
		[sortParam, page, pageSize, filters]
	)

	const createItem = useAxiosSubmit({
		url: ApiUrls.STAFF_CERTIFICATE_TYPE.MANAGEMENT.INDEX,
		method: 'POST',
		onSuccess: () => {
			refetch()
		},
	})

	const updateItem = useAxiosSubmit({
		url: selectedItem?.id
			? ApiUrls.STAFF_CERTIFICATE_TYPE.MANAGEMENT.DETAIL(selectedItem.id)
			: undefined,
		method: 'PUT',
		onSuccess: () => {
			refetch()
		},
	})

	const deleteItem = useAxiosSubmit({
		method: 'DELETE',
		onSuccess: () => {
			refetch()
		},
	})

	const handleDelete = async (row) => {
		const isConfirm = await confirm({
			title: t('staff_certificate_type.dialog.confirm_delete_title'),
			description: t('staff_certificate_type.dialog.confirm_delete_description', {
				name: row?.name ?? '',
			}),
			confirmText: t('button.delete'),
			confirmColor: 'error',
		})

		if (!isConfirm) return

		await deleteItem.submit({
			overrideUrl: ApiUrls.STAFF_CERTIFICATE_TYPE.MANAGEMENT.DETAIL(row.id),
		})
	}

	const handleDeleteSelected = async () => {
		if (!selectedIds.length) return

		const isConfirm = await confirm({
			title: t('staff_certificate_type.dialog.confirm_delete_title'),
			description: t('text.confirm_delete'),
			confirmText: t('button.delete'),
			confirmColor: 'error',
		})

		if (!isConfirm) return

		await deleteItem.submit({
			overrideUrl: ApiUrls.STAFF_CERTIFICATE_TYPE.MANAGEMENT.DELETE_SELECTED,
			overrideParam: { ids: selectedIds },
		})

		setSelectedIds([])
	}

	return (
		<Paper sx={{ p: 2, display: 'flex', flexDirection: 'column', gap: 2 }}>
			<Typography variant='h5'>{t('staff_certificate_type.page_title')}</Typography>
			<StaffCertificateTypeManagementFilterSection
				filters={filters}
				setFilters={setFilters}
				loading={loading}
			/>
			<StaffCertificateTypeManagementTableSection
				data={data?.collection || []}
				loading={loading}
				totalPage={data?.totalPage}
				sort={sort}
				setSort={setSort}
				page={page}
				setPage={setPage}
				pageSize={pageSize}
				setPageSize={setPageSize}
				selectedRows={selectedIds}
				setSelectedRows={setSelectedIds}
				onCreateClick={() => setOpenCreateForm(true)}
				onUpdateClick={(row) => {
					setSelectedItem(row)
					setOpenUpdateForm(true)
				}}
				onDeleteClick={handleDelete}
				onDeleteSelectedClick={handleDeleteSelected}
			/>
			<StaffCertificateTypeManagementFormSection
				openCreateForm={openCreateForm}
				setOpenCreateForm={setOpenCreateForm}
				onCreateSubmit={async ({ values, closeDialog }) => {
					const response = await createItem.submit({ overrideData: values })
					if (response) closeDialog()
				}}
				openUpdateForm={openUpdateForm}
				setOpenUpdateForm={setOpenUpdateForm}
				selectedItem={selectedItem}
				onUpdateSubmit={async ({ values, closeDialog }) => {
					if (!selectedItem?.id) return
					const response = await updateItem.submit({
						overrideUrl: ApiUrls.STAFF_CERTIFICATE_TYPE.MANAGEMENT.DETAIL(selectedItem.id),
						overrideData: values,
					})
					if (response) closeDialog()
				}}
			/>
		</Paper>
	)
}

export default StaffCertificateTypeManagementPage