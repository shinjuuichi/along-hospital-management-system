import useEnum from '@/hooks/useEnum'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import { getImageFromCloud } from '@/utils/commons'
import { isEmail, maxLen, numberRange } from '@/utils/validateUtil'
import { Bloodtype, Close } from '@mui/icons-material'
import {
	Avatar,
	Box,
	Button,
	Chip,
	Dialog,
	DialogActions,
	DialogContent,
	DialogTitle,
	Divider,
	Grid,
	IconButton,
	Stack,
	Typography,
} from '@mui/material'
import { useState } from 'react'

const PatientInfoDialog = ({
	open,
	onClose,
	onSave = (values) => Promise.resolve(values),
	patientInfo = {},
	loading = false,
	isEditable,
}) => {
	const [submitted, setSubmitted] = useState(false)
	const { t } = useTranslation()
	const _enum = useEnum()

	const { values, setField, handleChange, registerRef, reset, validateAll } = useForm(patientInfo)
	const { renderField, hasRequiredMissing } = useFieldRenderer(
		values,
		setField,
		handleChange,
		registerRef,
		submitted,
		'outlined',
		'small'
	)

	const basicInfoFields = [
		{ key: 'name', title: t('profile.field.name'), type: 'text' },
		{
			key: 'dateOfBirth',
			title: t('profile.field.date_of_birth'),
			type: 'date',
			maxValue: new Date().toISOString().split('T')[0],
		},
		{
			key: 'gender',
			title: t('profile.field.gender'),
			type: 'select',
			options: _enum.genderOptions,
		},
		{ key: 'phone', title: t('profile.field.phone'), validate: [maxLen(15)], type: 'tel' },
		{
			key: 'email',
			title: t('profile.field.email'),
			type: 'email',
			validate: [isEmail(), maxLen(255)],
			required: false,
			disabled: true,
		},
	]

	const healthInfoFields = [
		{
			key: 'medicalNumber',
			title: t('profile.field.medical_number'),
			type: 'text',
			required: false,
			disabled: true,
		},
		{
			key: 'height',
			title: t('profile.field.height'),
			type: 'number',
			required: false,
			validate: [numberRange(30, 300)],
		},
		{
			key: 'weight',
			title: t('profile.field.weight'),
			type: 'number',
			required: false,
			validate: [numberRange(1, 500)],
		},
		{
			key: 'bloodType',
			title: t('profile.field.blood_type'),
			type: 'select',
			required: false,
			options: _enum.bloodTypeOptions,
		},
	]
	const allergyFields = [
		{
			key: 'allergies',
			type: 'array',
			required: false,
			props: { disabled: !isEditable },
			of: [
				{
					key: 'name',
					title: t('profile.field.allergy.name'),
					validate: [maxLen(100)],
				},
				{
					key: 'severityLevel',
					title: t('profile.field.allergy.severity'),
					type: 'select',
					options: _enum.severityLevelOptions,
				},
				{
					key: 'reaction',
					title: t('profile.field.allergy.reaction'),
					multiple: 1,
					validate: [maxLen(500)],
					required: false,
				},
			],
		},
	]
	const fields = [...basicInfoFields, ...healthInfoFields, ...allergyFields]

	const height = parseFloat(values.height)
	const weight = parseFloat(values.weight)
	const bmi = height && weight ? (weight / (height / 100) ** 2).toFixed(1) : null

	const handleClose = () => {
		onClose?.()
		reset()
	}

	const handleSave = async () => {
		setSubmitted(true)
		const ok = validateAll()
		const isMissing = hasRequiredMissing(fields)

		if (!ok || isMissing) {
			return
		}

		const response = await onSave(values)
		if (response) {
			onClose?.()
		}
	}

	return (
		<Dialog open={open} onClose={handleClose} maxWidth='md' fullWidth scroll='paper'>
			<DialogTitle
				sx={{ p: 3, display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}
			>
				<Stack direction='row' spacing={2} alignItems='center'>
					<Avatar src={getImageFromCloud(values.image)} sx={{ width: 80, height: 80 }} />
					<Typography variant='h6'>{values.name}</Typography>
					{values.bloodType && (
						<Chip
							label={`${t('profile.field.blood_type')}: ${values.bloodType}`}
							icon={<Bloodtype />}
							color='error'
							size='small'
						/>
					)}
				</Stack>
				<IconButton>
					<Close onClick={handleClose} />
				</IconButton>
			</DialogTitle>
			<DialogContent sx={{ p: 3 }}>
				<Grid container spacing={2}>
					<Grid size={{ xs: 12, md: 6 }}>
						<Typography variant='subtitle1' sx={{ mb: 1 }}>
							{t('dialog.patient_info.basic_info')}
						</Typography>
						<Stack spacing={2}>
							{basicInfoFields.map((f) =>
								renderField({ ...f, props: { ...f.props, disabled: f.disabled ?? !isEditable } })
							)}
						</Stack>
					</Grid>
					<Grid size={{ xs: 12, md: 6 }}>
						<Typography variant='subtitle1' sx={{ mb: 1 }}>
							{t('dialog.patient_info.health_info')}
						</Typography>
						<Stack spacing={2}>
							{healthInfoFields.map((f) =>
								renderField({ ...f, props: { ...f.props, disabled: f.disabled ?? !isEditable } })
							)}
							{bmi && (
								<Box sx={{ mt: 1 }}>
									<Typography variant='body2'>BMI: {bmi}</Typography>
								</Box>
							)}
						</Stack>
					</Grid>
				</Grid>
				<Divider sx={{ my: 3 }} />
				<Typography variant='subtitle1' sx={{ mb: 1 }}>
					{t('dialog.patient_info.allergies')}
				</Typography>
				<Box sx={{ border: 1, borderColor: 'divider' }}>{allergyFields.map(renderField)}</Box>
			</DialogContent>
			<DialogActions
				sx={{
					bottom: 0,
					bgcolor: 'background.paper',
					borderTop: '1px solid',
					borderColor: 'divider',
					p: 2,
				}}
			>
				{isEditable ? (
					<>
						<Button onClick={handleClose} variant='outlined'>
							{t('button.cancel')}
						</Button>
						<Button onClick={handleSave} loading={loading} loadingPosition='start' variant='contained'>
							{t('button.save')}
						</Button>
					</>
				) : (
					<Button onClick={handleClose} variant='contained'>
						{t('button.close')}
					</Button>
				)}
			</DialogActions>
		</Dialog>
	)
}

export default PatientInfoDialog
