import SkeletonBox from '@/components/skeletons/SkeletonBox'
import { Grid, Paper, Skeleton, Stack } from '@mui/material'

const QueueManagementLoadingSection = () => {
	return (
		<Stack spacing={2}>
			<Paper sx={{ p: 2.5, borderRadius: 3 }}>
				<Stack spacing={2}>
					<Stack
						direction={{ xs: 'column', md: 'row' }}
						justifyContent='space-between'
						alignItems={{ xs: 'flex-start', md: 'center' }}
						spacing={1}
					>
						<Stack spacing={0.8} sx={{ width: { xs: '100%', md: '40%' } }}>
							<Skeleton variant='text' width='70%' height={38} />
							<Skeleton variant='text' width='50%' height={22} />
						</Stack>

						<Skeleton variant='rounded' height={42} sx={{ width: { xs: '100%', md: 360 } }} />
					</Stack>

					<Stack
						direction={{ xs: 'column', md: 'row' }}
						justifyContent='space-between'
						alignItems={{ xs: 'flex-start', md: 'center' }}
						spacing={1}
					>
						<Skeleton variant='text' width={140} height={24} />
						<Skeleton variant='rounded' width={180} height={34} />
					</Stack>
				</Stack>
			</Paper>

			<Grid container spacing={2}>
				{Array.from({ length: 4 }).map((_, index) => (
					<Grid key={index} size={{ xs: 12, sm: 6, md: 3 }}>
						<Paper sx={{ p: 2, borderRadius: 3 }}>
							<SkeletonBox numberOfBoxes={5} rounded heights={[32, 20, 94, 18, 52]} />
						</Paper>
					</Grid>
				))}
			</Grid>
		</Stack>
	)
}

export default QueueManagementLoadingSection
