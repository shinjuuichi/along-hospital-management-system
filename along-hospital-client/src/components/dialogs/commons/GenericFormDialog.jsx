/* eslint-disable react-hooks/exhaustive-deps */
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import { Button, Dialog, DialogActions, DialogContent, DialogTitle, Stack } from '@mui/material'
import { useCallback, useEffect, useMemo, useState } from 'react'

/**
 * @param {Object} props
 * @param {boolean} props.open
 * @param {Function} props.onClose
 * @param {string} [props.title='Form']
 * @param {import('@/hooks/useFieldRenderer').FieldDefinition[]} props.fields
 * @param {Object} [props.initialValues={}]
 * @param {string} [props.submitLabel]
 * @param {'primary'|'secondary'|'success'|'error'|'info'|'warning'} [props.submitButtonColor='primary']
 * @param {'xs'|'sm'|'md'|'lg'|'xl'} [props.maxWidth='sm']
 * @param {(values: Object) => Promise<Object>} [props.onValuesChange]
 * @param {'standard'|'outlined'|'filled'} [props.textFieldVariant='standard']
 * @param {(params: {values: any, closeDialog: Function, setField: Function}) => Promise<any>} [props.onSubmit]
 * @param {Array<{label: string,
 *  color?: 'primary'|'secondary'|'success'|'error'|'info'|'warning',
 *  variant?: 'text'|'outlined'|'contained',
 *  onClick: ({ values, closeDialog, setField }) => void}>} [props.additionalButtons]
 */
const GenericFormDialog = ({
	open,
	onClose,
	title = 'Form',
	fields = [],
	initialValues = {},
	submitLabel,
	submitButtonColor = 'primary',
	maxWidth = 'sm',
	onValuesChange = (values) => Promise.resolve(values),
	textFieldVariant = 'standard',
	onSubmit = ({ values, closeDialog, setField }) =>
		Promise.resolve({ values, closeDialog, setField }),
	additionalButtons = [],
	showSubmit = true,
}) => {
	const startValues = useMemo(() => {
		const v = { ...initialValues }
		for (const field of fields)
			if (v[field.key] === undefined)
				v[field.key] =
					field.defaultValue ??
					(field.type === 'file' || (field.type === 'image' && (field.imageInput ?? 'file') === 'file')
						? null
						: '')
		return v
	}, [fields, initialValues])

	const [submitted, setSubmitted] = useState(false)
	const [loading, setLoading] = useState(false)
	const { t } = useTranslation()
	const { values, handleChange, setField, reset, registerRef, validateAll } = useForm(startValues)
	const { renderField, hasRequiredMissing } = useFieldRenderer(
		values,
		setField,
		handleChange,
		registerRef,
		submitted,
		textFieldVariant
	)

	useEffect(() => {
		if (open) {
			setSubmitted(false)
			reset(startValues)
		}
	}, [open])

	useEffect(() => {
		onValuesChange?.(values)
	}, [values, onValuesChange])

	const handleClose = useCallback(() => {
		reset(startValues)
		onClose?.()
	}, [onClose, reset, startValues])

	const handleSubmit = useCallback(async () => {
		setSubmitted(true)
		const ok = validateAll()
		const missingField = hasRequiredMissing(fields)
		if (missingField || !ok) return

		if (typeof onSubmit !== 'function') return

		setLoading(true)
		try {
			await onSubmit({ values, closeDialog: handleClose, setField })
		} finally {
			setLoading(false)
		}
	}, [fields, handleClose, hasRequiredMissing, onSubmit, setField, validateAll, values])

	return (
		<Dialog open={!!open} onClose={handleClose} fullWidth maxWidth={maxWidth}>
			<DialogTitle>{title}</DialogTitle>
			<DialogContent dividers>
				<Stack spacing={2.25}>
					{fields.map((field, index) =>
						renderField(index === 0 ? { ...field, props: { ...field.props, autoFocus: true } } : field)
					)}
				</Stack>
			</DialogContent>
			<DialogActions>
				<Button onClick={handleClose} color='inherit' disabled={loading}>
					{t('button.cancel')}
				</Button>
				{additionalButtons &&
					additionalButtons.length > 0 &&
					additionalButtons.map((btn, idx) => (
						<Button
							key={idx}
							onClick={() => btn.onClick({ values, closeDialog: handleClose, setField })}
							color={btn.color || 'primary'}
							variant={btn.variant || 'contained'}
							loading={loading}
							loadingPosition='start'
						>
							{btn.label}
						</Button>
					))}
				{showSubmit && (
					<Button
						onClick={handleSubmit}
						color={submitButtonColor}
						variant='contained'
						loading={loading}
						loadingPosition='start'
					>
						{submitLabel || t('button.submit')}
					</Button>
				)}
			</DialogActions>
		</Dialog>
	)
}

export default GenericFormDialog

// Usage Example
/*
const fields = [
    // Normal field
    { key: 'name', title: 'Name', validate: [maxLen(255)] }, 

    // Changed type to 'email'
    { key: 'email', title: 'Email', type: 'email', validate: [maxLen(255)] },

    // Multiline field
    { key: 'description', title: 'Description', multiple: 4, validate: [maxLen(1000)] }, 

    // Number field with numberRange validation
    { key: 'age', title: 'Age', type: 'number', validate: [numberRange(0, 100)] },

    // Image upload field with required false
    { key: 'avatar', title: 'Avatar', type: 'image', required: false, imagePreview: 'https://example.com/avatar.jpg' },

	{ key: 'images', title: 'Images', type: 'image', required: true, multiple: 5 },

	// Select field with options
	{ key: 'role', title: 'Role', type: 'select', options: ['User', 'Manager', { label: 'Super Manager', value: 'supermanager', disabled: true }] },

	// Object field with nested fields
	{ key: 'address', title: 'Address', type: 'object', of: [
		{ key: 'street', title: 'Street' },
		{ key: 'city', title: 'City' },
		{ key: 'zip', title: 'ZIP Code', validate: [maxLen(10)] },
	]},

	// Array field with nested fields
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
   	avatar: '/avatar.jpg',
	images: ['/image1.jpg', '/image2.jpg'],
   	role: 'User',
   	address: { street: 'Street Name', city: 'City Name', zip: '12345' },
	contacts: [ { type: 'Phone', value: '123-456-7890' }, { type: 'Email', value: 'Doe@example.com' }],
}

const handleSuccess = (res) => {
	dispatch(setProfileStore(res.data))
}

<GenericFormDialog
    open={createDialogOpen}
    onClose={() => setCreateDialogOpen(false)}
    fields={fields}
    submitLabel={submitLabel}
    submitButtonColor={submitButtonColor}
	onSubmit={() => console.log('Created')}
/>

<GenericFormDialog
    open={updateDialogOpen}
    onClose={() => setUpdateDialogOpen(false)}
	initialValues={initialValues}
    fields={fields}
    submitLabel={submitLabel}
    submitButtonColor={submitButtonColor}
	onSubmit={() => console.log('Updated')}
	additionalButtons={[
		{
			label: 'Delete',
			color: 'error',
			variant: 'contained',
			onClick: () => console.log('Deleted'),
		},
	]}
/>
*/
