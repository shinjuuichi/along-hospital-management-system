import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import MedicineSkuOptionValuesField from '@/pages/pharmacists/medicineSkuManagementPage/sections/MedicineSkuOptionValuesField'
import { numberHigherThan } from '@/utils/validateUtil'
import { useEffect, useState } from 'react'

const MedicineSkuDialogsSection = ({
	t,
	openCreate,
	onCloseCreate,
	openUpdate,
	onCloseUpdate,
	medicineOptions = [],
	options = [],
	selected,
	createSku,
	updateSku,
}) => {
	const [submitted, setSubmitted] = useState(false)

	useEffect(() => {
		if (openCreate) setSubmitted(false)
	}, [openCreate])

	const buildCreateInitialValues = () => ({
		medicineId: '',
		name: '',
		price: '',
		minQuantity: '',
		maxQuantity: '',
		optionValueIds: [],
	})

	const buildUpdateInitialValues = (row) => {
		if (!row) return { name: '', price: '', isActive: 'true' }
		return {
			name: row.name ?? '',
			price: row.price,
			isActive: row.isActive === false ? 'false' : 'true',
			inventory: {
				quantity: String(row.inventory?.quantity ?? 0),
				minQuantity: row.inventory?.minQuantity ?? '',
				maxQuantity: row.inventory?.maxQuantity ?? '',
			},
		}
	}

	const createFields = [
		{
			key: 'medicineId',
			title: t('medicine_sku.field.medicine'),
			type: 'select-dialog',
			options: medicineOptions,
		},
		{
			key: 'name',
			title: t('medicine_sku.field.name'),
			type: 'text',
		},
		{
			key: 'price',
			title: t('medicine_sku.field.price'),
			type: 'number',
			validate: [numberHigherThan(0)],
		},
		{
			key: 'minQuantity',
			title: t('medicine.field.min_quantity'),
			type: 'number',
			required: false,
		},
		{
			key: 'maxQuantity',
			title: t('medicine.field.max_quantity'),
			type: 'number',
			required: false,
		},
		{
			key: 'optionValueIds',
			title: t('medicine_sku.field.option_values'),
			type: 'custom',
			render: ({ value = [], onChange, values }) => {
				const selectedMedicine = medicineOptions.find((m) => m.value === values.medicineId)
				const unitOptIds = selectedMedicine?.medicineUnit?.options?.map((o) => o.option?.id) || []
				const unitOptions = (options || []).filter((o) => unitOptIds.includes(o.id))

				return (
					<MedicineSkuOptionValuesField
						value={value}
						onChange={onChange}
						values={values}
						options={unitOptions}
						t={t}
						submitted={submitted}
					/>
				)
			},
		},
	]

	const updateFields = [
		{
			key: 'name',
			title: t('medicine_sku.field.name'),
			type: 'text',
		},
		{
			key: 'price',
			title: t('medicine_sku.field.price'),
			type: 'number',
			validate: [numberHigherThan(0)],
		},
		{
			key: 'isActive',
			title: t('medicine_sku.field.active'),
			type: 'select',
			options: [
				{ value: 'true', label: t('medicine_sku.text.active') },
				{ value: 'false', label: t('medicine_sku.text.inactive') },
			],
		},
		{
			key: 'inventory',
			title: '',
			type: 'object',
			direction: 'row',
			of: [
				{
					key: 'minQuantity',
					title: t('medicine.field.min_quantity'),
					type: 'number',
					required: false,
				},
				{
					key: 'maxQuantity',
					title: t('medicine.field.max_quantity'),
					type: 'number',
					required: false,
				},
			],
		},
	]

	return (
		<>
			<GenericFormDialog
				title={t('medicine_sku.dialog.create_title')}
				open={openCreate}
				onClose={onCloseCreate}
				fields={createFields}
				initialValues={buildCreateInitialValues()}
				submitLabel={t('button.create')}
				submitButtonColor='success'
				onSubmit={async ({ values, closeDialog }) => {
					setSubmitted(true)

					const rows = values.optionValueIds || []
					const hasInvalidRow = rows.some(
						(row) => row.optionId && (!row.optionValueIds || row.optionValueIds.length === 0)
					)
					if (hasInvalidRow) return

					const validRows = rows.filter((row) => row.optionId && row.optionValueIds?.length > 0)
					if (validRows.length === 0) return

					const optionValueIds = validRows
						.flatMap((row) => row.optionValueIds || [])
						.filter(Boolean)
						.map(Number)

					const payload = {
						name: values.name || null,
						price: values.price,
						medicineId: values.medicineId,
						optionValueIds,
						minQuantity:
							values.minQuantity !== '' && values.minQuantity != null ? Number(values.minQuantity) : null,
						maxQuantity:
							values.maxQuantity !== '' && values.maxQuantity != null ? Number(values.maxQuantity) : null,
					}

					const response = await createSku.submit({
						overrideData: payload,
					})

					if (response) closeDialog()
				}}
			/>

			<GenericFormDialog
				title={t('medicine_sku.dialog.update_title')}
				open={openUpdate}
				onClose={onCloseUpdate}
				fields={updateFields}
				initialValues={buildUpdateInitialValues(selected)}
				submitLabel={t('button.update')}
				submitButtonColor='info'
				onSubmit={async ({ values, closeDialog }) => {
					const payload = {
						name: values.name || null,
						price: values.price,
						isActive: values.isActive === 'true',
						minQuantity:
							values.inventory?.minQuantity !== '' && values.inventory?.minQuantity != null
								? Number(values.inventory.minQuantity)
								: null,
						maxQuantity:
							values.inventory?.maxQuantity !== '' && values.inventory?.maxQuantity != null
								? Number(values.inventory.maxQuantity)
								: null,
					}

					const response = await updateSku.submit({
						overrideData: payload,
					})

					if (response) closeDialog()
				}}
			/>
		</>
	)
}

export default MedicineSkuDialogsSection
