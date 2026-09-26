import GenericTabs from '@/components/generals/GenericTabs'
import { ApiUrls } from '@/configs/apiUrls'
import { defaultQueueStatusStyle } from '@/configs/defaultStylesConfig'
import { EnumConfig } from '@/configs/enumConfig'
import useAuth from '@/hooks/useAuth'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useConfirm from '@/hooks/useConfirm'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import { getEnv } from '@/utils/commons'
import * as signalR from '@microsoft/signalr'
import { Grid, Paper, Stack, Typography, alpha, useTheme } from '@mui/material'
import { useEffect, useMemo, useRef, useState } from 'react'
import { useParams } from 'react-router-dom'
import { toast } from 'react-toastify'
import { getVisibleQueues } from '../queueManagementUtil'
import {
	canTransitQueueStatus,
	getNextQueueStatuses,
	getQueueStatusConfirmConfig,
	getQueueUpdateStatusUrlBasedOnStatus,
} from './queueDetailManagementUtil'
import QueueDetailManagementFooterSection from './sections/QueueDetailManagementFooterSection'
import QueueDetailManagementLoadingSection from './sections/QueueDetailManagementLoadingSection'
import QueueDetailManagementMainActionSection from './sections/QueueDetailManagementMainActionSection'
import QueueDetailManagementMainHeaderSection from './sections/QueueDetailManagementMainHeaderSection'
import QueueDetailManagementMainInfoSection from './sections/QueueDetailManagementMainInfoSection'
import QueueDetailManagementMedicalHistorySelectionDialog from './sections/QueueDetailManagementMedicalHistorySelectionDialog'
import QueueDetailManagementQueueListSection from './sections/QueueDetailManagementQueueListSection'

const RECEIVE_QUEUE_SNAPSHOT_EVENT = 'ReceiveQueueSnapshot'
const QUEUE_SOURCE_FILTER = {
	Online: 'online',
	Offline: 'offline',
}

