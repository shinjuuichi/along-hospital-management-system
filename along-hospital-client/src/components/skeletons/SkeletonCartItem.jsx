import { Box, Card, Skeleton, Stack } from '@mui/material'

const SkeletonCartItem = () => {
	return (
		<Card sx={{ display: 'flex', overflow: 'visible', width: '100%', minHeight: 180 }}>
			<Box
				sx={{
					width: 180,
					height: 180,
					flexShrink: 0,
					display: 'flex',
					alignItems: 'center',
					justifyContent: 'center',
				}}
			>
				<Skeleton variant='rectangular' width={180} height={180} />
			</Box>

			<Box sx={{ p: 2, flex: 1 }}>
				<Stack spacing={2}>
					<Stack direction='row' justifyContent='space-between' alignItems='flex-start'>
						<Box flex={1}>
							<Skeleton variant='text' width='80%' height={32} sx={{ mb: 1 }} />
							<Skeleton variant='text' width='60%' height={20} sx={{ mb: 0.5 }} />
							<Skeleton variant='text' width='50%' height={20} sx={{ mb: 1 }} />
							<Skeleton variant='text' width='40%' height={20} />
						</Box>
						<Skeleton variant='text' width={80} height={32} />
					</Stack>

					<Stack direction='row' alignItems='center' spacing={2}>
						<Skeleton variant='rectangular' width={120} height={36} sx={{ borderRadius: 1 }} />
						<Skeleton variant='rectangular' width={80} height={36} sx={{ borderRadius: 1 }} />
						<Skeleton variant='rectangular' width={80} height={36} sx={{ borderRadius: 1 }} />
					</Stack>
				</Stack>
			</Box>
		</Card>
	)
}

export default SkeletonCartItem
