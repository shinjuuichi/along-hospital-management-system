import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import useTranslation from '@/hooks/useTranslation'
import { maxLen } from '@/utils/validateUtil'
import { useMemo } from 'react'

const TeleRoomManagementFormSection = ({
	openCreate,
	setOpenCreate,
	openUpdate,
	setOpenUpdate,
	selectedRow,
	specialties = [],
	onCreate,
	onUpdate,
}) => {
	const { t } = useTranslation()

	const specialtyOptions = useMemo(
		() =>
			Array.isArray(specialties)
				? specialties.map((item) => ({ label: item?.name, value: item?.id }))
				: [],
		[specialties]
	)

	const createInitialValues = useMemo(
		() => ({
			roomCode: '',
			roomDisplayName: '',
			specialtyId: '',
		}),
		[]
	)

	const upsertField = useMemo(
		() => [
			{
				key: 'roomCode',
				title: t('tele_room.field.room_code'),
				type: 'text',
				validate: [maxLen(50)],
			},
			{
				key: 'roomDisplayName',
				title: t('tele_room.field.room_display_name'),
				type: 'text',
				validate: [maxLen(100)],
			},
			{
				key: 'specialtyId',
				title: t('tele_room.field.specialty_id'),
				type: 'select-dialog',
				options: specialtyOptions,
			},
		],
		[t, specialtyOptions]
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
				title={t('tele_room.title.create')}
				onSubmit={onCreate}
			/>
			<GenericFormDialog
				open={openUpdate}
				onClose={() => setOpenUpdate(false)}
				fields={upsertField}
				initialValues={selectedRow}
				submitLabel={t('button.update')}
				submitButtonColor='success'
				title={t('tele_room.title.update')}
				onSubmit={onUpdate}
			/>
		</>
	)
}

export default TeleRoomManagementFormSection
