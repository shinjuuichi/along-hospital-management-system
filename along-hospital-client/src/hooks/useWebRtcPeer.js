/* eslint-disable react-hooks/exhaustive-deps */
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
 * Custom hook for managing WebRTC peer connections with audio/video streaming
 *
 * @param {Object} config - Configuration object for the WebRTC peer connection
 * @param {RTCIceServer[]} [config.iceServers=[]] - Array of ICE servers for NAT traversal (STUN/TURN servers)
 * @param {function(MediaStream): void} [config.onLocalStream] - Callback when local media stream is obtained
 * @param {function(MediaStream): void} [config.onRemoteStream] - Callback when remote media stream is received
 * @param {function(RTCIceCandidateInit): void} [config.onIceCandidate] - Callback when ICE candidate is generated
 *
 * @returns {Object} WebRTC peer connection interface
 * @returns {function(): Promise<RTCSessionDescriptionInit>} returns.createOffer - Creates an SDP offer for initiating connection
 * @returns {function(): Promise<RTCSessionDescriptionInit>} returns.createAnswer - Creates an SDP answer in response to an offer
 * @returns {function(RTCSessionDescriptionInit): Promise<void>} returns.setRemoteDescription - Sets the remote peer's SDP description
 * @returns {function(RTCIceCandidateInit): Promise<void>} returns.addIceCandidate - Adds an ICE candidate from the remote peer
 * @returns {function(): Promise<RTCSessionDescriptionInit>} returns.renegotiate - Renegotiates the connection (creates new offer)
 * @returns {function(): void} returns.toggleAudio - Toggles local audio track on/off
 * @returns {function(): Promise<void>} returns.toggleVideo - Toggles local video track on/off
 * @returns {function(): void} returns.hangUp - Closes connection and stops all media tracks
 * @returns {function(): void} returns.resetForNewSession - Resets connection state for a new session
 * @returns {function(): void} returns.clearRemoteStream - Clears and stops the remote media stream
 * @returns {MediaStream|null} returns.localStream - Current local media stream (audio/video)
 * @returns {MediaStream|null} returns.remoteStream - Current remote media stream (audio/video)
 * @returns {boolean} returns.isAudioEnabled - Whether local audio is currently enabled
 * @returns {boolean} returns.isVideoEnabled - Whether local video is currently enabled
 */

