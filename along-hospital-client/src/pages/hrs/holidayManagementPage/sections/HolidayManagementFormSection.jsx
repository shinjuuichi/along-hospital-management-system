import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import useTranslation from '@/hooks/useTranslation'
import { parseDateKey } from '@/utils/formatDateUtil'
import { maxLen, numberRange } from '@/utils/validateUtil'
import { useEffect, useMemo, useState } from 'react'

const MAX_NAME_LENGTH = 255
const MAX_DAY = 31
const MAX_MONTH = 12
const MAX_DATE_RANGE_DAYS = 366

const useHolidayFields = ({ t, values, lockHolidayType = false }) => {
	const holidayType = values?.holidayType || 'ANNUAL'
	const dateType = values?.dateType || 'SPECIFIC'

	const isSpecificDay =
		holidayType === 'ANNUAL' || (holidayType === 'SPECIFIC' && dateType === 'SPECIFIC')
	const isDateRange = holidayType === 'SPECIFIC' && dateType === 'DATERANGE'

	return useMemo(
		() => [
			{
				key: 'name',
				title: t('holiday.field.name'),
				type: 'text',
				validate: [maxLen(MAX_NAME_LENGTH)],
			},
			{
				key: 'holidayType',
				title: t('holiday.field.holiday_type'),
				type: 'radio',
				options: [
					{ label: t('holiday.option.annual'), value: 'ANNUAL' },
					{ label: t('holiday.option.specific'), value: 'SPECIFIC' },
				],
				props: lockHolidayType ? { disabled: true } : {},
			},
			...(holidayType === 'SPECIFIC'
				? [
						{
							key: 'dateType',
							title: t('holiday.field.date_type'),
							type: 'radio',
							options: [
								{ label: t('holiday.option.specific_day'), value: 'SPECIFIC' },
								{ label: t('holiday.option.date_range'), value: 'DATERANGE' },
							],
							props: lockHolidayType ? { disabled: true } : {},
						},
					]
				: []),
			...(isSpecificDay
				? [
						{
							key: 'day',
							title: t('holiday.field.day'),
							type: 'number',
							validate: [numberRange(1, MAX_DAY)],
						},
						{
							key: 'month',
							title: t('holiday.field.month'),
							type: 'number',
							validate: [numberRange(1, MAX_MONTH)],
						},
					]
				: []),
			...(isSpecificDay && holidayType === 'SPECIFIC'
				? [
						{
							key: 'year',
							title: t('holiday.field.year'),
							type: 'number',
							required: false,
						},
					]
				: []),
			...(isDateRange
				? [
						{
							key: 'dateRange',
							title: t('holiday.field.date_range'),
							type: 'daterange',
							from: {
								key: 'fromDate',
								label: t('holiday.field.from_date'),
								validate: [],
							},
							to: {
								key: 'toDate',
								label: t('holiday.field.to_date'),
								validate: [],
							},
						},
					]
				: []),
		],
		[holidayType, dateType, lockHolidayType, t]
	)
}

const validate = ({ values, t }) => {
	const newErrors = {}

	if (!values.name?.trim()) {
		newErrors.name = t('error.required')
	} else if (maxLen(MAX_NAME_LENGTH)(values.name) !== true) {
		newErrors.name = maxLen(MAX_NAME_LENGTH)(values.name)
	}

	const { holidayType, dateType, day, month, fromDate, toDate } = values

	if (holidayType === 'ANNUAL' || (holidayType === 'SPECIFIC' && dateType === 'SPECIFIC')) {
		if (!day) {
			newErrors.day = t('error.required')
		} else if (numberRange(1, MAX_DAY)(day) !== true) {
			newErrors.day = numberRange(1, MAX_DAY)(day)
		}
		if (!month) {
			newErrors.month = t('error.required')
		} else if (numberRange(1, MAX_MONTH)(month) !== true) {
			newErrors.month = numberRange(1, MAX_MONTH)(month)
		}
	}

	if (holidayType === 'SPECIFIC' && dateType === 'DATERANGE') {
		if (!fromDate) newErrors.fromDate = t('error.required')
		if (!toDate) newErrors.toDate = t('error.required')
		if (fromDate && toDate) {
			const from = parseDateKey(fromDate)
			const to = parseDateKey(toDate)
			if (!from || !to) {
				newErrors.fromDate = t('error.invalid_date')
			} else if (from > to) {
				newErrors.fromDate = t('holiday.error.from_after_to')
			} else {
				const diffDays = Math.ceil((to - from) / (1000 * 60 * 60 * 24))
				if (diffDays + 1 > MAX_DATE_RANGE_DAYS) {
					newErrors.toDate = t('holiday.error.exceeds_max_days')
				}
			}
		}
	}

	return newErrors
}

