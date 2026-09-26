import SkeletonBox from '@/components/skeletons/SkeletonBox'
import { Grid, Paper, Skeleton, Stack, alpha, useTheme } from '@mui/material'

const QueueDetailManagementLoadingSection = ({ isReceptionRoom }) => {
	const theme = useTheme()

	return (
		<Stack spacing={1.1}>
			{isReceptionRoom && <Skeleton variant='rounded' height={38} />}

			<Grid container spacing={1.1} sx={{ minHeight: { md: 600 } }}>
				<Grid size={{ xs: 12, md: 3 }} sx={{ display: 'flex' }}>
					<Paper
						elevation={0}
						sx={{
							height: '100%',
							width: '100%',
							border: `1px solid ${alpha(theme.palette.primary.main, 0.2)}`,
							p: 1,
							backgroundColor: alpha(theme.palette.primary.main, 0.02),
						}}
					>
						<SkeletonBox numberOfBoxes={6} rounded heights={[56]} />
					</Paper>
				</Grid>

				<Grid size={{ xs: 12, md: 9 }} sx={{ display: 'flex' }}>
					<Paper
						elevation={0}
						sx={{
							height: '100%',
							width: '100%',
							border: `1px solid ${alpha(theme.palette.primary.main, 0.2)}`,
							p: 1,
							backgroundColor: alpha(theme.palette.primary.main, 0.02),
						}}
					>
						<SkeletonBox numberOfBoxes={3} rounded heights={[84, 210, 116]} />
					</Paper>
				</Grid>
			</Grid>

			<Paper sx={{ p: 1, borderRadius: 2 }}>
				<SkeletonBox numberOfBoxes={4} direction='row' rounded heights={[44]} />
			</Paper>
		</Stack>
	)
}

export default QueueDetailManagementLoadingSection
