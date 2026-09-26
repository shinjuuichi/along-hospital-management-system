import useEnum from '@/hooks/useEnum'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import { getImageFromCloud } from '@/utils/commons'
import { isEmail, maxLen } from '@/utils/validateUtil'
import { Close, School } from '@mui/icons-material'
import {
	Avatar,
	Chip,
	Dialog,
	DialogContent,
	DialogTitle,
	Grid,
	IconButton,
	Stack,
	Typography,
} from '@mui/material'

const DoctorInfoDialog = ({ open, onClose, doctorInfo = {} }) => {
	const { t } = useTranslation()
	const _enum = useEnum()

	const { values, setField, handleChange, registerRef } = useForm(doctorInfo)
	const { renderField } = useFieldRenderer(
		values,
		setField,
		handleChange,
		registerRef,
		false,
		'outlined',
		'small'
	)

	const basicInfoFields = [
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
		},
		{
			key: 'address',
			title: t('profile.field.address'),
			multiple: 3,
			validate: [maxLen(255)],
			required: false,
		},
	]
	const professionalInfoFields = [
		{
			key: 'hireDate',
			title: t('profile.field.hire_date'),
			type: 'date',
		},
		{
			key: 'qualificationName',
			title: t('profile.field.qualification'),
			type: 'text',
			validate: [maxLen(100)],
		},
		{
			key: 'specialtyName',
			title: t('profile.field.specialty'),
			type: 'text',
			validate: [maxLen(100)],
		},
	]

	return (
		<Dialog open={open} onClose={onClose} maxWidth='md' fullWidth scroll='paper'>
			<DialogTitle
				sx={{ p: 3, display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}
			>
				<Stack direction='row' spacing={2} alignItems='center'>
					<Avatar src={getImageFromCloud(values.image)} sx={{ width: 80, height: 80 }} />
					<Typography variant='h6'>{values.name}</Typography>
					{values.specialty && (
						<Chip
							label={`${t('profile.field.specialty')}: ${values.specialty}`}
							icon={<School />}
							color='success'
							size='small'
						/>
					)}
				</Stack>
				<IconButton>
					<Close onClick={onClose} />
				</IconButton>
			</DialogTitle>
			<DialogContent sx={{ p: 3 }}>
				<Grid container spacing={2}>
					<Grid size={{ xs: 12, md: 6 }}>
						<Typography variant='subtitle1' sx={{ mb: 1 }}>
							{t('dialog.doctor_info.basic_info')}
						</Typography>
						<Stack spacing={2}>
							{basicInfoFields.map((f) => renderField({ ...f, props: { ...f.props, disabled: true } }))}
						</Stack>
					</Grid>
					<Grid size={{ xs: 12, md: 6 }}>
						<Typography variant='subtitle1' sx={{ mb: 1 }}>
							{t('dialog.doctor_info.professional_info')}
						</Typography>
						<Stack spacing={2}>
							{professionalInfoFields.map((f) =>
								renderField({ ...f, props: { ...f.props, disabled: true } })
							)}
						</Stack>
					</Grid>
				</Grid>
			</DialogContent>
		</Dialog>
	)
}

export default DoctorInfoDialog
