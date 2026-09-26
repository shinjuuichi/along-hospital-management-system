import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import { EnumConfig } from '@/configs/enumConfig'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { hasNoDuplicateValues, isFutureDate, isPercentage, maxLen } from '@/utils/validateUtil'
import { InputAdornment } from '@mui/material'
import { useEffect, useMemo, useState } from 'react'
import { toast } from 'react-toastify'

const normalizeMedicineIds = (medicineIds) => {
	if (!Array.isArray(medicineIds)) {
		return []
	}

	return medicineIds
		.map((item) => item?.medicineId ?? item?.id ?? item)
		.filter((id) => id !== null && id !== undefined && id !== '')
}

const VoucherFormDialog = ({
	open,
	onClose,
	title,
	initialValues,
	medicineOptions,
	submitLabel,
	submitButtonColor,
	onSubmit,
	isUpdate = false,
}) => {
	const { t } = useTranslation()
	const normalizedInitialValues = useMemo(
		() => ({
			...(initialValues || {}),
			medicineIds: normalizeMedicineIds(initialValues?.medicineIds),
		}),
		[initialValues]
	)
	const [values, setValues] = useState(normalizedInitialValues)
	const _enum = useEnum()
	const isMedicineVoucher = values.voucherType === EnumConfig.VoucherType.Medicine
	const isPatientVoucher = values.voucherType === EnumConfig.VoucherType.Patient
	const isPercentageDiscount = values.discountType === EnumConfig.VoucherDiscountType.Percentage
	const discountTypeOptions = isMedicineVoucher
		? _enum.voucherDiscountTypeOptions
				.filter(
					(option) =>
						option.value === EnumConfig.VoucherDiscountType.Percentage ||
						option.value === values.discountType
				)
				.map((option) =>
					option.value === EnumConfig.VoucherDiscountType.FixedAmount
						? { ...option, disabled: true }
						: option
				)
		: _enum.voucherDiscountTypeOptions
	const discountValueTitle = isPercentageDiscount
		? t('voucher.field.discount_value_percentage')
		: t('voucher.field.discount_value_fixed_amount')
	const currencyFieldProps = {
		slotProps: {
			input: {
				startAdornment: <InputAdornment position='start'>$</InputAdornment>,
			},
		},
	}

	useEffect(() => {
		setValues(normalizedInitialValues)
	}, [normalizedInitialValues, open])

	const baseFields = [
		{
			key: 'voucherType',
			title: t('voucher.field.voucher_type'),
			type: 'select',
			options: _enum.voucherTypeOptions,
			props: { readOnly: isUpdate },
		},
		{
			key: 'name',
			title: t('voucher.field.name'),
			type: 'text',
			validate: [maxLen(255)],
		},
		{
			key: 'description',
			title: t('voucher.field.description'),
			type: 'text',
			required: false,
			validate: [maxLen(1000)],
			multiple: 3,
		},
		{
			key: 'discountType',
			title: t('voucher.field.discount_type'),
			type: 'select',
			options: discountTypeOptions,
		},
		{
			key: 'discountValue',
			title: discountValueTitle,
			type: 'number',
			validate: isPercentageDiscount ? [isPercentage()] : [],
			props: isPercentageDiscount ? undefined : currencyFieldProps,
		},
		{
			key: 'expireDate',
			title: t('voucher.field.expire_date'),
			type: 'date',
			minValue: new Date().toISOString().split('T')[0],
			validate: [isFutureDate()],
		},
	]

	const patientFields = [
		{
			key: 'minPurchaseAmount',
			title: t('voucher.field.min_purchase_amount'),
			type: 'number',
			required: false,
			props: currencyFieldProps,
		},
		...(isPercentageDiscount
			? [
					{
						key: 'maxDiscount',
						title: t('voucher.field.max_discount'),
						type: 'number',
						required: false,
						props: currencyFieldProps,
					},
				]
			: []),
		{
			key: 'quantity',
			title: t('voucher.field.quantity'),
			type: 'number',
		},
		{
			key: 'image',
			title: t('voucher.field.image'),
			type: 'image',
			required: false,
		},
	]

	const medicineFields = [
		{
			key: 'medicineIds',
			title: t('voucher.field.applicable_medicines'),
			type: 'select',
			multiple: true,
			options: medicineOptions,
			renderOptionValue: (value, label) => label || value,
		},
	]

	const fields = [
		...baseFields,
		...(isPatientVoucher ? patientFields : []),
		...(isMedicineVoucher ? medicineFields : []),
	]

	const handleSubmit = async ({ values: formValues, closeDialog, setField }) => {
		if (formValues.voucherType === EnumConfig.VoucherType.Medicine) {
			if (formValues.discountType === EnumConfig.VoucherDiscountType.FixedAmount) {
				toast.error(t('voucher.error.medicine_fixed_amount_not_supported'))
				return
			}

			const duplicateMedicineValidation = hasNoDuplicateValues(
				(item) => item?.medicineId ?? item,
				t('voucher.error.duplicate_applicable_medicines')
			)(formValues.medicineIds)

			if (!duplicateMedicineValidation) {
				toast.error(duplicateMedicineValidation)
				return
			}
		}

		return onSubmit({ values: formValues, closeDialog, setField })
	}

	return (
		<GenericFormDialog
			open={open}
			onClose={onClose}
			title={title}
			initialValues={normalizedInitialValues}
			fields={fields}
			submitLabel={submitLabel}
			submitButtonColor={submitButtonColor}
			onValuesChange={setValues}
			textFieldVariant={'outlined'}
			onSubmit={handleSubmit}
		/>
	)
}

export default VoucherFormDialog
