import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import { EnumConfig } from '@/configs/enumConfig'
import useEnum from '@/hooks/useEnum'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import {
	setInfusionMedicinesStore,
	setMedicalServicesStore,
} from '@/redux/reducers/managementReducer'
import { getImageFromCloud } from '@/utils/commons'
import { formatCurrencyBasedOnCurrentLanguage } from '@/utils/formatNumberUtil'
import { getObjectValueFromStringPath } from '@/utils/handleObjectUtil'
import { renderEmptyFallback } from '@/utils/handleStringUtil'
import { maxLen, numberHigherThan } from '@/utils/validateUtil'
import { Avatar, Typography } from '@mui/material'
import { Stack } from '@mui/system'
import { useState } from 'react'
import {
	nursingCareOrderFields,
	nutritionOrderFields,
	positionOrderFields,
	respiratorySupportOrderFields,
} from '../../helpers/instructionMedicalOrderFields'

const CreateMedicalOrderDialog = ({
	open,
	onClose,
	onSubmit = (values) => Promise.resolve(values),
	onCreateInstructionMedicalOrderAsDraft = (values) => Promise.resolve(values),
}) => {
	const { t } = useTranslation()
	const _enum = useEnum()

	const togglesInitialValues = {
		isPositionOrderEnabled: false,
		isRespiratorySupportOrderEnabled: false,
		isNutritionOrderEnabled: false,
		isNursingCareOrderEnabled: false,
	}
	const arrayInitialValues = {
		clinicalMedicalOrder: { clinicalMedicalOrderDetails: [] },
		infusionMedicalOrder: { infusionMedicalOrderDetails: [] },
	}
	const finalInitialValues = {
		...arrayInitialValues,
		instructionOrderToggles: togglesInitialValues,
	}

	const [values, setValues] = useState(finalInitialValues)

	const medicalServiceStore = useReduxStore({
		selector: (state) => state.management.medicalServices,
		setStore: setMedicalServicesStore,
	})

	const infusionMedicineStore = useReduxStore({
		selector: (state) => state.management.infusionMedicines,
		setStore: setInfusionMedicinesStore,
	})

	const remainMedicalServices = medicalServiceStore.data.filter((medicalService) => {
		return Array.isArray(values?.clinicalMedicalOrder?.clinicalMedicalOrderDetails)
			? !values?.clinicalMedicalOrder.clinicalMedicalOrderDetails.some(
					(clinicalDetail) => clinicalDetail.medicalServiceId === medicalService.id
				)
			: true
	})

	const remainMedicines = infusionMedicineStore.data.filter((medicine) =>
		Array.isArray(values?.infusionMedicalOrder?.infusionMedicalOrderDetails)
			? !values?.infusionMedicalOrder.infusionMedicalOrderDetails.some(
					(infusionDetail) => infusionDetail.medicineId === medicine.id
				)
			: true
	)

	const commonsFields = [
		{
			key: 'instruction',
			title: t('medical_order.field.instruction'),
			type: 'text',
			multiple: 2,
			required: false,
			validate: [maxLen(1000)],
		},
		{
			key: 'medicalOrderType',
			title: t('medical_order.field.medical_order_type'),
			type: 'radio',
			options: _enum.medicalOrderTypeOptions,
		},
	].filter(Boolean)

	const dynamicFields = () => {
		switch (values?.medicalOrderType) {
			case EnumConfig.MedicalOrderType.Clinical:
				return [
					{
						key: 'clinicalMedicalOrder.clinicalMedicalOrderDetails',
						title: t('medical_order.field.clinical_medical_order.details.details'),
						type: 'array',
						of: [
							{
								key: 'medicalServiceId',
								title: t('medical_order.field.clinical_medical_order.details.medical_service'),
								type: 'select-dialog',
								options: medicalServiceStore.data.map((medicalService) => ({
									value: medicalService.id,
									label: medicalService,
									searchKey: `${medicalService.name}_${medicalService.code}`,
								})),
								remainOptions: remainMedicalServices.map((medicalService) => ({
									value: medicalService.id,
									label: medicalService,
									searchKey: `${medicalService.name}_${medicalService.code}`,
								})),
								renderOption: (_, label) => (
									<Stack key={label.id} sx={{ width: '100%' }}>
										{[
											{ value: 'name', label: t('medical_service.field.name') },
											{ value: 'code', label: t('medical_service.field.code') },
											{ value: 'specialtyName', label: t('medical_service.field.specialty') },
											{
												value: (medicalService) => formatCurrencyBasedOnCurrentLanguage(medicalService.price),
												label: t('medical_service.field.price'),
											},
										].map((field) => {
											const displayValue =
												typeof field.value === 'function'
													? field.value(label)
													: getObjectValueFromStringPath(label, field.value)

											return (
												<Stack key={field.label} direction={'row'} justifyContent={'space-between'}>
													<Typography variant='body2' color='text.secondary'>
														{field.label}
													</Typography>
													<Typography variant='body2' color='text.secondary' textAlign={'right'}>
														{displayValue}
													</Typography>
												</Stack>
											)
										})}
									</Stack>
								),
								renderOptionValue: (_, label) => label?.name || '',
							},
							{
								key: 'quantity',
								title: t('medical_order.field.clinical_medical_order.details.quantity'),
								type: 'number',
								validate: [numberHigherThan(0)],
							},
						],
					},
				]
			case EnumConfig.MedicalOrderType.Infusion:
				return [
					{
						key: 'infusionMedicalOrder.infusionMedicalOrderDetails',
						title: t('medical_order.field.infusion_medical_order.details.details'),
						type: 'array',
						direction: 'column',
						of: [
							{
								key: 'rate',
								title: t('medical_order.field.infusion_medical_order.details.rate'),
								type: 'text',
								validate: [maxLen(100)],
							},
							{
								key: 'frequency',
								title: t('medical_order.field.infusion_medical_order.details.frequency'),
								type: 'text',
								validate: [maxLen(100)],
							},
							{
								key: 'duration',
								title: t('medical_order.field.infusion_medical_order.details.duration'),
								type: 'text',
								validate: [maxLen(100)],
							},
							{
								key: 'note',
								title: t('medical_order.field.infusion_medical_order.details.note'),
								type: 'text',
								multiple: 2,
								required: false,
								validate: [maxLen(1000)],
							},
							{
								key: 'medicineId',
								title: t('medical_order.field.infusion_medical_order.details.medicine'),
								type: 'select-dialog',
								options: infusionMedicineStore.data.map((medicine) => ({
									value: medicine.id,
									label: medicine,
									searchKey: medicine.name,
								})),
								remainOptions: remainMedicines.map((medicine) => ({
									value: medicine.id,
									label: medicine,
									searchKey: medicine.name,
								})),
								renderOption: (_, label) => (
									<Stack
										key={label.id}
										direction='row'
										spacing={1}
										alignItems='center'
										sx={{ width: '100%' }}
									>
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
						],
					},
				]
			case EnumConfig.MedicalOrderType.Instruction:
				return [
					{
						key: 'instructionOrderToggles',
						title: '',
						type: 'object',
						direction: 'row',
						of: [
							{
								key: 'isPositionOrderEnabled',
								title: t('medical_order.field.instruction_medical_order.position_order.position_order'),
								type: 'checkbox',
							},
							{
								key: 'isRespiratorySupportOrderEnabled',
								title: t(
									'medical_order.field.instruction_medical_order.respiratory_support_order.respiratory_support_order'
								),
								type: 'checkbox',
							},
							{
								key: 'isNutritionOrderEnabled',
								title: t('medical_order.field.instruction_medical_order.nutrition_order.nutrition_order'),
								type: 'checkbox',
							},
							{
								key: 'isNursingCareOrderEnabled',
								title: t(
									'medical_order.field.instruction_medical_order.nursing_care_order.nursing_care_order'
								),
								type: 'checkbox',
							},
						],
					},
					values.instructionOrderToggles?.isPositionOrderEnabled && positionOrderFields(t, _enum),
					values.instructionOrderToggles?.isRespiratorySupportOrderEnabled &&
						respiratorySupportOrderFields(t, _enum),
					values.instructionOrderToggles?.isNutritionOrderEnabled && nutritionOrderFields(t, _enum),
					values.instructionOrderToggles?.isNursingCareOrderEnabled && nursingCareOrderFields(t, _enum),
				].filter(Boolean)
			default:
				return []
		}
	}

	const fields = [...commonsFields, ...dynamicFields()]

	const buildMedicalOrderPayload = (formValues) => {
		const applyInstructionToggles = (payload, toggles) => {
			const toggleFieldMap = {
				isPositionOrderEnabled: 'positionOrder',
				isRespiratorySupportOrderEnabled: 'respiratorySupportOrder',
				isNutritionOrderEnabled: 'nutritionOrder',
				isNursingCareOrderEnabled: 'nursingCareOrder',
			}

			Object.entries(toggleFieldMap).forEach(([toggle, field]) => {
				if (!toggles?.[toggle]) payload[field] = null
			})
		}

		const cleanupByType = (payload, type) => {
			const typeFieldMap = {
				[EnumConfig.MedicalOrderType.Instruction]: 'instructionMedicalOrder',
				[EnumConfig.MedicalOrderType.Clinical]: 'clinicalMedicalOrder',
				[EnumConfig.MedicalOrderType.Infusion]: 'infusionMedicalOrder',
			}

			const activeField = typeFieldMap[type]

			Object.values(typeFieldMap).forEach((field) => {
				if (field !== activeField) {
					payload[field] = null
				}
			})
		}

		const { instructionOrderToggles, ...payload } = formValues

		if (formValues.medicalOrderType === EnumConfig.MedicalOrderType.Instruction) {
			applyInstructionToggles(payload, instructionOrderToggles)
		}

		cleanupByType(payload, formValues.medicalOrderType)

		return payload
	}

	return (
		<GenericFormDialog
			open={open}
			onClose={onClose}
			title={t('medical_order.dialog.title.create_medical_order')}
			fields={fields}
			initialValues={finalInitialValues}
			onValuesChange={(values) => setValues(values)}
			onSubmit={async ({ values }) => {
				await onSubmit(buildMedicalOrderPayload(values))
			}}
			additionalButtons={
				values?.medicalOrderType === EnumConfig.MedicalOrderType.Instruction
					? [
							{
								label: t('medical_order.button.save_as_draft'),
								color: 'secondary',
								variant: 'outlined',
								onClick: async ({ values }) => {
									await onCreateInstructionMedicalOrderAsDraft(buildMedicalOrderPayload(values))
								},
							},
						]
					: undefined
			}
			submitButtonColor={'primary'}
			submitLabel={t('button.create')}
			maxWidth='lg'
		/>
	)
}

export default CreateMedicalOrderDialog
