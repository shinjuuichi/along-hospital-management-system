import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import ActionMenu from '@/components/generals/ActionMenu'
import { GenericTablePagination } from '@/components/generals/GenericPagination'
import GenericTable from '@/components/tables/GenericTable'
import { ApiUrls } from '@/configs/apiUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useConfirm from '@/hooks/useConfirm'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import { Button, Paper, Stack, Typography } from '@mui/material'
import { useEffect, useMemo, useState } from 'react'
import { renderEmptyFallback } from '@/utils/handleStringUtil'

import OptionManagementFilterSection from './sections/OptionManagementFilterSection'

const OptionManagementPage = () => {
	const { t } = useTranslation()
	const confirm = useConfirm()
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)
	const [filters, setFilters] = useState({ optionName: '' })
	const [sort, setSort] = useState({ key: 'id', direction: 'desc' })
	const [options, setOptions] = useState([])
	const [totalPage, setTotalPage] = useState(1)
	const [selected, setSelected] = useState(null)
	const [openCreate, setOpenCreate] = useState(false)
	const [openUpdate, setOpenUpdate] = useState(false)

	const fetchParams = useMemo(
		() => ({ page, pageSize, sort: `${sort.key} ${sort.direction}`, ...filters }),
		[page, pageSize, sort, filters],
	)
	const getOptions = useFetch(ApiUrls.OPTION.MANAGEMENT.INDEX, fetchParams, [fetchParams])

	useEffect(() => {
		if (getOptions.data) {
			const collection = getOptions.data.collection || []
			setOptions(collection.map((o) => ({ id: o.id, optionName: o.optionName || '' })))
			setTotalPage(getOptions.data.totalPage || 1)
		}
	}, [getOptions.data])

	const createOption = useAxiosSubmit({
		url: ApiUrls.OPTION.MANAGEMENT.INDEX,
		method: 'POST',
		onSuccess: async () => {
			setOpenCreate(false)
			await getOptions.fetch()
		},
	})

	const updateOption = useAxiosSubmit({
		url: ApiUrls.OPTION.MANAGEMENT.DETAIL(selected?.id),
		method: 'PUT',
		onSuccess: async () => {
			setOpenUpdate(false)
			setSelected(null)
			await getOptions.fetch()
		},
	})

	const deleteOption = useAxiosSubmit({
		method: 'DELETE',
		onSuccess: async () => {
			setSelected(null)
			await getOptions.fetch()
		},
	})

	const tableFields = [
		{ key: 'id', title: 'ID', width: 8, sortable: true },
		{
			key: 'optionName',
			title: t('option.field.name'),
			width: 40,
			sortable: true,
			render: (val) => renderEmptyFallback(val),
		},
		{
			key: 'actions',
			title: t('option.actions'),
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
						{
							title: t('button.delete'),
							onClick: async () => {
								const isConfirmed = await confirm({
									title: t('option.dialog.confirm_delete_title'),
									description: t('option.dialog.confirm_delete_description', { name: row.optionName }),
									confirmText: t('button.delete'),
									confirmColor: 'error',
								})
								if (isConfirmed) {
									await deleteOption.submit({ overrideUrl: ApiUrls.OPTION.MANAGEMENT.DETAIL(row.id) })
								}
							},
						},
					]}
				/>
			),
		},
	]

	const formFields = [{ key: 'optionName', title: t('option.field.name') }]

	return (
		<Paper sx={{ p: 2 }}>
			<Stack spacing={2}>
				<Typography variant='h5'>{t('option.title.management')}</Typography>

				<OptionManagementFilterSection
					filters={filters}
					loading={getOptions.loading}
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
					<GenericTable data={options} fields={tableFields} rowKey='id' loading={getOptions.loading} sort={sort} setSort={setSort} />
					<Stack justifyContent='center' px={2}>
						<GenericTablePagination
							totalPage={totalPage}
							page={page}
							setPage={setPage}
							pageSize={pageSize}
							setPageSize={setPageSize}
							pageSizeOptions={[5, 10, 20]}
							loading={getOptions.loading}
						/>
					</Stack>
				</Stack>
			</Stack>

			<GenericFormDialog
				title={t('option.dialog.create_title')}
				open={openCreate}
				onClose={() => setOpenCreate(false)}
				fields={formFields}
				submitLabel={t('button.create')}
				submitButtonColor='primary'
				onSubmit={async ({ values, closeDialog }) => {
					const response = await createOption.submit({ overrideData: values })
					if (response) closeDialog()
				}}
			/>

			<GenericFormDialog
				title={t('option.dialog.update_title')}
				open={openUpdate}
				onClose={() => {
					setOpenUpdate(false)
					setSelected(null)
				}}
				fields={formFields}
				initialValues={selected ? { optionName: selected.optionName || '' } : {}}
				submitLabel={t('button.update')}
				submitButtonColor='info'
				onSubmit={async ({ values, closeDialog }) => {
					const response = await updateOption.submit({ overrideData: values })
					if (response) closeDialog()
				}}
			/>
		</Paper>
	)
}

export default OptionManagementPage
