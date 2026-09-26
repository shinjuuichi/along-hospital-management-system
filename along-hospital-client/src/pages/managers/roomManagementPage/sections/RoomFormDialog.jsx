import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import useEnum from '@/hooks/useEnum'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import {
	setBuildingsStore,
	setFloorsStore,
	setRoomCategoriesStore,
	setSpecialtiesStore,
} from '@/redux/reducers/managementReducer'
import { useEffect, useState } from 'react'

const RoomFormDialog = ({
	open,
	onClose,
	onSubmit,
	initialValues = {},
	submitLabel,
	title,
	submitButtonColor,
}) => {
	const { t } = useTranslation()

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

	const _enum = useEnum()

	const isUpdate = !!initialValues.id

	const [selectedBuildingId, setSelectedBuildingId] = useState(null)

	useEffect(() => {
		if (!open) {
			setSelectedBuildingId(null)
			return
		}
		let buildingId = initialValues.buildingId
		if (!buildingId && initialValues.floorId && floorsStore?.data) {
			const floor = floorsStore.data.find((f) => f.id === initialValues.floorId)
			buildingId = floor?.buildingId || null
		}
		setSelectedBuildingId(buildingId)
	}, [open, floorsStore?.data, initialValues.floorId, initialValues.buildingId])

	const filteredFloors = selectedBuildingId
		? floorsStore?.data?.filter((f) => f.buildingId === selectedBuildingId) || []
		: []

	const handleValuesChange = (values) => {
		if (values?.buildingId !== selectedBuildingId) {
			setSelectedBuildingId(values?.buildingId)
		}
	}

	const handleClose = () => {
		setSelectedBuildingId(null)
		onClose?.()
	}
	const formInitialValues = {
		...initialValues,
		buildingId:
			initialValues.buildingId ||
			(initialValues.floorId && floorsStore?.data
				? floorsStore.data.find((f) => f.id === initialValues.floorId)?.buildingId
				: null),
		floorId: initialValues.floorId || '',
	}

	const fields = [
		{
			key: 'buildingId',
			title: t('room.field.building'),
			type: 'select',
			options:
				buildingsStore?.data?.map((b) => ({
					label: b.name,
					value: b.id,
				})) || [],
		},
		{
			key: 'floorId',
			title: t('room.field.floor'),
			type: 'select',
			disabled: !selectedBuildingId,
			options: filteredFloors.map((f) => ({
				label: `${t('room.field.floor')} ${f.floorNumber}`,
				value: f.id,
			})),
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
		},
	]

	if (isUpdate) {
		fields.push({
			key: 'status',
			title: t('room.field.status'),
			type: 'select',
			options: _enum.roomStatusOptions,
		})
	}

	const handleSubmit = async ({ values, closeDialog, setField }) => {
		const submitValues = { ...values }
		delete submitValues.buildingId
		return onSubmit({ values: submitValues, closeDialog, setField })
	}

	return (
		<GenericFormDialog
			open={open}
			onClose={handleClose}
			title={title}
			fields={fields}
			initialValues={formInitialValues}
			onValuesChange={handleValuesChange}
			onSubmit={handleSubmit}
			submitLabel={submitLabel}
			submitButtonColor={submitButtonColor}
			textFieldVariant='outlined'
		/>
	)
}

export default RoomFormDialog
