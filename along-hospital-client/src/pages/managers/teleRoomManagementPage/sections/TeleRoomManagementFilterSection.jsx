import FilterButton from '@/components/buttons/FilterButton'
import ResetFilterButton from '@/components/buttons/ResetFilterButton'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import { Grid, Paper, Stack } from '@mui/material'
import { useMemo } from 'react'

const TeleRoomManagementFilterSection = ({
	filters = {},
	setFilters,
	specialties = [],
	loading = false,
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

	const specialtyOptions = useMemo(
		() =>
			Array.isArray(specialties)
				? specialties.map((item) => ({ label: item?.name, value: item?.id }))
				: [],
		[specialties]
	)

	const filterFields = [
		{
			key: 'roomCode',
			title: t('tele_room.field.room_code'),
			type: 'text',
			required: false,
		},
		{
			key: 'roomDisplayName',
			title: t('tele_room.field.room_display_name'),
			type: 'text',
			required: false,
		},
		{
			key: 'specialtyId',
			title: t('tele_room.field.specialty_id'),
			type: 'select-dialog',
			options: specialtyOptions,
			required: false,
		},
	]

	return (
		<Paper
			sx={{
				bgcolor: 'background.default',
				p: 2,
			}}
		>
			<Stack spacing={2}>
				<Grid container spacing={2}>
					{filterFields.map((field) => (
						<Grid size={{ xs: 12, md: 4 }} key={field.key}>
							{renderField(field)}
						</Grid>
					))}
					<Grid size={{ xs: 6, md: 3 }}>
						<FilterButton
							onFilterClick={() => setFilters(values)}
							fullWidth
							loading={loading}
							sx={{ flexGrow: 1 }}
						/>
					</Grid>
					<Grid size={{ xs: 6, md: 3 }}>
						<ResetFilterButton
							onResetFilterClick={() => {
								reset({})
								setFilters({})
							}}
							fullWidth
							loading={loading}
							sx={{ flexGrow: 1 }}
						/>
					</Grid>
				</Grid>
			</Stack>
		</Paper>
	)
}

export default TeleRoomManagementFilterSection
