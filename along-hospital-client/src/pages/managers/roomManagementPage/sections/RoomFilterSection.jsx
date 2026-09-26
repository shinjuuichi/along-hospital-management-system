import FilterButton from '@/components/buttons/FilterButton'
import ResetFilterButton from '@/components/buttons/ResetFilterButton'
import useEnum from '@/hooks/useEnum'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import {
	setBuildingsStore,
	setFloorsStore,
	setRoomCategoriesStore,
	setSpecialtiesStore,
} from '@/redux/reducers/managementReducer'
import { Grid, Stack, Typography } from '@mui/material'
import { useEffect, useMemo, useRef } from 'react'

const RoomFilterSection = ({ filters, setFilters, loading }) => {
	const { t } = useTranslation()
	const _enum = useEnum()

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

	const floorsStore = useReduxStore({
		selector: (state) => state.management.floors,
		setStore: setFloorsStore,
	})
	const roomCategoriesStore = useReduxStore({
		selector: (state) => state.management.roomCategories,
		setStore: setRoomCategoriesStore,
	})
	const buildingsStore = useReduxStore({
		selector: (state) => state.management.buildings,
		setStore: setBuildingsStore,
	})
	const specialtiesStore = useReduxStore({
		selector: (state) => state.management.specialties,
		setStore: setSpecialtiesStore,
	})

	const prevBuildingId = useRef(values.buildingId)
	useEffect(() => {
		if (prevBuildingId.current !== values.buildingId) {
			setField('floorId', '')
			prevBuildingId.current = values.buildingId
		}
	}, [values.buildingId, setField])

	const filteredFloors = useMemo(() => {
		if (!values.buildingId) return floorsStore?.data || []
		return floorsStore?.data?.filter((f) => f.buildingId === values.buildingId) || []
	}, [floorsStore?.data, values.buildingId])

	const fields = [
		{
			key: 'code',
			title: t('room.placeholder.search'),
			type: 'search',
			required: false,
		},
		{
			key: 'status',
			title: t('room.field.status'),
			type: 'select',
			options: _enum.roomStatusOptions || [],
			required: false,
		},
		{
			key: 'buildingId',
			title: t('room.field.building'),
			type: 'select',
			options:
				buildingsStore?.data?.map((b) => ({
					label: b.name,
					value: b.id,
				})) || [],
			required: false,
		},
		{
			key: 'floorId',
			title: t('room.field.floor'),
			type: 'select',
			options: filteredFloors.map((f) => ({
				label: `${t('room.field.floor')} ${f.floorNumber}`,
				value: f.id,
			})),
			required: false,
		},
		{
			key: 'roomCategoryId',
			title: t('room.field.category'),
			type: 'select',
			options:
				roomCategoriesStore?.data?.map((c) => ({
					label: c.name,
					value: c.id,
				})) || [],
			required: false,
		},
		{
			key: 'specialtyId',
			title: t('room.field.specialty'),
			type: 'select',
			options:
				specialtiesStore?.data?.map((s) => ({
					label: s.name,
					value: s.id,
				})) || [],
			required: false,
		},
	]

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
			<Typography variant='caption'>{t('room.title.filters')}</Typography>

			<Grid container spacing={2}>
				<Grid size={{ xs: 12, md: 4 }}>{renderField(fields.find((f) => f.key === 'code'))}</Grid>
				<Grid size={{ xs: 12, md: 4 }} />
				<Grid size={{ xs: 6, md: 2 }}>
					<FilterButton onFilterClick={() => setFilters({ ...values })} fullWidth loading={loading} />
				</Grid>
				<Grid size={{ xs: 6, md: 2 }}>
					<ResetFilterButton
						onResetFilterClick={() => {
							reset({})
							setFilters({})
						}}
						fullWidth
						loading={loading}
					/>
				</Grid>
			</Grid>

			<Grid container spacing={2}>
				{fields
					.filter((f) => f.key !== 'code')
					.map((field) => (
						<Grid size={{ xs: 12, md: 2 }} key={field.key}>
							{renderField(field)}
						</Grid>
					))}
			</Grid>
		</Stack>
	)
}

export default RoomFilterSection
