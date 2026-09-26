import GenericTabs from '@/components/generals/GenericTabs'
import { ApiUrls } from '@/configs/apiUrls'
import { EnumConfig } from '@/configs/enumConfig'
import useEnum from '@/hooks/useEnum'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import { formatQueueNumber, getVisibleQueues } from '@/pages/staffs/queues/queueManagementUtil'
import { getEnv } from '@/utils/commons'
import * as signalR from '@microsoft/signalr'
import { Box, Grid, Stack } from '@mui/material'
import { useEffect, useMemo, useRef, useState } from 'react'
import { toast } from 'react-toastify'
import QueueRoomSelectionDrawer from '../commons/QueueRoomSelectionDrawer'
import ViewQueueHeaderSection from './sections/ViewQueueHeaderSection'
import ViewQueueNowServingSection from './sections/ViewQueueNowServingSection'
import ViewQueueUpNextSection from './sections/ViewQueueUpNextSection'

const RECEIVE_QUEUE_SNAPSHOT_EVENT = 'ReceiveQueueSnapshot'
const QUEUE_SOURCE_FILTER = {
	Online: 'online',
	Offline: 'offline',
}

const ViewQueuePage = () => {
	const { t } = useTranslation()
	const _enum = useEnum()
	const [selectedRoomId, setSelectedRoomId] = useState(null)
	const [selectedSnapshot, setSelectedSnapshot] = useState(null)
	const [queueSourceFilter, setQueueSourceFilter] = useState(QUEUE_SOURCE_FILTER.Offline)
	const connectionRef = useRef(null)
	const isReceptionRoom = selectedRoomId == null || String(selectedRoomId).toLowerCase() === 'null'
	const queueSourceTabs = [
		{ key: QUEUE_SOURCE_FILTER.Offline, title: t('queue.button.offline_queue') },
		{ key: QUEUE_SOURCE_FILTER.Online, title: t('queue.button.online_queue') },
	]

	const getAllQueueSnapshots = useFetch(ApiUrls.QUEUE.GET_ALL)

	const roomOptions = useMemo(() => {
		const snapshots = Array.isArray(getAllQueueSnapshots.data) ? getAllQueueSnapshots.data : []

		return snapshots
			.filter((item) => Number(item?.roomId) > 0)
			.map((item) => ({
				id: Number(item.roomId),
				code: item?.room?.code || '',
			}))
	}, [getAllQueueSnapshots.data])

	useEffect(() => {
		setSelectedSnapshot(null)
		let isCancelled = false

		const queueHubUrl = getEnv('VITE_BASE_API_URL') + ApiUrls.HUB.QUEUE(selectedRoomId)
		const connection = new signalR.HubConnectionBuilder()
			.withUrl(queueHubUrl)
			.withAutomaticReconnect()
			.build()

		const handleReceiveQueueSnapshot = (snapshot) => {
			if (isCancelled) return

			setSelectedSnapshot(snapshot || null)

			const message = snapshot?.message || snapshot?.Message
			if (message) {
				toast.info(message)
			}
		}

		const createConnection = async () => {
			try {
				await connection.start()

				if (isCancelled) {
					if (connection.state !== signalR.HubConnectionState.Disconnected) {
						connection.stop()
					}
					return
				}

				connectionRef.current = connection
				connection.on(RECEIVE_QUEUE_SNAPSHOT_EVENT, handleReceiveQueueSnapshot)
			} catch {
				if (!isCancelled) {
					setSelectedSnapshot(null)
				}
			}
		}

		createConnection()

		return () => {
			isCancelled = true
			connection.off(RECEIVE_QUEUE_SNAPSHOT_EVENT, handleReceiveQueueSnapshot)

			if (connectionRef.current === connection) {
				connectionRef.current = null
			}

			if (connection.state !== signalR.HubConnectionState.Disconnected) {
				connection.stop()
			}
		}
	}, [selectedRoomId])

	const roomCode = selectedSnapshot?.room?.code || t('queue.text.reception_room')
	const roomSpecialtyName = selectedSnapshot?.room?.specialtyName
	const doctorName = selectedSnapshot?.doctor?.name

	const boardQueues = useMemo(() => {
		const sourceQueues = Array.isArray(selectedSnapshot?.queues) ? selectedSnapshot.queues : []
		const sourceQueuesByType = isReceptionRoom
			? sourceQueues.filter((queue) => {
					const isOnlineQueue = Number(queue?.appointmentId) > 0
					return queueSourceFilter === QUEUE_SOURCE_FILTER.Online ? isOnlineQueue : !isOnlineQueue
				})
			: sourceQueues

		return getVisibleQueues(sourceQueuesByType)
	}, [isReceptionRoom, queueSourceFilter, selectedSnapshot])

	const currentInProgressQueue = useMemo(
		() =>
			boardQueues.find((queue) => queue.queueStatus === EnumConfig.QueueStatus.InProgress) || null,
		[boardQueues]
	)

	const summaryItems = useMemo(
		() => [
			{
				key: 'total',
				label: t('text.total'),
				value: selectedSnapshot?.summary?.total ?? 0,
			},
			{
				key: 'waiting',
				label: t('enum.queue_status.waiting'),
				value: selectedSnapshot?.summary?.waiting ?? 0,
			},
			{
				key: 'called',
				label: t('enum.queue_status.called'),
				value: selectedSnapshot?.summary?.called ?? 0,
			},
			{
				key: 'inProgress',
				label: t('enum.queue_status.in_progress'),
				value: selectedSnapshot?.summary?.inProgress ?? 0,
			},
			{
				key: 'awaitingResults',
				label: t('enum.queue_status.awaiting_results'),
				value: selectedSnapshot?.summary?.awaitingResults ?? 0,
			},
			{
				key: 'completed',
				label: t('enum.queue_status.completed'),
				value: selectedSnapshot?.summary?.completed ?? 0,
			},
			{
				key: 'cancelled',
				label: t('enum.queue_status.cancelled'),
				value: selectedSnapshot?.summary?.cancelled ?? 0,
			},
		],
		[selectedSnapshot, t]
	)

	const currentQueueNumber = currentInProgressQueue
		? formatQueueNumber(currentInProgressQueue.queueNumber)
		: '--'
	const currentPatientName =
		currentInProgressQueue?.medicalHistorySnapshot?.patient?.name ||
		t('queue.guest.placeholder.patient_name_pending')

	return (
		<Box
			sx={{
				height: '100dvh',
				minHeight: '100vh',
				maxHeight: '100dvh',
				overflow: 'hidden',
				position: 'relative',
				p: { xs: 2, md: 5 },
				display: 'flex',
			}}
		>
			<QueueRoomSelectionDrawer
				roomOptions={roomOptions}
				selectedRoomId={selectedRoomId}
				onApplyRoom={setSelectedRoomId}
				currentRoomCode={roomCode}
			/>

			<Stack spacing={1.2} width={'100%'}>
				{isReceptionRoom && (
					<GenericTabs
						tabs={queueSourceTabs}
						currentTab={queueSourceFilter}
						setCurrentTab={(tab) => setQueueSourceFilter(tab.key)}
					/>
				)}

				<ViewQueueHeaderSection
					roomCode={roomCode}
					doctorName={doctorName}
					roomSpecialtyName={roomSpecialtyName}
				/>

				<Grid container spacing={1.2} height={'100%'}>
					<Grid size={{ xs: 12, md: 7 }}>
						<ViewQueueNowServingSection
							currentQueueNumber={currentQueueNumber}
							currentPatientName={currentPatientName}
							hasInProgress={!!currentInProgressQueue}
							summaryItems={summaryItems}
						/>
					</Grid>

					<Grid size={{ xs: 12, md: 5 }}>
						<ViewQueueUpNextSection queues={boardQueues} queueStatusOptions={_enum.queueStatusOptions} />
					</Grid>
				</Grid>
			</Stack>
		</Box>
	)
}

export default ViewQueuePage
