import { Skeleton, Stack } from '@mui/material'

const SkeletonTextField = ({ numberOfRow = 3, withTitle = false, withLabel = false }) => {
	return (
		<Stack spacing={3}>
			{withTitle && <Skeleton variant='rectangular' width={200} height={50} animation='wave' />}
			<Stack spacing={2}>
				{Array.from({ length: numberOfRow }).map((_, i) => (
					<Stack key={i} spacing={1} sx={{ width: '100%' }}>
						{withLabel && (
							<Skeleton
								key={`label-${i}`}
								variant='rectangular'
								width={100}
								height={30}
								animation='wave'
							/>
						)}
						<Skeleton key={i} variant='rectangular' height={50} animation='wave' />
					</Stack>
				))}
			</Stack>
		</Stack>
	)
}

export default SkeletonTextField
