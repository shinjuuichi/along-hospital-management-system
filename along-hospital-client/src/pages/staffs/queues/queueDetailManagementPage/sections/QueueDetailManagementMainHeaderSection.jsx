import { defaultQueueStatusStyle } from '@/configs/defaultStylesConfig'
import { EnumConfig } from '@/configs/enumConfig'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { getImageFromCloud } from '@/utils/commons'
import { getEnumLabelByValue, renderEmptyFallback } from '@/utils/handleStringUtil'
import {
	Avatar,
	Button,
	Chip,
	Grid,
	Paper,
	Stack,
	Typography,
	alpha,
	useTheme,
} from '@mui/material'
import { useMemo } from 'react'
import { formatQueueNumber } from '../../queueManagementUtil'
import { getQueueStatusActionLabel } from '../queueDetailManagementUtil'

const QueueDetailManagementMainHeaderSection = ({
	currentQueue,
	transitionStatuses = [],
	onChangeQueueStatus = () => {},
}) => {
	const theme = useTheme()
	const { t } = useTranslation()
	const _enum = useEnum()

	const patient = currentQueue?.medicalHistorySnapshot?.patient
	const priority = Number(currentQueue?.priority) || 0

	const demographicText = useMemo(() => {
		const segments = [patient?.dateOfBirth, patient?.gender].filter(Boolean)
		return segments.length ? segments.join(' / ') : t('queue.placeholder.demographic_pending')
	}, [currentQueue, t])

	const renderTransitionButton = (status) => {
		const label = getQueueStatusActionLabel(currentQueue?.queueStatus, status, t)
		const isPositive =
			status === EnumConfig.QueueStatus.InProgress ||
			status === EnumConfig.QueueStatus.Completed ||
			status === EnumConfig.QueueStatus.Called
		const variant =
			status === EnumConfig.QueueStatus.Cancelled ? 'outlined' : isPositive ? 'contained' : 'outlined'

		return (
			<Button
				key={status}
				variant={variant}
				color={defaultQueueStatusStyle(status)}
				size='small'
				disabled={!currentQueue}
				onClick={() => onChangeQueueStatus(currentQueue?.id, status)}
				sx={{
					minHeight: 42,
					borderRadius: 1.5,
					fontWeight: 700,
					fontSize: '0.82rem',
					letterSpacing: '0.01em',
					justifyContent: 'center',
					textTransform: 'none',
				}}
			>
				{label}
			</Button>
		)
	}

	return (
		<Paper
			variant='outlined'
			sx={{
				p: { xs: 1.1, md: 1.4 },
				borderColor: alpha(theme.palette.primary.main, 0.2),
			}}
		>
			<Grid container spacing={1.2} alignItems='center'>
				<Grid size={{ xs: 12, md: 3 }}>
					<Stack
						spacing={0.25}
						sx={{
							pr: { xs: 0, md: 1 },
							borderRight: { xs: 'none', md: `1px solid ${alpha(theme.palette.divider, 0.75)}` },
						}}
					>
						<Typography
							variant='caption'
							sx={{
								fontWeight: 800,
								letterSpacing: '0.08em',
								textTransform: 'uppercase',
								color: alpha(theme.palette.text.primary, 0.5),
							}}
						>
							{t('queue.title.now_serving')}
						</Typography>
						<Typography
							sx={{
								fontWeight: 900,
								fontSize: { xs: '2.2rem', md: '2.9rem' },
								lineHeight: 1,
								letterSpacing: '-0.03em',
								color: theme.palette.primary.main,
							}}
						>
							{formatQueueNumber(currentQueue?.queueNumber)}
						</Typography>
						<Typography variant='subtitle2' sx={{ fontWeight: 600, color: 'text.secondary' }}>
							{t('queue.field.number_of_calls')}: {currentQueue?.numberOfCalls || 0}
						</Typography>
					</Stack>
				</Grid>

				<Grid size={{ xs: 12, md: 6 }}>
					<Stack direction='row' spacing={1.1} alignItems='center' sx={{ minWidth: 0 }}>
						<Avatar
							src={getImageFromCloud(patient?.image) || undefined}
							sx={{ width: 50, height: 50, bgcolor: alpha(theme.palette.primary.main, 0.2) }}
						/>

						<Stack spacing={0.3} sx={{ minWidth: 0, flex: 1 }}>
							<Stack direction='row' spacing={0.7} useFlexGap flexWrap='wrap' alignItems='center'>
								<Typography
									sx={{
										fontWeight: 800,
										fontSize: { xs: '1.05rem', md: '1.18rem' },
										lineHeight: 1.2,
										overflow: 'hidden',
										textOverflow: 'ellipsis',
										whiteSpace: 'nowrap',
									}}
								>
									{patient?.name || t('queue.placeholder.patient_name_pending')}
								</Typography>

								{priority > 0 && (
									<Chip
										size='small'
										color='error'
										label={`${t('queue.text.priority')} ${priority}`}
										sx={{ fontWeight: 800, height: 22, fontSize: '0.68rem' }}
									/>
								)}

								<Chip
									size='small'
									label={getEnumLabelByValue(_enum.queueStatusOptions, currentQueue?.queueStatus)}
									variant='outlined'
									color='primary'
									sx={{ fontWeight: 700, height: 22, fontSize: '0.68rem' }}
								/>
							</Stack>

							<Typography variant='body2' sx={{ fontWeight: 600, color: 'text.secondary' }}>
								{demographicText}
							</Typography>
							<Typography variant='body2' sx={{ fontWeight: 700, color: 'text.secondary' }}>
								{t('profile.field.medical_number')}: {renderEmptyFallback(patient?.medicalNumber)}
							</Typography>
						</Stack>
					</Stack>
				</Grid>

				<Grid size={{ xs: 12, md: 3 }}>
					<Stack spacing={0.8}>
						{transitionStatuses.length > 0 ? (
							transitionStatuses.map((status) => renderTransitionButton(status))
						) : (
							<Button size='small' variant='outlined' disabled sx={{ minHeight: 42, fontWeight: 700 }}>
								{t('queue.title.queue_actions')}
							</Button>
						)}
					</Stack>
				</Grid>
			</Grid>
		</Paper>
	)
}

export default QueueDetailManagementMainHeaderSection