const prepareData = (values) => {
	const { name, holidayType, dateType, day, month, year, fromDate, toDate } = values

	if (holidayType === 'ANNUAL' || (holidayType === 'SPECIFIC' && dateType === 'SPECIFIC')) {
		return {
			name,
			day: parseInt(day),
			month: parseInt(month),
			...(holidayType === 'SPECIFIC' && year ? { year: parseInt(year) } : {}),
		}
	}

	if (holidayType === 'SPECIFIC' && dateType === 'DATERANGE') {
		return { name, type: 'DATERANGE', fromDate, toDate }
	}

	return values
}

const detectHolidayType = (item) => {
	const isSpecific = !!item?.year || !!item?.fromDate
	const isDateRange = !!item?.fromDate && !!item?.toDate
	return {
		holidayType: isSpecific ? 'SPECIFIC' : 'ANNUAL',
		dateType: isDateRange ? 'DATERANGE' : 'SPECIFIC',
	}
}

const CREATE_INITIAL_VALUES = {
	name: '',
	holidayType: 'ANNUAL',
	dateType: 'SPECIFIC',
	day: '',
	month: '',
	year: '',
	fromDate: '',
	toDate: '',
}

const CreateHolidayDialog = ({ open, onClose, onSubmit }) => {
	const { t } = useTranslation()
	const [values, setValues] = useState(CREATE_INITIAL_VALUES)

	const fields = useHolidayFields({ t, values, lockHolidayType: false })

	const handleSubmit = ({ values, closeDialog }) => {
		const errors = validate({ values, t })
		if (Object.keys(errors).length > 0) return { errors }
		onSubmit({ ...prepareData(values), closeDialog })
	}

	return (
		<GenericFormDialog
			open={open}
			onClose={onClose}
			title={t('holiday.dialog.create_title')}
			initialValues={CREATE_INITIAL_VALUES}
			fields={fields}
			submitLabel={t('button.create')}
			submitButtonColor='success'
			onValuesChange={setValues}
			onSubmit={handleSubmit}
		/>
	)
}

const UpdateHolidayDialog = ({ open, onClose, onSubmit, selectedItem }) => {
	const { t } = useTranslation()

	const initialValues = useMemo(() => {
		if (!selectedItem) return CREATE_INITIAL_VALUES
		const { holidayType, dateType } = detectHolidayType(selectedItem)
		return {
			name: selectedItem.name || '',
			holidayType,
			dateType,
			day: selectedItem.day?.toString() || '',
			month: selectedItem.month?.toString() || '',
			year: selectedItem.year?.toString() || '',
			fromDate: selectedItem.fromDate || '',
			toDate: selectedItem.toDate || '',
		}
	}, [selectedItem])

	const [values, setValues] = useState(initialValues)

	useEffect(() => {
		setValues(initialValues)
	}, [initialValues])

	const fields = useHolidayFields({ t, values, lockHolidayType: true })

	const handleSubmit = ({ values, closeDialog }) => {
		const errors = validate({ values, t })
		if (Object.keys(errors).length > 0) return { errors }
		onSubmit({ ...prepareData(values), closeDialog })
	}

	return (
		<GenericFormDialog
			open={open}
			onClose={onClose}
			title={t('holiday.dialog.update_title')}
			initialValues={initialValues}
			fields={fields}
			submitLabel={t('button.update')}
			submitButtonColor='success'
			onValuesChange={setValues}
			onSubmit={handleSubmit}
		/>
	)
}

const HolidayManagementFormSection = ({
	openCreateForm,
	setOpenCreateForm,
	onCreateSubmit,
	openUpdateForm,
	setOpenUpdateForm,
	selectedItem,
	onUpdateSubmit,
}) => {
	return (
		<>
			<CreateHolidayDialog
				open={openCreateForm}
				onClose={() => setOpenCreateForm(false)}
				onSubmit={onCreateSubmit}
			/>
			<UpdateHolidayDialog
				open={openUpdateForm}
				onClose={() => setOpenUpdateForm(false)}
				onSubmit={onUpdateSubmit}
				selectedItem={selectedItem}
			/>
		</>
	)
}

export default HolidayManagementFormSection
