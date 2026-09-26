import { getEnumLabelByValue, renderEmptyFallback } from '@/utils/handleStringUtil'
import { ArrowBack } from '@mui/icons-material'
import { Button, Card, Chip, Grid, Stack, Typography } from '@mui/material'

const MedicineInfoSection = ({
	t,
	selectedMedicine,
	stats,
	medicineStatusOptions = [],
	onBack = () => {},
}) => {
	return (
		<>
			<Stack direction='row' alignItems='center' justifyContent='space-between' spacing={2}>
				<Stack direction='row' spacing={1} alignItems='center'>
					<Button startIcon={<ArrowBack />} onClick={onBack}>
						{t('button.back')}
					</Button>
				</Stack>
			</Stack>

			<Stack
				direction={{ xs: 'column', md: 'row' }}
				spacing={2}
				alignItems={{ xs: 'flex-start', md: 'center' }}
			>
				<Typography variant='h6'>{renderEmptyFallback(selectedMedicine?.name)}</Typography>
				<Typography variant='body2' color='text.secondary'>
					{t('medicine.field.brand')}: {renderEmptyFallback(selectedMedicine?.brand)}
				</Typography>
			</Stack>

			<Card variant='outlined' sx={{ p: 2 }}>
				<Stack direction='row' alignItems='center' justifyContent='space-between' sx={{ mb: 2 }}>
					<Typography variant='subtitle1' sx={{ fontWeight: 700 }}>
						{t('medicine.title.medicine_information')}
					</Typography>
					<Stack direction='row' spacing={1}>
						<Chip size='small' label={`${t('medicine.label.total_skus')}: ${stats.total}`} />
						<Chip
							size='small'
							color='success'
							label={`${t('medicine.label.active_skus')}: ${stats.active}`}
						/>
					</Stack>
				</Stack>

				<Grid container spacing={2}>
					<Grid size={{ xs: 12, md: 3 }}>
						<Typography variant='caption' color='text.secondary'>
							{t('medicine.field.medicine_category.name')}
						</Typography>
						<Typography variant='body1'>{renderEmptyFallback(selectedMedicine?.medicineCategory?.name)}</Typography>
					</Grid>
					<Grid size={{ xs: 12, md: 3 }}>
						<Typography variant='caption' color='text.secondary'>
							{t('medicine.field.unit')}
						</Typography>
						<Typography variant='body1'>{renderEmptyFallback(selectedMedicine?.medicineUnit?.name)}</Typography>
					</Grid>
					<Grid size={{ xs: 12, md: 3 }}>
						<Typography variant='caption' color='text.secondary'>
							{t('medicine.field.status')}
						</Typography>
						<Typography variant='body1'>
							{renderEmptyFallback(
								getEnumLabelByValue(medicineStatusOptions, selectedMedicine?.status) ||
									selectedMedicine?.status
							)}
						</Typography>
					</Grid>
					<Grid size={{ xs: 12, md: 3 }}>
						<Typography variant='caption' color='text.secondary'>
							{t('medicine.filter.is_public')}
						</Typography>
						<Typography variant='body1'>
							{selectedMedicine?.isPublic ? t('medicine.text.true') : t('medicine.text.false')}
						</Typography>
					</Grid>
					<Grid size={{ xs: 12, md: 3 }}>
						<Typography variant='caption' color='text.secondary'>
							{t('medicine.field.options')}
						</Typography>
						<Typography variant='body1'>
							{renderEmptyFallback(
								(selectedMedicine?.medicineUnit?.options || [])
									.map((opt) => opt.option?.optionName || '')
									.filter(Boolean)
									.join(', ')
							)}
						</Typography>
					</Grid>
				</Grid>
			</Card>
		</>
	)
}

export default MedicineInfoSection
