/* eslint-disable react-hooks/exhaustive-deps */
import AddTileRenderField from '@/components/fieldRenderers/AddTileRenderField'
import DrawingRenderField from '@/components/fieldRenderers/DrawingRenderField'
import FileRenderField from '@/components/fieldRenderers/FileRenderField'
import ImageRenderField from '@/components/fieldRenderers/ImageRenderField'
import ImageTileRenderField from '@/components/fieldRenderers/ImageTileRenderField'
import SearchBar from '@/components/generals/SearchBar'
import DateRangeField from '@/components/textFields/DateRangeField'
import PasswordTextField from '@/components/textFields/PasswordTextField'
import TimeRangeField from '@/components/textFields/TimeRangeField'
import ValidationTextField from '@/components/textFields/ValidationTextField'
import { getImageFromCloud } from '@/utils/commons'
import { isStringArray } from '@/utils/handleBooleanUtil'
import { getObjectValueFromStringPath, normalizeOptions } from '@/utils/handleObjectUtil'
import { Delete } from '@mui/icons-material'
import {
	Box,
	Button,
	Checkbox,
	IconButton,
	Stack,
	ToggleButton,
	ToggleButtonGroup,
	Typography,
} from '@mui/material'
import { useCallback, useEffect, useRef } from 'react'
import useFileUrls from './helpers/useFileUrls'
import useTranslation from './useTranslation'

/**
 * @typedef {Object} FieldDefinition
 * @property {string} key
 * @property {string} title
 * @property {"text" | "search" | "date" | "number" | "email" | "tel" | "password" | "select" | "select-dialog" | "radio" | "checkbox" | "image" | "file" | "object" | "array" | "draw" | "custom" | "daterange" | "timerange"} [type='text']
 * @property {boolean} [required=true]
 * @property {number} [multiple=undefined]
 * @property {Array<string|Object>} [options]
 * @property {Array<string|Object>} [remainOptions]
 * @property {(value: string|number, label: any) => ReactNode} [renderOption]
 * @property {(value: string|number, label: any) => ReactNode} [renderOptionValue]
 * @property {Array<FieldDefinition>} [of]
 * @property {Array<function(string):string>} [validate]
 * @property {string|number} [minValue]
 * @property {string|number} [maxValue]
 * @property {function():void} [onEnterDown]
 * @property {{ key?: string, label?: string, validate?: Array, min?: string }} [from] - Config for the "from" sub-field (daterange/timerange types)
 * @property {{ key?: string, label?: string, validate?: Array, max?: string }} [to] - Config for the "to" sub-field (daterange/timerange types)
 * @property {{ key?: string, label?: string, validate?: Array, min?: string, max?: string }} [date] - Config for the "date" sub-field (timerange type, presence enables date input)
 * @property {import('@mui/material').TextFieldProps} [props]
 */

/**
 * @param {import('@mui/material').TextFieldProps['variant']} [textFieldVariant='standard']
 * @param {import('@mui/material').TextFieldProps['size']} [textFieldSize='medium']
 * @returns {{ renderField: function(FieldDefinition):JSX.Element, hasRequiredMissing: function(Array<FieldDefinition>):boolean }}
 */
