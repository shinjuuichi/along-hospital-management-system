import FilterButton from '@/components/buttons/FilterButton'
import ResetFilterButton from '@/components/buttons/ResetFilterButton'
import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import ActionMenu from '@/components/generals/ActionMenu'
import { GenericTablePagination } from '@/components/generals/GenericPagination'
import GenericTable from '@/components/tables/GenericTable'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import MedicineSkuOptionValuesField from '@/pages/pharmacists/medicineSkuManagementPage/sections/MedicineSkuOptionValuesField'
import { renderEmptyFallback } from '@/utils/handleStringUtil'
import { Button, Card, Chip, Stack, Typography } from '@mui/material'
import { useEffect, useMemo, useState } from 'react'

const MedicineSkuSection = ({
	t,
	skus = [],
	loading = false,
	sort,
	setSort,
	filters,
	setFilters,
	onResetFilters,
	optionOptions = [],
	totalPage,
	page,
	setPage,
	pageSize,
	setPageSize,
	createSku,
	updateSku,
	medicineId,
	openEditSku,
	setOpenEditSku,
	selectedSku,
	setSelectedSku,
}) => {
	const { reset, values, handleChange, setField, registerRef } = useForm(filters)

	const [openCreate, setOpenCreate] = useState(false)
	const [openUpdate, setOpenUpdate] = useState(false)

	const statusOptions = [
		{ value: 'true', label: t('medicine_sku.text.active') },
		{ value: 'false', label: t('medicine_sku.text.inactive') },
	]

	const { renderField } = useFieldRenderer(
		values,
		setField,
		handleChange,
		registerRef,
		false,
		'outlined',
		'small'
	)

	useEffect(() => {
		reset(filters)
	}, [filters, reset])

	useEffect(() => {
		if (openEditSku) {
			if (selectedSku) setOpenUpdate(true)
			else setOpenCreate(true)
		} else {
			setOpenCreate(false)
			setOpenUpdate(false)
		}
	}, [openEditSku])

	const optionValueOptions = useMemo(() => {
		return (optionOptions || []).flatMap((opt) =>
			(opt.optionValues || [])
				.filter((ov) => ov.isActive)
				.map((ov) => ({
					value: ov.id,
					label: `${ov.valueName} (${opt.optionName})`,
					optionId: ov.optionId,
				}))
		)
	}, [optionOptions])

	const filterFields = [
		{
			key: 'skuCode',
			title: t('medicine_sku.field.sku_code'),
			props: { sx: { width: 200 } },
		},
		{
			key: 'optionValueId',
			title: t('medicine_sku.field.option_values'),
			type: 'select',
			options: [{ value: '', label: t('text.all') }, ...optionValueOptions],
			props: { sx: { width: 160 } },
		},
		{
			key: 'isActive',
			title: t('medicine_sku.field.active'),
			type: 'select',
			options: [
				{ value: '', label: t('text.all') },
				{ value: 'true', label: t('medicine_sku.text.active') },
				{ value: 'false', label: t('medicine_sku.text.inactive') },
			],
			props: { sx: { width: 160 } },
		},
	]

	const tableFields = [
		{ key: 'id', title: 'ID', width: 8, sortable: true },
		{
			key: 'name',
			title: t('medicine_sku.field.name'),
			width: 16,
			sortable: true,
			render: (val) => renderEmptyFallback(val ?? '-'),
		},
		{
			key: 'isActive',
			title: t('medicine_sku.field.active'),
			width: 12,
			sortable: true,
			render: (val) => (
				<Chip
					size='small'
					color={val === false ? undefined : 'success'}
					label={val === false ? t('medicine_sku.text.inactive') : t('medicine_sku.text.active')}
				/>
			),
		},
		{ key: 'skuCode', title: t('medicine_sku.field.sku_code'), width: 16, sortable: true },
		{ key: 'price', title: t('medicine_sku.field.price'), width: 12, sortable: true },
		{
			key: 'inventory',
			title: t('medicine.field.quantity'),
			width: 12,
			render: (val) => renderEmptyFallback(val?.quantity),
		},
		{
			key: 'optionValues',
			title: t('medicine_sku.field.option_values'),
			render: (_, row) =>
				renderEmptyFallback(
					(row.skuValues || [])
						.map((sv) => sv.optionValue?.valueName)
						.filter(Boolean)
						.join(', ')
				),
		},
		{
			key: 'actions',
			title: t('medicine_sku.field.actions'),
			width: 14,
			render: (_, row) => (
				<ActionMenu
					actions={[
						{
							title: t('button.update'),
							onClick: () => {
								setSelectedSku(row)
								setOpenEditSku(true)
							},
						},
					]}
				/>
			),
		},
	]

	const createSkuFields = [
		{ key: 'name', title: t('medicine_sku.field.name'), type: 'text' },
		{ key: 'price', title: t('medicine_sku.field.price'), type: 'number' },
		{ key: 'minQuantity', title: t('medicine.field.min_quantity'), type: 'number', required: false },
		{ key: 'maxQuantity', title: t('medicine.field.max_quantity'), type: 'number', required: false },
		{
			key: 'optionValueIds',
			title: t('medicine_sku.field.option_values'),
			type: 'custom',
			render: ({ value = [], onChange }) => (
				<MedicineSkuOptionValuesField value={value} onChange={onChange} options={optionOptions} t={t} />
			),
		},
	]

	const updateSkuFields = [
		{ key: 'name', title: t('medicine_sku.field.name'), type: 'text' },
		{ key: 'price', title: t('medicine_sku.field.price'), type: 'number' },
		{
			key: 'isActive',
			title: t('medicine_sku.field.active'),
			type: 'select',
			options: statusOptions,
		},
	]

	const skuFields = openUpdate ? updateSkuFields : createSkuFields

	const skuInitialValues = useMemo(() => {
		if (openUpdate && selectedSku)
			return {
				name: selectedSku.name ?? '',
				price: selectedSku.price,
				isActive: selectedSku.isActive === false ? 'false' : 'true',
			}

		return {
			optionValueIds: [],
			minQuantity: '',
			maxQuantity: '',
		}
	}, [openUpdate, selectedSku])

	const handleSubmitSku = async ({ values, closeDialog }) => {
		if (!medicineId) return

		if (openUpdate) {
			const res = await updateSku.submit({
				overrideData: {
					name: values.name || null,
					price: values.price,
					isActive: values.isActive === 'true',
				},
			})
			if (res) {
				setOpenEditSku(false)
				setSelectedSku(null)
				closeDialog()
			}
		} else {
			const validRows = (values.optionValueIds || []).filter((row) => row.optionId && row.optionValueIds?.length > 0)
			if (validRows.length === 0) return

			const optionValueIds = validRows.flatMap((row) => row.optionValueIds || []).filter(Boolean).map(Number)

			const res = await createSku.submit({
				overrideData: {
					name: values.name || null,
					price: values.price,
					medicineId,
					isActive: true,
					optionValueIds,
					minQuantity:
						values.minQuantity !== '' && values.minQuantity != null ? values.minQuantity : null,
					maxQuantity:
						values.maxQuantity !== '' && values.maxQuantity != null ? values.maxQuantity : null,
				},
			})
			if (res) {
				setOpenEditSku(false)
				setSelectedSku(null)
				closeDialog()
			}
		}
	}

	return (
		<>
			<Card variant='outlined' sx={{ p: 2 }}>
				<Stack direction='row' justifyContent='space-between' mb={2}>
					<Typography variant='subtitle1' fontWeight={700}>
						{t('medicine_sku.title.management')}
					</Typography>
					<Button
						variant='contained'
						onClick={() => {
							setSelectedSku(null)
							setOpenEditSku(true)
						}}
					>
						{t('button.create')}
					</Button>
				</Stack>

				<Stack spacing={1.5} mb={2}>
					<Stack direction='row' spacing={2} alignItems='center'>
						{filterFields.map(renderField)}
						<FilterButton
							onFilterClick={() => {
								setFilters(values)
								setPage(1)
							}}
							loading={loading}
						/>
						<ResetFilterButton
							onResetFilterClick={() => {
								onResetFilters?.()
								setPage(1)
							}}
							loading={loading}
						/>
					</Stack>
				</Stack>

				<GenericTable
					data={skus}
					fields={tableFields}
					rowKey='id'
					sort={sort}
					setSort={setSort}
					loading={loading}
				/>

				<GenericTablePagination
					totalPage={totalPage}
					page={page}
					setPage={setPage}
					pageSize={pageSize}
					setPageSize={setPageSize}
					pageSizeOptions={[5, 10, 20]}
					loading={loading}
				/>
			</Card>

			<GenericFormDialog
				title={
					openUpdate ? t('medicine_sku.dialog.update_title') : t('medicine_sku.dialog.create_title')
				}
				open={openCreate || openUpdate}
				onClose={() => {
					setOpenEditSku(false)
					setSelectedSku(null)
				}}
				fields={skuFields}
				initialValues={skuInitialValues}
				submitLabel={openUpdate ? t('button.update') : t('button.create')}
				submitButtonColor={openUpdate ? 'info' : 'success'}
				onSubmit={handleSubmitSku}
			/>
		</>
	)
}
export default MedicineSkuSection
