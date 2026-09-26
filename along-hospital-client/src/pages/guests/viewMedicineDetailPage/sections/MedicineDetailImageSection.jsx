import { getImageFromCloud } from '@/utils/commons'
import { Box } from '@mui/material'
import { useTheme } from '@mui/material/styles'

const MedicineDetailImageSection = ({ medicine, currentImage, setCurrentImage }) => {
	const theme = useTheme()

	return (
		<>
			<Box
				sx={{
					width: '100%',
					aspectRatio: '1 / 1',
					borderRadius: 2,
					overflow: 'hidden',
					bgcolor: theme.palette.background.default,
					display: 'flex',
					alignItems: 'center',
					justifyContent: 'center',
					boxShadow: theme.shadows[1],
				}}
			>
				{currentImage ? (
					<Box
						component='img'
						src={getImageFromCloud(currentImage)}
						alt={medicine.name || 'Medicine Image'}
						sx={{ width: 500, height: 500, objectFit: 'cover', display: 'block' }}
						onError={(e) => {
							e.currentTarget.onerror = null
							e.currentTarget.src = '/placeholder-image.png'
						}}
					/>
				) : (
					<Box
						sx={{ width: '100%', height: '100%', bgcolor: theme.palette.action.disabledBackground }}
					/>
				)}
			</Box>

			{medicine.images?.length > 0 && (
				<Box display='flex' gap={1.5} mt={2} flexWrap='wrap'>
					{medicine.images.map((img, index) => (
						<Box
							key={index}
							component='img'
							src={getImageFromCloud(img)}
							alt={`Thumbnail ${index}`}
							onClick={() => setCurrentImage(img)}
							sx={{
								width: 70,
								height: 70,
								objectFit: 'cover',
								borderRadius: 1,
								cursor: 'pointer',
								border:
									currentImage === img
										? `2px solid ${theme.palette.primary.main}`
										: `1px solid ${theme.palette.divider}`,
								transition: 'all 0.2s ease',
								'&:hover': { borderColor: theme.palette.primary.main },
							}}
						/>
					))}
				</Box>
			)}
		</>
	)
}

export default MedicineDetailImageSection
