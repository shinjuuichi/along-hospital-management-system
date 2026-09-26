import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import { EnumConfig } from '@/configs/enumConfig'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { maxLen, numberHigherThan } from '@/utils/validateUtil'
import { useMemo } from 'react'

const AllowanceTypeManagementFormSection = ({
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

	const upsertField = useMemo(
		() => [
			{
				key: 'name',
				title: t('allowance_type.field.name'),
				validate: [maxLen(50)],
			},
			{
				key: 'description',
				title: t('allowance_type.field.description'),
				required: false,
				validate: [maxLen(1000)],
			},
			{
				key: 'price',
				title: t('allowance_type.field.price'),
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
				title: t('allowance_type.field.is_taxable'),
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
				fields={upsertField}
				submitLabel={t('button.create')}
				submitButtonColor='success'
				title={t('allowance_type.title.create')}
				onSubmit={onCreate}
			/>
			<GenericFormDialog
				open={openUpdate}
				onClose={() => setOpenUpdate(false)}
				fields={upsertField}
				initialValues={selectedRow}
				submitLabel={t('button.update')}
				submitButtonColor='success'
				title={t('allowance_type.title.update')}
				onSubmit={onUpdate}
			/>
		</>
	)
}

export default AllowanceTypeManagementFormSection
