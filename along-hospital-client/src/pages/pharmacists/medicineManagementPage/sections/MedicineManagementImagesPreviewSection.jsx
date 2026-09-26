import { getImageFromCloud } from '@/utils/commons'
import { Box, Stack } from '@mui/material'

const MedicineManagementImagesPreviewSection = ({ images = [], maxVisible = 1, rowId }) => {
	const extraCount = images.length - maxVisible

	return (
		<Stack direction='row' spacing={1}>
			{images.slice(0, maxVisible).map((imgPath, idx) => (
				<Box
					key={idx}
					component='img'
					src={getImageFromCloud(imgPath)}
					alt={`medicine-${rowId}-${idx}`}
					sx={{ width: 60, height: 60, objectFit: 'cover', borderRadius: 1 }}
				/>
			))}

			{extraCount > 0 && (
				<Box
					sx={{
						width: 60,
						height: 60,
						borderRadius: 1,
						backgroundColor: 'grey.300',
						display: 'flex',
						alignItems: 'center',
						justifyContent: 'center',
						fontSize: 14,
						fontWeight: 'bold',
						color: 'text.primary',
					}}
				>
					+{extraCount}
				</Box>
			)}
		</Stack>
	)
}

export default MedicineManagementImagesPreviewSection
