import { Box, Skeleton, Stack } from '@mui/material'
import SkeletonBox from './SkeletonBox'

const SkeletonPatientCard = () => {
	return (
		<Box>
			<Stack direction='row' spacing={1.5} alignItems='center' sx={{ mb: 2 }}>
				<Skeleton variant='circular' width={56} height={56} />
				<Box sx={{ flex: 1 }}>
					<Skeleton variant='text' width='60%' />
					<Skeleton variant='text' width='40%' />
				</Box>
			</Stack>

			<Stack spacing={1.5}>
				<SkeletonBox numberOfBoxes={2} heights={[36]} direction='row' rounded />
				<SkeletonBox numberOfBoxes={2} heights={[36]} direction='row' rounded />
				<SkeletonBox numberOfBoxes={1} heights={[68]} rounded />
			</Stack>
		</Box>
	)
}

export default SkeletonPatientCard
