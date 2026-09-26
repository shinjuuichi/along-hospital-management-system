import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import { renderEmptyFallback } from '@/utils/handleStringUtil'
import { CloseRounded, SettingsRounded } from '@mui/icons-material'
import {
	Box,
	Button,
	Chip,
	Divider,
	Drawer,
	IconButton,
	Stack,
	Typography,
	useTheme,
} from '@mui/material'
import { alpha } from '@mui/material/styles'
import { useEffect, useState } from 'react'

const QueueRoomSelectionDrawer = ({
	roomOptions = [],
	selectedRoomId,
	onApplyRoom = () => {},
	currentRoomCode = '',
}) => {
	const { t } = useTranslation()
	const theme = useTheme()
	const [open, setOpen] = useState(false)

	const { values, setField, handleChange, registerRef } = useForm({
		roomId: selectedRoomId ?? '',
	})

	useEffect(() => {
		setField('roomId', selectedRoomId ?? '')
	}, [selectedRoomId, setField])

	const { renderField } = useFieldRenderer(
		values,
		setField,
		handleChange,
		registerRef,
		false,
		'outlined',
		'small'
	)

	const roomField = {
		key: 'roomId',
		title: t('queue.guest.field.select_room'),
		type: 'select',
		required: false,
		options: [
			{ value: '', label: t('queue.text.reception_room') },
			...roomOptions.map((room) => ({ value: room.id, label: room.code })),
		],
	}

	const selectedPreviewRoomCode =
		String(values.roomId) === ''
			? t('queue.text.reception_room')
			: roomOptions.find((room) => String(room.id) === String(values.roomId))?.code ||
				currentRoomCode ||
				t('queue.text.reception_room')

	const handleApply = () => {
		if (String(values.roomId) === '') {
			onApplyRoom(null)
			setOpen(false)
			return
		}

		const nextRoomId = Number(values.roomId)
		if (Number.isNaN(nextRoomId)) return

		onApplyRoom(nextRoomId)
		setOpen(false)
	}

	return (
		<>
			<Box
				sx={{
					position: 'absolute',
					top: { xs: 12, md: 18 },
					right: { xs: 12, md: 18 },
					zIndex: 10,
				}}
			>
				<IconButton
					onClick={() => setOpen(true)}
					sx={{
						width: { xs: 44, md: 52 },
						height: { xs: 44, md: 52 },
						border: `1px solid ${alpha(theme.palette.primary.main, 0.22)}`,
						bgcolor: alpha(theme.palette.background.paper, 0.88),
						color: theme.palette.primary.main,
						'&:hover': {
							bgcolor: alpha(theme.palette.primary.main, 0.08),
						},
					}}
				>
					<SettingsRounded />
				</IconButton>
			</Box>

			<Drawer
				anchor='right'
				open={open}
				onClose={() => setOpen(false)}
				slotProps={{
					paper: {
						sx: {
							width: { xs: '100%', sm: 420 },
						},
					},
				}}
			>
				<Stack sx={{ height: '100%' }}>
					<Stack direction='row' alignItems='center' justifyContent='space-between' sx={{ p: 2 }}>
						<Typography variant='h6' fontWeight={700}>
							{t('queue.guest.title.settings')}
						</Typography>
						<IconButton onClick={() => setOpen(false)}>
							<CloseRounded />
						</IconButton>
					</Stack>

					<Divider />

					<Stack spacing={2} sx={{ p: 2, flex: 1 }}>
						{renderField(roomField)}
						<Chip
							label={t('queue.guest.text.current_room_code', {
								roomCode: renderEmptyFallback(selectedPreviewRoomCode),
							})}
							variant='outlined'
							sx={{
								borderColor: alpha(theme.palette.primary.main, 0.26),
								color: theme.palette.primary.main,
								fontWeight: 600,
							}}
						/>
					</Stack>

					<Stack
						direction='row'
						spacing={1.5}
						sx={{ p: 2, borderTop: `1px solid ${theme.palette.divider}` }}
					>
						<Button fullWidth variant='outlined' onClick={() => setOpen(false)}>
							{t('button.cancel')}
						</Button>
						<Button fullWidth variant='contained' onClick={handleApply}>
							{t('queue.guest.button.apply_room')}
						</Button>
					</Stack>
				</Stack>
			</Drawer>
		</>
	)
}

export default QueueRoomSelectionDrawer
