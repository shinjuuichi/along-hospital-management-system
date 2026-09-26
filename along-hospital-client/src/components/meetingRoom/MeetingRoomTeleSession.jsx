import { MeetingRoomRecordingStatus } from '@/constants/meetingRoomConstants'
import useMeetingSession from '@/hooks/useMeetingSession'
import useTranslation from '@/hooks/useTranslation'
import ChatSidebar from '@/pages/patients/meetingRoomPage/components/ChatSidebar'
import ControlBar from '@/pages/patients/meetingRoomPage/components/ControlBar'
import VideoContainer from '@/pages/patients/meetingRoomPage/components/VideoContainer'
import { ScreenShare } from '@mui/icons-material'
import { Alert, Box, Chip, CircularProgress, Paper, Stack, Typography } from '@mui/material'
import { lazy, Suspense, useCallback, useEffect, useMemo, useRef, useState } from 'react'
import { toast } from 'react-toastify'

const MeetingRoomRecorderBridge = lazy(() => import('./MeetingRoomRecorderBridge'))

const resolveFetchError = (error) => {
	const data = error?.response?.data || {}
	let message = data?.message

	if (!message && data?.error !== undefined && data?.error !== null) {
		const errorField = data.error

		if (typeof errorField === 'string') {
			message = errorField
		} else if (Array.isArray(errorField)) {
			const firstString = errorField.find((item) => typeof item === 'string')
			message = firstString || errorField.join(', ')
		} else if (typeof errorField === 'object') {
			if (typeof errorField.message === 'string') {
				message = errorField.message
			} else {
				for (const value of Object.values(errorField)) {
					if (typeof value === 'string') {
						message = value
						break
					}

					if (Array.isArray(value)) {
						const innerString = value.find((item) => typeof item === 'string')
						if (innerString) {
							message = innerString
							break
						}
					}
				}
			}
		}
	}

	return {
		message: message || '',
	}
}

const defaultResolveStatusContent = ({ message }, t) => ({
	title: t('telehealth.status.unavailable_title'),
	description: message || t('telehealth.error.session_not_ready'),
})

const resolveSessionRuntimeError = (message, t) => {
	switch (message) {
		case 'MEETING_CONNECTION_FAILED':
		case 'MEETING_REJOIN_FAILED':
			return t('telehealth.error.connection_failed')
		case 'ROOM_MEMBERSHIP_REQUIRED':
			return t('telehealth.error.session_not_ready')
		default:
			return message || t('telehealth.error.session_not_ready')
	}
}

const formatDateForFileName = (date = new Date()) => {
	const day = String(date.getDate()).padStart(2, '0')
	const month = String(date.getMonth() + 1).padStart(2, '0')
	const year = date.getFullYear()

	return `${day}-${month}-${year}`
}

const downloadRecordingBlob = (blob, fileName) => {
	const blobUrl = URL.createObjectURL(blob)
	const anchor = document.createElement('a')
	anchor.href = blobUrl
	anchor.download = fileName
	document.body.appendChild(anchor)
	anchor.click()
	anchor.remove()
	window.setTimeout(() => URL.revokeObjectURL(blobUrl), 1500)
}

