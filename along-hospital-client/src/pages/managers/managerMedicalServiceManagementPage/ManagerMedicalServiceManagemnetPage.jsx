import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import ActionMenu from '@/components/generals/ActionMenu'
import { GenericTablePagination } from '@/components/generals/GenericPagination'
import GenericTable from '@/components/tables/GenericTable'
import { ApiUrls } from '@/configs/apiUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useEnum from '@/hooks/useEnum'
import useFetch from '@/hooks/useFetch'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import MedicalServiceFilterBarSection from '@/pages/managers/managerMedicalServiceManagementPage/section/ManagerMedicalServiceManagementFilterBarSection'
import { setSpecialtiesStore } from '@/redux/reducers/managementReducer'
import { maxLen } from '@/utils/validateUtil'
import { Button, Chip, Paper, Stack, Typography } from '@mui/material'
import { useEffect, useMemo, useState } from 'react'

const ManagerMedicalServiceManagementPage = () => {
	const { t } = useTranslation()
	const { roleOptions } = useEnum()

	const [filters, setFilters] = useState({ name: '', specialtyId: '', isActive: '' })

	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)
	const [sort, setSort] = useState({ key: 'id', direction: 'desc' })
	const [medicalServices, setMedicalServices] = useState([])
	const [totalPage, setTotalPage] = useState(1)
	const [selectedRows, setSelectedRows] = useState([])
	const [selectedMedicalService, setSelectedMedicalService] = useState(null)
	const [openCreateDialog, setOpenCreateDialog] = useState(false)
	const [openUpdateDialog, setOpenUpdateDialog] = useState(false)

	const initialValues = useMemo(
		() => ({
			name: '',
			description: '',
			price: '',
			isActive: true,
			roles: [],
			specialtyId: '',
		}),
		[]
	)

	const getAllMedicalServices = useFetch(
		ApiUrls.MEDICAL_SERVICE.MANAGEMENT.INDEX,
		{ ...filters, page, pageSize, sort: `${sort.key} ${sort.direction}` },
		[filters, page, pageSize, sort]
	)

	const { data: specialties = [] } = useReduxStore({
		selector: (state) => state.management.specialties,
		setStore: setSpecialtiesStore,
		dataToGet: (storeData) =>
			(storeData || []).map((s) => ({
				label: s.name,
				value: s.id,
			})),
	})

	useEffect(() => {
		if (getAllMedicalServices.data) {
			const data = getAllMedicalServices.data
			setMedicalServices(data.collection || [])
			setTotalPage(data.totalPage)
		}
	}, [getAllMedicalServices.data])

	const createMedicalService = useAxiosSubmit({
		url: ApiUrls.MEDICAL_SERVICE.MANAGEMENT.INDEX,
		method: 'POST',
		onSuccess: async () => {
			setOpenCreateDialog(false)
			await getAllMedicalServices.fetch()
		},
	})

	const updateMedicalService = useAxiosSubmit({
		url: ApiUrls.MEDICAL_SERVICE.MANAGEMENT.DETAIL(selectedMedicalService?.id),
		method: 'PUT',
		onSuccess: async () => {
			setOpenUpdateDialog(false)
			setSelectedMedicalService(null)
			await getAllMedicalServices.fetch()
		},
	})

	const tableFields = [
		{ key: 'id', title: 'ID', width: 10, sortable: true, fixedColumn: true },
		{ key: 'name', title: t('medical_service.field.name'), width: 15, sortable: true },
		{ key: 'code', title: t('medical_service.field.code'), width: 10, sortable: true },
		{
			key: 'price',
			title: t('medical_service.field.price'),
			width: 10,
			sortable: true,
			isNumeric: true,
		},
		{ key: 'specialtyName', title: t('medical_service.field.specialty'), width: 14 },
		{
			key: 'isActive',
			title: t('medical_service.field.table_status'),
			width: 10,
			sortable: true,
			render: (val) => (
				<Chip
					label={val ? t('medical_service.label.active') : t('medical_service.label.inactive')}
					color={val ? 'success' : 'error'}
					size='small'
				/>
			),
		},
		{
			key: 'roles',
			title: t('medical_service.field.roles'),
			width: 21,
			render: (val) => {
				const maxShow = 2
				const list = val || []
				const visible = list.slice(0, maxShow)
				const remain = list.length - maxShow

				return (
					<Stack direction='row' spacing={0.5} sx={{ flexWrap: 'wrap', maxWidth: '100%' }}>
						{visible.map((r, i) => {
							const option = roleOptions.find((opt) => opt.value === r)
							const label = option ? option.label : r?.role || r
							return <Chip key={i} label={label} size='small' variant='outlined' />
						})}
						{remain > 0 && <Chip label={`+${remain}`} size='small' />}
					</Stack>
				)
			},
		},
		{
			key: 'actions',
			title: t('medical_service.field.actions'),
			width: 10,
			render: (_, row) => (
				<ActionMenu
					actions={[
						{
							title: t('button.update'),
							onClick: () => {
								setSelectedMedicalService(row)
								setOpenUpdateDialog(true)
							},
						},
					]}
				/>
			),
		},
	]

	const formFields = [
		{ key: 'name', title: t('medical_service.field.name'), validate: [maxLen(255)] },
		{
			key: 'description',
			title: t('medical_service.field.description'),
			validate: [maxLen(1000)],
			multiple: 4,
			required: false,
		},
		{ key: 'price', title: t('medical_service.field.price'), type: 'number' },
		{
			key: 'roles',
			title: t('medical_service.field.roles'),
			type: 'select',
			multiple: true,
			options: roleOptions,
		},
		{
			key: 'specialtyId',
			title: t('medical_service.field.specialty'),
			type: 'select',
			options: specialties,
		},
		{
			key: 'isActive',
			title: t('medical_service.field.active'),
			type: 'checkbox',
		},
	]

	return (
		<Paper sx={{ p: 2 }}>
			<Stack spacing={2}>
				<Typography variant='h5'>{t('medical_service.title.management')}</Typography>

				<MedicalServiceFilterBarSection
					filters={filters}
					loading={getAllMedicalServices.loading}
					specialties={specialties || []}
					onFilterClick={(newFilters) => {
						setFilters(newFilters)
						setPage(1)
					}}
					onResetFilterClick={() => {
						setFilters({ name: '', specialtyId: '', isActive: '' })
						setPage(1)
					}}
				/>

				<Stack direction='row' justifyContent='flex-end'>
					<Button variant='contained' color='primary' onClick={() => setOpenCreateDialog(true)}>
						{t('button.create')}
					</Button>
				</Stack>

				<GenericTable
					data={medicalServices}
					fields={tableFields}
					rowKey='id'
					sort={sort}
					setSort={setSort}
					selectedRows={selectedRows}
					setSelectedRows={setSelectedRows}
					loading={getAllMedicalServices.loading}
				/>

				<GenericTablePagination
					totalPage={totalPage}
					page={page}
					setPage={setPage}
					pageSize={pageSize}
					setPageSize={setPageSize}
					pageSizeOptions={[5, 10, 20]}
					loading={getAllMedicalServices.loading}
				/>
			</Stack>

			<GenericFormDialog
				title={t('medical_service.dialog.create_title')}
				open={openCreateDialog}
				onClose={() => setOpenCreateDialog(false)}
				fields={formFields}
				initialValues={initialValues}
				submitLabel={t('button.create')}
				submitButtonColor='success'
				onSubmit={async ({ values, closeDialog }) => {
					var response = await createMedicalService.submit({ overrideData: values })
					if (response) closeDialog()
				}}
			/>

			<GenericFormDialog
				title={t('medical_service.dialog.update_title')}
				open={openUpdateDialog}
				onClose={() => setOpenUpdateDialog(false)}
				fields={formFields}
				initialValues={selectedMedicalService || {}}
				submitLabel={t('button.update')}
				submitButtonColor='info'
				onSubmit={async ({ values, closeDialog }) => {
					var response = await updateMedicalService.submit({ overrideData: values })
					if (response) closeDialog()
				}}
			/>
		</Paper>
	)
}

export default ManagerMedicalServiceManagementPage
