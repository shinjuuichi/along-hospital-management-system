import { MeetingRoomRecordingStatus } from '@/constants/meetingRoomConstants'
import { useEffect } from 'react'
import { useReactMediaRecorder } from 'react-media-recorder'

const MeetingRoomRecorderBridge = ({ onStateChange, onStopBlob }) => {
	const { status, startRecording, stopRecording } = useReactMediaRecorder({
		screen: true,
		audio: true,
		video: false,
		stopStreamsOnStop: true,
		askPermissionOnMount: false,
		blobPropertyBag: {
			type: 'video/webm',
		},
		onStop: (_, blob) => {
			onStopBlob?.(blob)
		},
	})

	useEffect(() => {
		const normalizedStatus = status || MeetingRoomRecordingStatus.IDLE

		onStateChange?.({
			status: normalizedStatus,
			startRecording,
			stopRecording,
		})
	}, [onStateChange, startRecording, status, stopRecording])

	return null
}

export default MeetingRoomRecorderBridge