export default function useFieldRenderer(
	values,
	setField,
	handleChange = () => {},
	registerRef = () => {},
	submitted,
	textFieldVariant = 'standard',
	textFieldSize = 'medium'
) {
	const normalizedImageKeysRef = useRef(new Set())
	const { getUrlForFile, revokeUrlForFile } = useFileUrls()
	const { t } = useTranslation()
	const baseRenderContext = { values, setField, handleChange, registerRef }

	useEffect(() => {
		normalizedImageKeysRef.current.clear()
	}, [values])

	//#region Helper Functions
	const getImageKeyNames = (fieldKey) => {
		const base = String(fieldKey).split('.').pop() || ''
		const cap = base ? base.charAt(0).toUpperCase() + base.slice(1) : ''
		return {
			remainKey: `remain${cap}`,
			newKey: `new${cap}`,
			removeKey: `remove${cap}`,
		}
	}

	const getImageCount = (f) => {
		const { remainKey, newKey } = getImageKeyNames(f.key)

		const remain = Array.isArray(getObjectValueFromStringPath(values, remainKey))
			? getObjectValueFromStringPath(values, remainKey)
			: []
		const news = Array.isArray(getObjectValueFromStringPath(values, newKey))
			? getObjectValueFromStringPath(values, newKey)
			: []

		return (remain?.length || 0) + (news?.length || 0)
	}

	const getValueFromEvent = (event) => {
		const target = event?.target
		if (!target) return event
		if (target.type === 'checkbox') return !!target.checked
		return target.value
	}

	const hasRequiredMissing = useCallback(
		(fields) => {
			const checkField = (f, v) => {
				if (f.required !== undefined && !f.required) return false

				if (f.type === 'checkbox') {
					if (f.required !== true) return false
					return v !== true
				}

				if (f.type === 'draw') {
					return !v || !(v instanceof File)
				}

				if (f.type === 'image') {
					const max = Number.isFinite(f.multiple) ? Math.max(1, Number(f.multiple)) : 1
					if (max === 1) {
						return !v
					}

					return getImageCount(f) === 0
				}

				if (f.type === 'file') {
					return Array.isArray(v) ? v.length === 0 : !v
				}

				if (f.type === 'array') return !Array.isArray(v) || v.length === 0
				if (f.type === 'object') {
					const obj = v || {}
					const children = f.of || []

					return children.some((c) => {
						const isRequired = c.required !== false

						if (!isRequired) return false

						const value = getObjectValueFromStringPath(obj, c.key)
						return value === '' || value == null
					})
				}
				if (f.type === 'daterange') {
					const from = f.from || {}
					const to = f.to || {}
					const fk = from.key || 'fromDate'
					const tk = to.key || 'toDate'
					return !getObjectValueFromStringPath(values, fk) || !getObjectValueFromStringPath(values, tk)
				}
				if (f.type === 'timerange') {
					const from = f.from || {}
					const to = f.to || {}
					const dateConfig = f.date || null
					const fk = from.key || 'startTime'
					const tk = to.key || 'endTime'
					const missingTime =
						!getObjectValueFromStringPath(values, fk) || !getObjectValueFromStringPath(values, tk)
					const missingDate =
						dateConfig && !getObjectValueFromStringPath(values, dateConfig.key || 'date')
					return missingTime || missingDate
				}

				return v == null || v === ''
			}

			return fields.some((f) => {
				/*

				// uncomment to debug missing required fields

				const error = checkField(f, getObjectValueFromStringPath(values, f.key))
				if (error) console.log(f, getObjectValueFromStringPath(values, f.key))
				
				*/
				return checkField(f, getObjectValueFromStringPath(values, f.key))
			})
		},
		[values]
	)
	//#endregion

	const createChildRenderContext = ({ containerValues, setChildValue, resolveRefName }) => ({
		values: containerValues || {},
		setField: (childKey, nextVal) => setChildValue(childKey, nextVal),
		handleChange: (event) => {
			const childKey = event?.target?.name
			if (!childKey) return
			setChildValue(childKey, getValueFromEvent(event))
		},
		registerRef: (childKey) => registerRef(resolveRefName(childKey)),
	})

	//#region Render Standard Field Types
	const renderStandard = (field, context = baseRenderContext) => {
		const currentValues = context.values
		const opts = normalizeOptions(field.options || [])
		const remainOpts = field.remainOptions ? normalizeOptions(field.remainOptions) : undefined

		return (
			<ValidationTextField
				key={field.key}
				variant={textFieldVariant}
				ref={context.registerRef(field.key)}
				name={field.key}
				label={field.title}
				required={field.required ?? true}
				type={field.type || 'text'}
				options={opts}
				remainOptions={remainOpts}
				renderOption={field.renderOption}
				renderOptionValue={field.renderOptionValue}
				value={getObjectValueFromStringPath(currentValues, field.key) || ''}
				onChange={context.handleChange}
				validate={field.validate}
				minValue={field.minValue}
				maxValue={field.maxValue}
				multiline={!!field.multiple}
				minRows={field.multiple}
				size={textFieldSize}
				{...(field.props || {})}
			/>
		)
	}

	const renderPassword = (field, context = baseRenderContext) => {
		const currentValues = context.values
		return (
			<PasswordTextField
				key={field.key}
				variant={textFieldVariant}
				ref={context.registerRef(field.key)}
				name={field.key}
				label={field.title}
				required={field.required ?? true}
				value={getObjectValueFromStringPath(currentValues, field.key) || ''}
				onChange={context.handleChange}
				validate={field.validate}
				size={textFieldSize}
				{...(field.props || {})}
			/>
		)
	}

	const renderCheckbox = (field, context = baseRenderContext) => {
		const checked = getObjectValueFromStringPath(context.values, field.key) === true
		const required = field.required ?? false
		const disabled = !!(field.props?.disabled || field.props?.readOnly)
		const showError = submitted && required && !checked

		const toggle = () => {
			if (disabled) return
			context.setField(field.key, !checked)
		}

		return (
			<Stack key={field.key} spacing={0.5}>
				<Box
					onClick={toggle}
					sx={(theme) => ({
						display: 'flex',
						alignItems: 'center',
						gap: 1,
						borderRadius: 1,
						px: 1,
						py: 0.5,
						border: '1px solid',
						borderColor: showError
							? theme.palette.error.main
							: checked
								? theme.palette.primary.main
								: theme.palette.text.primary,
						backgroundColor: checked ? theme.palette.primary.main + '10' : undefined,
						cursor: disabled ? 'not-allowed' : 'pointer',
						opacity: disabled ? 0.6 : 1,
						transition: 'all 0.2s ease',
						'&:hover': {
							backgroundColor: disabled
								? undefined
								: checked
									? theme.palette.primary.main + '15'
									: theme.palette.action.hover,
						},
					})}
				>
					<Checkbox
						checked={checked}
						disabled={disabled}
						onChange={(e) => context.setField(field.key, e.target.checked)}
						onClick={(e) => e.stopPropagation()}
						disableRipple
					/>
					<Typography sx={{ flex: 1, userSelect: 'none' }}>{field.title}</Typography>
				</Box>
				{showError && (
					<Typography variant='caption' color='error'>
						{t('error.required')}
					</Typography>
				)}
			</Stack>
		)
	}

	const renderRadio = (field, context = baseRenderContext) => {
		const value = getObjectValueFromStringPath(context.values, field.key) ?? ''
		const options = normalizeOptions(field.options || [])
		const required = field.required ?? true
		const disabled = !!(field.props?.disabled || field.props?.readOnly)
		const showError = submitted && required && (value == null || value === '')

		const columns = 6
		const remainder = options.length % columns
		const lastRowStart = options.length - remainder

		return (
			<Stack key={field.key} spacing={0.75}>
				{field.title ? <Typography variant='subtitle2'>{field.title}</Typography> : null}
				<ToggleButtonGroup
					fullWidth
					exclusive
					value={value}
					onChange={(_, nextValue) => {
						if (disabled || nextValue == null) return
						context.setField(field.key, nextValue)
					}}
					sx={{
						display: 'grid',
						gridTemplateColumns: `repeat(${columns}, minmax(0, 1fr))`,
						bgcolor: 'background.paper',
						border: '1px solid',
						borderColor: showError ? 'error.main' : 'divider',
						borderRadius: 1,
						overflow: 'hidden',
					}}
				>
					{options.map((opt, index) => {
						let span = 2

						if (remainder !== 0 && index >= lastRowStart) {
							span = Math.max(1, Math.floor(columns / remainder))
						}

						return (
							<ToggleButton
								key={String(opt.value)}
								value={opt.value}
								disabled={disabled || opt.disabled}
								color='warning'
								sx={{
									gridColumn: `span ${span}`,
								}}
							>
								{field.renderOption ? field.renderOption(opt.value, opt.label) : opt.label}
							</ToggleButton>
						)
					})}
				</ToggleButtonGroup>
				{showError && (
					<Typography variant='caption' color='error'>
						{t('error.required')}
					</Typography>
				)}
			</Stack>
		)
	}
	//#endregion

	//#region Custom Render Types
	const renderSearch = (field, context = baseRenderContext) => {
		const value = getObjectValueFromStringPath(context.values, field.key) ?? ''
		const setValue = (val) => context.setField(field.key, val)
		const renderOption = field.renderOption

		const getOptionLabel = renderOption
			? (opt) => {
					try {
						return renderOption(opt)?.props?.children || renderOption(opt) || ''
					} catch {
						return opt?.label || String(opt)
					}
				}
			: (opt) => opt?.label || String(opt)

		return (
			<SearchBar
				key={field.key}
				widthPercent={field.widthPercent ?? 0}
				value={value}
				setValue={setValue}
				placeholder={field.title}
				options={field.options || []}
				getOptionLabel={getOptionLabel}
				onEnterDown={field.onEnterDown}
			/>
		)
	}

	const renderDrawing = (field, context = baseRenderContext) => {
		const required = field.required ?? true
		const hasDrawing = getObjectValueFromStringPath(context.values, field.key) instanceof File
		const showError = submitted && required && !hasDrawing

		return (
			<DrawingRenderField
				key={field.key}
				field={field}
				showError={showError}
				onDrawingChange={(file) => context.setField(field.key, file)}
			/>
		)
	}
	//#endregion

	//#region Image & File Render
	const renderFile = (field, context = baseRenderContext) => {
		const file = getObjectValueFromStringPath(context.values, field.key)
		const required = field.required ?? true
		const showError = submitted && required && !file

		return (
			<FileRenderField
				key={field.key}
				field={field}
				values={context.values}
				setField={context.setField}
				showError={showError}
			/>
		)
	}

	const renderImageSingle = (field, context = baseRenderContext) => {
		const file = getObjectValueFromStringPath(context.values, field.key)
		const required = field.required ?? true
		const showError = submitted && required && !file

		let preview = ''
		if (file instanceof File) {
			preview = URL.createObjectURL(file)
		} else if (typeof file === 'string') {
			preview = getImageFromCloud(file)
		}

		return (
			<ImageRenderField
				key={field.key}
				field={field}
				textFieldVariant={textFieldVariant}
				setField={context.setField}
				showError={showError}
				preview={preview}
			/>
		)
	}

	const renderImageMultiple = (field, context = baseRenderContext) => {
		const currentValues = context.values
		const key = field.key
		const { remainKey, newKey, removeKey } = getImageKeyNames(key)
		const required = field.required ?? true
		const max = Math.max(1, Number(field.multiple) || 1)

		const toPreviewSrc = (val) => {
			if (val instanceof File) return getUrlForFile(val)
			if (typeof val === 'string') return getImageFromCloud(val)
			return ''
		}

		const legacyVal = getObjectValueFromStringPath(currentValues, key)
		if (isStringArray(legacyVal) && !normalizedImageKeysRef.current.has(key)) {
			normalizedImageKeysRef.current.add(key)
			const initialRemain = legacyVal.filter(Boolean).slice()
			context.setField(remainKey, initialRemain)
			context.setField(newKey, [])
			context.setField(removeKey, [])
			context.setField(key, undefined)
		}

		const remain = Array.isArray(getObjectValueFromStringPath(currentValues, remainKey))
			? getObjectValueFromStringPath(currentValues, remainKey)
			: []
		const news = Array.isArray(getObjectValueFromStringPath(currentValues, newKey))
			? getObjectValueFromStringPath(currentValues, newKey)
			: []
		const removedList = Array.isArray(getObjectValueFromStringPath(currentValues, removeKey))
			? getObjectValueFromStringPath(currentValues, removeKey)
			: []

		const total = remain.length + news.length
		const capacityLeft = Math.max(0, max - total)
		const showError = submitted && required && total === 0

		const addFiles = (filesList) => {
			if (!filesList?.length || capacityLeft <= 0) return
			const picked = Array.from(filesList).slice(0, capacityLeft)
			context.setField(newKey, [...news, ...picked])
		}

		const removeRemain = (idx) => {
			if (idx < 0 || idx >= remain.length) return
			const nextRemain = remain.slice()
			const removed = nextRemain.splice(idx, 1)[0]
			context.setField(remainKey, nextRemain)
			context.setField(removeKey, [...removedList, removed])
		}

		const removeNew = (idx) => {
			if (idx < 0 || idx >= news.length) return
			const nextNew = news.slice()
			const removed = nextNew.splice(idx, 1)[0]
			context.setField(newKey, nextNew)
			if (removed instanceof File) revokeUrlForFile(removed)
		}

		const inputId = `${key}__picker`

		return (
			<Stack key={key} spacing={1.25}>
				<Typography variant='subtitle2'>
					{field.title} ({total}/{max})
				</Typography>
				<input
					id={inputId}
					type='file'
					accept='image/*'
					multiple
					style={{ display: 'none' }}
					onChange={(e) => {
						addFiles(e.target.files)
						e.target.value = ''
					}}
				/>
				<Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 1.5, alignItems: 'flex-start' }}>
					{remain.map((url, i) => (
						<ImageTileRenderField
							key={`remain-${i}`}
							src={toPreviewSrc(url)}
							alt={`${field.title}-remain-${i}`}
							onRemove={() => removeRemain(i)}
						/>
					))}

					{news.map((file, i) => {
						const fileKey = (f) => `${f.name}_${f.size}_${f.lastModified}_${i}`
						return (
							<ImageTileRenderField
								key={`new-${fileKey(file)}`}
								src={toPreviewSrc(file)}
								alt={`${field.title}-new-${fileKey(file)}`}
								onRemove={() => removeNew(i)}
							/>
						)
					})}
					{capacityLeft > 0 && <AddTileRenderField remaining={capacityLeft} inputId={inputId} />}
				</Box>

				{showError && (
					<Typography variant='caption' color='error'>
						{t('error.required')}
					</Typography>
				)}
			</Stack>
		)
	}
	//#endregion

	//#region Object and Array Render
	const renderObject = (field) => {
		const obj = getObjectValueFromStringPath(values, field.key) || {}
		const parentProps = field.props || {}
		const direction = field.direction === 'row' ? 'row' : 'column'

		const children = (field.of || []).map((child) => ({
			...child,
			props: { ...parentProps, ...(child.props || {}) },
		}))

		const updateChild = (childKey, nextVal) => setField(field.key, { ...obj, [childKey]: nextVal })
		const childItemSx = direction === 'column' ? { width: '100%' } : { minWidth: 220, flex: 1 }

		const renderChild = (child) => {
			const name = `${field.key}.${child.key}`
			const childContext = createChildRenderContext({
				containerValues: obj,
				setChildValue: updateChild,
				resolveRefName: (childKey) => `${field.key}.${childKey}`,
			})

			return (
				<Box key={name} sx={childItemSx}>
					{renderFieldWithContext(child, childContext)}
				</Box>
			)
		}

		return (
			<Stack
				key={field.key}
				spacing={1.25}
				sx={{
					p: 1.5,
					borderRadius: 1.5,
					border: '1px dashed',
					borderColor: 'divider',
					backgroundColor: 'action.hover',
				}}
			>
				<Typography variant='subtitle2' sx={{ fontWeight: 600 }}>
					{field.title}
				</Typography>
				<Stack
					direction={direction}
					gap={1}
					sx={{
						flexWrap: direction === 'row' ? 'wrap' : 'nowrap',
						width: '100%',
					}}
				>
					{children.map(renderChild)}
				</Stack>
			</Stack>
		)
	}

	const renderArray = (field) => {
		const rows = getObjectValueFromStringPath(values, field.key) || []
		const required = field.required ?? true
		const showListError = submitted && required && rows.length === 0
		const parentProps = field.props || {}
		const isDisabled = parentProps.disabled ?? parentProps.readOnly ?? false
		const direction = field.direction === 'column' ? 'column' : 'row'

		const childFields = (field.of || []).map((child) => ({
			...child,
			props: { ...parentProps, ...(child.props || {}) },
		}))
		const childItemSx = direction === 'column' ? { width: '100%' } : { minWidth: 220, flex: 1 }

		const makeDefaultOf = (children = []) => {
			const o = {}
			for (const f of children) o[f.key] = f.defaultValue ?? (f.type === 'image' ? null : '')
			return o
		}

		const addRow = () => setField(field.key, [...rows, makeDefaultOf(childFields)])
		const removeRow = (idx) =>
			setField(
				field.key,
				rows.filter((_, i) => i !== idx)
			)

		const updateCell = (idx, childKey, nextVal) => {
			const next = rows.slice()
			next[idx] = { ...next[idx], [childKey]: nextVal }
			setField(field.key, next)
		}

		const renderChild = (child, idx) => {
			const name = `${field.key}[${idx}].${child.key}`
			const row = rows[idx] || {}
			const childContext = createChildRenderContext({
				containerValues: row,
				setChildValue: (childKey, nextVal) => updateCell(idx, childKey, nextVal),
				resolveRefName: (childKey) => `${field.key}[${idx}].${childKey}`,
			})

			return (
				<Box key={name} sx={childItemSx}>
					{renderFieldWithContext(child, childContext)}
				</Box>
			)
		}

		return (
			<Stack
				key={field.key}
				spacing={1.25}
				sx={{
					p: 1.5,
					borderRadius: 1.5,
					border: '1px dashed',
					borderColor: 'divider',
					backgroundColor: 'action.hover',
				}}
			>
				<Typography variant='subtitle2' sx={{ fontWeight: 600 }}>
					{field.title}
				</Typography>

				{rows.length === 0 ? (
					<Box
						sx={{
							py: 2,
							px: 1.5,
							textAlign: 'center',
						}}
					>
						<Typography variant='body2' color='text.secondary'>
							{t('text.placeholder.no_data')}
						</Typography>
					</Box>
				) : (
					<Stack spacing={1.5}>
						{rows.map((_, idx) => (
							<Stack
								key={idx}
								direction={direction}
								spacing={1}
								alignItems={direction === 'column' ? 'stretch' : 'center'}
								p={1}
								sx={{
									borderRadius: 1,
									border: '1px solid',
									borderColor: 'divider',
									backgroundColor: 'background.paper',
								}}
							>
								<Stack
									direction={direction}
									gap={1}
									sx={{ flex: 1, flexWrap: direction === 'row' ? 'wrap' : 'nowrap' }}
								>
									{childFields.map((child) => renderChild(child, idx))}
								</Stack>
								{isDisabled ? null : (
									<Box sx={{ alignSelf: direction === 'column' ? 'flex-end' : 'auto' }}>
										<IconButton variant='outlined' color='error' onClick={() => removeRow(idx)}>
											<Delete />
										</IconButton>
									</Box>
								)}
							</Stack>
						))}
					</Stack>
				)}
				{showListError && (
					<Typography variant='caption' color='error'>
						{t('error.required')}
					</Typography>
				)}
				{!isDisabled && (
					<Button variant='outlined' onClick={addRow} sx={{ width: 'min(50%, 200px)' }}>
						+ {t('button.add_row')}
					</Button>
				)}
			</Stack>
		)
	}
	//#endregion

	const renderImage = (field, context = baseRenderContext) => {
		const max = Number.isFinite(field.multiple) ? Math.max(1, Number(field.multiple)) : 1
		return max > 1 ? renderImageMultiple(field, context) : renderImageSingle(field, context)
	}

	const renderCustom = (field, context = baseRenderContext) => {
		const value = getObjectValueFromStringPath(context.values, field.key)
		const setValue = (val) => context.setField(field.key, val)
		const required = field.required ?? true
		const showError = submitted && required && (value == null || value === '')

		return (
			<Stack key={field.key} spacing={0.5}>
				{field.title ? <Typography variant='subtitle2'>{field.title}</Typography> : null}
				{typeof field.render === 'function'
					? field.render({ value, onChange: setValue, values: context.values })
					: null}
				{showError && (
					<Typography variant='caption' color='error'>
						{t('error.required')}
					</Typography>
				)}
			</Stack>
		)
	}

	const renderDateRange = (field) => {
		const from = field.from || {}
		const to = field.to || {}
		const fromKey = from.key || 'fromDate'
		const toKey = to.key || 'toDate'
		const fromDate = getObjectValueFromStringPath(values, fromKey) || ''
		const toDate = getObjectValueFromStringPath(values, toKey) || ''

		return (
			<DateRangeField
				key={field.key}
				ref={registerRef(field.key)}
				fromDate={fromDate}
				toDate={toDate}
				onChange={(f, t) => {
					setField(fromKey, f)
					setField(toKey, t)
				}}
				fromLabel={from.label}
				toLabel={to.label}
				minDate={from.min}
				maxDate={to.max}
				fromValidate={from.validate}
				toValidate={to.validate}
				variant={textFieldVariant}
				size={textFieldSize}
				{...(field.props || {})}
			/>
		)
	}

	const renderTimeRange = (field) => {
		const from = field.from || {}
		const to = field.to || {}
		const dateConfig = field.date || null
		const showDate = !!dateConfig
		const fromKey = from.key
		const toKey = to.key
		const dateKey = dateConfig?.key
		const fromTime = getObjectValueFromStringPath(values, fromKey) || ''
		const toTime = getObjectValueFromStringPath(values, toKey) || ''
		const date = showDate ? getObjectValueFromStringPath(values, dateKey) || '' : ''

		return (
			<TimeRangeField
				key={field.key}
				ref={registerRef(field.key)}
				date={date}
				fromTime={fromTime}
				toTime={toTime}
				onChange={(d, f, t) => {
					setField(fromKey, f)
					setField(toKey, t)
					if (showDate) setField(dateKey, d)
				}}
				showDate={showDate}
				dateLabel={dateConfig?.label}
				fromLabel={from.label}
				toLabel={to.label}
				minDate={dateConfig?.min}
				maxDate={dateConfig?.max}
				minTime={from.min}
				maxTime={to.max}
				fromValidate={from.validate}
				toValidate={to.validate}
				dateValidate={dateConfig?.validate}
				variant={textFieldVariant}
				size={textFieldSize}
				{...(field.props || {})}
			/>
		)
	}

	//#region Main Render Function
	const map = {
		text: renderStandard,
		password: renderPassword,
		search: renderSearch,
		number: renderStandard,
		email: renderStandard,
		tel: renderStandard,
		select: renderStandard,
		radio: renderRadio,
		image: renderImage,
		file: renderFile,
		object: renderObject,
		array: renderArray,
		draw: renderDrawing,
		checkbox: renderCheckbox,
		custom: renderCustom,
		daterange: renderDateRange,
		timerange: renderTimeRange,
		_default: renderStandard,
	}

	const renderFieldWithContext = useCallback(
		(field, context = baseRenderContext) => {
			if (!field || !field.key) {
				return null
			}
			const type = field.type || 'text'
			const fn = map[type] || map._default
			return fn(field, context)
		},
		[map, baseRenderContext]
	)

	const renderField = useCallback(
		(field) => renderFieldWithContext(field, baseRenderContext),
		[renderFieldWithContext, baseRenderContext]
	)
	//#endregion

	return { renderField, hasRequiredMissing }
}

