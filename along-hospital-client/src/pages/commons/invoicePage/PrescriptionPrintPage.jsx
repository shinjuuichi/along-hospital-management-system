import SkeletonInvoice from '@/components/skeletons/SkeletonInvoice'
import { ApiUrls } from '@/configs/apiUrls'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import { formatDateBasedOnCurrentLanguage } from '@/utils/formatDateUtil'
import { renderEmptyFallback } from '@/utils/handleStringUtil'
import {
	Box,
	Button,
	Divider,
	Grid,
	Paper,
	Table,
	TableBody,
	TableCell,
	TableHead,
	TableRow,
	Typography,
} from '@mui/material'
import { useParams } from 'react-router-dom'

const PrescriptionPrintPage = () => {
	const { id } = useParams()
	const { t } = useTranslation()

	const { data, loading } = useFetch(ApiUrls.MEDICAL_HISTORY.DETAIL(id), {}, [id])

	if (loading || !data) {
		return <SkeletonInvoice />
	}

	return (
		<Box sx={{ p: 4 }}>
			<style>
				{`
				@media print {
					body * {
						visibility: hidden;
					}
					#prescription-paper, #prescription-paper * {
						visibility: visible;
					}
					#prescription-paper {
						position: absolute;
						top: 0;
						left: 50%;
						transform: translateX(-50%);
						width: 100%;
					}
				}
				`}
			</style>

			<Box sx={{ display: 'flex', justifyContent: 'flex-end', mb: 2, gap: 2 }}>
				<Button variant='outlined' onClick={() => window.print()}>
					{t('medical_history.button.print_prescription')}
				</Button>
			</Box>

			<Paper
				id='prescription-paper'
				elevation={3}
				sx={{
					maxWidth: 900,
					mx: 'auto',
					p: 4,
					bgcolor: 'background.paper',
					'@media print': {
						boxShadow: 'none',
						borderRadius: 0,
					},
				}}
			>
				<Grid container spacing={2} alignItems='center'>
					<Grid size={8}>
						<Typography variant='h5' fontWeight={700} textTransform={'uppercase'}>
							{t('about_us.information.name')}
						</Typography>
						<Typography variant='body2'>
							{t('about_us.field.address')}: {t('about_us.information.address')}
						</Typography>
						<Typography variant='body2'>
							{t('about_us.field.phone')}: {t('about_us.information.phone')}
						</Typography>
					</Grid>
					<Grid size={4} sx={{ textAlign: 'right' }}>
						<Typography variant='h6' fontWeight={700} textTransform={'uppercase'}>
							{t('medical_history.title.prescription')}
						</Typography>
						<Typography variant='body2'>
							{t('medical_history.field.medical_history_number')}: {data.medicalHistoryNumber}
						</Typography>
						<Typography variant='body2'>
							{t('medical_history.field.discharge_date')}:{' '}
							{formatDateBasedOnCurrentLanguage(data.dischargeDate)}
						</Typography>
					</Grid>
				</Grid>

				<Divider sx={{ my: 3 }} />

				<Grid container spacing={2} rowSpacing={3}>
					<Grid size={6}>
						<Typography variant='subtitle1' fontWeight={600} gutterBottom>
							{t('medical_history.title.patient_info')}
						</Typography>
						<Typography variant='body2'>
							{t('profile.field.name')}: {data.patient?.name || '—'}
						</Typography>
						<Typography variant='body2'>
							{t('profile.field.medical_number')}: {data.patient?.medicalNumber || data.patientId}
						</Typography>
						<Typography variant='body2'>
							{t('profile.field.phone')}: {data.patient?.phone || '—'}
						</Typography>
						<Typography variant='body2'>
							{t('profile.field.email')}: {data.patient?.email || '—'}
						</Typography>
					</Grid>
					<Grid size={6}>
						<Typography variant='subtitle1' fontWeight={600} gutterBottom>
							{t('medical_history.title.doctor_info')}
						</Typography>
						<Typography variant='body2'>
							{t('profile.field.name')}: {data.doctor?.name || '—'}
						</Typography>
						<Typography variant='body2'>
							{t('profile.field.specialty')}: {data.doctor?.specialtyName || '—'}
						</Typography>
						<Typography variant='body2'>
							{t('profile.field.phone')}: {data.doctor?.phone || '—'}
						</Typography>
					</Grid>

					<Grid size={6}>
						<Typography variant='subtitle1' fontWeight={600} gutterBottom>
							{t('medical_history.field.diagnosis')}
						</Typography>
						<Typography variant='body2'>{data.diagnosis || '—'}</Typography>
					</Grid>

					<Grid size={6}>
						<Typography variant='subtitle1' fontWeight={600} gutterBottom>
							{t('medical_history.title.medication_guide')}
						</Typography>
						<Typography variant='body2'>
							{t('medical_history.field.prescription.medication_days')}:{' '}
							{data.prescription?.medicationDays || 0} {t('text.day')}
						</Typography>
						<Typography variant='body2'>
							{t('medical_history.field.prescription.doctor_note')}: {renderEmptyFallback(data.prescription?.doctorNote)}
						</Typography>
					</Grid>
				</Grid>

				<Box sx={{ mt: 4 }}>
					<Typography variant='subtitle1' fontWeight={600} gutterBottom>
						{t('medical_history.title.prescription_details')}
					</Typography>
					<Table size='small'>
						<TableHead>
							<TableRow>
								<TableCell>{t('text.no_')}</TableCell>
								<TableCell>
									{t('medical_history.field.prescription.prescription_detail.medicine_name')}
								</TableCell>
								<TableCell>
									{t('medical_history.field.prescription.prescription_detail.medicine_brand')}
								</TableCell>
								<TableCell align='center'>
									{t('medical_history.field.prescription.prescription_detail.dosage')}
								</TableCell>
								<TableCell align='center'>
									{t('medical_history.field.prescription.prescription_detail.frequency_per_day')}
								</TableCell>
								<TableCell>
									{t('medical_history.field.prescription.prescription_detail.medicine_unit')}
								</TableCell>
							</TableRow>
						</TableHead>
						<TableBody>
							{data.prescription?.prescriptionDetails?.map((item, index) => {
								const medicine = item.medicineSnapshot

								return (
									<TableRow key={item.medicineId}>
										<TableCell>{index + 1}</TableCell>
										<TableCell>{renderEmptyFallback(medicine?.medicineName)}</TableCell>
										<TableCell>{renderEmptyFallback(medicine?.medicineBrand)}</TableCell>
										<TableCell align='center'>{item.dosage}</TableCell>
										<TableCell align='center'>{item.frequencyPerDay}</TableCell>
										<TableCell>{renderEmptyFallback(medicine?.medicineUnit)}</TableCell>
									</TableRow>
								)
							})}
						</TableBody>
					</Table>
				</Box>

				<Box sx={{ mt: 3 }}>
					<Typography variant='body2' fontStyle='italic'>
						{t('medical_history.placeholder.prescription_note')}
					</Typography>
				</Box>

				<Box sx={{ mt: 6, display: 'flex', justifyContent: 'space-between' }}>
					<Box sx={{ textAlign: 'center' }}>
						<Typography variant='body2' fontWeight={600}>
							{t('medical_history.field.patient')}
						</Typography>
						<Typography variant='caption'>({t('text.sign_here')})</Typography>
					</Box>
					<Box sx={{ textAlign: 'center' }}>
						<Typography variant='body2' fontWeight={600}>
							{t('medical_history.field.doctor')}
						</Typography>
						<Typography variant='caption'>({t('text.sign_here')})</Typography>
					</Box>
				</Box>
			</Paper>
		</Box>
	)
}

export default PrescriptionPrintPage