const MeetingRoomTeleSession = ({
	credentials,
	transactionId = null,
	roomCode = null,
	isCaller = false,
	fetchError = null,
	loading = false,
	header = null,
	onAfterEndCall,
	onAfterSessionExpired,
	resolveStatusContent = defaultResolveStatusContent,
	expiringNotice = '',
	enablePatientRecording = false,
}) => {
	const { t } = useTranslation()
	const [sessionErrorMessage, setSessionErrorMessage] = useState('')
	const [sessionNotice, setSessionNotice] = useState('')
	const [showChat, setShowChat] = useState(false)
	const [chatInput, setChatInput] = useState('')
	const [remainingTimeLabel, setRemainingTimeLabel] = useState('')
	const [recorderStatus, setRecorderStatus] = useState(MeetingRoomRecordingStatus.IDLE)
	const recorderControlsRef = useRef({
		startRecording: null,
		stopRecording: null,
	})

	const resolvedFetchError = useMemo(() => resolveFetchError(fetchError), [fetchError])
	const recordingEnabled = enablePatientRecording && isCaller

	const handleRecorderStateChange = useCallback((nextState) => {
		recorderControlsRef.current = {
			startRecording: nextState?.startRecording || null,
			stopRecording: nextState?.stopRecording || null,
		}
		setRecorderStatus((prev) => {
			const nextStatus = nextState?.status || MeetingRoomRecordingStatus.IDLE
			return prev === nextStatus ? prev : nextStatus
		})
	}, [])

	const handleRecordingStopped = useCallback(
		(blob) => {
			if (!recordingEnabled) {
				return
			}

			if (!blob || blob.size === 0) {
				toast.error(t('telehealth.error.record_empty_file'))
				return
			}

			const fileName = `TeleHealth Meeting - ${formatDateForFileName()}.webm`
			downloadRecordingBlob(blob, fileName)
			toast.success(t('telehealth.notice.record_downloaded'))
		},
		[recordingEnabled, t]
	)

	const isRecording = recorderStatus === MeetingRoomRecordingStatus.RECORDING
	const isRecordingBusy = recorderStatus === MeetingRoomRecordingStatus.ACQUIRING_MEDIA

	const {
		localVideoRef,
		remoteVideoRef,
		isMicOn,
		isCamOn,
		remoteMicOn,
		remoteCamOn,
		handleToggleMic,
		handleToggleCam,
		handleEndCall,
		messages,
		handleSendMessage,
	} = useMeetingSession({
		credentials,
		transactionId,
		roomCode,
		isCaller,
		onJoinFailed: (payload) => {
			setSessionErrorMessage(resolveSessionRuntimeError(payload?.message, t))
		},
		onSessionExpiring: () => {
			if (expiringNotice) {
				setSessionNotice(expiringNotice)
			}
		},
		onSessionExpired: (payload) => {
			setSessionNotice('')
			onAfterSessionExpired?.(payload)
		},
	})

	useEffect(() => {
		if (!credentials?.expireAt) {
			setRemainingTimeLabel('')
			return
		}

		const clientStartTime = Date.now()
		const serverStartTime = credentials?.serverNow
			? new Date(credentials.serverNow).getTime()
			: clientStartTime
		const expireAt = new Date(credentials.expireAt).getTime()

		const updateRemainingTime = () => {
			const elapsedMs = Date.now() - clientStartTime
			const currentServerTime = serverStartTime + elapsedMs
			const diffMs = expireAt - currentServerTime
			if (diffMs <= 0) {
				setRemainingTimeLabel(t('meeting_room.time.ended'))
				return
			}

			const totalSeconds = Math.floor(diffMs / 1000)
			const minutes = Math.floor(totalSeconds / 60)
			const seconds = totalSeconds % 60
			setRemainingTimeLabel(`${minutes}:${String(seconds).padStart(2, '0')}`)
		}

		updateRemainingTime()
		const timer = window.setInterval(updateRemainingTime, 1000)
		return () => window.clearInterval(timer)
	}, [credentials?.expireAt, credentials?.serverNow, t])

	const renderStatus = (errorState) => {
		const { title, description } = resolveStatusContent(errorState, t)

		return (
			<Paper variant='outlined' sx={{ p: 3, borderRadius: 2 }}>
				<Stack spacing={1.5}>
					<Typography variant='h6'>{title}</Typography>
					<Box color='text.secondary'>{description}</Box>
				</Stack>
			</Paper>
		)
	}

	if (loading && !credentials) {
		return (
			<Paper variant='outlined' sx={{ p: 3, borderRadius: 2 }}>
				<Stack direction='row' spacing={1.5} alignItems='center'>
					<CircularProgress size={20} />
					<Typography color='text.secondary'>{t('telehealth.status.loading')}</Typography>
				</Stack>
			</Paper>
		)
	}

	if (fetchError) {
		return renderStatus(resolvedFetchError)
	}

	if (sessionErrorMessage) {
		return renderStatus({ message: sessionErrorMessage })
	}

	const handleChatSend = () => {
		if (!chatInput.trim()) return
		handleSendMessage(chatInput)
		setChatInput('')
	}

	const handleStartRecording = () => {
		if (!recordingEnabled) {
			return
		}

		recorderControlsRef.current.startRecording?.()
	}

	const handleStopRecording = () => {
		if (!recordingEnabled) {
			return
		}

		recorderControlsRef.current.stopRecording?.()
	}

	return (
		<Box>
			{recordingEnabled && (
				<Suspense fallback={null}>
					<MeetingRoomRecorderBridge
						onStateChange={handleRecorderStateChange}
						onStopBlob={handleRecordingStopped}
					/>
				</Suspense>
			)}

			<Stack direction={{ xs: 'column', md: 'row' }} spacing={2}>
				<Stack sx={{ flex: 1 }}>
					<Paper
						variant='outlined'
						sx={{
							mb: 2,
							p: 2,
							borderRadius: 2,
							background: 'linear-gradient(135deg, rgba(18,101,160,0.08) 0%, rgba(18,101,160,0.02) 100%)',
						}}
					>
						<Stack
							direction={{ xs: 'column', md: 'row' }}
							spacing={1.5}
							justifyContent='space-between'
							alignItems={{ xs: 'flex-start', md: 'center' }}
						>
							<Box>
								{header}
								<Typography variant='body2' color='text.secondary'>
									{t('meeting_room.subtitle.session_ready')}
								</Typography>
							</Box>

							<Stack direction='row' spacing={1} flexWrap='wrap' useFlexGap>
								{remainingTimeLabel && (
									<Chip
										size='small'
										variant='outlined'
										label={`${t('meeting_room.title.duration_left')}: ${remainingTimeLabel}`}
									/>
								)}
								{isRecording && (
									<Chip
										size='small'
										color='error'
										icon={<ScreenShare />}
										label={t('telehealth.status.recording')}
									/>
								)}
								{isRecordingBusy && (
									<Chip size='small' variant='outlined' label={t('telehealth.status.record_acquiring')} />
								)}
							</Stack>
						</Stack>
					</Paper>
					{sessionNotice && (
						<Alert severity='warning' sx={{ mb: 2 }}>
							{sessionNotice}
						</Alert>
					)}

					<VideoContainer
						remoteVideoRef={remoteVideoRef}
						localVideoRef={localVideoRef}
						remoteMicOn={remoteMicOn}
						remoteCamOn={remoteCamOn}
						isMicOn={isMicOn}
						isCamOn={isCamOn}
					/>

					<ControlBar
						micOn={isMicOn}
						camOn={isCamOn}
						onToggleMic={handleToggleMic}
						onToggleCam={handleToggleCam}
						onToggleChat={() => setShowChat(!showChat)}
						recordingEnabled={recordingEnabled}
						isRecording={isRecording}
						isRecordingBusy={isRecordingBusy}
						onStartRecording={handleStartRecording}
						onStopRecording={handleStopRecording}
						onEndCall={async () => {
							if (isRecording) {
								handleStopRecording()
							}
							await handleEndCall()
							await onAfterEndCall?.()
						}}
					/>
				</Stack>

				<ChatSidebar
					show={showChat}
					messages={messages}
					chatInput={chatInput}
					setShow={setShowChat}
					setChatInput={setChatInput}
					onSend={handleChatSend}
				/>
			</Stack>
		</Box>
	)
}

export default MeetingRoomTeleSession
