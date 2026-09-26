import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import useTranslation from '@/hooks/useTranslation'
import { useMemo } from 'react'

const RegionalWageManagementFormSection = ({
	openUpdate,
	setOpenUpdate,
	selectedRow,
	onUpdate,
}) => {
	const { t } = useTranslation()

	const updateFields = useMemo(
		() => [
			{
				key: 'monthlyWage',
				title: t('regional_wage.field.monthly_wage'),
				type: 'number',
			},
		],
		[t]
	)

	return (
		<>
			<GenericFormDialog
				open={openUpdate}
				onClose={() => setOpenUpdate(false)}
				fields={updateFields}
				initialValues={selectedRow}
				submitLabel={t('button.update')}
				submitButtonColor='success'
				title={t('regional_wage.title.update')}
				onSubmit={onUpdate}
			/>
		</>
	)
}

export default RegionalWageManagementFormSection
