import MultipleSelectDialog from '@/components/dialogs/commons/MultipleSelectDialog'
import SelectDialog from '@/components/dialogs/commons/SelectDialog'
import useTranslation from '@/hooks/useTranslation'
import { getObjectMerged } from '@/utils/handleObjectUtil'
import { isEmail, isNumber, isPhone, isRequired } from '@/utils/validateUtil'
import { Box, MenuItem, TextField } from '@mui/material'
import React, { forwardRef, useCallback, useImperativeHandle, useMemo, useState } from 'react'

/**
 * @typedef {Object} CustomProps
 * @property {string} label
 * @property {string|number} value
 * @property {(value: string|number) => void} onChange
 * @property {((value: string|number) => string | true) | ((value: string|number) => string | true)[]} [validate]
 * @property {{value: string|number, label: any, disabled?: boolean}[]} [options]
 * @property {{value: string|number, label: any, disabled?: boolean}[]} [remainOptions]
 * @property {(value: string|number, label: any) => React.ReactNode} [renderOption]
 * @property {(value: string|number, label: any) => React.ReactNode} [renderOptionValue]
 * @property {string|number} [minValue]
 * @property {string|number} [maxValue]
 * @property {boolean} [readOnly=false]
 */

/**
 * @param {import('@mui/material').TextFieldProps & CustomProps} props
 * @param {React.Ref} ref
 *
 */
