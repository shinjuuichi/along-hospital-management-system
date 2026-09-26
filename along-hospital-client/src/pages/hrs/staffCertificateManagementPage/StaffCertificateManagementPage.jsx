import { ApiUrls } from '@/configs/apiUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useConfirm from '@/hooks/useConfirm'
import useFetch from '@/hooks/useFetch'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import { setStaffCertificateTypesStore, setStaffsStore } from '@/redux/reducers/managementReducer'
import { Paper, Typography } from '@mui/material'
import { useMemo, useState } from 'react'
import StaffCertificateManagementFilterSection from './sections/StaffCertificateManagementFilterSection'
import StaffCertificateManagementFormSection from './sections/StaffCertificateManagementFormSection'
import StaffCertificateManagementTableSection from './sections/StaffCertificateManagementTableSection'

const StaffCertificateManagementPage = () => {
	const { t } = useTranslation()
	const confirm = useConfirm()

	const [filters, setFilters] = useState({})
	const [sort, setSort] = useState({ key: 'id', direction: 'desc' })
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)

	const [openCreateForm, setOpenCreateForm] = useState(false)
	const [openUpdateForm, setOpenUpdateForm] = useState(false)
	const [selectedItem, setSelectedItem] = useState(null)
	const [selectedIds, setSelectedIds] = useState([])

	const sortParam = useMemo(() => `${sort.key ?? 'id'} ${sort.direction ?? 'asc'}`, [sort])

	const {
		data,
		loading,
		fetch: refetch,
	} = useFetch(
		ApiUrls.STAFF_CERTIFICATE.MANAGEMENT.INDEX,
		{
			page,
			pageSize,
			sort: sortParam,
			...filters,
		},
		[sort, filters, page, pageSize]
	)

	const { data: staffData } = useReduxStore({
		selector: (state) => state.management.staffs,
		setStore: setStaffsStore,
	})

	const { data: certTypeData } = useReduxStore({
		selector: (state) => state.management.staffCertificateTypes,
		setStore: setStaffCertificateTypesStore,
	})

	const staffOptions = (staffData || []).map((s) => ({
		value: s.id,
		label: s.name,
	}))

	const certTypeOptions = (certTypeData || []).map((ct) => ({
		value: ct.id,
		label: ct.name,
	}))

	const createItem = useAxiosSubmit({
		url: ApiUrls.STAFF_CERTIFICATE.MANAGEMENT.INDEX,
		method: 'POST',
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

	const suspendItem = useAxiosSubmit({
		method: 'PUT',
		onSuccess: () => {
			refetch()
		},
	})

	const activateItem = useAxiosSubmit({
		method: 'PUT',
		onSuccess: () => {
			refetch()
		},
	})

	const approveItem = useAxiosSubmit({
		method: 'PUT',
		onSuccess: () => {
			refetch()
		},
	})

	const handleDelete = async (row) => {
		const isConfirm = await confirm({
			title: t('staff_certificate.dialog.confirm_delete_title'),
			description: t('staff_certificate.dialog.confirm_delete_description', {
				no: row?.certificateNo ?? '',
			}),
			confirmText: t('button.delete'),
			confirmColor: 'error',
		})

		if (!isConfirm) return

		await deleteItem.submit({
			overrideUrl: ApiUrls.STAFF_CERTIFICATE.MANAGEMENT.DETAIL(row.id),
		})
	}

	const handleDeleteSelected = async () => {
		if (!selectedIds.length) return

		const isConfirm = await confirm({
			title: t('staff_certificate.dialog.confirm_delete_title'),
			description: t('text.confirm_delete'),
			confirmText: t('button.delete'),
			confirmColor: 'error',
		})

		if (!isConfirm) return

		await deleteItem.submit({
			overrideUrl: ApiUrls.STAFF_CERTIFICATE.MANAGEMENT.DELETE_SELECTED,
			overrideParam: { ids: selectedIds },
		})

		setSelectedIds([])
	}

	const handleSuspend = async (id, reason) => {
		await suspendItem.submit({
			overrideUrl: ApiUrls.STAFF_CERTIFICATE.MANAGEMENT.SUSPEND(id),
			overrideData: reason ? { reason } : {},
		})
	}

	const handleActivate = async (id, expiredDate) => {
		await activateItem.submit({
			overrideUrl: ApiUrls.STAFF_CERTIFICATE.MANAGEMENT.ACTIVE(id),
			overrideData: expiredDate ? { expiredDate } : {},
		})
	}

	const handleApprove = async (id) => {
		await approveItem.submit({
			overrideUrl: ApiUrls.STAFF_CERTIFICATE.MANAGEMENT.APPROVE(id),
		})
	}

	return (
		<Paper sx={{ p: 2, display: 'flex', flexDirection: 'column', gap: 2 }}>
			<Typography variant='h5'>{t('staff_certificate.page_title')}</Typography>
			<StaffCertificateManagementFilterSection
				filters={filters}
				setFilters={setFilters}
				loading={loading}
				staffOptions={staffOptions}
				staffCertificateTypeOptions={certTypeOptions}
			/>
			<StaffCertificateManagementTableSection
				data={data?.collection || []}
				loading={loading}
				totalPage={data?.totalPage || 1}
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
			<StaffCertificateManagementFormSection
				openCreateForm={openCreateForm}
				setOpenCreateForm={setOpenCreateForm}
				onCreateSubmit={async ({ values, closeDialog }) => {
					const response = await createItem.submit({ overrideData: values })
					if (response) closeDialog()
				}}
				openUpdateForm={openUpdateForm}
				setOpenUpdateForm={setOpenUpdateForm}
				selectedItem={selectedItem}
				onSuspendSubmit={handleSuspend}
				onActivateSubmit={handleActivate}
				onApproveSubmit={handleApprove}
				staffOptions={staffOptions}
				staffCertificateTypeOptions={certTypeOptions}
			/>
		</Paper>
	)
}

export default StaffCertificateManagementPage
