import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import ActionMenu from '@/components/generals/ActionMenu'
import { GenericTablePagination } from '@/components/generals/GenericPagination'
import GenericTable from '@/components/tables/GenericTable'
import { ApiUrls } from '@/configs/apiUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useFetch from '@/hooks/useFetch'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import { setOptionsStore } from '@/redux/reducers/managementReducer'
import { Button, Chip, Paper, Stack, Typography } from '@mui/material'
import { useMemo, useState } from 'react'

import OptionValueManagementFilterSection from './sections/OptionValueManagementFilterSection'

const OptionValueManagementPage = () => {
	const { t } = useTranslation()
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)
	const [filters, setFilters] = useState({ name: '', isActive: '', optionId: '' })
	const [sort, setSort] = useState({ key: 'id', direction: 'desc' })
	const [selected, setSelected] = useState(null)
	const [openCreate, setOpenCreate] = useState(false)
	const [openUpdate, setOpenUpdate] = useState(false)

	const fetchParams = useMemo(
		() => ({ page, pageSize, sort: `${sort.key} ${sort.direction}`, ...filters }),
		[page, pageSize, sort, filters],
	)
	const getOptionValues = useFetch(ApiUrls.OPTION_VALUE.MANAGEMENT.INDEX, fetchParams, [fetchParams])

	const { data: options = [] } = useReduxStore({
		selector: (state) => state.management.options,
		setStore: setOptionsStore,
		dataToGet: (storeData) =>
			(storeData || []).map((o) => ({
				label: o.optionName,
				value: o.id,
			})),
	})

	const optionValues = getOptionValues.data?.collection || []
	const totalPage = getOptionValues.data?.totalPage || 0

	const createOptionValue = useAxiosSubmit({
		url: ApiUrls.OPTION_VALUE.MANAGEMENT.INDEX,
		method: 'POST',
		onSuccess: async () => {
			setOpenCreate(false)
			await getOptionValues.fetch()
		},
	})

	const updateOptionValue = useAxiosSubmit({
		url: ApiUrls.OPTION_VALUE.MANAGEMENT.DETAIL(selected?.id),
		method: 'PUT',
		onSuccess: async () => {
			setOpenUpdate(false)
			setSelected(null)
			await getOptionValues.fetch()
		},
	})

	const tableFields = [
		{ key: 'id', title: 'ID', width: 8, sortable: true },
		{ key: 'valueName', title: t('option_value.field.name'), width: 24, sortable: true },
		{
			key: 'unitMultiplier',
			title: t('option_value.field.unit_multiplier'),
			width: 16,
			sortable: true,
			isNumeric: true,
		},
		{
			key: 'isActive',
			title: t('option_value.field.active'),
			width: 14,
			sortable: true,
			render: (val) => {
				const label = val === false ? t('option_value.text.inactive') : t('option_value.text.active')
				const color = val === false ? 'default' : 'success'
				return <Chip size='small' color={color === 'default' ? undefined : color} label={label} />
			},
		},
		{ key: 'option.optionName', title: t('option_value.field.option'), width: 24, sortable: true },
		{
			key: 'actions',
			title: t('option_value.actions'),
			width: 16,
			render: (_, row) => (
				<ActionMenu
					actions={[
						{
							title: t('button.update'),
							onClick: () => {
								setSelected(row)
								setOpenUpdate(true)
							},
						},
					]}
				/>
			),
		},
	]

	const createFormFields = [
		{
			key: 'optionId',
			title: t('option_value.field.option'),
			type: 'select',
			options: options,
		},
		{ key: 'valueName', title: t('option_value.field.name') },
		{
			key: 'unitMultiplier',
			title: t('option_value.field.unit_multiplier'),
			type: 'number',
		},
	]

	const updateFormFields = [
		{
			key: 'optionId',
			title: t('option_value.field.option'),
			type: 'select',
			options: options,
			props: { readOnly: true },
		},
		{ key: 'valueName', title: t('option_value.field.name') },
		{
			key: 'unitMultiplier',
			title: t('option_value.field.unit_multiplier'),
			type: 'number',
		},
		{
			key: 'isActive',
			title: t('option_value.field.active'),
			type: 'checkbox',
		},
	]

	return (
		<Paper sx={{ p: 2 }}>
			<Stack spacing={2}>
				<Typography variant='h5'>{t('option_value.title.management')}</Typography>

				<OptionValueManagementFilterSection
					filters={filters}
					optionOptions={options}
					loading={getOptionValues.loading}
					onFilterClick={(newFilters) => {
						setFilters(newFilters)
						setPage(1)
					}}
					onResetFilterClick={(resetFn) => {
						const resetFilters = { name: '', isActive: '', optionId: '' }
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

				<GenericTable
					data={optionValues}
					fields={tableFields}
					rowKey='id'
					loading={getOptionValues.loading}
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
					loading={getOptionValues.loading}
				/>
			</Stack>

			<GenericFormDialog
				title={t('option_value.dialog.create_title')}
				open={openCreate}
				onClose={() => setOpenCreate(false)}
				fields={createFormFields}
				submitLabel={t('button.create')}
				submitButtonColor='primary'
				onSubmit={async ({ values, closeDialog }) => {
					const response = await createOptionValue.submit({ overrideData: values })
					if (response) closeDialog()
				}}
			/>

			<GenericFormDialog
				title={t('option_value.dialog.update_title')}
				open={openUpdate}
				onClose={() => setOpenUpdate(false)}
				fields={updateFormFields}
				initialValues={selected || {}}
				submitLabel={t('button.update')}
				submitButtonColor='info'
				onSubmit={async ({ values, closeDialog }) => {
					const response = await updateOptionValue.submit({ overrideData: values })
					if (response) closeDialog()
				}}
			/>
		</Paper>
	)
}

export default OptionValueManagementPage
