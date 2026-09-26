import MedicineUnitManagementFilterSection from './sections/MedicineUnitManagementFilterSection'
import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import ActionMenu from '@/components/generals/ActionMenu'
import { GenericTablePagination } from '@/components/generals/GenericPagination'
import GenericTable from '@/components/tables/GenericTable'
import { ApiUrls } from '@/configs/apiUrls'
import { routeUrls } from '@/configs/routeUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useConfirm from '@/hooks/useConfirm'
import useFetch from '@/hooks/useFetch'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import { setOptionsStore } from '@/redux/reducers/managementReducer'
import { renderEmptyFallback } from '@/utils/handleStringUtil'
import { Button, Paper, Stack, Typography } from '@mui/material'
import { useEffect, useMemo, useState } from 'react'
import { useNavigate } from 'react-router-dom'

const MedicineUnitManagementPage = () => {
	const { t } = useTranslation()
	const confirm = useConfirm()
	const navigate = useNavigate()

	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)
	const [filters, setFilters] = useState({ optionName: '' })
	const [sort, setSort] = useState({ key: 'id', direction: 'desc' })
	const [units, setUnits] = useState([])
	const [totalPage, setTotalPage] = useState(0)
	const [selected, setSelected] = useState(null)
	const [openCreate, setOpenCreate] = useState(false)
	const [openUpdate, setOpenUpdate] = useState(false)

	const fetchParams = useMemo(
		() => ({ page, pageSize, name: filters.optionName, sort: `${sort.key} ${sort.direction}` }),
		[page, pageSize, filters, sort],
	)
	const getUnits = useFetch(ApiUrls.MEDICINE_UNIT.MANAGEMENT.INDEX, fetchParams, [fetchParams])

	const { data: options = [] } = useReduxStore({
		selector: (state) => state.management.options,
		setStore: setOptionsStore,
		dataToGet: (storeData) =>
			(storeData || []).map((o) => ({
				label: o.optionName,
				value: o.id,
			})),
	})

	useEffect(() => {
		if (getUnits.data) {
			setUnits(getUnits.data.collection || [])
			setTotalPage(getUnits.data.totalPage || 0)
		}
	}, [getUnits.data])

	const createUnit = useAxiosSubmit({
		url: ApiUrls.MEDICINE_UNIT.MANAGEMENT.INDEX,
		method: 'POST',
		onSuccess: async () => {
			setOpenCreate(false)
			await getUnits.fetch()
		},
	})

	const updateUnit = useAxiosSubmit({
		url: ApiUrls.MEDICINE_UNIT.MANAGEMENT.DETAIL(selected?.id),
		method: 'PUT',
		onSuccess: async () => {
			setOpenUpdate(false)
			setSelected(null)
			await getUnits.fetch()
		},
	})

	const deleteUnit = useAxiosSubmit({
		method: 'DELETE',
		onSuccess: async () => {
			setSelected(null)
			await getUnits.fetch()
		},
	})

	const tableFields = [
		{ key: 'id', title: 'ID', width: 8, sortable: true },
		{ key: 'name', title: t('medicine_unit.field.name'), width: 20, sortable: true },
		{ key: 'description', title: t('medicine_unit.field.description'), width: 30 },
		{
			key: 'options',
			title: t('medicine_unit.field.options'),
			render: (_, row) => {
				const optionNames = (row.options || []).map((opt) => opt.option?.optionName || '').filter(Boolean)
				return renderEmptyFallback(optionNames.length ? optionNames.join(', ') : null)
			},
		},
		{
			key: 'actions',
			title: t('medicine_unit.actions'),
			width: 16,
			render: (_, row) => (
				<ActionMenu
					actions={[
						{
							title: t('medicine_unit.detail'),
							onClick: () => {
								navigate(
									routeUrls.BASE_ROUTE.PHARMACIST(
										routeUrls.PHARMACIST.MEDICINE_UNIT_MANAGEMENT.DETAIL(row.id)
									)
								)
							},
						},
						{
							title: t('button.update'),
							onClick: () => {
								setSelected(row)
								setOpenUpdate(true)
							},
						},
						{
							title: t('button.delete'),
							onClick: async () => {
								const isConfirmed = await confirm({
									title: t('medicine_unit.dialog.confirm_delete_title'),
									description: t('medicine_unit.dialog.confirm_delete_description', { name: row.name }),
									confirmText: t('button.delete'),
									confirmColor: 'error',
								})
								if (isConfirmed) {
									await deleteUnit.submit({ overrideUrl: ApiUrls.MEDICINE_UNIT.MANAGEMENT.DETAIL(row.id) })
								}
							},
						},
					]}
				/>
			),
		},
	]

	const formFields = [
		{ key: 'name', title: t('medicine_unit.field.name'), type: 'text' },
		{ key: 'description', title: t('medicine_unit.field.description'), type: 'text', multiple: 1, required: false },
		{
			key: 'optionIds',
			title: t('medicine_unit.field.options'),
			type: 'select',
			multiple: true,
			options: options,
		},
	]

	const buildInitialValues = (row) => {
		if (!row) return { optionIds: [] }
		return {
			...row,
			optionIds: (row.options || []).map((o) => o.option?.id).filter(Boolean),
		}
	}

	return (
		<Paper sx={{ p: 2 }}>
			<Stack spacing={2}>
				<Typography variant='h5'>{t('medicine_unit.title.management')}</Typography>

				<MedicineUnitManagementFilterSection
					filters={filters}
					loading={getUnits.loading}
					onFilterClick={(newFilters) => {
						setFilters(newFilters)
						setPage(1)
					}}
					onResetFilterClick={(resetFn) => {
						const resetFilters = { optionName: '' }
						setFilters(resetFilters)
						resetFn(resetFilters)
						setPage(1)
					}}
				/>

				<Stack direction='row' justifyContent='flex-end'>
					<Button variant='contained' color='primary' onClick={() => setOpenCreate(true)}>
						{t('button.create')}
					</Button>
				</Stack>

				<Stack spacing={2}>
					<GenericTable data={units} fields={tableFields} rowKey='id' loading={getUnits.loading} sort={sort} setSort={setSort} />
					<Stack justifyContent='center' px={2}>
						<GenericTablePagination
							totalPage={totalPage}
							page={page}
							setPage={setPage}
							pageSize={pageSize}
							setPageSize={setPageSize}
							pageSizeOptions={[5, 10, 20]}
							loading={getUnits.loading}
						/>
					</Stack>
				</Stack>
			</Stack>

			<GenericFormDialog
				title={t('medicine_unit.dialog.create_title')}
				open={openCreate}
				onClose={() => setOpenCreate(false)}
				fields={formFields}
				initialValues={buildInitialValues()}
				submitLabel={t('button.create')}
				submitButtonColor='success'
				onSubmit={async ({ values, closeDialog }) => {
					const response = await createUnit.submit({ overrideData: values })
					if (response) closeDialog()
				}}
			/>

			<GenericFormDialog
				title={t('medicine_unit.dialog.update_title')}
				open={openUpdate}
				onClose={() => setOpenUpdate(false)}
				fields={formFields}
				initialValues={buildInitialValues(selected)}
				submitLabel={t('button.update')}
				submitButtonColor='info'
				onSubmit={async ({ values, closeDialog }) => {
					const response = await updateUnit.submit({ overrideData: values })
					if (response) closeDialog()
				}}
			/>
		</Paper>
	)
}

export default MedicineUnitManagementPage
