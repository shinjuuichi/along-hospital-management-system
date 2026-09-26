import { GenericPagination } from '@/components/generals/GenericPagination'
import SearchBar from '@/components/generals/SearchBar'
import useTranslation from '@/hooks/useTranslation'
import { Box, Paper, Stack, Typography } from '@mui/material'

const QueueManagementToolbarSection = ({
	searchTerm,
	setSearchTerm,
	page,
	setPage,
	totalPage,
	totalRooms,
}) => {
	const { t } = useTranslation()

	return (
		<Paper sx={{ p: 2.5, borderRadius: 3 }}>
			<Stack spacing={2}>
				<Stack
					direction={{ xs: 'column', md: 'row' }}
					justifyContent='space-between'
					alignItems={{ xs: 'flex-start', md: 'center' }}
					spacing={1}
				>
					<Box>
						<Typography variant='h5' fontWeight={700}>
							{t('queue.title.queue_management')}
						</Typography>
						<Typography variant='body2' color='text.secondary'>
							{t('queue.text.rooms_found')}: {totalRooms}
						</Typography>
					</Box>

					<Box sx={{ width: { xs: '100%', md: 360 } }}>
						<SearchBar
							widthPercent={0}
							value={searchTerm}
							setValue={setSearchTerm}
							placeholder={t('queue.placeholder.search_room_code')}
						/>
					</Box>
				</Stack>

				{totalRooms > 0 && (
					<Stack
						direction={{ xs: 'column', md: 'row' }}
						justifyContent='space-between'
						alignItems={{ xs: 'flex-start', md: 'center' }}
						spacing={1}
					>
						<Typography variant='body2' color='text.secondary'>
							{t('queue.text.page')}: {page}/{Math.max(totalPage, 1)}
						</Typography>

						<GenericPagination
							totalPage={totalPage}
							page={page}
							setPage={setPage}
							siblingCount={0}
							boundaryCount={1}
						/>
					</Stack>
				)}
			</Stack>
		</Paper>
	)
}

export default QueueManagementToolbarSection
