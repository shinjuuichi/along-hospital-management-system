import { EnumConfig } from '@/configs/enumConfig'
import { routeUrls } from '@/configs/routeUrls'
import useAuth from '@/hooks/useAuth'
import useTranslation from '@/hooks/useTranslation'
import { AddTaskRounded, CloseRounded, Feed, PlayArrowRounded } from '@mui/icons-material'
import { Button, Grid, Stack, Typography, useTheme } from '@mui/material'
import { useNavigate } from 'react-router-dom'

const QueueDetailManagementMainActionSection = ({
	currentQueue,
	canComplete = false,
	canCancel = false,
	onChangeQueueStatus = () => {},
	onOpenAssignMedicalHistory = () => {},
}) => {
	const theme = useTheme()
	const navigate = useNavigate()
	const { t } = useTranslation()
	const { auth } = useAuth()

	const isReceptionist = auth?.role === EnumConfig.Role.Receptionist
	const isInProgress = currentQueue?.queueStatus === EnumConfig.QueueStatus.InProgress
	const hasMedicalHistory = !!currentQueue?.medicalHistoryId

	const shouldShowAssignButton = isReceptionist && !hasMedicalHistory && isInProgress

	return (
		<>
			{shouldShowAssignButton ? (
				<Button
					variant='contained'
					disabled={!currentQueue}
					onClick={onOpenAssignMedicalHistory}
					sx={{
						minHeight: { xs: 120, md: 185 },
						bgcolor: theme.palette.warning.main,
						'&:hover': {
							bgcolor: theme.palette.warning.dark,
						},
					}}
				>
					<Stack spacing={0.55} alignItems='center'>
						<AddTaskRounded sx={{ fontSize: 42 }} />
						<Typography
							sx={{
								fontWeight: 800,
								fontSize: { xs: '1.2rem', md: '2rem' },
								letterSpacing: '0.02em',
								textTransform: 'uppercase',
							}}
						>
							{t('queue.button.assign_medical_history')}
						</Typography>
						<Typography
							sx={{
								fontSize: { xs: '0.8rem', md: '0.98rem' },
								fontWeight: 500,
								opacity: 0.9,
							}}
						>
							{t('queue.text.assign_medical_history_hint')}
						</Typography>
					</Stack>
				</Button>
			) : (
				<Button
					variant='contained'
					disabled={!hasMedicalHistory}
					onClick={() =>
						navigate(
							routeUrls.BASE_ROUTE.STAFF(
								routeUrls.STAFF.MEDICAL_HISTORY.DETAIL(currentQueue?.medicalHistoryId)
							)
						)
					}
					sx={{
						minHeight: { xs: 120, md: 185 },
						bgcolor: theme.palette.primary.main,
						'&:hover': {
							bgcolor: theme.palette.primary.dark,
						},
					}}
				>
					<Stack spacing={0.55} alignItems='center'>
						<Feed sx={{ fontSize: 42 }} />
						<Typography
							sx={{
								fontWeight: 800,
								fontSize: { xs: '1.2rem', md: '2rem' },
								letterSpacing: '0.02em',
								textTransform: 'uppercase',
							}}
						>
							{t('queue.button.view_medical_history_detail')}
						</Typography>
						<Typography
							sx={{
								fontSize: { xs: '0.8rem', md: '0.98rem' },
								fontWeight: 500,
								opacity: 0.9,
							}}
						>
							{t('queue.text.view_medical_history_hint')}
						</Typography>
					</Stack>
				</Button>
			)}

			<Grid container spacing={1}>
				<Grid size={{ xs: 12, sm: 6 }}>
					<Button
						variant='contained'
						fullWidth
						color='success'
						disabled={!currentQueue || !canComplete}
						onClick={() => onChangeQueueStatus(currentQueue.id, EnumConfig.QueueStatus.Completed)}
						startIcon={<PlayArrowRounded />}
						sx={{
							minHeight: { xs: 62, md: 76 },
							fontWeight: 800,
							fontSize: { xs: '1rem', md: '1.18rem' },
						}}
					>
						{t('button.complete')}
					</Button>
				</Grid>
				<Grid size={{ xs: 12, sm: 6 }}>
					<Button
						variant='outlined'
						fullWidth
						disabled={!currentQueue || !canCancel}
						color='error'
						onClick={() => onChangeQueueStatus(currentQueue.id, EnumConfig.QueueStatus.Cancelled)}
						startIcon={<CloseRounded />}
						sx={{
							minHeight: { xs: 62, md: 76 },
							fontWeight: 800,
							fontSize: { xs: '1rem', md: '1.18rem' },
						}}
					>
						{t('button.cancel')}
					</Button>
				</Grid>
			</Grid>
		</>
	)
}

export default QueueDetailManagementMainActionSection