const ValidationTextField = (
	{
		label,
		type = 'text',
		required = true,
		value,
		onChange,
		validate,
		options = [],
		remainOptions = undefined,
		renderOption,
		renderOptionValue,
		minValue,
		maxValue,
		readOnly = false,
		...props
	},
	ref
) => {
	const [error, setError] = useState('')
	const [openMultipleSelect, setOpenMultipleSelect] = useState(false)
	const [openSelectDialog, setOpenSelectDialog] = useState(false)
	const { t } = useTranslation()

	const { slotProps, ...restProps } = props

	const isReadOnlyOrDisabled = useMemo(() => {
		return readOnly || restProps.disabled || restProps.readOnly
	}, [readOnly, restProps.disabled, restProps.readOnly])

	const isMultipleSelect = useMemo(() => {
		return type === 'select' && restProps.multiline === true
	}, [type, restProps.multiline])

	const isSelectDialog = useMemo(() => {
		return type === 'select-dialog'
	}, [type])

	const userRules = useMemo(() => {
		if (!validate) return []
		return Array.isArray(validate) ? validate : [validate]
	}, [validate])

	const builtinRules = useMemo(() => {
		const rs = []
		if (required) rs.push(isRequired())
		if (type === 'email') rs.push(isEmail())
		if (type === 'number') rs.push(isNumber())
		if (type === 'tel') rs.push(isPhone())

		return rs
	}, [required, type])

	const allRules = useMemo(() => [...builtinRules, ...userRules], [builtinRules, userRules])

	const runWith = useCallback(
		(val, { skipEmpty = false } = {}) => {
			const isEmpty = val === '' || val === undefined || val === null
			if (skipEmpty && isEmpty) {
				setError('')
				return true
			}
			for (const r of allRules) {
				const res = r(val)
				if (res !== true) {
					setError(res)
					return false
				}
			}
			setError('')
			return true
		},
		[allRules]
	)

	const run = useCallback(() => runWith(value), [runWith, value])

	useImperativeHandle(ref, () => ({ validate: run }), [run])

	const internalSlotProps = useMemo(() => {
		const inputProps = {}

		if (minValue !== undefined) inputProps.min = minValue
		if (maxValue !== undefined) inputProps.max = maxValue

		const result = {}

		if (Object.keys(inputProps).length > 0) {
			result.input = { ...result.input, inputProps }
		}

		if (readOnly) {
			result.input = { ...result.input, readOnly: true }
		}

		if (type === 'select' || type === 'select-dialog') {
			result.inputLabel = { shrink: true }
			result.select = {
				displayEmpty: true,
				multiple: !!restProps.multiline,
				open: isMultipleSelect || isSelectDialog ? false : undefined,
			}

			if (renderOptionValue) {
				result.select.native = false
				result.select.renderValue = (selected) => {
					const findOption = (val) => options.find((opt) => String(opt.value) === String(val))
					const render = Array.isArray(selected) ? (
						<Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 0.5 }}>
							{selected.map((value) => renderOptionValue(value, findOption(value)?.label))}
						</Box>
					) : (
						renderOptionValue(selected, findOption(selected)?.label)
					)

					return React.isValidElement(render)
						? render
						: React.createElement(React.Fragment, null, render)
				}
			}
		} else if (type === 'date' || type === 'time' || type === 'datetime-local' || type === 'file') {
			result.inputLabel = { shrink: true }
		}

		return Object.keys(result).length > 0 ? result : undefined
	}, [
		type,
		minValue,
		maxValue,
		readOnly,
		restProps.multiline,
		isMultipleSelect,
		renderOptionValue,
		options,
	])

	const mergedSlotProps = useMemo(
		() => getObjectMerged(internalSlotProps, slotProps),
		[internalSlotProps, slotProps]
	)

	const displayOptions = useMemo(() => {
		const map = new Map()
		const push = (opt) => {
			if (!opt) return
			const k = String(opt.value)
			if (!map.has(k)) map.set(k, opt)
		}

		const src = (remainOptions && Array.isArray(remainOptions) ? remainOptions : options) || []
		src.forEach(push)

		if (value !== undefined && value !== null && value !== '') {
			const vKey = String(value)
			if (!map.has(vKey)) {
				const found = (options || []).find((o) => String(o.value) === vKey)
				push(found)
			}
		}

		return Array.from(map.values())
	}, [remainOptions, options, value])

	return (
		<>
			<TextField
				label={label}
				value={value ?? undefined}
				onChange={(e) => {
					if (error) runWith(e.target.value, { skipEmpty: true })
					onChange?.(e)
				}}
				onBlur={() => {
					if ((!isMultipleSelect || !openMultipleSelect) && (!isSelectDialog || !openSelectDialog)) {
						run()
					}
				}}
				onClick={() => {
					if (isMultipleSelect && !isReadOnlyOrDisabled) setOpenMultipleSelect(true)
					if (isSelectDialog && !isReadOnlyOrDisabled) setOpenSelectDialog(true)
				}}
				error={!!error}
				type={type}
				helperText={error}
				required={required}
				fullWidth
				variant='outlined'
				select={type === 'select' || type === 'select-dialog'}
				slotProps={mergedSlotProps}
				{...restProps}
			>
				<MenuItem value='' disabled>
					-- {t('text.select_options')} --
				</MenuItem>
				{displayOptions &&
					displayOptions.length > 0 &&
					displayOptions.map((opt) => (
						<MenuItem key={String(opt.value)} value={opt.value} disabled={opt.disabled}>
							{renderOption ? renderOption(opt.value, opt.label) : opt.label}
						</MenuItem>
					))}
			</TextField>

			{isMultipleSelect && !isReadOnlyOrDisabled && (
				<MultipleSelectDialog
					open={openMultipleSelect}
					onClose={() => setOpenMultipleSelect(false)}
					options={options}
					value={value || []}
					onChange={(selectedValues) => {
						const e = {
							target: {
								name: props.name,
								value: selectedValues,
							},
						}
						onChange?.(e)
					}}
					renderOption={renderOption}
					title={label}
				/>
			)}

			{isSelectDialog && !isReadOnlyOrDisabled && (
				<SelectDialog
					open={openSelectDialog}
					onClose={() => setOpenSelectDialog(false)}
					options={displayOptions}
					value={value}
					onChange={(selectedValue) => {
						const e = {
							target: {
								name: props.name,
								value: selectedValue,
							},
						}
						onChange?.(e)
					}}
					renderOption={renderOption}
					title={label}
				/>
			)}
		</>
	)
}

export default forwardRef(ValidationTextField)

// For select text field
/*
<ValidationTextField
  label={'Project Name'}
  select
  value={value || ''}
  onChange={setValue}
  >
    {array.map((element) => (
      <MenuItem key={element.id} value={element.id}>
        {element.name}
      </MenuItem>
    ))}
</ValidationTextField>
*/
