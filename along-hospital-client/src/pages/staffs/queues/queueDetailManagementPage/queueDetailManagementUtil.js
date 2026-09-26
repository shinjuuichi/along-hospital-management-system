import { ApiUrls } from '@/configs/apiUrls'
import { EnumConfig } from '@/configs/enumConfig'

export const getNextQueueStatuses = (queueStatus) => {
	const NEXT_QUEUE_STATUS_MAP = {
		[EnumConfig.QueueStatus.Waiting]: [
			EnumConfig.QueueStatus.Called,
			EnumConfig.QueueStatus.Cancelled,
		],
		[EnumConfig.QueueStatus.Called]: [
			EnumConfig.QueueStatus.InProgress,
			EnumConfig.QueueStatus.Waiting,
			EnumConfig.QueueStatus.Cancelled,
		],
		[EnumConfig.QueueStatus.InProgress]: [
			EnumConfig.QueueStatus.AwaitingResults,
			EnumConfig.QueueStatus.Completed,
			EnumConfig.QueueStatus.Cancelled,
		],
	}

	return NEXT_QUEUE_STATUS_MAP[queueStatus] || []
}

export const getQueueStatusActionLabel = (fromStatus, toStatus, t) => {
	const ACTION_LABEL_MAP = {
		[EnumConfig.QueueStatus.Waiting]: {
			[EnumConfig.QueueStatus.Called]: t('queue.button.call'),
			[EnumConfig.QueueStatus.Cancelled]: t('button.cancel'),
		},
		[EnumConfig.QueueStatus.Called]: {
			[EnumConfig.QueueStatus.InProgress]: t('queue.button.start'),
			[EnumConfig.QueueStatus.Waiting]: t('queue.button.uncall'),
			[EnumConfig.QueueStatus.Cancelled]: t('button.cancel'),
		},
		[EnumConfig.QueueStatus.InProgress]: {
			[EnumConfig.QueueStatus.AwaitingResults]: t('queue.button.await_results'),
			[EnumConfig.QueueStatus.Completed]: t('button.complete'),
			[EnumConfig.QueueStatus.Cancelled]: t('button.cancel'),
		},
	}
	return ACTION_LABEL_MAP[fromStatus]?.[toStatus] || toStatus
}

export const getQueueStatusConfirmConfig = (fromStatus, toStatus, t) => {
	switch (toStatus) {
		case EnumConfig.QueueStatus.Called:
			return {
				title: t('queue.dialog.call_title'),
				description: t('queue.dialog.call_description'),
				confirmText: t('queue.button.call'),
			}
		case EnumConfig.QueueStatus.Waiting:
			return {
				title: t('queue.dialog.uncall_title'),
				description: t('queue.dialog.uncall_description'),
				confirmText: t('queue.button.uncall'),
			}
		case EnumConfig.QueueStatus.InProgress:
			return {
				title: t('queue.dialog.start_title'),
				description: t('queue.dialog.start_description'),
				confirmText: t('queue.button.start'),
			}
		case EnumConfig.QueueStatus.AwaitingResults:
			return {
				title: t('queue.dialog.await_results_title'),
				description: t('queue.dialog.await_results_description'),
				confirmText: t('queue.button.await_results'),
			}
		case EnumConfig.QueueStatus.Completed:
			return {
				title: t('queue.dialog.complete_title'),
				description: t('queue.dialog.complete_description'),
				confirmText: t('button.complete'),
			}
		case EnumConfig.QueueStatus.Cancelled:
			return {
				title: t('queue.dialog.cancel_title'),
				description: t('queue.dialog.cancel_description'),
				confirmText: t('button.cancel'),
			}
		default:
			return {
				title: t('queue.dialog.change_status_title'),
				description: t('queue.dialog.change_status_description'),
				confirmText: getQueueStatusActionLabel(fromStatus, toStatus, t),
			}
	}
}

export const getQueueUpdateStatusUrlBasedOnStatus = (queueId, status) => {
	switch (status) {
		case EnumConfig.QueueStatus.Called:
			return ApiUrls.QUEUE.MANAGEMENT.CALL(queueId)
		case EnumConfig.QueueStatus.Waiting:
			return ApiUrls.QUEUE.MANAGEMENT.UNCALL(queueId)
		case EnumConfig.QueueStatus.InProgress:
			return ApiUrls.QUEUE.MANAGEMENT.START(queueId)
		case EnumConfig.QueueStatus.AwaitingResults:
			return ApiUrls.QUEUE.MANAGEMENT.AWAIT_RESULTS(queueId)
		case EnumConfig.QueueStatus.Completed:
			return ApiUrls.QUEUE.MANAGEMENT.COMPLETE(queueId)
		case EnumConfig.QueueStatus.Cancelled:
			return ApiUrls.QUEUE.MANAGEMENT.CANCEL(queueId)
		default:
			return null
	}
}

export const canTransitQueueStatus = (currentStatus, nextStatus) => {
	return getNextQueueStatuses(currentStatus).includes(nextStatus)
}