// Usage Example
////// JUST USE 'require = false' IF THE FIELD IS NOT REQUIRED, OR ELSE THE FIELD IS ALWAYS REQUIRED //////
/*
const fields = [
	##### Normal field
	{ key: 'name', title: 'Name', validate: [maxLen(255)] },
	##### Changed type to 'email' and some customize props
	{ key: 'email', title: 'Email', type: 'email', validate: [maxLen(255)], props: { variant: 'outlined', readOnly: true } },
	##### Multiline field
	{ key: 'description', title: 'Description', multiple: 4, validate: [maxLen(1000)] },
	##### Number field with numberRange validation
	{ key: 'age', title: 'Age', type: 'number', validate: [numberRange(0, 100)] },
	#### Checkbox field
	// Note: for checkbox, required means it must be checked (true). If not required, it can be either true or false.
	{ key: 'subscribe', title: 'Subscribe to newsletter', type: 'checkbox' },

	##### Select field with options
	{ key: 'role', title: 'Role', type: 'select', options: ['Manager', 'User', { label: 'Guest', value: 'guest', disabled: true }] },
	##### Select dialog field (single choice with search + confirm/cancel)
	{ key: 'departmentId', title: 'Department', type: 'select-dialog', options: [
			{ value: 1, label: 'Cardiology', searchKey: 'cardiology' },
			{ value: 2, label: 'Neurology', searchKey: 'neurology' },
			{ value: 3, label: 'Pediatrics', searchKey: 'pediatrics' },
		]
	},
	##### Select field with renderOption and renderOptionValue
	{ key: 'status', title: 'Status', type: 'select', options: [
			{ label: 'Active', value: 'active' },
			{ label: 'Inactive', value: 'inactive' },
		], 
		renderOption: (value, label) => (<span style={{ color: value === 'active' ? 'green' : 'red' }}>{label}</span>), 
		renderOptionValue: (value, label) => label === 'Active' ? 'Active User' : 'Inactive User' 
	},
	##### Multiple select field with options
	{ key: 'permissions', title: 'Permissions', type: 'select', multiple: true, options: [
			{ value: 1, label: 'Create', searchKey: 'create' },
			{ value: 2, label: 'Update', searchKey: 'update' },
			{ value: 3, label: 'Delete', searchKey: 'delete' },
		]
	},
	##### Select with custom renderOption
	{ key: 'customSelect', title: 'Custom Select', type: 'select', options: [
			{ label: 'Option 1', value: 'opt1' },
			{ label: 'Option 2', value: 'opt2' },
		], 
		renderOption: (value, label) => (<span style={{ fontWeight: value === 'opt1' ? 'bold' : 'normal' }}>{label}</span>) 
	},

	##### Image upload field with required false
	{ key: 'avatar', title: 'Avatar', type: 'image', required: false },
	##### Image upload field allowing multiple images (max 3)
	{ key: 'images', title: 'Images', type: 'image', multiple: 5 },

	##### Object field with child fields
	{ key: 'address', title: 'Address', type: 'object', of: [
		{ key: 'city', title: 'City', validate: [maxLen(255)] },
		{ key: 'country', title: 'Country', validate: [maxLen(255)] },
	]},

	##### Array field with child fields
	{ key: 'contacts', title: 'Contacts', type: 'array', of: [
		{ key: 'type', title: 'Type', type: 'select', options: ['Phone', 'Email'] },
		{ key: 'value', title: 'Value', validate: [maxLen(255)] },
	]},
]

const initialValues = {
	name: 'Doe',
	email: 'Doe@example.com',
	description: 'Description here',
	age: '25',
	role: 'User',
	departmentId: 1,
	permissions: [1, 2],
	status: 'active',
	customSelect: 'opt1',
	avatar: '/avatar.jpg',
	images: ['/image1.jpg', '/image2.jpg'],
	address: { city: 'City Name', country: 'Country Name' },
	contacts: [ { type: 'Phone', value: '123-456-7890' }, { type: 'Email', value: 'Doe@example.com' }],
}

// useForm with useFieldRenderer
const [submitted, setSubmitted] = useState(false)
const { values, handleChange, setField, reset, registerRef, validateAll } = useForm(initialValues)
const { renderField, hasRequiredMissing } = useFieldRenderer(values, setField, handleChange, registerRef, submitted, 'standard'/'outlined'/'filled', 'small'/'medium')

const handleSubmit = () => {
	setSubmitted(true)
	const ok = validateAll()
	const isMissing = hasRequiredMissing(fields)

	if (!ok || isMissing) {
		alert('Please fill all required fields')
		return
	}

	alert('Form submitted: ' + JSON.stringify(values, null, 2))
}

<Stack spacing={2}>
	{fields.map((f) => renderField(f))}
	<Button
		variant='contained'
		onClick={handleSubmit}
	>
		Submit
	</Button>
</Stack>

*/
