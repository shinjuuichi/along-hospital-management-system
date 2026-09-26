import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import { setSuppliersStore } from '@/redux/reducers/managementReducer'
import { isNumber } from '@/utils/validateUtil'
import { useMemo } from 'react'

const CreateImportFromRequestDialog = ({ open, onClose, request, onCreateImport, onSuccess }) => {
	const { t } = useTranslation()

	const { data: suppliers = [] } = useReduxStore({
		selector: (state) => state.management.suppliers,
		setStore: setSuppliersStore,
	})

	const supplierOptions = useMemo(
		() => suppliers.map((s) => ({ value: s.id, label: s.name })),
		[suppliers]
	)

	const details = useMemo(() => {
		if (!request?.details) return []
		return request.details.map((d) => ({
			skuCode: d.skuCode,
			medicineName: d.medicineName || '',
			quantity: d.requestQuantity,
		}))
	}, [request])

	const initialValues = useMemo(() => {
		return {
			importRequestId: request?.id || '',
			supplierId: '',
			note: '',
			details: details.map((d) => ({
				skuCode: d.skuCode,
				medicineName: d.medicineName,
				quantity: d.quantity,
				unitPrice: '',
			})),
		}
	}, [request, details])

	const formFields = useMemo(
		() => [
			{
				key: 'importRequestId',
				title: t('import_request.table.id'),
				type: 'text',
				required: false,
				props: { readOnly: true, variant: 'filled' },
			},
			{
				key: 'supplierId',
				title: t('import_management.field.supplier'),
				type: 'select',
				required: true,
				options: supplierOptions,
			},
			{
				key: 'note',
				title: t('import_management.field.note'),
				type: 'text',
				required: false,
				multiple: 3,
			},
			{
				key: 'details',
				title: t('import_management.field.import_details'),
				type: 'array',
				required: true,
				props: { readOnly: true },
				of: [
					{
						key: 'skuCode',
						title: t('import_request.field.sku_code'),
						type: 'text',
						required: false,
						props: { readOnly: true, variant: 'filled' },
					},
					{
						key: 'medicineName',
						title: t('medicine.field.name'),
						type: 'text',
						required: false,
						props: { readOnly: true, variant: 'filled' },
					},
					{
						key: 'quantity',
						title: t('import_management.field.quantity'),
						type: 'text',
						required: false,
						props: { readOnly: true, variant: 'filled' },
					},
					{
						key: 'unitPrice',
						title: t('import_management.field.unit_price'),
						type: 'number',
						required: true,
						validate: [isNumber()],
						props: { readOnly: false },
					},
				],
			},
		],
		[t, supplierOptions]
	)

	const handleSubmit = async ({ values, closeDialog }) => {
		if (!request?.id || !values.supplierId) return

		const payload = {
			importRequestId: request.id,
			supplierId: Number(values.supplierId),
			note: values.note || null,
			details: values.details.map((d) => ({
				skuCode: d.skuCode,
				quantity: d.quantity,
				unitPrice: Number(d.unitPrice),
			})),
		}

		const ok = await onCreateImport(payload)
		if (ok) {
			await onSuccess?.()
			closeDialog()
		}
	}

	if (!request) return null

	return (
		<GenericFormDialog
			open={open}
			onClose={onClose}
			title={t('import_management.dialog.create.title')}
			fields={formFields}
			initialValues={initialValues}
			submitLabel={t('button.create')}
			submitButtonColor='success'
			maxWidth='md'
			onSubmit={handleSubmit}
		/>
	)
}

export default CreateImportFromRequestDialog
