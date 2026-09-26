import { Close } from '@mui/icons-material'
import { Box, IconButton } from '@mui/material'

const ImageTileRenderField = ({ src, alt, onRemove }) => {
	return (
		<Box
			sx={{
				position: 'relative',
				flex: '1 1 max(50%, 160px)',
				maxWidth: 160,
				aspectRatio: 1,
				borderRadius: 2,
				overflow: 'hidden',
				boxShadow: 1,
				alignItems: 'center',
			}}
		>
			<Box
				component='img'
				src={src}
				onError={(e) => {
					e.currentTarget.onerror = null
					e.currentTarget.src = '/placeholder-image.png'
				}}
				alt={alt}
				sx={{ width: '100%', height: '100%', objectFit: 'cover' }}
			/>
			<IconButton
				size='small'
				onClick={onRemove}
				sx={{
					position: 'absolute',
					top: 4,
					right: 4,
					bgcolor: 'background.paper',
					boxShadow: 1,
					'&:hover': { bgcolor: 'background.paper' },
				}}
			>
				<Close fontSize='small' />
			</IconButton>
		</Box>
	)
}

export default ImageTileRenderField
