import { defaultBedStatusStyle } from '@/configs/defaultStylesConfig'
import { EnumConfig } from '@/configs/enumConfig'
import { AssignBedDialogMode } from '@/constants/medicalHistoryConstants'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import { renderEmptyFallback } from '@/utils/handleStringUtil'
import { maxLen } from '@/utils/validateUtil'
import { BedOutlined } from '@mui/icons-material'
import {
	Box,
	Button,
	Chip,
	CircularProgress,
	Dialog,
	DialogActions,
	DialogContent,
	DialogTitle,
	Grid,
	Paper,
	Stack,
	Typography,
} from '@mui/material'
import { useCallback, useEffect, useMemo, useState } from 'react'

const assignBedInitialValues = {
	roomId: '',
	bedId: '',
	transferNote: '',
}

const renderRoomOption = (roomOption, t) => {
	if (!roomOption) return null

	return (
		<Stack direction='row' justifyContent='space-between' alignItems='center' width='100%' spacing={2}>
			<Stack>
				<Typography variant='body2' fontWeight={500}>
					{roomOption.code}
				</Typography>
				<Typography variant='caption' color='text.secondary'>
					{t('room.field.building')}: {roomOption.building} - {t('room.field.floor')}: {roomOption.floor}
				</Typography>
			</Stack>
			<Chip
				icon={<BedOutlined sx={{ fontSize: 16 }} />}
				label={roomOption.availableBedCount}
				size='small'
				color={roomOption.availableBedCount > 0 ? 'primary' : 'default'}
				variant='outlined'
			/>
		</Stack>
	)
}

const renderSelectedRoomValue = (roomOption, t) => {
	if (!roomOption) {
		return t('bed_occupancy.placeholder.select_room')
	}

	return `${roomOption.code} - ${t('room.field.building')} ${roomOption.building}`
}

const BedSelectionField = ({ beds, selectedBedId, onSelectBed, loading, t }) => {
	if (loading) {
		return (
			<Box display='flex' justifyContent='center' py={2}>
				<CircularProgress size={24} />
			</Box>
		)
	}

	if (beds.length === 0) {
		return (
			<Typography color='text.secondary' variant='body2'>
				{t('bed_occupancy.placeholder.no_beds_available')}
			</Typography>
		)
	}

	return (
		<Box sx={{ maxHeight: 300, overflowY: 'auto' }}>
			<Grid container spacing={2}>
				{beds.map((bed) => {
					const isSelected = String(selectedBedId || '') === String(bed.id)

					return (
						<Grid key={bed.id} size={{ xs: 6 }}>
							<Paper
								variant='outlined'
								sx={{
									p: 2,
									cursor: 'pointer',
									bgcolor: isSelected ? 'primary.light' : 'background.paper',
									border: isSelected ? 2 : 1,
									borderColor: isSelected ? 'primary.main' : 'divider',
									transition: 'all 0.2s ease',
									'&:hover': {
										bgcolor: isSelected ? 'primary.light' : 'action.hover',
										borderColor: 'primary.light',
									},
								}}
								onClick={() => onSelectBed(bed.id)}
							>
								<Stack spacing={1} alignItems='center'>
									<BedOutlined
										sx={{
											fontSize: 32,
											color: isSelected ? 'primary.dark' : 'text.secondary',
										}}
									/>
									<Typography
										variant='subtitle2'
										fontWeight={600}
										color={isSelected ? 'primary.dark' : 'text.primary'}
									>
										{bed.code}
									</Typography>
									<Typography variant='caption' color='text.secondary' textAlign='center'>
										{bed.bedCategoryName}
									</Typography>
									<Chip
										label={t(`enum.bed_status.${bed.status?.toLowerCase()}`)}
										color={defaultBedStatusStyle(bed.status)}
										size='small'
									/>
								</Stack>
							</Paper>
						</Grid>
					)
				})}
			</Grid>
		</Box>
	)
}

