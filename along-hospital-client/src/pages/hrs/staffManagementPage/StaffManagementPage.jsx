import { GenericTablePagination } from '@/components/generals/GenericPagination'
import { ApiUrls } from '@/configs/apiUrls'
import { EnumConfig } from '@/configs/enumConfig'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useFetch from '@/hooks/useFetch'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import { setQualificationsStore, setSpecialtiesStore } from '@/redux/reducers/managementReducer'
import { Paper, Stack, Typography } from '@mui/material'
import { useEffect, useState } from 'react'
import StaffManagementFilterSection from './sections/StaffManagementFilterSection'
import StaffManagementFormSection from './sections/StaffManagementFormSection'
import StaffManagementTableSection from './sections/StaffManagementTableSection'

const StaffManagementPage = () => {
	const { t } = useTranslation()

	const [filters, setFilters] = useState({
		name: '',
		phone: '',
		email: '',
		role: '',
		status: '',
		gender: '',
		specialtyId: '',
		qualificationId: '',
	})
	const [sort, setSort] = useState({ key: 'id', direction: 'desc' })
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)
	const [openCreate, setOpenCreate] = useState(false)
	const [openUpdate, setOpenUpdate] = useState(false)
	const [selectedRow, setSelectedRow] = useState({})
	const [selectedIds, setSelectedIds] = useState([])

	const getStaffs = useFetch(
		ApiUrls.STAFF.MANAGEMENT.INDEX,
		{ sort: `${sort.key} ${sort.direction}`, ...filters, page, pageSize },
		[sort, filters, page, pageSize]
	)

	const specialtyStore = useReduxStore({
		selector: (state) => state.management.specialties,
		setStore: setSpecialtiesStore,
	})

	const qualificationStore = useReduxStore({
		selector: (state) => state.management.qualifications,
		setStore: setQualificationsStore,
	})

	const createStaff = useAxiosSubmit({ url: ApiUrls.STAFF.MANAGEMENT.CREATE, method: 'POST' })
	const updateStaff = useAxiosSubmit({
		url: ApiUrls.STAFF.MANAGEMENT.DETAIL(selectedRow.id),
		method: 'PUT',
	})
	const updateStaffStatus = useAxiosSubmit({ method: 'PUT' })

	const handleUpdateStatusSelected = async (status) => {
		if (!selectedIds.length) return

		const routeByStatus = {
			[EnumConfig.StaffStatus.Active]: ApiUrls.STAFF.MANAGEMENT.ACTIVATE,
			[EnumConfig.StaffStatus.OnLeave]: ApiUrls.STAFF.MANAGEMENT.ON_LEAVE,
			[EnumConfig.StaffStatus.Terminated]: ApiUrls.STAFF.MANAGEMENT.TERMINATE,
		}
		const overrideUrl = routeByStatus[status]
		if (!overrideUrl) return

		const response = await updateStaffStatus.submit({
			overrideUrl,
			overrideParam: { ids: selectedIds },
		})
		if (!response) return

		setSelectedIds([])
		await getStaffs.fetch()
	}

	useEffect(() => {
		const currentIds = new Set(
			(getStaffs.data?.collection || []).map((item) => String(item?.id)).filter(Boolean)
		)
		setSelectedIds((prev) => prev.filter((id) => currentIds.has(String(id))))
	}, [getStaffs.data?.collection])

	return (
		<Paper sx={{ p: 2 }}>
			<Stack spacing={2}>
				<Typography variant='h5'>{t('staff.title.staff_management')}</Typography>
				<StaffManagementFilterSection
					filters={filters}
					setFilters={setFilters}
					loading={getStaffs.loading}
					specialties={specialtyStore.data}
					qualifications={qualificationStore.data}
				/>
				<StaffManagementTableSection
					staffs={getStaffs.data?.collection}
					loading={getStaffs.loading}
					sort={sort}
					setSort={setSort}
					selectedIds={selectedIds}
					setSelectedIds={setSelectedIds}
					actionLoading={updateStaffStatus.loading}
					onActivate={() => handleUpdateStatusSelected(EnumConfig.StaffStatus.Active)}
					onOnLeave={() => handleUpdateStatusSelected(EnumConfig.StaffStatus.OnLeave)}
					onTerminate={() => handleUpdateStatusSelected(EnumConfig.StaffStatus.Terminated)}
					onCreate={() => setOpenCreate(true)}
					onEdit={(row) => {
						const processedRow = { ...row }
						Object.keys(processedRow).forEach((key) => {
							if (processedRow[key] == null || processedRow[key] === 'N/A') {
								processedRow[key] = ''
							}
						})
						setSelectedRow(processedRow)
						setOpenUpdate(true)
					}}
				/>
				<GenericTablePagination
					totalPage={getStaffs.data?.totalPage}
					page={page}
					setPage={setPage}
					pageSize={pageSize}
					setPageSize={setPageSize}
					loading={getStaffs.loading}
				/>
			</Stack>
			<StaffManagementFormSection
				openCreate={openCreate}
				setOpenCreate={setOpenCreate}
				openUpdate={openUpdate}
				setOpenUpdate={setOpenUpdate}
				selectedRow={selectedRow}
				onCreateSubmit={createStaff.submit}
				onUpdateSubmit={updateStaff.submit}
				refetch={getStaffs.fetch}
				specialties={specialtyStore.data}
				qualifications={qualificationStore.data}
			/>
		</Paper>
	)
}

export default StaffManagementPage
