import FilterButton from '@/components/buttons/FilterButton'
import ResetFilterButton from '@/components/buttons/ResetFilterButton'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import { Stack, Typography } from '@mui/material'
import { useEffect } from 'react'

const ManagerMedicalServiceFilterBarSection = ({
	filters,
	loading = false,
	specialties = [],
	onFilterClick = () => {},
	onResetFilterClick = () => {},
}) => {
	const { t } = useTranslation()
	const { values, handleChange, setField, registerRef, reset } = useForm(filters)
	const { renderField } = useFieldRenderer(
		values,
		setField,
		handleChange,
		registerRef,
		false,
		'outlined',
		'small'
	)

	const fields = [
		{ key: 'name', title: t('text.search'), type: 'search', required: false },
		{
			key: 'specialtyId',
			title: t('medical_service.field.specialty'),
			type: 'select',
			options: [{ value: '', label: t('text.all') }, ...(specialties || [])],
			required: false,
		},
		{
			key: 'isActive',
			title: t('medical_service.field.table_status'),
			type: 'select',
			options: [
				{ value: '', label: t('text.all') },
				{ value: 'true', label: t('medical_service.label.active') },
				{ value: 'false', label: t('medical_service.label.inactive') },
			],
			required: false,
		},
	]

	useEffect(() => {
		reset(filters)
	}, [filters, reset])

	return (
		<Stack
			spacing={1.5}
			sx={{
				pt: 1,
				pb: 2,
				px: 2,
				bgcolor: 'background.paper',
				border: (theme) => `1px solid ${theme.palette.divider}`,
				borderRadius: 1,
			}}
		>
			<Typography variant='caption'>{t('medical_service.title.filter')}</Typography>
			<Stack direction='row' spacing={2} alignItems='center'>
				{fields.map(renderField)}
				<FilterButton
					onFilterClick={() => onFilterClick(values)}
					loading={loading}
					sx={{ minWidth: 180 }}
				/>
				<ResetFilterButton
					loading={loading}
					onResetFilterClick={() => onResetFilterClick(reset)}
					sx={{ minWidth: 180 }}
				/>
			</Stack>
		</Stack>
	)
}

export default ManagerMedicalServiceFilterBarSection
