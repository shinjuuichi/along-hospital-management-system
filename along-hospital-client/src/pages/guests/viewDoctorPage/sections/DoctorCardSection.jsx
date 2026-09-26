import useTranslation from '@/hooks/useTranslation'
import { getImageFromCloud } from '@/utils/commons'
import { formatDateBasedOnCurrentLanguage } from '@/utils/formatDateUtil'
import { AccessTimeRounded, BadgeRounded, SchoolRounded, WcRounded } from '@mui/icons-material'
import { Avatar, Box, Card, Chip, Divider, Stack, Typography } from '@mui/material'

const detailRows = (t, doctor) => [
	{
		key: 'gender',
		label: t('profile.field.gender'),
		value: doctor?.gender,
		icon: <WcRounded fontSize='small' />,
	},
	{
		key: 'dateOfBirth',
		label: t('profile.field.date_of_birth'),
		value: doctor?.dateOfBirth ? formatDateBasedOnCurrentLanguage(doctor.dateOfBirth) : '',
		icon: <AccessTimeRounded fontSize='small' />,
	},
]

const DoctorCardSection = ({ doctor = {} }) => {
	const { t } = useTranslation()
	const queueNumber = `BS-${String(doctor?.id || 0).padStart(3, '0')}`
	const detailItems = detailRows(t, doctor).filter((item) => item.value)

	return (
		<Card
			sx={(theme) => ({
				height: '100%',
				position: 'relative',
				overflow: 'hidden',
				borderRadius: 4,
				border: '1px solid',
				borderColor: 'divider',
				background:
					theme.palette.mode === 'dark'
						? 'linear-gradient(180deg, rgba(15,23,42,0.98) 0%, rgba(17,24,39,0.98) 100%)'
						: 'linear-gradient(180deg, rgba(255,255,255,0.98) 0%, rgba(247,251,252,1) 100%)',
				transition: '0.24s',
				'&:before, &:after': {
					content: '""',
					position: 'absolute',
					top: '42%',
					width: 20,
					height: 20,
					borderRadius: '50%',
					bgcolor: theme.palette.mode === 'dark' ? theme.palette.background.default : '#eef7f7',
					border: '1px solid',
					borderColor: 'divider',
					zIndex: 2,
				},
				'&:before': { left: -10 },
				'&:after': { right: -10 },
				'&:hover': {
					boxShadow: 8,
					transform: 'translateY(-4px)',
				},
			})}
		>
			<Stack sx={{ height: '100%' }}>
				<Box
					sx={(theme) => ({
						px: 2.25,
						py: 1.75,
						color: 'common.white',
						background:
							theme.palette.mode === 'dark'
								? 'linear-gradient(135deg, #1d4d77 0%, #116466 100%)'
								: 'linear-gradient(135deg, #1c3f67 0%, #1d7a83 100%)',
					})}
				>
					<Stack direction='row' justifyContent='space-between' spacing={2} alignItems='flex-start'>
						<Stack spacing={0.75}>
							<Typography variant='overline' sx={{ letterSpacing: 1.8, opacity: 0.8, fontSize: '0.62rem' }}>
								{t('doctor.ticket.ticket_label')}
							</Typography>
							<Typography variant='h6' sx={{ fontWeight: 800 }}>
								{queueNumber}
							</Typography>
							<Typography variant='caption' sx={{ opacity: 0.84 }}>
								{t('doctor.ticket.view_doctor')}
							</Typography>
						</Stack>
						<Typography variant='caption' sx={{ opacity: 0.88, fontWeight: 700 }}>
							#{doctor?.id || 0}
						</Typography>
					</Stack>
				</Box>

				<Stack spacing={2} sx={{ p: 2.25, flexGrow: 1 }}>
					<Stack direction={{ xs: 'column', sm: 'row' }} spacing={1.75} alignItems={{ xs: 'flex-start', sm: 'center' }}>
						<Avatar
							src={getImageFromCloud(doctor.image)}
							alt={doctor.name || t('doctor.field.image')}
							sx={{
								width: 72,
								height: 72,
								border: '3px solid',
								borderColor: 'rgba(14, 116, 144, 0.12)',
								boxShadow: '0 10px 20px rgba(15, 23, 42, 0.12)',
							}}
						/>
						<Stack spacing={1} sx={{ minWidth: 0 }}>
							<Typography variant='h6' sx={{ fontWeight: 800 }}>
								{doctor.name || t('doctor.ticket.pending_update')}
							</Typography>
							<Stack direction='row' spacing={1} useFlexGap flexWrap='wrap'>
								{doctor.specialtyName && (
									<Chip
										icon={<BadgeRounded />}
										label={doctor.specialtyName}
										size='small'
										sx={{ bgcolor: 'rgba(14, 116, 144, 0.12)', fontWeight: 700 }}
									/>
								)}
								{doctor.qualificationName && (
									<Chip
										icon={<SchoolRounded />}
										label={doctor.qualificationName}
										size='small'
										variant='outlined'
										sx={{ fontWeight: 600 }}
									/>
								)}
							</Stack>
						</Stack>
					</Stack>

					<Divider sx={{ borderStyle: 'dashed' }} />

					<Stack spacing={1}>
						<Typography variant='subtitle2' sx={{ fontWeight: 800, letterSpacing: 0.6 }}>
							{t('doctor.ticket.quick_info')}
						</Typography>
						{detailItems.length > 0 ? (
							detailItems.map((item) => (
								<Stack
									key={item.key}
									direction='row'
									spacing={1.5}
									alignItems='flex-start'
									sx={{ color: 'text.secondary' }}
								>
									<Box
										sx={{
											mt: 0.25,
											display: 'flex',
											alignItems: 'center',
											justifyContent: 'center',
											width: 28,
											height: 28,
											borderRadius: 2,
											bgcolor: 'rgba(14, 116, 144, 0.12)',
											color: 'primary.main',
											flexShrink: 0,
										}}
									>
										{item.icon}
									</Box>
									<Box sx={{ minWidth: 0 }}>
										<Typography variant='caption' sx={{ display: 'block', color: 'text.disabled' }}>
											{item.label}
										</Typography>
										<Typography variant='body2' sx={{ color: 'text.primary', wordBreak: 'break-word' }}>
											{item.value}
										</Typography>
									</Box>
								</Stack>
							))
						) : (
							<Typography variant='body2' color='text.secondary'>
								{t('doctor.ticket.pending_update')}
							</Typography>
						)}
					</Stack>

					<Box
						sx={(theme) => ({
							mt: 'auto',
							borderRadius: 3,
							px: 1.5,
							py: 1.2,
							bgcolor:
								theme.palette.mode === 'dark'
									? 'rgba(255,255,255,0.04)'
									: 'rgba(15, 23, 42, 0.03)',
							border: '1px solid',
							borderColor: theme.palette.mode === 'dark' ? 'rgba(255,255,255,0.08)' : 'rgba(15, 23, 42, 0.06)',
						})}
					>
						<Typography variant='caption' sx={{ color: 'text.disabled', display: 'block', mb: 0.5 }}>
							{t('doctor.ticket.footer_label')}
						</Typography>
						<Typography variant='body2' sx={{ fontWeight: 700 }}>
							{doctor.specialtyName || t('doctor.ticket.pending_update')}
						</Typography>
					</Box>
				</Stack>
			</Stack>
		</Card>
	)
}

export default DoctorCardSection
