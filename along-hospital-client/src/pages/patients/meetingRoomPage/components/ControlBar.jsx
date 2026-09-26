import useTranslation from '@/hooks/useTranslation'
import {
	CallEnd,
	Chat,
	FiberManualRecord,
	Mic,
	MicOff,
	Stop,
	Videocam,
	VideocamOff,
} from '@mui/icons-material'
import { Button, IconButton, Stack, Tooltip } from '@mui/material'

const ControlButton = ({ title, active = true, onClick, children }) => (
	<Tooltip title={title} arrow>
		<IconButton
			onClick={onClick}
			sx={{
				bgcolor: (t) => (active ? t.palette.success.softBg : t.palette.action.hover),
				color: (t) => (active ? t.palette.success.main : t.palette.text.secondary),
				'&:hover': { bgcolor: (t) => t.palette.success.softBg },
				width: 52,
				height: 52,
			}}
		>
			{children}
		</IconButton>
	</Tooltip>
)

const ControlBar = ({
	micOn,
	camOn,
	onToggleMic,
	onToggleCam,
	onToggleChat,
	onEndCall,
	recordingEnabled = false,
	isRecording = false,
	isRecordingBusy = false,
	onStartRecording,
	onStopRecording,
}) => {
	const { t } = useTranslation()
	return (
		<Stack direction='row' spacing={1.5} alignItems='center' justifyContent='center' sx={{ mt: 2.5 }}>
			<ControlButton title={t('meeting_room.mic')} onClick={onToggleMic}>
				{micOn ? <Mic /> : <MicOff />}
			</ControlButton>
			<ControlButton title={t('meeting_room.camera')} onClick={onToggleCam}>
				{camOn ? <Videocam /> : <VideocamOff />}
			</ControlButton>
			<ControlButton title={t('meeting_room.chat')} onClick={onToggleChat}>
				<Chat />
			</ControlButton>
			{recordingEnabled && (
				<Button
					variant='contained'
					color={isRecording ? 'warning' : 'secondary'}
					startIcon={isRecording ? <Stop /> : <FiberManualRecord />}
					sx={{ borderRadius: 999, px: 2.5 }}
					disabled={isRecordingBusy}
					onClick={isRecording ? onStopRecording : onStartRecording}
				>
					{isRecording ? t('telehealth.button.stop_recording') : t('telehealth.button.start_recording')}
				</Button>
			)}
			<Button
				variant='contained'
				color='error'
				startIcon={<CallEnd />}
				sx={{ ml: 2, borderRadius: 999, px: 2.5 }}
				onClick={onEndCall}
			>
				{t('meeting_room.end_call')}
			</Button>
		</Stack>
	)
}

export default ControlBar
