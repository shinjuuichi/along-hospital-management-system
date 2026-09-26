import { getImageFromCloud } from '@/utils/commons'
import { Box, Typography } from '@mui/material'

const ImageDisplayField = ({ src, alt, label, maxHeight = 240, sx = {} }) => {
	const imageSrc = getImageFromCloud(src)

	return (
		<Box>
			{label && (
				<Typography variant='caption' sx={{ display: 'block', mb: 0.5 }}>
					{label}
				</Typography>
			)}
			<Box
				component='img'
				src={imageSrc}
				alt={alt || label || 'Image'}
				sx={{
					width: '100%',
					maxHeight,
					objectFit: 'cover',
					borderRadius: 2,
					boxShadow: 1,
					...sx,
				}}
				onError={(e) => {
					e.currentTarget.onerror = null
					e.currentTarget.src = '/placeholder-image.png'
				}}
			/>
		</Box>
	)
}

export default ImageDisplayField
