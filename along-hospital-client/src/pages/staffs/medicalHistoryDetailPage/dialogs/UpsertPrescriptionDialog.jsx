import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import { setMedicinesStore } from '@/redux/reducers/managementReducer'
import { getImageFromCloud } from '@/utils/commons'
import { getObjectValueFromStringPath } from '@/utils/handleObjectUtil'
import { renderEmptyFallback } from '@/utils/handleStringUtil'
import { maxLen, numberHigherThan } from '@/utils/validateUtil'
import { Avatar, Typography } from '@mui/material'
import { Stack } from '@mui/system'
import { useState } from 'react'

const UpsertPrescriptionDialog = ({
	open,
	onClose,
	initialValues,
	onSubmit = (values) => Promise.resolve(values),
}) => {
	const { t } = useTranslation()

	const [values, setValues] = useState(initialValues)

	const medicineStore = useReduxStore({
		selector: (state) => state.management.medicines,
		setStore: setMedicinesStore,
	})

	const remainMedicines = medicineStore.data.filter((medicine) =>
		values?.prescriptionDetails
			? !values?.prescriptionDetails?.some(
					(prescriptionDetail) => prescriptionDetail.medicineId === medicine.id
				)
			: true
	)

	const fields = [
		{
			key: 'doctorNote',
			title: t('medical_history.field.prescription.doctor_note'),
			type: 'text',
			multiple: 2,
			required: false,
			validate: [maxLen(1000)],
		},
		{
			key: 'medicationDays',
			title: t('medical_history.field.prescription.medication_days'),
			type: 'number',
			validate: [numberHigherThan(0)],
		},
		{
			key: 'prescriptionDetails',
			title: t('medical_history.field.prescription.prescription_details'),
			type: 'array',
			of: [
				{
					key: 'medicineId',
					title: t('medical_history.field.prescription.prescription_detail.medicine'),
					type: 'select-dialog',
					options:
						medicineStore.data.map((medicine) => ({
							value: medicine.id,
							label: medicine,
							searchKey: medicine.name,
						})) || [],
					remainOptions:
						remainMedicines.map((medicine) => ({
							value: medicine.id,
							label: medicine,
							searchKey: medicine.name,
						})) || [],
					renderOption: (_, label) => (
						<Stack key={label.id} direction='row' spacing={1} alignItems='center' sx={{ width: '100%' }}>
							<Avatar
								src={getImageFromCloud(Array.isArray(label.images) ? label.images[0] : null)}
								alt={label.name}
							/>
							<Stack sx={{ width: '100%' }}>
								{[
									{
										key: 'name',
										label: t('medicine.field.name'),
									},
									{ key: 'brand', label: t('medicine.field.brand') },
									{ key: 'medicineUnit.name', label: t('medicine.field.unit') },
								].map((field) => (
									<Stack key={field.key} direction={'row'} justifyContent={'space-between'}>
										<Typography variant='body2' color='text.secondary'>
											{field.label}
										</Typography>
										<Typography variant='body2' color='text.secondary' textAlign={'right'}>
											{renderEmptyFallback(getObjectValueFromStringPath(label, field.key))}
										</Typography>
									</Stack>
								))}
							</Stack>
						</Stack>
					),
					renderOptionValue: (_, label) => label?.name || '',
				},
				{
					key: 'dosage',
					title: t('medical_history.field.prescription.prescription_detail.dosage'),
					type: 'number',
					validate: [numberHigherThan(0)],
				},
				{
					key: 'frequencyPerDay',
					title: t('medical_history.field.prescription.prescription_detail.frequency_per_day'),
					type: 'number',
					validate: [numberHigherThan(0)],
				},
			],
		},
	]

	return (
		<GenericFormDialog
			open={open}
			onClose={onClose}
			title={
				initialValues
					? t('medical_history.dialog.title.update_prescription')
					: t('medical_history.dialog.title.create_prescription')
			}
			fields={fields}
			initialValues={initialValues}
			onValuesChange={(values) => setValues(values)}
			onSubmit={({ values }) => onSubmit(values)}
			submitButtonColor={initialValues ? 'success' : 'primary'}
			submitLabel={initialValues ? t('button.update') : t('button.create')}
			maxWidth='lg'
		/>
	)
}

export default UpsertPrescriptionDialog
