import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import useTranslation from '@/hooks/useTranslation'
import { maxLen, numberHigherThan } from '@/utils/validateUtil'
import { useMemo } from 'react'

const SalaryAdvanceFormSection = ({
	openCreate,
	setOpenCreate,
	openUpdate,
	setOpenUpdate,
	selectedRow,
	onCreate,
	onUpdate,
}) => {
	const { t } = useTranslation()

	const initialValues = useMemo(
		() => ({
			amount: '',
			reason: '',
		}),
		[]
	)

	const upsertFields = useMemo(
		() => [
			{
				key: 'amount',
				title: t('salary_advance.field.amount'),
				type: 'number',
				validate: [numberHigherThan(0)],
			},
			{
				key: 'reason',
				title: t('salary_advance.field.reason'),
				multiple: 4,
				required: false,
				validate: [maxLen(1000)],
			},
		],
		[t]
	)

	return (
		<>
			<GenericFormDialog
				open={openCreate}
				onClose={() => setOpenCreate(false)}
				initialValues={initialValues}
				fields={upsertFields}
				submitLabel={t('salary_advance.button.create_request')}
				submitButtonColor='success'
				title={t('salary_advance.title.create_request')}
				onSubmit={onCreate}
			/>
			<GenericFormDialog
				open={openUpdate}
				onClose={() => setOpenUpdate(false)}
				initialValues={selectedRow}
				fields={upsertFields}
				submitLabel={t('salary_advance.button.update_request')}
				submitButtonColor='success'
				title={t('salary_advance.title.edit_request')}
				onSubmit={onUpdate}
			/>
		</>
	)
}

export default SalaryAdvanceFormSection
