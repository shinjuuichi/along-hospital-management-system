import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import ActionMenu from '@/components/generals/ActionMenu'
import ConfirmationButton from '@/components/generals/ConfirmationButton'
import { GenericTablePagination } from '@/components/generals/GenericPagination'
import GenericTabs from '@/components/generals/GenericTabs'
import SearchBar from '@/components/generals/SearchBar'
import GenericTable from '@/components/tables/GenericTable'
import { ApiUrls } from '@/configs/apiUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useConfirm from '@/hooks/useConfirm'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import { renderEmptyFallback } from '@/utils/handleStringUtil'
import { maxLen, numberRange } from '@/utils/validateUtil'
import { DisabledVisible, ListAlt, Settings } from '@mui/icons-material'
import { Button, Chip, Paper, Stack, Typography } from '@mui/material'
import { useMemo, useState } from 'react'

const testData = [
	{
		id: 1,
		name: 'John Doe',
		age: 28,
		images: ['/image1.jpg', '/image2.jpg'],
		permissions: [1, 2],
		status: 'active',
		signature: '/signature.jpg',
		address: { city: 'New York', country: 'USA' },
	},
	{
		id: 2,
		name: 'Jane Smith',
		age: 34,
		images: ['/image3.jpg'],
		permissions: [2, 3],
		status: 'inactive',
		signature: '/signature2.jpg',
		address: { city: 'London', country: 'UK' },
	},
	{
		id: 3,
		name: 'Sam Brown',
		age: 22,
		images: ['/image4.jpg', '/image5.jpg', '/image6.jpg'],
		permissions: [1],
		status: 'active',
		signature: '/signature3.jpg',
		address: { city: 'Sydney', country: 'Australia' },
	},
]

