import { EnumConfig } from '@/configs/enumConfig'

export const getVisibleQueues = (sourceQueues) => {
	const QUEUE_BOARD_STATUSES = [
		EnumConfig.QueueStatus.Waiting,
		EnumConfig.QueueStatus.Called,
		EnumConfig.QueueStatus.InProgress,
	]

	const visibleQueues = sourceQueues.filter((queue) =>
		QUEUE_BOARD_STATUSES.includes(queue.queueStatus)
	)
	const inProgressQueues = visibleQueues.filter(
		(queue) => queue.queueStatus === EnumConfig.QueueStatus.InProgress
	)
	const nonInProgressQueues = visibleQueues.filter(
		(queue) => queue.queueStatus !== EnumConfig.QueueStatus.InProgress
	)

	return [...inProgressQueues, ...nonInProgressQueues]
}

export const formatQueueNumber = (queueNumber) => {
	const numericValue = Number(queueNumber)
	if (Number.isFinite(numericValue)) return String(numericValue).padStart(4, '0')
	return '----'
}

export const getRoomDisplayCode = (snapshot, t) => {
	return snapshot?.room?.code || t('queue.text.reception_room')
}

export const getRoomCardState = (snapshot) => {
	if (snapshot?.roomId == null) return 'reception'
	if (snapshot?.doctorId == null) return 'non_doctor_room'
	return 'doctor_assigned'
}
