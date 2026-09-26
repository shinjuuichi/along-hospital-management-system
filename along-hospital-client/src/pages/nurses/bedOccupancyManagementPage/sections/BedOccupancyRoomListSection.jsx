import useTranslation from '@/hooks/useTranslation'
import { Chip, Paper, Stack, Typography } from '@mui/material'
import { alpha, useTheme } from '@mui/material/styles'

const BedOccupancyRoomListSection = ({ rooms, selectedRoomId, onSelectRoom, panelHeight }) => {
	const { t } = useTranslation()
	const theme = useTheme()

	return (
		<Paper
			sx={{
				p: 2.5,
				borderRadius: 3,
				height: panelHeight || '100%',
				minHeight: 0,
				overflow: 'hidden',
			}}
		>
			<Stack spacing={2} sx={{ height: '100%', minHeight: 0 }}>
				<Typography variant='subtitle1' fontWeight={600}>
					{t('bed_occupancy.title.room_list')}
				</Typography>
				<Stack
					spacing={1.25}
					sx={{
						flex: 1,
						minHeight: 0,
						overflowY: { xs: 'visible', lg: 'auto' },
						pr: { xs: 0, lg: 0.5 },
					}}
				>
					{rooms.length === 0 ? (
						<Typography color='text.secondary'>
							{t('bed_occupancy.placeholder.no_matching_rooms')}
						</Typography>
					) : (
						rooms.map((room) => {
							const selected = room.id === selectedRoomId
							const selectedBackground = alpha(
								theme.palette.primary.main,
								theme.palette.mode === 'dark' ? 0.22 : 0.12
							)
							const selectedHoverBackground = alpha(
								theme.palette.primary.main,
								theme.palette.mode === 'dark' ? 0.3 : 0.18
							)

							return (
								<Paper
									key={room.id}
									variant='outlined'
									onClick={() => onSelectRoom(room.id)}
									sx={{
										p: 2,
										cursor: 'pointer',
										borderRadius: 2,
										borderColor: selected ? 'primary.main' : 'divider',
										bgcolor: selected ? selectedBackground : 'background.paper',
										boxShadow: selected
											? `0 0 0 1px ${alpha(theme.palette.primary.main, 0.18)}`
											: 'none',
										transition: 'all 0.2s ease',
										'&:hover': {
											borderColor: 'primary.main',
											bgcolor: selected ? selectedHoverBackground : 'action.hover',
										},
									}}
								>
									<Stack spacing={1.25}>
										<Stack direction='row' justifyContent='space-between' spacing={1}>
											<Stack spacing={0.25}>
												<Typography
													variant='subtitle2'
													fontWeight={700}
													color={selected ? 'primary.main' : 'text.primary'}
												>
													{room.code}
												</Typography>
												<Typography
													variant='body2'
													color={selected ? 'text.primary' : 'text.secondary'}
												>
													{room.buildingName} · {t('room.field.floor')} {room.floorNumber}
												</Typography>
											</Stack>
											<Chip label={room.totalBeds} size='small' color='primary' />
										</Stack>
										<Typography
											variant='body2'
											color={selected ? 'text.primary' : 'text.secondary'}
										>
											{room.specialtyName || '-'}
										</Typography>
										<Stack direction='row' spacing={1} flexWrap='wrap' useFlexGap>
											<Chip
												size='small'
												label={`${t('bed_occupancy.summary.occupied')}: ${room.occupiedBeds}`}
												color='info'
												variant='outlined'
											/>
											<Chip
												size='small'
												label={`${t('bed_occupancy.summary.available')}: ${room.availableBeds}`}
												color='success'
												variant='outlined'
											/>
										</Stack>
									</Stack>
								</Paper>
							)
						})
					)}
				</Stack>
			</Stack>
		</Paper>
	)
}

export default BedOccupancyRoomListSection