const QueueDetailManagementPage = () => {
	const theme = useTheme()
	const { t } = useTranslation()
	const confirm = useConfirm()
	const { auth } = useAuth()
	const { roomId } = useParams()

	const [openAssignMedicalHistoryDialog, setOpenAssignMedicalHistoryDialog] = useState(false)
	const [snapshot, setSnapshot] = useState(null)
	const [isQueueSnapshotLoading, setIsQueueSnapshotLoading] = useState(true)
	const [queueSourceFilter, setQueueSourceFilter] = useState(QUEUE_SOURCE_FILTER.Offline)

	const connectionRef = useRef(null)

	const isReceptionistRole = auth?.role === EnumConfig.Role.Receptionist || false
	const isReceptionRoom = roomId == null || String(roomId).toLowerCase() === 'null'
	const queueSourceTabs = [
		{ key: QUEUE_SOURCE_FILTER.Offline, title: t('queue.button.offline_queue') },
		{ key: QUEUE_SOURCE_FILTER.Online, title: t('queue.button.online_queue') },
	]

	const getAllPendingMedicalHistoriesAPI = useFetch(
		ApiUrls.MEDICAL_HISTORY.MANAGEMENT.GET_ALL_PENDING,
		{},
		[],
		isReceptionistRole
	)

	const assignMedicalHistoryAPI = useAxiosSubmit({
		url: ApiUrls.QUEUE.MANAGEMENT.ASSIGN_MEDICAL_HISTORY(),
		method: 'PUT',
	})
	const updateStatusAPI = useAxiosSubmit({
		method: 'PUT',
	})

	useEffect(() => {
		setSnapshot(null)
		setIsQueueSnapshotLoading(true)
		let isCancelled = false

		const queueHubUrl = getEnv('VITE_BASE_API_URL') + ApiUrls.HUB.QUEUE(roomId)
		const connection = new signalR.HubConnectionBuilder()
			.withUrl(queueHubUrl)
			.withAutomaticReconnect()
			.build()

		const handleReceiveQueueSnapshot = (nextSnapshot) => {
			if (isCancelled) return

			setIsQueueSnapshotLoading(false)
			setSnapshot(nextSnapshot || null)

			const message = nextSnapshot?.message || nextSnapshot?.Message
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
					setIsQueueSnapshotLoading(false)
					setSnapshot(null)
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
	}, [roomId])

	const visibleQueues = useMemo(() => {
		const sourceQueues = Array.isArray(snapshot?.queues) ? snapshot.queues : []
		const sourceQueuesByType = isReceptionRoom
			? sourceQueues.filter((queue) => {
					const isOnlineQueue = Number(queue?.appointmentId) > 0
					return queueSourceFilter === QUEUE_SOURCE_FILTER.Online ? isOnlineQueue : !isOnlineQueue
				})
			: sourceQueues

		if (isReceptionRoom) {
			return getVisibleQueues(sourceQueuesByType)
		}

		return getVisibleQueues(sourceQueues)
	}, [isReceptionRoom, queueSourceFilter, snapshot])

	const currentQueue = visibleQueues[0] || null

	const transitionStatuses = useMemo(
		() =>
			getNextQueueStatuses(currentQueue?.queueStatus).filter(
				(status) =>
					status !== EnumConfig.QueueStatus.Cancelled && status !== EnumConfig.QueueStatus.Completed
			),
		[currentQueue]
	)

	const summary = snapshot?.summary || {}

	const canComplete = canTransitQueueStatus(
		currentQueue?.queueStatus,
		EnumConfig.QueueStatus.Completed
	)
	const canCancel = canTransitQueueStatus(
		currentQueue?.queueStatus,
		EnumConfig.QueueStatus.Cancelled
	)

	const handleChangeQueueStatus = async (queueId, newStatus) => {
		const confirmContent = getQueueStatusConfirmConfig(currentQueue?.queueStatus, newStatus, t)

		const isConfirmed = await confirm({
			title: confirmContent.title,
			description: confirmContent.description,
			confirmText: confirmContent.confirmText,
			confirmColor: defaultQueueStatusStyle(newStatus),
		})

		if (!isConfirmed) {
			return
		}

		let overrideUrl = getQueueUpdateStatusUrlBasedOnStatus(queueId, newStatus)
		if (!overrideUrl) {
			return
		}

		await updateStatusAPI.submit({ overrideUrl })
	}

	const handleAssignMedicalHistory = async (queueId, medicalHistoryId) => {
		await assignMedicalHistoryAPI.submit({
			overrideUrl: ApiUrls.QUEUE.MANAGEMENT.ASSIGN_MEDICAL_HISTORY(queueId),
			overrideData: { medicalHistoryId },
		})
	}

	if (isQueueSnapshotLoading) {
		return <QueueDetailManagementLoadingSection isReceptionRoom={isReceptionRoom} />
	}

	if (!snapshot) {
		return (
			<Paper sx={{ p: 4, borderRadius: 3, textAlign: 'center' }}>
				<Typography variant='h6'>{t('queue.placeholder.queue_room_not_found')}</Typography>
			</Paper>
		)
	}

	return (
		<>
			<Stack spacing={1.1}>
				{isReceptionRoom && (
					<GenericTabs
						tabs={queueSourceTabs}
						currentTab={queueSourceFilter}
						setCurrentTab={(tab) => setQueueSourceFilter(tab.key)}
					/>
				)}

				<Grid container spacing={1.1} sx={{ minHeight: { md: 600 } }}>
					<Grid size={{ xs: 12, md: 3 }} sx={{ display: 'flex' }}>
						<QueueDetailManagementQueueListSection
							queues={visibleQueues}
							currentQueueId={currentQueue?.id}
						/>
					</Grid>

					<Grid size={{ xs: 12, md: 9 }} sx={{ display: 'flex' }}>
						<Paper
							elevation={0}
							sx={{
								height: '100%',
								width: '100%',
								border: `1px solid ${alpha(theme.palette.primary.main, 0.2)}`,
								p: 1,
								backgroundColor: alpha(theme.palette.primary.main, 0.02),
								display: 'flex',
								flexDirection: 'column',
							}}
						>
							{currentQueue ? (
								<Stack spacing={1.5}>
									<QueueDetailManagementMainHeaderSection
										currentQueue={currentQueue}
										transitionStatuses={transitionStatuses}
										onChangeQueueStatus={handleChangeQueueStatus}
									/>
									<QueueDetailManagementMainInfoSection currentQueue={currentQueue} />
									<QueueDetailManagementMainActionSection
										currentQueue={currentQueue}
										canComplete={canComplete}
										canCancel={canCancel}
										onChangeQueueStatus={handleChangeQueueStatus}
										onOpenAssignMedicalHistory={() => setOpenAssignMedicalHistoryDialog(true)}
									/>
								</Stack>
							) : (
								<Stack sx={{ height: '100%', alignItems: 'center', justifyContent: 'center' }}>
									<Typography variant='h6' color='text.secondary'>
										{t('queue.placeholder.no_queue_in_room')}
									</Typography>
								</Stack>
							)}
						</Paper>
					</Grid>
				</Grid>

				<QueueDetailManagementFooterSection summary={summary} />
			</Stack>

			<QueueDetailManagementMedicalHistorySelectionDialog
				open={openAssignMedicalHistoryDialog}
				onClose={() => setOpenAssignMedicalHistoryDialog(false)}
				medicalHistories={getAllPendingMedicalHistoriesAPI.data || []}
				onSubmit={async (medicalHistoryId) => {
					if (!currentQueue?.id) return
					await handleAssignMedicalHistory(currentQueue.id, medicalHistoryId)
				}}
			/>
		</>
	)
}

export default QueueDetailManagementPage
