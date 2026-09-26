/* eslint-disable react-hooks/exhaustive-deps */
import * as signalR from '@microsoft/signalr'
import { useLocalStorage } from '@/hooks/useStorage'
import { useCallback, useEffect, useMemo, useRef, useState } from 'react'

const resolveParticipant = (payload) => {
	if (typeof payload === 'string') {
		return { connectionId: payload, role: null }
	}

	if (!payload || typeof payload !== 'object') {
		return { connectionId: null, role: null }
	}

	return {
		connectionId: payload.connectionId || payload.ConnectionId || null,
		role: payload.role || payload.Role || null,
	}
}

const useMeetingSignalR = ({
	transactionId,
	roomCode,
	hubUrl,
	onJoinSucceeded,
	onJoinFailed,
	onParticipantJoined,
	onParticipantLeft,
	onOffer,
	onAnswer,
	onIceCandidate,
	onStateUpdated,
	onSessionExpiring,
	onSessionExpired,
	onMessage,
}) => {
	const connectionRef = useRef(null)
	const startedRef = useRef(false)
	const intentionalStopRef = useRef(false)
	const callbacksRef = useRef({})
	const [accessToken] = useLocalStorage('accessToken', '')

	useEffect(() => {
		callbacksRef.current = {
			onJoinSucceeded,
			onJoinFailed,
			onParticipantJoined,
			onParticipantLeft,
			onOffer,
			onAnswer,
			onIceCandidate,
			onStateUpdated,
			onSessionExpiring,
			onSessionExpired,
			onMessage,
		}
	}, [
		onJoinSucceeded,
		onJoinFailed,
		onParticipantJoined,
		onParticipantLeft,
		onOffer,
		onAnswer,
		onIceCandidate,
		onStateUpdated,
		onSessionExpiring,
		onSessionExpired,
		onMessage,
	])

	const resolvedHubUrl = useMemo(() => hubUrl, [hubUrl])
	const [joinedRoomCode, setJoinedRoomCode] = useState(null)

	const joinSession = useCallback(
		async (conn) => {
			await conn.invoke('JoinSession', transactionId || null, roomCode || null)
		},
		[transactionId, roomCode]
	)

	const buildConnection = useCallback(() => {
		const conn = new signalR.HubConnectionBuilder()
			.withUrl(resolvedHubUrl, {
				accessTokenFactory: () => accessToken || '',
			})
			.withAutomaticReconnect()
			.configureLogging(signalR.LogLevel.Information)
			.build()

		conn.onreconnected(async () => {
			try {
				await joinSession(conn)
			} catch (error) {
				callbacksRef.current.onJoinFailed?.({
					message: 'MEETING_REJOIN_FAILED',
					detail: error?.message,
				})
				await conn.stop()
			}
		})

		conn.onclose(() => {
			setJoinedRoomCode(null)
			if (!intentionalStopRef.current) {
				startedRef.current = false
			}
		})

		conn.on('JoinSucceeded', (payload) => {
			const room = payload.roomCode ?? payload.RoomCode
			setJoinedRoomCode(room)

			const existingParticipants = payload.existingParticipants || payload.ExistingParticipants || []

			if (existingParticipants.length > 0) {
				existingParticipants.forEach((participant) => {
					callbacksRef.current.onParticipantJoined?.(resolveParticipant(participant))
				})
			}

			console.info('[MeetingSignalR][JoinSucceeded]', {
				room,
				connectionId: payload.connectionId ?? payload.ConnectionId,
				role: payload.role ?? payload.Role,
				shouldCreateOffer: payload.shouldCreateOffer ?? payload.ShouldCreateOffer,
				existingParticipants,
			})

			callbacksRef.current.onJoinSucceeded?.(payload)
		})

		conn.on('JoinFailed', (err) => {
			callbacksRef.current.onJoinFailed?.(err)
		})

		conn.on('LeaveSucceeded', () => {
			setJoinedRoomCode(null)
		})

		conn.on('ParticipantJoined', (participant) => {
			const resolvedParticipant = resolveParticipant(participant)
			console.info('[MeetingSignalR][ParticipantJoined]', resolvedParticipant)
			callbacksRef.current.onParticipantJoined?.(resolvedParticipant)
		})

		conn.on('ParticipantLeft', (connId) => {
			const id = typeof connId === 'string' ? connId : connId?.connectionId || connId?.ConnectionId
			console.info('[MeetingSignalR][ParticipantLeft]', { connectionId: id })
			callbacksRef.current.onParticipantLeft?.(id)
		})

		conn.on('SessionExpiring', (payload) => {
			callbacksRef.current.onSessionExpiring?.(payload)
		})

		conn.on('SessionExpired', (payload) => {
			callbacksRef.current.onSessionExpired?.(payload)
		})
		conn.on('ReceiveOffer', ({ from, offer }) => {
			console.info('[MeetingSignalR][ReceiveOffer]', { from, type: offer?.type })
			callbacksRef.current.onOffer?.(from, offer)
		})

		conn.on('ReceiveAnswer', ({ from, answer }) => {
			console.info('[MeetingSignalR][ReceiveAnswer]', { from, type: answer?.type })
			callbacksRef.current.onAnswer?.(from, answer)
		})

		conn.on('ReceiveIceCandidate', ({ from, candidate }) => {
			callbacksRef.current.onIceCandidate?.(from, candidate)
		})

		conn.on('StateUpdated', ({ from, state }) => {
			console.info('[MeetingSignalR][StateUpdated]', { from, state })
			callbacksRef.current.onStateUpdated?.(from, state)
		})

		conn.on('ReceiveMessage', ({ from, message }) => {
			callbacksRef.current.onMessage?.(from, message)
		})
		return conn
	}, [accessToken, joinSession, resolvedHubUrl])

	const startConnection = useCallback(async () => {
		if (startedRef.current) return
		if (!resolvedHubUrl) return

		startedRef.current = true
		intentionalStopRef.current = false

		if (!connectionRef.current) {
			connectionRef.current = buildConnection()
		}

		if (connectionRef.current.state === signalR.HubConnectionState.Disconnected) {
			try {
				await connectionRef.current.start()
				await joinSession(connectionRef.current)
			} catch (error) {
				startedRef.current = false
				callbacksRef.current.onJoinFailed?.({
					message: 'MEETING_CONNECTION_FAILED',
					detail: error?.message,
				})
				throw error
			}
		}
	}, [buildConnection, joinSession, resolvedHubUrl])

	const stopConnection = useCallback(async () => {
		const conn = connectionRef.current
		intentionalStopRef.current = true
		startedRef.current = false

		if (conn && conn.state !== signalR.HubConnectionState.Disconnected) {
			await conn.stop()
		}

		connectionRef.current = null
		setJoinedRoomCode(null)
	}, [])

	useEffect(() => {
		if (!hubUrl) return

		startConnection().catch(() => undefined)

		return () => {
			stopConnection()
		}
	}, [hubUrl])

	const sendOffer = useCallback(
		async (offer) => {
			if (!connectionRef.current || !joinedRoomCode) return
			await connectionRef.current.invoke('SendOffer', joinedRoomCode, offer)
		},
		[joinedRoomCode]
	)

	const sendAnswer = useCallback(
		async (answer) => {
			if (!connectionRef.current || !joinedRoomCode) return
			await connectionRef.current.invoke('SendAnswer', joinedRoomCode, answer)
		},
		[joinedRoomCode]
	)

	const sendIceCandidate = useCallback(
		async (candidate) => {
			if (!connectionRef.current || !joinedRoomCode) return
			await connectionRef.current.invoke('SendIceCandidate', joinedRoomCode, candidate)
		},
		[joinedRoomCode]
	)

	const notifyState = useCallback(
		async (state) => {
			if (!connectionRef.current || !joinedRoomCode) return
			await connectionRef.current.invoke('NotifyState', joinedRoomCode, state)
		},
		[joinedRoomCode]
	)

	const sendMessage = useCallback(
		async (message) => {
			if (!connectionRef.current || !joinedRoomCode) return
			await connectionRef.current.invoke('SendMessage', joinedRoomCode, message)
		},
		[joinedRoomCode]
	)

	const leaveSession = useCallback(async () => {
		if (!connectionRef.current) return
		try {
			await connectionRef.current.invoke('LeaveSession')
			await new Promise((resolve) => setTimeout(resolve, 100))
		} finally {
			await stopConnection()
		}
	}, [stopConnection])

	return {
		sendOffer,
		sendAnswer,
		sendIceCandidate,
		notifyState,
		sendMessage,
		leaveSession,
		startConnection,
		stopConnection,
		joinedRoomCode,
	}
}

export default useMeetingSignalR