const AssignBedDialog = ({
	open,
	onClose,
	onSubmit,
	loading = false,
	medicalHistoryId,
	specialtyId,
	mode = AssignBedDialogMode.Assign,
	currentBedId = null,
	currentBedCode = '',
	rooms = [],
	beds = [],
	loadingRooms = false,
	loadingBeds = false,
}) => {
	const { t } = useTranslation()
	const isTransferMode = mode === AssignBedDialogMode.Transfer
	const [submitted, setSubmitted] = useState(false)
	const { values, handleChange, setField, reset, registerRef, validateAll } =
		useForm(assignBedInitialValues)
	const { renderField, hasRequiredMissing } = useFieldRenderer(
		values,
		setField,
		handleChange,
		registerRef,
		submitted,
		'outlined',
		'small'
	)

	const roomOptions = useMemo(
		() =>
			rooms
				.filter(
					(room) =>
						room.status === EnumConfig.RoomStatus.Active &&
						(!specialtyId || String(room.specialtyId) === String(specialtyId)) &&
						Array.isArray(room.roles) &&
						room.roles.includes(EnumConfig.Role.Patient)
				)
				.map((room) => ({
					value: room.id,
					searchKey: `${room.code} ${room.buildingName} ${room.floorNumber}`,
					label: {
						code: room.code,
						building: room.buildingName,
						floor: room.floorNumber,
						availableBedCount: room.availableBedCount ?? 0,
					},
				})),
		[rooms, specialtyId]
	)

	const bedList = useMemo(
		() =>
			beds.filter(
				(bed) =>
					String(bed.roomId) === String(values.roomId) &&
					bed.status === EnumConfig.BedStatus.Active &&
					(!isTransferMode || String(bed.id) !== String(currentBedId))
			),
		[beds, currentBedId, isTransferMode, values.roomId]
	)

	useEffect(() => {
		if (open) {
			setSubmitted(false)
			reset(assignBedInitialValues)
		}
	}, [open, reset])

	useEffect(() => {
		if (!values.bedId) {
			return
		}

		const stillExistsInSelectedRoom = bedList.some(
			(bed) => String(bed.id) === String(values.bedId)
		)

		if (!stillExistsInSelectedRoom) {
			setField('bedId', '')
		}
	}, [bedList, setField, values.bedId])

	const fields = useMemo(() => {
		const roomField = {
			key: 'roomId',
			title: t('bed_occupancy.field.room'),
			type: 'select-dialog',
			required: roomOptions.length > 0,
			options: roomOptions,
			renderOption: (_, label) => renderRoomOption(label, t),
			renderOptionValue: (_, label) => renderSelectedRoomValue(label, t),
			props: {
				disabled: loadingRooms || roomOptions.length === 0,
			},
		}

		const bedField =
			values.roomId
				? {
						key: 'bedId',
						title:
							bedList.length > 0
								? `${t('bed_occupancy.field.bed')} *`
								: t('bed_occupancy.field.bed'),
						type: 'custom',
						required: bedList.length > 0,
						render: ({ value, onChange }) => (
							<BedSelectionField
								beds={bedList}
								selectedBedId={value}
								onSelectBed={onChange}
								loading={loadingBeds}
								t={t}
							/>
						),
					}
				: null

		const transferNoteField = isTransferMode
			? {
					key: 'transferNote',
					title: t('bed_occupancy.field.transfer_note'),
					type: 'text',
					multiple: 3,
					required: true,
					validate: [maxLen(500)],
					props: {
						placeholder: t('bed_occupancy.placeholder.enter_transfer_note'),
						inputProps: { maxLength: 500 },
					},
				}
			: null

		return [roomField, bedField, transferNoteField].filter(Boolean)
	}, [bedList, isTransferMode, loadingBeds, loadingRooms, roomOptions, t, values.roomId])

	const isSubmitDisabled =
		loading ||
		loadingRooms ||
		(Boolean(values.roomId) && loadingBeds) ||
		roomOptions.length === 0 ||
		(Boolean(values.roomId) && bedList.length === 0)

	const handleClose = useCallback(() => {
		setSubmitted(false)
		reset(assignBedInitialValues)
		onClose?.()
	}, [onClose, reset])

	const handleSubmit = useCallback(async () => {
		setSubmitted(true)

		const isValid = validateAll()
		const isMissing = hasRequiredMissing(fields)

		if (!isValid || isMissing || loadingRooms || loadingBeds) {
			return
		}

		await onSubmit({
			medicalHistoryId,
			bedId: values.bedId,
			...(isTransferMode ? { transferNote: values.transferNote.trim() } : {}),
		})
	}, [
		fields,
		hasRequiredMissing,
		isTransferMode,
		loadingBeds,
		loadingRooms,
		medicalHistoryId,
		onSubmit,
		validateAll,
		values.bedId,
		values.transferNote,
	])

	return (
		<Dialog open={open} onClose={handleClose} maxWidth='sm' fullWidth>
			<DialogTitle>
				{isTransferMode
					? t('bed_occupancy.dialog.transfer_bed_title')
					: t('bed_occupancy.dialog.assign_bed_title')}
			</DialogTitle>
			<DialogContent>
				<Stack spacing={3} sx={{ mt: 1 }}>
					{isTransferMode && (
						<Paper variant='outlined' sx={{ p: 2, borderRadius: 2 }}>
							<Stack spacing={0.5}>
								<Typography variant='subtitle2'>{t('bed_occupancy.text.current_bed')}</Typography>
								<Typography variant='body2' color='text.secondary'>
									{renderEmptyFallback(currentBedCode || null)}
								</Typography>
							</Stack>
						</Paper>
					)}

					{fields.map((field, index) =>
						renderField(index === 0 ? { ...field, props: { ...field.props, autoFocus: true } } : field)
					)}

					{loadingRooms && (
						<Box display='flex' justifyContent='center' py={1}>
							<CircularProgress size={24} />
						</Box>
					)}

					{!loadingRooms && roomOptions.length === 0 && (
						<Typography color='text.secondary' variant='body2'>
							{t('bed_occupancy.placeholder.no_rooms_available')}
						</Typography>
					)}
				</Stack>
			</DialogContent>
			<DialogActions>
				<Button onClick={handleClose} disabled={loading}>
					{t('button.cancel')}
				</Button>
				<Button
					variant='contained'
					onClick={handleSubmit}
					disabled={isSubmitDisabled}
					startIcon={loading && <CircularProgress size={16} />}
				>
					{isTransferMode ? t('bed_occupancy.button.transfer_bed') : t('button.assign')}
				</Button>
			</DialogActions>
		</Dialog>
	)
}

export default AssignBedDialog
