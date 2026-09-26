import {
	maxLen,
	numberHigherThan,
	numberHigherThanOrEqual,
	numberRange,
} from '@/utils/validateUtil'

export const positionOrderFields = (t, _enum, prefixKey = 'instructionMedicalOrder.') => ({
	key: `${prefixKey}positionOrder`,
	title: t('medical_order.field.instruction_medical_order.position_order.position_order'),
	type: 'object',
	direction: 'column',
	of: [
		{
			key: 'positionOrderType',
			title: t('medical_order.field.instruction_medical_order.position_order.position_order_type'),
			type: 'select',
			options: _enum.positionOrderTypeOptions,
		},
		{
			key: 'instruction',
			title: t('medical_order.field.instruction_medical_order.position_order.instruction'),
			type: 'text',
			multiple: 2,
			required: false,
			validate: [maxLen(500)],
		},
	],
})

export const respiratorySupportOrderFields = (
	t,
	_enum,
	prefixKey = 'instructionMedicalOrder.'
) => ({
	key: `${prefixKey}respiratorySupportOrder`,
	title: t(
		'medical_order.field.instruction_medical_order.respiratory_support_order.respiratory_support_order'
	),
	type: 'object',
	direction: 'column',
	of: [
		{
			key: 'respiratorySupportOrderType',
			title: t(
				'medical_order.field.instruction_medical_order.respiratory_support_order.respiratory_support_order_type'
			),
			type: 'select',
			options: _enum.respiratorySupportOrderTypeOptions,
		},
		{
			key: 'oxygenFlow',
			title: t('medical_order.field.instruction_medical_order.respiratory_support_order.oxygen_flow'),
			type: 'number',
			validate: [numberHigherThan(0)],
		},
		{
			key: 'fiO2',
			title: t('medical_order.field.instruction_medical_order.respiratory_support_order.fio2'),
			type: 'number',
			validate: [numberRange(0.21, 1.0)],
		},
		{
			key: 'instruction',
			title: t('medical_order.field.instruction_medical_order.respiratory_support_order.instruction'),
			type: 'text',
			multiple: 2,
			required: false,
			validate: [maxLen(500)],
		},
	],
})

export const nutritionOrderFields = (t, _enum, prefixKey = 'instructionMedicalOrder.') => ({
	key: `${prefixKey}nutritionOrder`,
	title: t('medical_order.field.instruction_medical_order.nutrition_order.nutrition_order'),
	type: 'object',
	direction: 'column',
	of: [
		{
			key: 'nutritionOrderType',
			title: t('medical_order.field.instruction_medical_order.nutrition_order.nutrition_order_type'),
			type: 'select',
			options: _enum.nutritionOrderTypeOptions,
		},
		{
			key: 'instruction',
			title: t('medical_order.field.instruction_medical_order.nutrition_order.instruction'),
			type: 'text',
			multiple: 2,
			required: false,
			validate: [maxLen(500)],
		},
	],
})

export const nursingCareOrderFields = (t, _enum, prefixKey = 'instructionMedicalOrder.') => ({
	key: `${prefixKey}nursingCareOrder`,
	title: t('medical_order.field.instruction_medical_order.nursing_care_order.nursing_care_order'),
	type: 'object',
	direction: 'column',
	of: [
		{
			key: 'nursingCareOrderLevel',
			title: t(
				'medical_order.field.instruction_medical_order.nursing_care_order.nursing_care_order_level'
			),
			type: 'select',
			options: _enum.nursingCareOrderLevelOptions,
		},
		{
			key: 'monitorIntervalHour',
			title: t(
				'medical_order.field.instruction_medical_order.nursing_care_order.monitor_interval_hour'
			),
			type: 'number',
			validate: [numberHigherThanOrEqual(1)],
		},
	],
})
