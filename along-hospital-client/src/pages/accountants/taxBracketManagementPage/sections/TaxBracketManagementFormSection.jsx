import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import useTranslation from '@/hooks/useTranslation'
import { numberHigherThanOrEqual, numberRange } from '@/utils/validateUtil'
import { useMemo } from 'react'

const TaxBracketManagementFormSection = ({ openUpdate, setOpenUpdate, selectedRow, onUpdate }) => {
	const { t } = useTranslation()

	const upsertField = useMemo(
		() => [
			{
				key: 'fromAmount',
				title: t('tax_bracket.field.from_amount'),
				type: 'number',
				validate: [numberHigherThanOrEqual(0)],
			},
			{
				key: 'taxRate',
				title: t('tax_bracket.field.tax_rate'),
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
			title={t('tax_bracket.title.update')}
			onSubmit={onUpdate}
		/>
	)
}

export default TaxBracketManagementFormSection
