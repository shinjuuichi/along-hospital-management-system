import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import useTranslation from '@/hooks/useTranslation'
import { maxLen } from '@/utils/validateUtil'
import { useMemo } from 'react'

const SpecialtyManagementFormSection = ({
	openCreate,
	setOpenCreate,
	openUpdate,
	setOpenUpdate,
	selectedRow,
	onCreate,
	onUpdate,
}) => {
	const { t } = useTranslation()

	const createInitialValues = useMemo(
		() => ({
			name: '',
			description: '',
			isMedical: false,
		}),
		[]
	)

	const upsertField = useMemo(
		() => [
			{
				key: 'name',
				title: t('specialty.field.name'),
				type: 'text',
				validate: [maxLen(255)],
			},
			{
				key: 'description',
				title: t('specialty.field.description'),
				type: 'text',
				required: false,
				validate: [maxLen(1000)],
			},
			{
				key: 'isMedical',
				title: t('specialty.field.is_medical'),
				type: 'checkbox',
			},
		],
		[t]
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
				title={t('specialty.title.create')}
				onSubmit={onCreate}
			/>
			<GenericFormDialog
				open={openUpdate}
				onClose={() => setOpenUpdate(false)}
				fields={upsertField}
				initialValues={selectedRow}
				submitLabel={t('button.update')}
				submitButtonColor='success'
				title={t('specialty.title.update')}
				onSubmit={onUpdate}
			/>
		</>
	)
}

export default SpecialtyManagementFormSection
