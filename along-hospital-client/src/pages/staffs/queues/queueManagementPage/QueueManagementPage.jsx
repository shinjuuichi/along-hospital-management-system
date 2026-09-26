import { ApiUrls } from '@/configs/apiUrls'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import { Stack } from '@mui/material'
import { useEffect, useMemo, useState } from 'react'
import { getRoomDisplayCode } from '../queueManagementUtil'
import QueueManagementLoadingSection from './sections/QueueManagementLoadingSection'
import QueueManagementRoomGridSection from './sections/QueueManagementRoomGridSection'
import QueueManagementToolbarSection from './sections/QueueManagementToolbarSection'

const ROOMS_PER_PAGE = 8

const QueueManagementPage = () => {
	const { t } = useTranslation()

	const [searchTerm, setSearchTerm] = useState('')
	const [page, setPage] = useState(1)

	const getAllQueueSnapshots = useFetch(ApiUrls.QUEUE.GET_ALL)

	const filteredSnapshots = useMemo(() => {
		const snapshots = Array.isArray(getAllQueueSnapshots.data) ? getAllQueueSnapshots.data : []
		const normalizedSearchTerm = searchTerm.trim().toLowerCase()

		if (!normalizedSearchTerm) return snapshots || []

		return snapshots.filter((snapshot) =>
			getRoomDisplayCode(snapshot, t).toLowerCase().includes(normalizedSearchTerm)
		)
	}, [getAllQueueSnapshots.data, searchTerm, t])

	const totalPage = Math.max(1, Math.ceil(filteredSnapshots.length / ROOMS_PER_PAGE))

	useEffect(() => {
		setPage(1)
	}, [searchTerm])

	useEffect(() => {
		if (page > totalPage) setPage(totalPage)
	}, [page, totalPage])

	const roomSnapshotsInPage = useMemo(() => {
		const startIndex = (page - 1) * ROOMS_PER_PAGE
		return filteredSnapshots.slice(startIndex, startIndex + ROOMS_PER_PAGE)
	}, [filteredSnapshots, page])

	if (getAllQueueSnapshots.loading) {
		return <QueueManagementLoadingSection />
	}

	return (
		<Stack spacing={2}>
			<QueueManagementToolbarSection
				searchTerm={searchTerm}
				setSearchTerm={setSearchTerm}
				page={page}
				setPage={setPage}
				totalPage={totalPage}
				totalRooms={filteredSnapshots.length}
			/>

			<QueueManagementRoomGridSection roomSnapshots={roomSnapshotsInPage} />
		</Stack>
	)
}

export default QueueManagementPage
