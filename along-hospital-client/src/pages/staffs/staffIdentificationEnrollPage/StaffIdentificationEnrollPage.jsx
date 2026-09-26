import FaceCaptureDialog from '@/components/dialogs/FaceCaptureDialog'
import { ApiUrls } from '@/configs/apiUrls'
import { routeUrls } from '@/configs/routeUrls'
import useAuth from '@/hooks/useAuth'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import { setAuthStore } from '@/redux/reducers/authReducer'
import CameraAltIcon from '@mui/icons-material/CameraAlt'
import Face6Icon from '@mui/icons-material/Face6'
import {
	Box,
	Button,
	Card,
	CardContent,
	Chip,
	CircularProgress,
	Container,
	Grid,
	Stack,
	Typography,
} from '@mui/material'
import { useState } from 'react'
import { useNavigate } from 'react-router-dom'

const StaffIdentificationEnrollPage = () => {
	const [openCapture, setOpenCapture] = useState(false)
	const [error, setError] = useState('')

	const navigate = useNavigate()
	const { t } = useTranslation()
	const { auth } = useAuth()
	const { fetch } = useReduxStore({
		selector: (s) => s.auth,
		setStore: setAuthStore,
	})

	const enrollFace = useAxiosSubmit({
		url: ApiUrls.ATTENDANCE.STAFF_ATTENDANCE.ENROLL,
		method: 'POST',
		onSuccess: async () => {
			await fetch()
			navigate(routeUrls.BASE_ROUTE.STAFF(routeUrls.STAFF.ATTENDANCE))
		},
	})

	const handleOpenCapture = () => {
		setOpenCapture(true)
	}

	const handleCaptureComplete = async (blobs) => {
		setError('')
		const formData = new FormData()
		blobs.forEach((blob, index) => {
			formData.append('images', blob, `face_${index + 1}.jpg`)
		})

		await enrollFace.submit({ overrideData: formData })
	}

	return (
		<Container maxWidth='md' sx={{ py: 4 }}>
			<Stack spacing={3}>
				<Stack direction='row' alignItems='center' spacing={1}>
					<Face6Icon fontSize='large' />
					<Box>
						<Typography variant='h5' fontWeight={600}>
							{t('attendance.title.face_enrollment')}
						</Typography>
						<Typography variant='body2' color='text.secondary'>
							{t('attendance.title.face_enrollment_subtitle')}
						</Typography>
					</Box>
				</Stack>

				<Grid container spacing={3}>
					<Grid size={{ xs: 12, md: 7 }}>
						<Card>
							<CardContent>
								<Stack spacing={2}>
									<Stack direction='row' alignItems='center' justifyContent='space-between'>
										<Typography variant='subtitle1' fontWeight={600}>
											{t('attendance.face_enrollment.status.current_status')}
										</Typography>
										{
											<Chip
												label={
													!auth.isIdentificationExist
														? t('attendance.face_enrollment.status.not_enrolled')
														: t('attendance.face_enrollment.status.enrolled')
												}
											/>
										}
									</Stack>

									<Box>
										<Typography variant='body2' color='text.secondary'>
											{t('attendance.face_enrollment.description.general_info')}
										</Typography>
									</Box>

									<Stack direction='row' spacing={2} alignItems='center'>
										<Button
											variant='contained'
											startIcon={<CameraAltIcon />}
											onClick={handleOpenCapture}
											disabled={enrollFace.loading}
										>
											{t('attendance.button.start_enrollment')}
										</Button>
										{enrollFace.loading && (
											<Stack direction='row' spacing={1} alignItems='center'>
												<CircularProgress size={18} />
												<Typography variant='body2'>
													{t('attendance.face_enrollment.description.processing_images')}
												</Typography>
											</Stack>
										)}
									</Stack>

									{error && (
										<Typography variant='body2' color='error'>
											{error}
										</Typography>
									)}

									<Box
										sx={{
											mt: 1,
											p: 1.5,
											borderRadius: 1.5,
											bgcolor: (theme) =>
												theme.palette.mode === 'light' ? theme.palette.grey[100] : theme.palette.grey[900],
										}}
									>
										<Typography variant='caption' color='text.secondary'>
											{t('attendance.face_enrollment.description.re_enroll')}
										</Typography>
									</Box>
								</Stack>
							</CardContent>
						</Card>
					</Grid>

					<Grid size={{ xs: 12, md: 5 }}>
						<Card sx={{ height: '100%' }}>
							<CardContent>
								<Typography variant='subtitle1' fontWeight={600} mb={1}>
									{t('attendance.face_enrollment.description.step.how_it_works')}
								</Typography>
								<Stack spacing={1.5}>
									<Box>
										<Typography variant='body2' fontWeight={500}>
											{t('attendance.face_enrollment.description.step.1.title')}
										</Typography>
										<Typography variant='body2' color='text.secondary'>
											{t('attendance.face_enrollment.description.step.1.description')}
										</Typography>
									</Box>
									<Box>
										<Typography variant='body2' fontWeight={500}>
											{t('attendance.face_enrollment.description.step.2.title')}
										</Typography>
										<Typography variant='body2' color='text.secondary'>
											{t('attendance.face_enrollment.description.step.2.description')}
										</Typography>
									</Box>
									<Box>
										<Typography variant='body2' fontWeight={500}>
											{t('attendance.face_enrollment.description.step.3.title')}
										</Typography>
										<Typography variant='body2' color='text.secondary'>
											{t('attendance.face_enrollment.description.step.3.description')}
										</Typography>
									</Box>
								</Stack>

								<Box
									sx={{
										mt: 2,
										p: 2,
										borderRadius: 2,
										border: (theme) => `1px dashed ${theme.palette.divider}`,
										display: 'flex',
										alignItems: 'center',
										justifyContent: 'center',
										textAlign: 'center',
									}}
								>
									<Typography variant='body2' color='text.secondary'>
										{t('attendance.face_enrollment.description.illustration_note')}
									</Typography>
								</Box>
							</CardContent>
						</Card>
					</Grid>
				</Grid>
			</Stack>

			<FaceCaptureDialog
				open={openCapture}
				onClose={() => setOpenCapture(false)}
				mode='burst'
				durationSeconds={5}
				onCaptureComplete={handleCaptureComplete}
			/>
		</Container>
	)
}

export default StaffIdentificationEnrollPage
