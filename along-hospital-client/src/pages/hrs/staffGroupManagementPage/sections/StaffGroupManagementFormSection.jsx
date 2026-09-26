import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import useTranslation from '@/hooks/useTranslation'
import { getImageFromCloud } from '@/utils/commons'
import { maxLen } from '@/utils/validateUtil'
import { Avatar, Chip, Stack, Typography } from '@mui/material'

const StaffGroupManagementFormSection = ({
	openCreateForm,
	setOpenCreateForm,
	onCreateSubmit,
	openUpdateForm,
	setOpenUpdateForm,
	selectedItem,
	onUpdateSubmit,
	staffs,
}) => {
	const { t } = useTranslation()

	const initialValues = {
		name: selectedItem?.name || '',
		staffIds: selectedItem?.staffGroupMembers?.map((s) => s.staffId) || [],
	}

	const upsertFields = [
		{
			key: 'name',
			title: t('staff_group.field.name'),
			type: 'text',
			validate: [maxLen(50)],
		},
		{
			key: 'staffIds',
			title: t('staff_group.field.staff_group_members'),
			type: 'select',
			multiple: true,
			options: (staffs || []).map((staff) => ({
				value: staff.id,
				label: staff,
				searchKey: staff.name,
			})),
			renderOption: (_, label) => (
				<Stack direction={'row'} alignItems={'center'} gap={2}>
					<Avatar src={getImageFromCloud(label.image)} alt={label.name} />
					<Stack gap={0.2}>
						<Stack direction={'row'} gap={1} alignItems={'center'}>
							<Typography>{label.name}</Typography>
							<Typography lineHeight='normal' variant='caption' color='text.secondary'>
								{label.phone}
							</Typography>
							<Typography lineHeight='normal' variant='caption' color='text.secondary'>
								{label.email}
							</Typography>
						</Stack>

						<Typography variant='caption' color='text.secondary'>
							{label.role}
						</Typography>
					</Stack>
				</Stack>
			),
			renderOptionValue: (_, label) => {
				return <Chip label={label?.name} avatar={<Avatar src={getImageFromCloud(label?.image)} />} />
			},
		},
	]

	return (
		<>
			<GenericFormDialog
				open={openCreateForm}
				onClose={() => setOpenCreateForm(false)}
				fields={upsertFields}
				initialValues={{ staffIds: [] }}
				onSubmit={onCreateSubmit}
				title={t('staff_group.title.create_form')}
				submitButtonColor='primary'
				submitLabel={t('button.create')}
			/>
			<GenericFormDialog
				open={openUpdateForm}
				onClose={() => setOpenUpdateForm(false)}
				fields={upsertFields}
				initialValues={initialValues}
				onSubmit={onUpdateSubmit}
				title={t('staff_group.title.update_form')}
				submitButtonColor='success'
				submitLabel={t('button.update')}
			/>
		</>
	)
}

export default StaffGroupManagementFormSection