export default function useWebRtcPeer({
	iceServers = [],
	onLocalStream,
	onRemoteStream,
	onIceCandidate,
}) {
	const pcRef = useRef(null)
	const localStreamRef = useRef(null)
	const remoteStreamRef = useRef(null)
	const candidateQueueRef = useRef([])
	const expectingAnswerRef = useRef(false)
	const desiredAudioEnabledRef = useRef(true)
	const desiredVideoEnabledRef = useRef(true)

	// Keep latest callbacks without forcing effect re-run
	const onLocalStreamRef = useRef(onLocalStream)
	const onRemoteStreamRef = useRef(onRemoteStream)
	const onIceCandidateRef = useRef(onIceCandidate)

	useEffect(() => {
		onLocalStreamRef.current = onLocalStream
	}, [onLocalStream])

	useEffect(() => {
		onRemoteStreamRef.current = onRemoteStream
	}, [onRemoteStream])

	useEffect(() => {
		onIceCandidateRef.current = onIceCandidate
	}, [onIceCandidate])

	// State for managing media streams and controls
	const [localStream, setLocalStream] = useState(null)
	const [remoteStream, setRemoteStream] = useState(null)
	const [isAudioEnabled, setIsAudioEnabled] = useState(true)
	const [isVideoEnabled, setIsVideoEnabled] = useState(true)
	const [sessionId, setSessionId] = useState(0) // Force PC re-init on reset

	useEffect(() => {
		desiredAudioEnabledRef.current = isAudioEnabled
	}, [isAudioEnabled])

	useEffect(() => {
		desiredVideoEnabledRef.current = isVideoEnabled
	}, [isVideoEnabled])

	// Memoized key to detect ICE servers changes
	const iceServersKey = useMemo(() => JSON.stringify(iceServers), [iceServers])

	/**
	 * Main effect: Initializes RTCPeerConnection and sets up media streams
	 * Runs when iceServers change or when session is reset
	 */
	useEffect(() => {
		if (!iceServers || iceServers.length === 0) {
			return
		}

		const pc = new RTCPeerConnection({
			iceServers,
			iceTransportPolicy: 'all',
		})
		pcRef.current = pc

		pc.onicecandidate = (e) => {
			const c = e.candidate
			if (!c) return
			const data = typeof c.toJSON === 'function' ? c.toJSON() : c
			onIceCandidateRef.current?.(data)
		}

		/**
		 * Ensures remote stream exists, creates if needed
		 * @returns {MediaStream} The remote media stream
		 */
		const ensureRemoteStream = () => {
			if (!remoteStreamRef.current) {
				remoteStreamRef.current = new MediaStream()
				setRemoteStream(remoteStreamRef.current)
				onRemoteStreamRef.current?.(remoteStreamRef.current)
			}
			return remoteStreamRef.current
		}

		// Handle incoming remote tracks (audio/video)
		pc.ontrack = (e) => {
			const rs = ensureRemoteStream()
			e.streams[0].getTracks().forEach((t) => {
				const exists = rs.getTracks().some((x) => x.id === t.id)
				if (!exists) rs.addTrack(t)
			})

			console.info('[WebRTC][Track]', {
				trackKind: e.track?.kind,
				trackId: e.track?.id,
				trackEnabled: e.track?.enabled,
				trackMuted: e.track?.muted,
				trackReadyState: e.track?.readyState,
				remoteTracks: describeStreamTracks(rs),
			})

			setRemoteStream(rs)
			onRemoteStreamRef.current?.(rs)
		}
		// Request local media (audio + video) with fallback to audio-only
		;(async () => {
			try {
				const stream = await navigator.mediaDevices.getUserMedia({
					audio: true,
					video: true,
				})
				stream.getAudioTracks().forEach((track) => {
					track.enabled = desiredAudioEnabledRef.current
				})
				stream.getVideoTracks().forEach((track) => {
					track.enabled = desiredVideoEnabledRef.current
				})
				localStreamRef.current = stream
				setLocalStream(stream)
				onLocalStreamRef.current?.(stream)
				stream.getTracks().forEach((track) => pc.addTrack(track, stream))
				console.info('[WebRTC][LocalStreamReady]', {
					localTracks: describeStreamTracks(stream),
				})
			} catch (err) {
				console.error('[WebRTC] Get user media error:', err)
				// Fallback: try audio-only if video fails
				try {
					const stream = await navigator.mediaDevices.getUserMedia({
						audio: true,
						video: false,
					})
					pc.addTransceiver('video', { direction: 'recvonly' })
					stream.getAudioTracks().forEach((track) => {
						track.enabled = desiredAudioEnabledRef.current
					})
					localStreamRef.current = stream
					setLocalStream(stream)
					onLocalStreamRef.current?.(stream)
					stream.getTracks().forEach((track) => pc.addTrack(track, stream))
					console.info('[WebRTC][LocalStreamReadyAudioOnly]', {
						localTracks: describeStreamTracks(stream),
					})
					setIsVideoEnabled(false)
				} catch (e) {
					console.error('[WebRTC] Get audio only media error:', e)
					setIsAudioEnabled(false)
					setIsVideoEnabled(false)
				}
			}
		})()

		// Cleanup on unmount or when iceServers/session changes
		return () => {
			pc.close()
			pcRef.current = null

			localStreamRef.current?.getTracks().forEach((t) => t.stop())
			localStreamRef.current = null

			remoteStreamRef.current = null
			setRemoteStream(null)

			expectingAnswerRef.current = false
			candidateQueueRef.current = []
		}
	}, [iceServersKey, sessionId])

	/**
	 * Creates an SDP offer to initiate a WebRTC connection
	 * Sets local description and marks that we're expecting an answer
	 * @returns {Promise<RTCSessionDescriptionInit>} The created offer
	 */
	const createOffer = useCallback(async () => {
		const pc = pcRef.current
		const offer = await pc.createOffer({ iceRestart: false })
		await pc.setLocalDescription(offer)
		expectingAnswerRef.current = true
		console.info('[WebRTC][CreateOffer]', {
			signalingState: pc?.signalingState,
			localTracks: describeStreamTracks(localStreamRef.current),
		})
		return offer
	}, [])

	/**
	 * Creates an SDP answer in response to a received offer
	 * Sets local description with the answer
	 * @returns {Promise<RTCSessionDescriptionInit>} The created answer
	 */
	const createAnswer = useCallback(async () => {
		const pc = pcRef.current
		const answer = await pc.createAnswer()
		await pc.setLocalDescription(answer)
		console.info('[WebRTC][CreateAnswer]', {
			signalingState: pc?.signalingState,
			localTracks: describeStreamTracks(localStreamRef.current),
		})
		return answer
	}, [])

	/**
	 * Sets the remote peer's SDP description (offer or answer)
	 * Validates signaling state and processes queued ICE candidates after setting
	 * @param {RTCSessionDescriptionInit} desc - The remote description (offer or answer)
	 * @returns {Promise<void>}
	 * @throws {Error} If PeerConnection is not initialized or state is invalid
	 */
	const setRemoteDescription = useCallback(async (desc) => {
		const pc = pcRef.current

		if (!pc) {
			throw new Error('PeerConnection not initialized')
		}

		if (desc?.type === 'offer' && pc.signalingState !== 'stable') {
			throw new Error(`Cannot process offer in state: ${pc.signalingState}`)
		}

		if (desc?.type === 'answer') {
			if (pc.signalingState !== 'have-local-offer' || !expectingAnswerRef.current) {
				return
			}
		}

		await pc.setRemoteDescription(new RTCSessionDescription(desc))
		console.info('[WebRTC][SetRemoteDescription]', {
			type: desc?.type,
			signalingState: pc.signalingState,
		})

		if (desc?.type === 'answer') expectingAnswerRef.current = false

		// Process any queued ICE candidates
		if (candidateQueueRef.current.length > 0) {
			for (const c of candidateQueueRef.current) {
				await pc.addIceCandidate(new RTCIceCandidate(c))
			}
			candidateQueueRef.current = []
		}
	}, [])

	/**
	 * Adds an ICE candidate from the remote peer
	 * Queues candidate if remote description not yet set
	 * @param {RTCIceCandidateInit} candidate - The ICE candidate to add
	 * @returns {Promise<void>}
	 */
	const addIceCandidate = useCallback(async (candidate) => {
		if (!candidate) return
		const pc = pcRef.current
		if (!pc.remoteDescription) {
			// Queue if remote description not ready yet
			candidateQueueRef.current.push(candidate)
			return
		}
		await pc.addIceCandidate(new RTCIceCandidate(candidate))
	}, [])

	/**
	 * Renegotiates the connection by creating a new offer
	 * Used when connection parameters need to change
	 * @returns {Promise<RTCSessionDescriptionInit>} The new offer
	 */
	const renegotiate = useCallback(async () => {
		const pc = pcRef.current
		const offer = await pc.createOffer({ iceRestart: false })
		await pc.setLocalDescription(offer)
		expectingAnswerRef.current = true
		console.info('[WebRTC][Renegotiate]', {
			signalingState: pc?.signalingState,
			localTracks: describeStreamTracks(localStreamRef.current),
		})
		return offer
	}, [])

	/**
	 * Toggles local audio track on/off (mute/unmute)
	 * @returns {void}
	 */
	const toggleAudio = useCallback(() => {
		const ls = localStreamRef.current
		if (!ls) return

		const audioTracks = ls.getAudioTracks()

		const newEnabled = !isAudioEnabled
		audioTracks.forEach((track) => {
			track.enabled = newEnabled
		})

		setIsAudioEnabled(newEnabled)
	}, [isAudioEnabled])

	/**
	 * Toggles local video track on/off (show/hide camera)
	 * Triggers onLocalStream callback after toggling
	 * @returns {Promise<void>}
	 */
	const toggleVideo = useCallback(async () => {
		const ls = localStreamRef.current
		if (!ls) return

		const videoTracks = ls.getVideoTracks()

		const newEnabled = !isVideoEnabled
		videoTracks.forEach((track) => {
			track.enabled = newEnabled
		})

		setIsVideoEnabled(newEnabled)
		onLocalStreamRef.current?.(ls)
	}, [isVideoEnabled])

	/**
	 * Clears and stops all tracks in the remote stream
	 * @returns {void}
	 */
	const clearRemoteStream = useCallback(() => {
		if (remoteStreamRef.current) {
			remoteStreamRef.current.getTracks().forEach((track) => {
				track.stop()
			})
			remoteStreamRef.current = null
			setRemoteStream(null)
		}
	}, [])

	/**
	 * Completely terminates the connection and stops all media
	 * Closes peer connection, stops all tracks, and resets state
	 * @returns {void}
	 */
	const hangUp = useCallback(() => {
		const pc = pcRef.current
		try {
			pc?.getSenders().forEach((s) => {
				if (s.track) {
					s.track.stop()
				}
			})
			pc?.close()
		} finally {
			pcRef.current = null
			localStreamRef.current?.getTracks().forEach((t) => {
				t.stop()
			})
			localStreamRef.current = null
			setLocalStream(null)
			clearRemoteStream()
			setIsVideoEnabled(false)
			setIsAudioEnabled(false)
			candidateQueueRef.current = []
			expectingAnswerRef.current = false
		}
	}, [clearRemoteStream])

	/**
	 * Resets connection state for a new session
	 * Clears remote stream and increments session ID to trigger reconnection
	 * @returns {void}
	 */
	const resetForNewSession = useCallback(() => {
		clearRemoteStream()
		candidateQueueRef.current = []
		expectingAnswerRef.current = false
		setIsAudioEnabled(true)
		setIsVideoEnabled(true)
		setSessionId((prev) => prev + 1)
	}, [clearRemoteStream])

	return {
		createOffer,
		createAnswer,
		setRemoteDescription,
		addIceCandidate,
		renegotiate,
		toggleAudio,
		toggleVideo,
		hangUp,
		resetForNewSession,
		clearRemoteStream,
		localStream,
		remoteStream,
		isAudioEnabled,
		isVideoEnabled,
	}
}
