import { Box, Card, CardActions, CardContent, Divider, Skeleton, Stack } from '@mui/material'

const SkeletonMedicineCard = () => {
	return (
		<Card
			component='div'
			sx={{
				height: '100%',
				display: 'flex',
				flexDirection: 'column',
				width: '100%',
			}}
		>
			<Skeleton variant='rectangular' sx={{ height: 200, flexShrink: 0, minWidth: '100%' }} />

			<CardContent
				sx={{
					flex: 1,
					display: 'flex',
					flexDirection: 'column',
					alignItems: 'stretch',
					p: 2,
					'&:last-child': { pb: 2 },
				}}
			>
				<Skeleton variant='text' width='100%' height={32} />
				<Stack direction='row' spacing={2}>
					<Skeleton variant='text' width='30%' height={20} />
					<Skeleton variant='text' width='30%' height={20} />
				</Stack>
				<Skeleton variant='text' width='100%' height={18} />
				<Skeleton variant='text' width='70%' height={18} />
				<Box sx={{ minHeight: 36 }}>
					<Skeleton variant='text' width='80%' height={18} />
				</Box>
				<Skeleton variant='text' width='45%' height={24} />
				<Divider />
				<Stack direction='row' spacing={1}>
					<Skeleton variant='rectangular' width={64} height={28} sx={{ borderRadius: 1 }} />
					<Skeleton variant='rectangular' width={64} height={28} sx={{ borderRadius: 1 }} />
					<Skeleton variant='rectangular' width={64} height={28} sx={{ borderRadius: 1 }} />
				</Stack>
			</CardContent>

			<CardActions sx={{ px: 2, pb: 2, mt: 'auto', flexShrink: 0 }}>
				<Skeleton variant='rectangular' width='100%' height={36} sx={{ borderRadius: 1 }} />
			</CardActions>
		</Card>
	)
}

export default SkeletonMedicineCard