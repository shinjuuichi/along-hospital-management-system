import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import { EnumConfig } from '@/configs/enumConfig'
import useAuth from '@/hooks/useAuth'
import useTranslation from '@/hooks/useTranslation'
import { numberHigherThan } from '@/utils/validateUtil'
import { useCallback, useEffect, useMemo, useState } from 'react'

const OrderManagementFormSection = ({ open, onClose, medicines, onSubmit }) => {
	const { t } = useTranslation()
	const { auth } = useAuth()

	const initialValues = useMemo(
		() => ({
			paymentType: EnumConfig.PaymentType.Cash,
			isPickupAtStore: true,
			description: '',
			details: [{ medicineId: null, skuCode: '', quantity: 1 }],
		}),
		[]
	)

	const [values, setValues] = useState(initialValues)

	useEffect(() => {
		if (open) {
			setValues(initialValues)
		}
	}, [open, initialValues])

	const skuOptions = useMemo(() => {
		return medicines.flatMap((m) => {
			if (m.status !== EnumConfig.MedicineStatus.Active || !m.isPublic || !m.skus?.length) return []

			return m.skus
				.filter((s) => s.isActive)
				.map((s) => ({
					value: s.skuCode,
					label: `${m.name} - ${s.name}`,
				}))
		})
	}, [medicines])

	const remainSkuOptions = useMemo(() => {
		const selectedSkus = (values?.details || []).map((d) => d?.skuCode).filter(Boolean)
		return skuOptions.filter((opt) => !selectedSkus.includes(opt.value))
	}, [skuOptions, values])

	const detailsFields = [
		{
			key: 'skuCode',
			title: t('order_management.create.field.sku'),
			type: 'select-dialog',
			required: true,
			options: skuOptions,
			remainOptions: remainSkuOptions,
		},
		{
			key: 'quantity',
			title: t('order_management.create.field.quantity'),
			type: 'number',
			required: true,
			validate: [numberHigherThan(0)],
			defaultValue: 1,
		},
	]

	const fields = [
		{
			key: 'description',
			title: t('order_management.create.field.description'),
			multiple: 2,
			required: false,
		},
		{
			key: 'isPickupAtStore',
			type: 'checkbox',
			title: t('order_management.field.is_pickup_at_store'),
			required: false,
		},
		{
			key: 'details',
			title: t('order_management.create.field.order_details'),
			type: 'array',
			of: detailsFields,
		},
	]

	const handleSubmit = useCallback(
		async ({ values: formValues, closeDialog }) => {
			if (!auth.userId) return false
			const details = (formValues.details || [])
				.filter((d) => d?.skuCode)
				.map((d) => ({
					skuCode: d.skuCode,
					quantity: parseInt(d.quantity, 10) || 1,
				}))
			if (details.length === 0) return false
			const payload = {
				patientId: auth.userId,
				paymentType: formValues.paymentType,
				isPickupAtStore: formValues.isPickupAtStore,
				description: formValues.description || '',
				details,
			}
			const ok = await onSubmit(payload)
			if (ok) closeDialog()
			return ok
		},
		[auth.userId, onSubmit]
	)

	const handleClose = useCallback(() => {
		onClose()
	}, [onClose])

	return (
		<GenericFormDialog
			open={open}
			onClose={handleClose}
			title={t('order_management.create.title')}
			fields={fields}
			initialValues={initialValues}
			onValuesChange={setValues}
			submitLabel={t('button.create')}
			submitButtonColor='primary'
			maxWidth='md'
			onSubmit={handleSubmit}
		/>
	)
}

export default OrderManagementFormSection
