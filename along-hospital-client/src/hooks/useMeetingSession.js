import useMeetingSignalR from '@/hooks/useMeetingSignalR'
import useWebRtcPeer from '@/hooks/useWebRtcPeer'
import { useCallback, useEffect, useMemo, useRef, useState } from 'react'

const describeStreamTracks = (stream) =>
	stream?.getTracks?.().map((track) => ({
		id: track.id,
		kind: track.kind,
		enabled: track.enabled,
		muted: track.muted,
		readyState: track.readyState,
	})) ?? []

/**
 * Custom hook for managing WebRTC meeting session
 * @param {Object} config
 * @param {Object} config.credentials - ICE servers and SignalR config
 * @param {string} config.transactionId - Transaction ID (for patient)
 * @param {string} config.roomCode - Room code (for doctor)
 * @param {boolean} config.isCaller - Whether this is the caller (patient = true)
 * @param {Function} config.onLocalStream - Callback for local stream
 * @param {Function} config.onRemoteStream - Callback for remote stream
 */
const useMeetingSession = ({
	credentials,
	transactionId,
	roomCode,
	isCaller = false,
	onLocalStream,
	onRemoteStream,
	onJoinFailed,
	onSessionExpiring,
	onSessionExpired,
}) => {
	const localVideoRef = useRef(null)
	const remoteVideoRef = useRef(null)
	const remoteConnectionIdRef = useRef(null)

	const playVideoElement = useCallback(async (videoElement) => {
		if (!videoElement?.play) {
			return
		}

		try {
			await videoElement.play()
		} catch (error) {
			if (error?.name === 'AbortError') {
				return
			}

			console.error('[Meeting] Video play error:', error)
		}
	}, [])

	const resetVideoElement = useCallback((videoElement) => {
		if (!videoElement) {
			return
		}

		videoElement.pause?.()
		videoElement.srcObject = null
		videoElement.load()
	}, [])

	const [hasRemoteParticipant, setHasRemoteParticipant] = useState(false)
	const [shouldCreateOffer, setShouldCreateOffer] = useState(false)
	const [pendingOffer, setPendingOffer] = useState(null)
	const [remoteMicOn, setRemoteMicOn] = useState(true)
	const [remoteCamOn, setRemoteCamOn] = useState(true)
	const [messages, setMessages] = useState([])

	const iceServers = useMemo(() => credentials?.iceServers ?? [], [credentials])
	const signalRHubUrl = useMemo(() => credentials?.signalR?.hubUrl, [credentials])

	const handleLocalStream = useCallback(
		(stream) => {
			if (localVideoRef.current) {
				localVideoRef.current.srcObject = stream
				void playVideoElement(localVideoRef.current)
			}
			console.info('[MeetingSession][LocalStreamBound]', {
				tracks: describeStreamTracks(stream),
			})
			onLocalStream?.(stream)
		},
		[onLocalStream, playVideoElement]
	)

	const handleRemoteStream = useCallback(
		(stream) => {
			if (remoteVideoRef.current) {
				remoteVideoRef.current.srcObject = stream
				void playVideoElement(remoteVideoRef.current)
			}
			console.info('[MeetingSession][RemoteStreamBound]', {
				tracks: describeStreamTracks(stream),
				remoteCamOn,
			})
			onRemoteStream?.(stream)
		},
		[onRemoteStream, playVideoElement, remoteCamOn]
	)

	const {
		createOffer,
		createAnswer,
		setRemoteDescription,
		addIceCandidate,
		toggleAudio,
		toggleVideo,
		hangUp,
		renegotiate,
		resetForNewSession,
		clearRemoteStream,
		localStream,
		isAudioEnabled,
		isVideoEnabled,
	} = useWebRtcPeer({
		iceServers,
		onLocalStream: handleLocalStream,
		onRemoteStream: handleRemoteStream,
		onIceCandidate: (c) => sendIceCandidate(c),
	})

	const {
		sendOffer,
		sendAnswer,
		sendIceCandidate,
		notifyState,
		sendMessage,
		leaveSession,
		startConnection,
		stopConnection,
		joinedRoomCode,
	} = useMeetingSignalR({
		transactionId,
		roomCode,
		hubUrl: signalRHubUrl,
		onJoinSucceeded: (payload) => {
			const serverWantsOffer = payload?.shouldCreateOffer ?? payload?.ShouldCreateOffer
			console.info('[MeetingSession][JoinSucceeded]', {
				isCaller,
				serverWantsOffer,
				existingParticipants: payload?.existingParticipants ?? payload?.ExistingParticipants ?? [],
			})
			if (serverWantsOffer && isCaller) {
				setShouldCreateOffer(true)
			}
		},
		onJoinFailed: (payload) => {
			onJoinFailed?.(payload)
		},
		onParticipantJoined: (participant) => {
			const connectionId = participant?.connectionId
			const role = participant?.role

			if (!connectionId) {
				return
			}

			remoteConnectionIdRef.current = connectionId
			setHasRemoteParticipant(true)
			console.info('[MeetingSession][ParticipantJoined]', {
				isCaller,
				connectionId,
				role,
			})

			if (isCaller && role === 'Doctor') {
				setShouldCreateOffer(true)
			}
		},
		onParticipantLeft: (id) => {
			if (id === remoteConnectionIdRef.current) {
				setHasRemoteParticipant(false)
				setShouldCreateOffer(false)
				remoteConnectionIdRef.current = null
				setRemoteMicOn(true)
				setRemoteCamOn(true)
				setPendingOffer(null)
				setMessages([])

				resetForNewSession()
				resetVideoElement(remoteVideoRef.current)
			}
		},
		onOffer: async (senderId, offer) => {
			console.info('[MeetingSession][OnOffer]', {
				senderId,
				type: offer?.type,
				hasLocalStream: Boolean(localStream),
			})
			if (!localStream) {
				setPendingOffer(offer)
				return
			}
			try {
				await setRemoteDescription(offer)
				const answer = await createAnswer()
				await sendAnswer(answer)
			} catch (error) {
				if (error.message?.includes('order of m-lines')) {
					setPendingOffer(offer)
				}
			}
		},
		onSessionExpiring: (payload) => {
			onSessionExpiring?.(payload)
		},
		onSessionExpired: (payload) => {
			if (!isCaller) {
				return
			}

			resetVideoElement(localVideoRef.current)
			resetVideoElement(remoteVideoRef.current)

			resetForNewSession()
			clearRemoteStream()
			remoteConnectionIdRef.current = null
			setHasRemoteParticipant(false)
			setShouldCreateOffer(false)
			setPendingOffer(null)
			setRemoteMicOn(true)
			setRemoteCamOn(true)
			setMessages([])

			hangUp()
			leaveSession().catch((err) => console.error('[Meeting] Leave session error:', err))
			onSessionExpired?.(payload)
		},
		onAnswer: async (senderId, answer) => {
			console.info('[MeetingSession][OnAnswer]', {
				senderId,
				type: answer?.type,
			})
			await setRemoteDescription(answer)
			setPendingOffer(null)
		},
		onIceCandidate: async (senderId, candidate) => {
			await addIceCandidate(candidate)
		},
		onStateUpdated: (senderId, state) => {
			console.info('[MeetingSession][RemoteStateUpdated]', {
				senderId,
				state,
			})
			if (typeof state?.micOn === 'boolean') setRemoteMicOn(state.micOn)
			if (typeof state?.camOn === 'boolean') setRemoteCamOn(state.camOn)
		},
		onMessage: (senderId, message) => {
			setMessages((prev) => [
				...prev,
				{
					id: Date.now(),
					from: senderId,
					content: message,
					timestamp: new Date(),
					isOwn: false,
				},
			])
		},
	})

	useEffect(() => {
		if (!credentials || !signalRHubUrl) return
		startConnection()
		return () => stopConnection()
	}, [credentials, signalRHubUrl, startConnection, stopConnection])

	useEffect(() => {
		if (!joinedRoomCode || !hasRemoteParticipant) {
			return
		}

		notifyState({
			micOn: isAudioEnabled,
			camOn: isVideoEnabled,
		}).catch((error) => {
			console.error('[Meeting] Sync state error:', error)
		})
	}, [joinedRoomCode, hasRemoteParticipant, isAudioEnabled, isVideoEnabled, notifyState])

	useEffect(() => {
		if (!isCaller) return
		if (!localStream) return
		if (!hasRemoteParticipant) return
		if (!shouldCreateOffer) return
		if (pendingOffer) return
		if (!joinedRoomCode && !roomCode) return
		;(async () => {
			try {
				console.info('[MeetingSession][CreateOfferTriggered]', {
					isCaller,
					hasRemoteParticipant,
					shouldCreateOffer,
				})
				setShouldCreateOffer(false)
				const offer = await createOffer()
				setPendingOffer(offer)
				try {
					await sendOffer(offer)
				} catch (error) {
					setPendingOffer(null)
					throw error
				}
			} catch (e) {
				console.error('[Meeting] Create offer error:', e)
			}
		})()
	}, [
		hasRemoteParticipant,
		shouldCreateOffer,
		pendingOffer,
		isCaller,
		localStream,
		joinedRoomCode,
		roomCode,
		createOffer,
		sendOffer,
	])

	useEffect(() => {
		if (!localStream) return
		if (isCaller) return
		if (!pendingOffer) return
		if (typeof pendingOffer !== 'object' || !pendingOffer.type) return
		;(async () => {
			try {
				console.info('[MeetingSession][CreateAnswerTriggered]', {
					isCaller,
					pendingOfferType: pendingOffer?.type,
				})
				await setRemoteDescription(pendingOffer)
				const answer = await createAnswer()
				await sendAnswer(answer)
				setPendingOffer(null)
			} catch (e) {
				console.error('[Meeting] Create answer error:', e)
			}
		})()
	}, [localStream, isCaller, pendingOffer, setRemoteDescription, createAnswer, sendAnswer])

	const handleToggleMic = useCallback(() => {
		const next = !isAudioEnabled
		toggleAudio()
		notifyState({ micOn: next })
	}, [isAudioEnabled, toggleAudio, notifyState])

	const handleToggleCam = useCallback(async () => {
		const next = !isVideoEnabled
		await toggleVideo()
		notifyState({ camOn: next })
		if (isCaller) {
			try {
				if (!hasRemoteParticipant || pendingOffer) {
					return
				}
				console.info('[MeetingSession][CameraToggleRenegotiate]', {
					nextCamOn: next,
				})
				const offer = await renegotiate()
				setPendingOffer(offer)
				try {
					await sendOffer(offer)
				} catch (error) {
					setPendingOffer(null)
					throw error
				}
			} catch (e) {
				console.error('[Meeting] Camera toggle renegotiate error:', e)
			}
		}
	}, [
		toggleVideo,
		notifyState,
		isCaller,
		hasRemoteParticipant,
		pendingOffer,
		renegotiate,
		sendOffer,
		isVideoEnabled,
	])

	useEffect(() => {
		console.info('[MeetingSession][RemoteCamFlagChanged]', {
			remoteCamOn,
			remoteTracks: describeStreamTracks(remoteVideoRef.current?.srcObject),
		})
	}, [remoteCamOn])

	const handleSendMessage = useCallback(
		async (message) => {
			if (!message.trim()) return
			try {
				await sendMessage(message)
				setMessages((prev) => [
					...prev,
					{
						id: Date.now(),
						from: 'self',
						content: message,
						timestamp: new Date(),
						isOwn: true,
					},
				])
			} catch (error) {
				console.error('[Meeting] Send message error:', error)
			}
		},
		[sendMessage]
	)

	const handleEndCall = useCallback(async () => {
		try {
			await leaveSession()

			resetVideoElement(localVideoRef.current)
			resetVideoElement(remoteVideoRef.current)

			hangUp()
		} catch (error) {
			console.error('[Meeting] End call error:', error)
		}
	}, [leaveSession, hangUp, resetVideoElement])

	return {
		localVideoRef,
		remoteVideoRef,
		isMicOn: isAudioEnabled,
		isCamOn: isVideoEnabled,
		remoteMicOn,
		remoteCamOn,
		hasRemoteParticipant,
		messages,
		handleToggleMic,
		handleToggleCam,
		handleEndCall,
		handleSendMessage,
	}
}

export default useMeetingSession
