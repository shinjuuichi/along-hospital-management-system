import { Box, Card, CardActions, CardContent, Skeleton, Stack } from '@mui/material'

const SkeletonVoucher = () => {
	return (
		<Card
			sx={{
				height: '100%',
				display: 'flex',
				flexDirection: 'row',
			}}
		>
			<Skeleton variant='rectangular' width={120} sx={{ minWidth: 120, flexShrink: 0 }} />

			<Box sx={{ display: 'flex', flexDirection: 'column', flex: 1 }}>
				<CardContent sx={{ flex: 1, pb: 1 }}>
					<Stack direction='row' spacing={1} mb={1.5}>
						<Skeleton variant='rounded' width={100} height={20} />
					</Stack>

					<Skeleton variant='text' sx={{ fontSize: '1rem', mb: 1, width: '80%' }} />
					<Skeleton variant='text' sx={{ fontSize: '0.875rem', mb: 0.5, width: '100%' }} />
					<Skeleton variant='text' sx={{ fontSize: '0.875rem', mb: 1.5, width: '60%' }} />

					<Skeleton variant='rounded' width={80} height={28} sx={{ mb: 1.5 }} />

					<Stack spacing={0.5}>
						<Skeleton variant='text' width='70%' height={16} />
						<Skeleton variant='text' width='60%' height={16} />
						<Skeleton variant='text' width='80%' height={16} />
					</Stack>
				</CardContent>

				<CardActions sx={{ p: 1.5, pt: 0 }}>
					<Skeleton variant='rounded' width={100} height={32} />
				</CardActions>
			</Box>
		</Card>
	)
}

export default SkeletonVoucher
