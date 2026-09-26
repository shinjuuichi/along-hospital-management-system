import { Box, Skeleton, Stack } from '@mui/material'
import SkeletonBox from './SkeletonBox'

const SkeletonLoadingPage = () => {
	return (
		<Box
			sx={{
				height: '100vh',
				overflow: 'hidden',
				p: { xs: 2, md: 3 },
			}}
		>
			<Stack spacing={3} height='100%'>
				<Stack spacing={1} mb={3}>
					<Skeleton variant='text' height={42} width='35%' />
					<Skeleton variant='text' height={20} width='100%' />
					<Skeleton variant='text' height={20} width='55%' />
				</Stack>
				<SkeletonBox direction='row' numberOfBoxes={3} heights={[110]} rounded />
				<SkeletonBox numberOfBoxes={4} heights={[56]} />
				<Box sx={{ flex: 1, overflow: 'hidden' }}>
					<SkeletonBox direction='row' numberOfBoxes={2} heights={[180]} rounded />
				</Box>
			</Stack>
		</Box>
	)
}

export default SkeletonLoadingPage