const TestTable = () => {
	const [sort, setSort] = useState({ key: 'name', direction: 'asc' })
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(5)
	const [currentStatusTab, setCurrentStatusTab] = useState()
	const [selectedIds, setSelectedIds] = useState([])
	const [selectedRow, setSelectedRow] = useState({})
	const [openCreate, setOpenCreate] = useState(false)
	const [openUpdate, setOpenUpdate] = useState(false)

	const confirm = useConfirm()
	const { t } = useTranslation()
	const { loading, data } = useFetch(ApiUrls.USER.INDEX, { sort, page, pageSize }, [
		sort,
		page,
		pageSize,
	])
	const postTest = useAxiosSubmit({
		url: ApiUrls.USER.INDEX,
		method: 'POST',
	})
	const putTest = useAxiosSubmit({
		url: ApiUrls.USER.INDEX,
		method: 'PUT',
	})
	const deleteTest = useAxiosSubmit({
		url: ApiUrls.USER.INDEX + `/${selectedRow.id}`,
		method: 'DELETE',
	})

	const statusTabs = useMemo(
		() => [
			{ key: 'all', title: 'All', icon: <ListAlt /> },
			{ key: 'active', title: 'Active', icon: <Settings /> },
			{ key: 'inactive', title: 'Inactive', icon: <DisabledVisible /> },
		],
		[]
	)

	const fields = useMemo(
		() => [
			{ key: 'id', title: 'ID', width: 10, sortable: true, fixedColumn: true },
			{ key: 'name', title: 'Name', width: 30, sortable: true },
			{
				key: 'age',
				title: 'Age',
				width: 10,
				isNumeric: true,
				sortable: true,
				render: (value) => renderEmptyFallback(value ? `${value} VND` : null),
			},
			{ key: 'address.city', title: 'City', width: 20, sortable: false },
			{ key: 'address.country', title: 'Country', width: 20, sortable: false },
			{
				key: '',
				title: '',
				width: 5,
				render: (value, row) => (
					<ActionMenu
						actions={[
							{
								title: 'Edit',
								onClick: () => {
									setSelectedRow(row)
									setOpenUpdate(true)
								},
							},
							{
								title: 'Delete',
								onClick: async () => {
									const isConfirmed = await confirm({
										confirmText: 'Delete',
										confirmColor: 'error',
										title: 'Delete user?',
										description: `Are you sure you want to delete ${row.name}?`,
									})

									if (isConfirmed) {
										await deleteTest.submit()
									}
								},
							},
						]}
					/>
				),
			},
		],
		[]
	)

	const upsertField = useMemo(
		() => [
			{
				key: 'name',
				title: 'Name',
				validate: [maxLen(255)],
				required: false,
				readOnly: true,
				props: { variant: 'outlined' },
			},
			{ key: 'age', title: 'Age', type: 'number', validate: [numberRange(0, 100)] },
			{ key: 'subscribe', title: 'Subscribe to newsletter', type: 'checkbox' },
			{ key: 'images', title: 'Images', type: 'image', multiple: 5 },
			{ key: 'signature', title: 'Signature', required: false, type: 'draw', height: 200 },
			{
				key: 'address',
				title: 'Address',
				type: 'object',
				direction: 'column',
				of: [
					{ key: 'city', title: 'City', validate: [maxLen(255)] },
					{ key: 'country', title: 'Country', validate: [maxLen(255)] },
				],
			},
			{
				key: 'tags',
				title: 'Tags',
				type: 'array',
				direction: 'column',
				of: [
					{ key: 'name', title: 'Name', validate: [maxLen(255)] },
					{ key: 'value', title: 'Value', validate: [maxLen(255)] },
				],
			},
			{
				key: 'permissions',
				title: 'Permissions',
				type: 'select',
				multiple: true,
				options: [
					{ value: 1, label: 'Create', searchKey: 'create' },
					{ value: 2, label: 'Update', searchKey: 'update' },
					{ value: 3, label: 'Delete', searchKey: 'delete' },
				],
				renderOptionValue: (_, label) => <Chip label={label} size='small' />,
			},
			{
				key: 'departmentId',
				title: 'Department',
				type: 'select-dialog',
				options: [
					{ value: 1, label: 'Cardiology', searchKey: 'cardiology' },
					{ value: 2, label: 'Neurology', searchKey: 'neurology' },
					{ value: 3, label: 'Pediatrics', searchKey: 'pediatrics' },
				],
			},
			{
				key: 'status',
				title: 'Status',
				type: 'select',
				options: [
					{ label: 'Active', value: 'active' },
					{ label: 'Inactive', value: 'inactive' },
				],
				renderOption: (value, label) => (
					<span style={{ color: value === 'active' ? 'green' : 'red' }}>{label}</span>
				),
				renderOptionValue: (value) => (value === 'active' ? 'Active User' : 'Inactive User'),
			},
			{
				key: 'customSelect',
				title: 'Custom Select',
				type: 'select',
				required: false,
				options: [
					{ label: 'Option 1', value: 'opt1' },
					{ label: 'Option 2', value: 'opt2' },
				],
				renderOption: (opt) => (
					<span style={{ fontWeight: opt.value === 'opt1' ? 'bold' : 'normal' }}>{opt.label}</span>
				),
			},
		],
		[]
	)

	return (
		<Paper sx={{ py: 1, px: 2, mt: 2 }}>
			<Stack direction='column' spacing={1} my={2}>
				<Typography fontWeight={'bold'} flexGrow={1}>
					Status:
				</Typography>
				<GenericTabs
					tabs={statusTabs}
					currentTab={currentStatusTab}
					setCurrentTab={setCurrentStatusTab}
				/>
			</Stack>
			<Stack direction='row' justifyContent='space-between' alignItems='center' my={2}>
				<SearchBar widthPercent={30} />
				<Stack spacing={2} direction='row' alignItems='center'>
					<Button variant='contained' color='primary' onClick={() => setOpenCreate(true)}>
						{t('button.create')}
					</Button>
					<ConfirmationButton
						confirmButtonColor='error'
						confirmButtonText={t('button.delete')}
						confirmationTitle={t('done_care_about_this.delete_title')}
						confirmationDescription={t('done_care_about_this.delete_description', {
							number: selectedIds.length,
						})}
						onConfirm={() => alert('Delete confirmed')}
						color='error'
						variant='outlined'
					>
						{t('done_care_about_this.delete_selected', { number: selectedIds.length })}
					</ConfirmationButton>
				</Stack>
			</Stack>
			<GenericTable
				data={testData}
				fields={fields}
				sort={sort}
				setSort={setSort}
				rowKey='id'
				canSelectRows={true}
				selectedRows={selectedIds}
				setSelectedRows={setSelectedIds}
				loading={loading}
			/>
			<GenericTablePagination
				totalItems={data?.length}
				page={page}
				setPage={setPage}
				pageSize={pageSize}
				setPageSize={setPageSize}
			/>
			<GenericFormDialog
				open={openCreate}
				onClose={() => setOpenCreate(false)}
				fields={upsertField}
				initialValues={{ permissions: [] }}
				submitLabel={t('button.create')}
				submitButtonColor='success'
				title={t('button.create') + ' User'}
				onSubmit={async ({ values, closeDialog }) => {
					if (await postTest.submit({ overrideData: values })) closeDialog()
				}}
			/>
			<GenericFormDialog
				open={openUpdate}
				onClose={() => setOpenUpdate(false)}
				fields={upsertField}
				initialValues={selectedRow}
				submitLabel={t('button.update')}
				submitButtonColor='success'
				title={t('button.update') + ' User'}
				onSubmit={async ({ values, closeDialog }) => {
					await putTest.submit({ overrideData: values })
					closeDialog()
				}}
			/>
		</Paper>
	)
}

export default TestTable
