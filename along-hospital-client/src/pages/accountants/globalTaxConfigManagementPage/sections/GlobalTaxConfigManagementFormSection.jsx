import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import useTranslation from '@/hooks/useTranslation'
import { numberHigherThan, numberRange } from '@/utils/validateUtil'
import { useMemo } from 'react'

const GlobalTaxConfigManagementFormSection = ({
	openUpdate,
	setOpenUpdate,
	selectedRow,
	onUpdate,
}) => {
	const { t } = useTranslation()

	const upsertField = useMemo(
		() => [
			{
				key: 'personalDeductionAmount',
				title: t('global_tax_config.field.personal_deduction_amount'),
				type: 'number',
				validate: [numberHigherThan(0)],
			},
			{
				key: 'dependentDeductionAmount',
				title: t('global_tax_config.field.dependent_deduction_amount'),
				type: 'number',
				validate: [numberHigherThan(0)],
			},
			{
				key: 'referenceBaseSalary',
				title: t('global_tax_config.field.reference_base_salary'),
				type: 'number',
				validate: [numberHigherThan(0)],
			},
			{
				key: 'socialInsuranceRate',
				title: t('global_tax_config.field.social_insurance_rate'),
				type: 'number',
				validate: [numberRange(0, 1)],
			},
			{
				key: 'healthInsuranceRate',
				title: t('global_tax_config.field.health_insurance_rate'),
				type: 'number',
				validate: [numberRange(0, 1)],
			},
			{
				key: 'unemploymentInsuranceRate',
				title: t('global_tax_config.field.unemployment_insurance_rate'),
				type: 'number',
				validate: [numberRange(0, 1)],
			},
		],
		[t]
	)

	return (
		<GenericFormDialog
			open={openUpdate}
			onClose={() => setOpenUpdate(false)}
			fields={upsertField}
			initialValues={selectedRow}
			submitLabel={t('button.update')}
			submitButtonColor='success'
			title={t('global_tax_config.title.update')}
			onSubmit={onUpdate}
		/>
	)
}

export default GlobalTaxConfigManagementFormSection
