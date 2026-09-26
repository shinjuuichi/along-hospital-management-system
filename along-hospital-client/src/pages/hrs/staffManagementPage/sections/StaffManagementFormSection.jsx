import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import { EnumConfig } from '@/configs/enumConfig'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { maxLen } from '@/utils/validateUtil'
import { useMemo } from 'react'

const StaffManagementFormSection = ({
	openCreate,
	setOpenCreate,
	openUpdate,
	setOpenUpdate,
	selectedRow,
	onCreateSubmit,
	onUpdateSubmit,
	refetch,
	specialties,
	qualifications,
}) => {
	const { t } = useTranslation()
	const _enum = useEnum()
	const roleOptions = useMemo(
		() => _enum.roleOptions.filter((opt) => opt.value !== EnumConfig.Role.Patient),
		[_enum.roleOptions]
	)

	const createInitialValues = useMemo(
		() => ({
			role: '',
			name: '',
			phone: '',
			email: '',
			gender: '',
			address: '',
			dateOfBirth: '',
			specialtyId: '',
			qualificationId: '',
			image: null,
			bankCode: '',
			accountNumber: '',
			dependentQuantity: 0,
		}),
		[]
	)

	const createFields = useMemo(
		() => [
			{
				key: 'role',
				title: t('staff.field.role'),
				type: 'select',
				options: roleOptions,
			},
			{
				key: 'gender',
				title: t('staff.field.gender'),
				type: 'select',
				options: _enum.genderOptions,
			},
			{
				key: 'specialtyId',
				title: t('staff.field.specialty'),
				type: 'select-dialog',
				options: (specialties || [])
					.map((s) => ({ value: s?.id, label: s?.name }))
					.filter((o) => o.value != null),
			},
			{
				key: 'qualificationId',
				title: t('staff.field.qualification'),
				type: 'select-dialog',
				options: (qualifications || [])
					.map((q) => ({ value: q?.id, label: q?.name }))
					.filter((o) => o.value != null),
			},
			{
				key: 'bankCode',
				title: t('staff.field.bank_code'),
				type: 'select-dialog',
				options: _enum.bankCodeOptions,
			},
			{
				key: 'name',
				title: t('staff.field.name'),
				validate: [maxLen(50)],
			},
			{
				key: 'phone',
				title: t('staff.field.phone'),
				validate: [maxLen(15)],
			},
			{
				key: 'email',
				title: t('staff.field.email'),
				type: 'email',
				validate: [maxLen(100)],
			},
			{
				key: 'address',
				title: t('staff.field.address'),
				validate: [maxLen(255)],
			},
			{
				key: 'accountNumber',
				title: t('staff.field.account_number'),
				validate: [maxLen(100)],
			},
			{
				key: 'dependentQuantity',
				title: t('staff.field.dependent_quantity'),
				type: 'number',
			},
			{ key: 'dateOfBirth', title: t('staff.field.date_of_birth'), type: 'date', required: true },
			{ key: 'image', title: t('staff.field.image'), type: 'image' },
		],
		[t, specialties, qualifications, _enum, roleOptions]
	)

	const updateFields = useMemo(
		() => [
			{
				key: 'role',
				title: t('staff.field.role'),
				type: 'select',
				options: roleOptions,
			},
			{
				key: 'gender',
				title: t('staff.field.gender'),
				type: 'select',
				options: _enum.genderOptions,
			},
			{
				key: 'specialtyId',
				title: t('staff.field.specialty'),
				type: 'select-dialog',
				options: (specialties || [])
					.map((s) => ({ value: s?.id, label: s?.name }))
					.filter((o) => o.value != null),
			},
			{
				key: 'qualificationId',
				title: t('staff.field.qualification'),
				type: 'select-dialog',
				options: (qualifications || [])
					.map((q) => ({ value: q?.id, label: q?.name }))
					.filter((o) => o.value != null),
			},
			{
				key: 'bankCode',
				title: t('staff.field.bank_code'),
				type: 'select-dialog',
				options: _enum.bankCodeOptions,
			},
			{
				key: 'name',
				title: t('staff.field.name'),
				validate: [maxLen(50)],
			},
			{
				key: 'phone',
				title: t('staff.field.phone'),
				validate: [maxLen(15)],
			},
			{
				key: 'email',
				title: t('staff.field.email'),
				type: 'email',
				validate: [maxLen(100)],
			},
			{
				key: 'address',
				title: t('staff.field.address'),
				validate: [maxLen(255)],
			},
			{
				key: 'accountNumber',
				title: t('staff.field.account_number'),
				validate: [maxLen(100)],
			},
			{
				key: 'dependentQuantity',
				title: t('staff.field.dependent_quantity'),
				type: 'number',
			},
			{ key: 'dateOfBirth', title: t('staff.field.date_of_birth'), type: 'date' },
			{ key: 'image', title: t('staff.field.image'), type: 'image' },
		],
		[t, specialties, qualifications, _enum, roleOptions]
	)

	const handleCreateSubmit = async ({ values, closeDialog }) => {
		const respond = await onCreateSubmit({ overrideData: values })
		if (respond) {
			closeDialog()
			refetch()
		}
	}

	const handleUpdateSubmit = async ({ values, closeDialog }) => {
		if (!selectedRow?.id) return
		const processedValues = { ...values }
		Object.keys(processedValues).forEach((key) => {
			if (processedValues[key] === 'N/A') {
				processedValues[key] = ''
			}
		})

		const res = await onUpdateSubmit({ overrideData: processedValues })
		if (res) {
			closeDialog()
			refetch()
		}
	}

	return (
		<>
			<GenericFormDialog
				open={openCreate}
				onClose={() => setOpenCreate(false)}
				initialValues={createInitialValues}
				fields={createFields}
				submitLabel={t('button.create')}
				submitButtonColor='success'
				title={t('staff.title.staff_management')}
				onSubmit={handleCreateSubmit}
			/>
			<GenericFormDialog
				open={openUpdate}
				onClose={() => setOpenUpdate(false)}
				fields={updateFields}
				initialValues={selectedRow}
				submitLabel={t('button.update')}
				submitButtonColor='success'
				title={t('staff.title.staff_management')}
				onSubmit={handleUpdateSubmit}
			/>
		</>
	)
}

export default StaffManagementFormSection
