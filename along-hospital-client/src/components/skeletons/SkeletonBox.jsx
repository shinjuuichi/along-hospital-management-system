import { Skeleton, Stack } from '@mui/material'

/**
 * @param {Object} props
 * @param {number} props.numberOfBoxes
 * @param {'column' | 'row'} props.direction
 * @param {number[]} props.heights
 * @param {boolean} props.rounded
 */
const SkeletonBox = ({
	numberOfBoxes = 3,
	direction = 'column',
	heights = [100, 200, 300],
	rounded = false,
}) => {
	return (
		<Stack spacing={2} direction={direction}>
			{Array.from({ length: numberOfBoxes }).map((_, index) => (
				<Skeleton
					key={index}
					variant={rounded ? 'rounded' : 'rectangular'}
					width='100%'
					height={heights[index % heights.length] || 200}
				/>
			))}
		</Stack>
	)
}

export default SkeletonBox
