import DetailCard from '@/components/generals/DetailCard'
import { GenericTablePagination } from '@/components/generals/GenericPagination'
import EmptyPage from '@/components/placeholders/EmptyPage'
import GenericTable from '@/components/tables/GenericTable'
import { ApiUrls } from '@/configs/apiUrls'
import { defaultBedStatusStyle, defaultRoomStatusStyle } from '@/configs/defaultStylesConfig'
import useEnum from '@/hooks/useEnum'
import useFetch from '@/hooks/useFetch'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import { setBedCategoriesStore } from '@/redux/reducers/managementReducer'
import { getEnumLabelByValue, renderEmptyFallback } from '@/utils/handleStringUtil'
import { Chip, Paper, Stack, Typography } from '@mui/material'
import { useEffect, useMemo, useState } from 'react'
import { useParams } from 'react-router-dom'

const RoomDetailPage = () => {
	const { t } = useTranslation()
	const _enum = useEnum()
	const { id } = useParams()
	const [room, setRoom] = useState(null)
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)
	const [sort, setSort] = useState({ key: 'id', direction: 'asc' })

	const { data: bedCategories } = useReduxStore({
		selector: (state) => state.management.bedCategories,
		setStore: setBedCategoriesStore,
	})

	const getRoomDetail = useFetch(ApiUrls.ROOM.MANAGEMENT.DETAIL(id))
	const getBeds = useFetch(
		ApiUrls.BED.MANAGEMENT.INDEX,
		{ roomId: id, page, pageSize, sort: `${sort.key} ${sort.direction}` },
		[id, page, pageSize, sort]
	)

	useEffect(() => {
		if (getRoomDetail.data) {
			setRoom(getRoomDetail.data)
		}
	}, [getRoomDetail.data])

	const getBedCategoryName = (bedCategoryId) => {
		const category = bedCategories?.find((cat) => cat.id === bedCategoryId)
		return renderEmptyFallback(category?.name)
	}

	const bedFields = useMemo(
		() => [
			{ key: 'id', title: 'ID', width: 20, sortable: true },
			{ key: 'code', title: t('bed.field.code'), width: 30, sortable: true },
			{
				key: 'status',
				title: t('bed.field.status'),
				width: 30,
				sortable: true,
				render: (value) => (
					<Chip
						label={getEnumLabelByValue(_enum.bedStatusOptions, value)}
						color={defaultBedStatusStyle(value)}
						size='small'
					/>
				),
			},
			{
				key: 'bedCategoryId',
				title: t('bed.field.bed_category'),
				width: 20,
				sortable: false,
				render: (value) => getBedCategoryName(value),
			},
		],
		[t, bedCategories]
	)

	const roomDetailFields = [
		{ label: t('room.field.room_code'), value: room?.code },
		{ label: t('room.field.building'), value: room?.buildingName },
		{ label: t('room.field.floor'), value: room?.floorNumber },
		{ label: t('room.field.room_category'), value: room?.roomCategoryName },
		{
			label: t('room.field.status'),
			value: room?.status,
			render: (value) => (
				<Chip
					label={getEnumLabelByValue(_enum.roomStatusOptions, value)}
					color={defaultRoomStatusStyle(value)}
					sx={{ width: 'fit-content' }}
				/>
			),
		},
		{ label: t('room.field.specialty'), value: room?.specialtyName },
		{ label: t('room.field.bed_count'), value: room?.bedCount },
		{
			label: t('room_category.field.roles'),
			value: room?.roles,
			render: (roles) => (
				<Stack direction='row' spacing={0.5} flexWrap='wrap' useFlexGap>
					{roles.map((role) => (
						<Chip
							key={role}
							label={getEnumLabelByValue(_enum.roleOptions, role)}
							size='small'
							color='primary'
							variant='outlined'
						/>
					))}
				</Stack>
			),
		},
	]

	if (!room) {
		return <EmptyPage title={t('text.placeholder.no_data')} />
	}

	return (
		<Stack spacing={3} p={2}>
			<Typography variant='h5'>{t('room.title.detail')}</Typography>

			<DetailCard fields={roomDetailFields} />

			<Paper sx={{ p: 2 }}>
				<Typography variant='h6' gutterBottom>
					{t('room.title.beds_list')}
				</Typography>
				<GenericTable
					data={getBeds.data?.collection || []}
					fields={bedFields}
					rowKey='id'
					sort={sort}
					setSort={setSort}
					loading={getBeds.loading}
				/>
				<Stack justifyContent='center' px={2} py={2}>
					<GenericTablePagination
						totalPage={getBeds.data?.totalPage}
						page={page}
						setPage={setPage}
						pageSize={pageSize}
						setPageSize={setPageSize}
						pageSizeOptions={[5, 10, 20]}
						loading={getBeds.loading}
					/>
				</Stack>
			</Paper>
		</Stack>
	)
}

export default RoomDetailPage
