import { ArrowForwardRounded } from '@mui/icons-material'
import { Stack } from '@mui/material'
import React, { forwardRef, useImperativeHandle, useRef } from 'react'
import ValidationTextField from './ValidationTextField'

/**
 * A date range field using native date inputs.
 *
 * @param {Object} props
 * @param {string} props.fromDate - ISO date string (yyyy-MM-dd)
 * @param {string} props.toDate - ISO date string (yyyy-MM-dd)
 * @param {(fromDate: string, toDate: string) => void} props.onChange
 * @param {string} [props.fromLabel='From']
 * @param {string} [props.toLabel='To']
 * @param {string} [props.minDate] - Minimum selectable date (yyyy-MM-dd).
 * @param {string} [props.maxDate] - Maximum selectable date (yyyy-MM-dd).
 * @param {((value: string) => string | true) | ((value: string) => string | true)[]} [props.fromValidate] - Validation for the "from" field.
 * @param {((value: string) => string | true) | ((value: string) => string | true)[]} [props.toValidate] - Validation for the "to" field.
 */

/**
 * @param {import('@mui/material').TextFieldProps & Object} props
 * @param {React.Ref} ref
 */
const DateRangeField = (
	{
		fromDate = '',
		toDate = '',
		onChange,
		fromLabel = 'From',
		toLabel = 'To',
		minDate,
		maxDate,
		fromValidate,
		toValidate,
		...props
	},
	ref
) => {
	const fromRef = useRef()
	const toRef = useRef()

	const normalizeRange = (nextFromDate, nextToDate) => {
		if (nextFromDate && nextToDate && nextFromDate > nextToDate) {
			return {
				fromDate: nextToDate,
				toDate: nextFromDate,
			}
		}

		return {
			fromDate: nextFromDate,
			toDate: nextToDate,
		}
	}

	useImperativeHandle(ref, () => ({
		validate: () => {
			const a = fromRef.current?.validate() ?? true
			const b = toRef.current?.validate() ?? true
			return a && b
		},
	}))

	return (
		<Stack direction='row' spacing={1.5} alignItems='center'>
			<ValidationTextField
				ref={fromRef}
				type='date'
				label={fromLabel}
				value={fromDate || ''}
				onChange={(e) => {
					const normalizedRange = normalizeRange(e.target.value, toDate)
					onChange?.(normalizedRange.fromDate, normalizedRange.toDate)
				}}
				validate={fromValidate}
				slotProps={{
					htmlInput: {
						min: minDate || undefined,
						max: toDate || maxDate || undefined,
					},
				}}
				sx={{ flex: 1 }}
				{...props}
			/>
			<ArrowForwardRounded color='action' />
			<ValidationTextField
				ref={toRef}
				type='date'
				label={toLabel}
				value={toDate || ''}
				onChange={(e) => {
					const normalizedRange = normalizeRange(fromDate, e.target.value)
					onChange?.(normalizedRange.fromDate, normalizedRange.toDate)
				}}
				validate={toValidate}
				slotProps={{
					htmlInput: {
						min: fromDate || minDate || undefined,
						max: maxDate || undefined,
					},
				}}
				sx={{ flex: 1 }}
				{...props}
			/>
		</Stack>
	)
}

export default forwardRef(DateRangeField)
