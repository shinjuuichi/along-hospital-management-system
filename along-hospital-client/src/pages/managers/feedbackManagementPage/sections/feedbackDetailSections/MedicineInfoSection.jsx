import useTranslation from '@/hooks/useTranslation'
import { getImageFromCloud } from '@/utils/commons'
import { Box, Card, CardContent, CardMedia, Divider, Grid, Stack, Typography } from '@mui/material'

const MedicineInfoSection = ({ feedback }) => {
	const { t } = useTranslation()

	const medicineName = feedback?.getMedicineDTO?.medicineName
	const medicineBrand = feedback?.getMedicineDTO?.medicineBrand
	const medicineImages = feedback?.getMedicineDTO?.medicineImages

	return (
		<Card variant='outlined'>
			<CardContent>
				<Typography variant='subtitle1' fontWeight='bold' gutterBottom color='primary'>
					{t('feedback.title.medicine_info')}
				</Typography>
				<Divider sx={{ mb: 2 }} />
				<Grid container spacing={2}>
					<Grid size={8}>
						<Stack spacing={1}>
							<Box>
								<Typography variant='caption' color='text.secondary'>
									{t('feedback.field.medicine_name')}
								</Typography>
								<Typography variant='body1'>{medicineName}</Typography>
							</Box>
							<Box>
								<Typography variant='caption' color='text.secondary'>
									{t('feedback.field.medicine_brand')}
								</Typography>
								<Typography variant='body1'>{medicineBrand}</Typography>
							</Box>
						</Stack>
					</Grid>
					<Grid size={4}>
						{medicineImages && medicineImages.length > 0 && (
							<Stack direction='row' spacing={1} flexWrap='wrap'>
								{medicineImages.slice(0, 2).map((img, idx) => (
									<CardMedia
										key={idx}
										component='img'
										sx={{
											width: 60,
											height: 60,
											objectFit: 'cover',
											borderRadius: 1,
										}}
										image={getImageFromCloud(img)}
										alt={`Medicine ${idx + 1}`}
									/>
								))}
							</Stack>
						)}
					</Grid>
				</Grid>
			</CardContent>
		</Card>
	)
}

export default MedicineInfoSection
