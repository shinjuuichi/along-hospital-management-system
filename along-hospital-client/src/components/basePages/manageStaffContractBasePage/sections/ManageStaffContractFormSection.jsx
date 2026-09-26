import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import StaffRenderOption from '@/components/renderOptions/StaffRenderOption'
import { EnumConfig } from '@/configs/enumConfig'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { getImageFromCloud } from '@/utils/commons'
import { maxLen, numberHigherThan, numberRange } from '@/utils/validateUtil'
import { useEffect, useState } from 'react'

const ManageStaffContractFormSection = ({
	openCreateForm,
	setOpenCreateForm,
	onCreateSubmit,
	openUpdateForm,
	setOpenUpdateForm,
	selectedItem,
	onUpdateSubmit,
	openRenewForm,
	setOpenRenewForm,
	onRenewSubmit,
	openSignForm,
	setOpenSignForm,
	onSignSubmit,
	staffs,
	regionalWages,
	isAlreadySigned = false,
}) => {
	const { t } = useTranslation()
	const [formValues, setFormValues] = useState({})
	const { contractTypeOptions } = useEnum()

	useEffect(() => {
		if (selectedItem) {
			setFormValues({
				contractType: selectedItem.contractType,
				status: selectedItem.status,
			})
		} else {
			setFormValues({})
		}
	}, [selectedItem])

	const staffOptions = (staffs || []).map((staff) => ({
		value: staff.id,
		label: staff,
		searchKey: `${staff.name || ''} ${staff.phone || ''}`.trim(),
	}))

	const regionalWageOptions = (regionalWages || []).map((rw) => ({
		value: rw.id,
		label: rw.code,
	}))

	const today = new Date().toISOString().split('T')[0]

	const getEndDateField = (isReadOnly = false) => {
		if (formValues.contractType === EnumConfig.ContractType.Indefinite) return null
		return {
			key: 'endDate',
			title: t('staff_contract.field.end_date'),
			type: 'date',
			minValue: today,
			props: { readOnly: isReadOnly },
		}
	}

	const baseFields = [
		{
			key: 'contractCode',
			title: t('staff_contract.field.contract_code'),
			type: 'text',
			validate: [maxLen(50)],
		},
		{
			key: 'contractType',
			title: t('staff_contract.field.contract_type'),
			type: 'select',
			options: contractTypeOptions,
		},
		{
			key: 'startDate',
			title: t('staff_contract.field.start_date'),
			type: 'date',
		},
		getEndDateField(),
		{
			key: 'hourlyRate',
			title: t('staff_contract.field.hourly_rate'),
			type: 'number',
			validate: [numberHigherThan(0)],
		},
		{
			key: 'workingHoursPerWeek',
			title: t('staff_contract.field.working_hours_per_week'),
			type: 'number',
			validate: [numberHigherThan(0)],
		},
		{
			key: 'insuranceSalaryRate',
			title: t('staff_contract.field.insurance_salary_rate'),
			type: 'number',
			validate: [numberRange(0, 1)],
		},
		{
			key: 'staffId',
			title: t('staff_contract.field.staff'),
			type: 'select-dialog',
			options: staffOptions,
			renderOption: (_, staff) => <StaffRenderOption staff={staff} />,
			renderOptionValue: (_, staff) => staff?.name || '',
		},
		{
			key: 'regionalWageId',
			title: t('staff_contract.field.regional_wage'),
			type: 'select',
			options: regionalWageOptions,
		},
	].filter(Boolean)

	const createFields = [...baseFields.slice(1)]

	const updateFields = [
		...baseFields.map((field) => {
			if (field.key === 'contractCode') {
				return { ...field, props: { readOnly: true } }
			}
			if (field.key === 'contractType') {
				return { ...field, props: { readOnly: true } }
			}
			if (field.key === 'startDate') {
				return { ...field, props: { readOnly: true } }
			}
			if (field.key === 'endDate') {
				return { ...field, props: { readOnly: true } }
			}
			if (field.key === 'staffId') {
				return { ...field, props: { readOnly: true } }
			}
			return field
		}),
		...(selectedItem?.signatureImage
			? [
					{
						key: 'signedDate',
						title: t('staff_contract.field.signed_date'),
						type: 'date',
						props: { readOnly: true },
					},
					{
						key: 'signatureImageFile',
						title: t('staff_contract.field.signature_image'),
						type: 'custom',
						required: false,
						render: () => {
							const imageUrl = getImageFromCloud(selectedItem.signatureImage)
							return (
								<img
									src={imageUrl}
									alt='signature'
									style={{ maxWidth: '100%', maxHeight: 200, objectFit: 'contain' }}
								/>
							)
						},
					},
				]
			: []),
	]

	const renewFields = [
		{
			key: 'startDate',
			title: t('staff_contract.field.start_date'),
			type: 'date',
			minValue: today,
			required: true,
		},
		getEndDateField(),
	].filter(Boolean)

	const signFields = [
		{
			key: 'startDate',
			title: t('staff_contract.field.start_date'),
			type: 'date',
			props: { readOnly: true },
		},
		getEndDateField(true),
		...(isAlreadySigned
			? [
					{
						key: 'signedDate',
						title: t('staff_contract.field.signed_date'),
						type: 'date',
						props: { readOnly: true },
					},
				]
			: []),
		{
			key: 'signatureImageFile',
			title: t('staff_contract.field.signature_image'),
			type: isAlreadySigned ? 'custom' : 'draw',
			required: !isAlreadySigned,
			...(isAlreadySigned && {
				render: () => {
					if (!selectedItem?.signatureImage) return null
					const imageUrl = getImageFromCloud(selectedItem.signatureImage)
					return (
						<img
							src={imageUrl}
							alt='signature'
							style={{ maxWidth: '100%', maxHeight: 200, objectFit: 'contain' }}
						/>
					)
				},
			}),
		},
	].filter(Boolean)

	const signInitialValues = isAlreadySigned
		? {
				startDate: selectedItem?.startDate || null,
				endDate: selectedItem?.endDate || null,
				signedDate: selectedItem?.signedDate || null,
				signatureImageFile: selectedItem?.signatureImage || null,
			}
		: {
				startDate: selectedItem?.startDate || null,
				endDate: selectedItem?.endDate || null,
				signatureImageFile: null,
			}

	const initialValues = {
		id: selectedItem?.id || null,
		contractCode: selectedItem?.contractCode || '',
		contractType: selectedItem?.contractType ?? EnumConfig.ContractType.Probation,
		startDate: selectedItem?.startDate || '',
		endDate: selectedItem?.endDate || null,
		hourlyRate: selectedItem?.hourlyRate || '',
		workingHoursPerWeek: selectedItem?.workingHoursPerWeek || '',
		status: selectedItem?.status ?? EnumConfig.StaffContractStatus.Active,
		insuranceSalaryRate: selectedItem?.insuranceSalaryRate || '',
		staffId: selectedItem?.staffId || null,
		regionalWageId: selectedItem?.regionalWageId || null,
		signedDate: selectedItem?.signedDate || null,
	}

	const defaultCreateValues = {
		contractType: EnumConfig.ContractType.Probation,
		status: EnumConfig.StaffContractStatus.Active,
		startDate: today,
		workingHoursPerWeek: 40,
		staffId: null,
		regionalWageId: null,
	}

	const renewInitialValues = {
		id: selectedItem?.id || null,
		startDate: '',
		endDate: null,
	}

	return (
		<>
			<GenericFormDialog
				open={openCreateForm}
				onClose={() => setOpenCreateForm(false)}
				fields={createFields}
				initialValues={defaultCreateValues}
				onSubmit={onCreateSubmit}
				onValuesChange={setFormValues}
				title={t('staff_contract.title.create_form')}
				submitButtonColor='primary'
				submitLabel={t('button.create')}
			/>

			<GenericFormDialog
				open={openUpdateForm}
				onClose={() => setOpenUpdateForm(false)}
				fields={updateFields}
				initialValues={initialValues}
				onSubmit={onUpdateSubmit}
				onValuesChange={setFormValues}
				title={t('staff_contract.title.update_form')}
				submitButtonColor='success'
				submitLabel={t('button.update')}
			/>

			<GenericFormDialog
				open={openRenewForm}
				onClose={() => setOpenRenewForm(false)}
				fields={renewFields}
				initialValues={renewInitialValues}
				onSubmit={onRenewSubmit}
				onValuesChange={setFormValues}
				title={t('staff_contract.title.renew_form')}
				submitButtonColor='success'
				submitLabel={t('button.update')}
			/>

			<GenericFormDialog
				open={openSignForm}
				onClose={() => setOpenSignForm(false)}
				fields={signFields}
				initialValues={signInitialValues}
				onSubmit={isAlreadySigned ? undefined : onSignSubmit}
				title={t('staff_contract.title.sign_form')}
				submitButtonColor='primary'
				submitLabel={isAlreadySigned ? undefined : t('button.submit')}
				showSubmit={!isAlreadySigned}
			/>
		</>
	)
}

export default ManageStaffContractFormSection
