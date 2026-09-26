import { EnumConfig } from '@/configs/enumConfig'
import useEnum from '@/hooks/useEnum'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import { formatTimeToHourMinute } from '@/utils/formatDateUtil'
import { isFutureDate, maxLen } from '@/utils/validateUtil'
import { Button, Stack, Typography } from '@mui/material'
import { useEffect, useMemo, useState } from 'react'
import { renderEmptyFallback } from '@/utils/handleStringUtil'

const leaveInitialValues = {
	leaveType: '',
	leaveUnit: EnumConfig.LeaveUnit.Day,
	fromDate: '',
	toDate: '',
	shiftDate: '',
	shiftId: '',
	reason: '',
}

const formatShiftOptionLabel = (shift) =>
	`${renderEmptyFallback(shift?.name)} (${formatTimeToHourMinute(shift?.startTime)} - ${formatTimeToHourMinute(shift?.endTime)})`

const LeaveRequestFormDialog = ({
	open = false,
	loading = false,
	shifts = [],
	onCancel = () => {},
	onSubmit = async (values) => values,
}) => {
	const { t } = useTranslation()
	const _enum = useEnum()
	const [submitted, setSubmitted] = useState(false)
	const { values, handleChange, setField, reset, registerRef, validateAll } =
		useForm(leaveInitialValues)
	const { renderField, hasRequiredMissing } = useFieldRenderer(
		values,
		setField,
		handleChange,
		registerRef,
		submitted,
		'outlined',
		'medium'
	)

	const isShiftUnit = values.leaveUnit === EnumConfig.LeaveUnit.Shift
	const shiftOptions = useMemo(
		() =>
			[...(Array.isArray(shifts) ? shifts : [])]
				.sort((first, second) =>
					String(first?.startTime || '').localeCompare(String(second?.startTime || ''))
				)
				.map((shift) => ({
					value: shift?.id,
					label: formatShiftOptionLabel(shift),
				})),
		[shifts]
	)

	useEffect(() => {
		if (open) {
			setSubmitted(false)
			reset(leaveInitialValues)
		}
	}, [open, reset])

	useEffect(() => {
		if (isShiftUnit) {
			if (values.fromDate || values.toDate) {
				setField('fromDate', '')
				setField('toDate', '')
			}
			return
		}

		if (values.shiftDate || values.shiftId) {
			setField('shiftDate', '')
			setField('shiftId', '')
		}
	}, [isShiftUnit, setField, values.fromDate, values.shiftDate, values.shiftId, values.toDate])

	const leaveFields = useMemo(() => {
		const baseFields = [
			{
				key: 'leaveType',
				title: t('leave_request.field.leave_type'),
				type: 'select',
				options: _enum.leaveTypeOptions,
			},
			{
				key: 'leaveUnit',
				title: t('leave_request.field.leave_unit'),
				type: 'select',
				options: _enum.leaveUnitOptions,
			},
		]

		const reasonField = {
			key: 'reason',
			title: t('leave_request.field.reason'),
			type: 'text',
			required: false,
			multiple: 3,
			validate: [maxLen(255)],
		}

		if (isShiftUnit) {
			return [
				...baseFields,
				{
					key: 'shiftDate',
					title: t('leave_request.field.date'),
					type: 'date',
					validate: [isFutureDate()],
				},
				{
					key: 'shiftId',
					title: t('leave_request.field.shift'),
					type: 'select',
					options: shiftOptions,
				},
				reasonField,
			]
		}

		return [
			...baseFields,
			{
				key: 'dateRange',
				title: t('leave_request.field.date_range'),
				type: 'daterange',
				from: {
					key: 'fromDate',
					label: t('leave_request.field.from_date'),
					validate: [isFutureDate()],
				},
				to: {
					key: 'toDate',
					label: t('leave_request.field.to_date'),
					validate: [isFutureDate()],
				},
			},
			reasonField,
		]
	}, [isShiftUnit, shiftOptions, _enum.leaveTypeOptions, _enum.leaveUnitOptions, t])

	const handleSubmit = async () => {
		setSubmitted(true)

		const isValid = validateAll()
		const isMissing = hasRequiredMissing(leaveFields)
		if (!isValid || isMissing) return

		const response = await onSubmit(values)
		if (!response) return

		setSubmitted(false)
		reset(leaveInitialValues)
	}

	return (
		<Stack spacing={2.5}>
			<Stack spacing={0.75}>
				<Typography variant='h6'>{t('leave_request.title.create_leave_request')}</Typography>
				<Typography variant='body2' color='text.secondary'>
					{t('leave_request.title.leave_request')}
				</Typography>
			</Stack>

			<Stack spacing={2}>{leaveFields.map((field) => renderField(field))}</Stack>

			<Stack direction='row' justifyContent='flex-end' spacing={1.5}>
				<Button variant='outlined' color='inherit' onClick={onCancel} disabled={loading}>
					{t('button.cancel')}
				</Button>
				<Button variant='contained' onClick={handleSubmit} disabled={loading}>
					{loading ? t('button.submitting') : t('leave_request.button.create_request')}
				</Button>
			</Stack>
		</Stack>
	)
}

export default LeaveRequestFormDialog
