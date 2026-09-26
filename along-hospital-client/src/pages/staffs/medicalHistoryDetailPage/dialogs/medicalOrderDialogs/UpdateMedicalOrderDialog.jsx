import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { maxLen } from '@/utils/validateUtil'
import { useState } from 'react'
import {
	nursingCareOrderFields,
	nutritionOrderFields,
	positionOrderFields,
	respiratorySupportOrderFields,
} from '../../helpers/instructionMedicalOrderFields'

const UpdateMedicalOrderDialog = ({
	open,
	onClose,
	medicalOrder,
	onSubmit = (values) => Promise.resolve(values),
}) => {
	const { t } = useTranslation()
	const _enum = useEnum()

	if (!medicalOrder) {
		return null
	}

	const togglesInitialValues = {
		isPositionOrderEnabled: medicalOrder.positionOrder ? true : false,
		isRespiratorySupportOrderEnabled: medicalOrder.respiratorySupportOrder ? true : false,
		isNutritionOrderEnabled: medicalOrder.nutritionOrder ? true : false,
		isNursingCareOrderEnabled: medicalOrder.nursingCareOrder ? true : false,
	}

	const finalInitialValues = {
		...medicalOrder,
		instructionOrderToggles: togglesInitialValues,
	}

	const [values, setValues] = useState(finalInitialValues)

	const commonsFields = [
		{
			key: 'instruction',
			title: t('medical_order.field.instruction'),
			type: 'text',
			multiple: 2,
			required: false,
			validate: [maxLen(1000)],
		},
	]

	const instructionOrderFields = [
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
		values.instructionOrderToggles?.isPositionOrderEnabled && positionOrderFields(t, _enum, ''),
		values.instructionOrderToggles?.isRespiratorySupportOrderEnabled &&
			respiratorySupportOrderFields(t, _enum, ''),
		values.instructionOrderToggles?.isNutritionOrderEnabled && nutritionOrderFields(t, _enum, ''),
		values.instructionOrderToggles?.isNursingCareOrderEnabled && nursingCareOrderFields(t, _enum, ''),
	].filter(Boolean)

	const fields = [...commonsFields, ...instructionOrderFields]

	return (
		<GenericFormDialog
			open={open}
			onClose={onClose}
			title={t('medical_order.dialog.title.update_medical_order')}
			fields={fields}
			initialValues={finalInitialValues}
			onValuesChange={(values) => setValues(values)}
			onSubmit={async ({ values }) => {
				const { instructionOrderToggles, ...restValues } = values

				const toggleFieldMap = {
					isPositionOrderEnabled: 'positionOrder',
					isRespiratorySupportOrderEnabled: 'respiratorySupportOrder',
					isNutritionOrderEnabled: 'nutritionOrder',
					isNursingCareOrderEnabled: 'nursingCareOrder',
				}

				Object.entries(toggleFieldMap).forEach(([toggleKey, fieldKey]) => {
					if (!instructionOrderToggles?.[toggleKey]) {
						restValues[fieldKey] = null
					}
				})

				await onSubmit(restValues)
			}}
			submitButtonColor={'success'}
			submitLabel={t('button.update')}
			maxWidth='lg'
		/>
	)
}

export default UpdateMedicalOrderDialog
