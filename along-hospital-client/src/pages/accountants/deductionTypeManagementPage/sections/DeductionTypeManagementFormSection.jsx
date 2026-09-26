import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import { EnumConfig } from '@/configs/enumConfig'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { maxLen, numberHigherThan } from '@/utils/validateUtil'
import { useMemo } from 'react'

const DeductionTypeManagementFormSection = ({
	openCreate,
	setOpenCreate,
	openUpdate,
	setOpenUpdate,
	selectedRow,
	onCreate,
	onUpdate,
}) => {
	const { t } = useTranslation()
	const _enum = useEnum()

	const createInitialValues = useMemo(
		() => ({
			name: '',
			description: '',
			price: 0,
			isTaxable: false,
			insuranceSubject: EnumConfig.InsuranceSubject.None,
		}),
		[]
	)

	const createField = useMemo(
		() => [
			{
				key: 'name',
				title: t('deduction_type.field.name'),
				validate: [maxLen(50)],
			},
			{
				key: 'description',
				title: t('deduction_type.field.description'),
				required: false,
				validate: [maxLen(1000)],
			},
			{
				key: 'price',
				title: t('deduction_type.field.price'),
				type: 'number',
				validate: [numberHigherThan(0)],
			},
			{
				key: 'insuranceSubject',
				title: t('allowance_type.field.insurance_subject'),
				type: 'select',
				options: _enum.insuranceSubjectOptions,
			},
			{
				key: 'isTaxable',
				title: t('deduction_type.field.is_taxable'),
				type: 'checkbox',
			},
		],
		[t, _enum.insuranceSubjectOptions]
	)

	const updateField = useMemo(
		() => [
			{
				key: 'name',
				title: t('deduction_type.field.name'),
				validate: [maxLen(50)],
			},
			{
				key: 'description',
				title: t('deduction_type.field.description'),
				required: false,
				validate: [maxLen(1000)],
			},
			{
				key: 'price',
				title: t('deduction_type.field.price'),
				type: 'number',
				validate: [numberHigherThan(0)],
			},
			{
				key: 'insuranceSubject',
				title: t('deduction_type.field.insurance_subject'),
				type: 'select',
				options: _enum.insuranceSubjectOptions,
			},
			{
				key: 'isTaxable',
				title: t('deduction_type.field.is_taxable'),
				type: 'checkbox',
			},
		],
		[_enum.insuranceSubjectOptions, t]
	)

	return (
		<>
			<GenericFormDialog
				open={openCreate}
				onClose={() => setOpenCreate(false)}
				initialValues={createInitialValues}
				fields={createField}
				submitLabel={t('button.create')}
				submitButtonColor='success'
				title={t('deduction_type.title.create')}
				onSubmit={onCreate}
			/>
			<GenericFormDialog
				open={openUpdate}
				onClose={() => setOpenUpdate(false)}
				fields={updateField}
				initialValues={selectedRow}
				submitLabel={t('button.update')}
				submitButtonColor='success'
				title={t('deduction_type.title.update')}
				onSubmit={onUpdate}
			/>
		</>
	)
}

export default DeductionTypeManagementFormSection
