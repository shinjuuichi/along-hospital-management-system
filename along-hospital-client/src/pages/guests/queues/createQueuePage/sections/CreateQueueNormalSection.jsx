import useTranslation from '@/hooks/useTranslation'
import {
	BadgeOutlined,
	ConfirmationNumberRounded,
	CreditCardRounded,
	InfoOutlined,
} from '@mui/icons-material'
import { Box, Divider, Paper, Stack, Typography, useTheme } from '@mui/material'
import { alpha } from '@mui/material/styles'

const CreateQueueNormalSection = ({ onCreateQueue }) => {
	const { t } = useTranslation()
	const theme = useTheme()

	const preparationItems = [
		{
			key: 'identity_card',
			icon: <BadgeOutlined fontSize='small' />,
			label: t('queue.guest.text.prepare_identity_card'),
		},
		{
			key: 'insurance_card',
			icon: <CreditCardRounded fontSize='small' />,
			label: t('queue.guest.text.prepare_health_insurance'),
		},
	]

	return (
		<Stack
			alignItems='center'
			spacing={{ xs: 2, md: 2.5 }}
			sx={{
				width: '100%',
				height: '100%',
				justifyContent: 'space-evenly',
				py: { xs: 0.5, md: 1.5 },
			}}
		>
			<Stack
				direction='row'
				alignItems='center'
				spacing={1}
				sx={{
					px: 2,
					py: 1,
					borderRadius: '999px',
					bgcolor: alpha(theme.palette.primary.main, 0.1),
					color: theme.palette.primary.main,
				}}
			>
				<InfoOutlined sx={{ fontSize: 20 }} />
				<Typography sx={{ fontWeight: 700, letterSpacing: '0.08em' }}>
					{t('queue.guest.title.badge')}
				</Typography>
			</Stack>

			<Typography
				component='h1'
				variant='h2'
				align='center'
				sx={{
					fontWeight: 800,
					fontSize: { xs: '1.9rem', sm: '2.4rem', md: 'clamp(2.4rem, 4.8vh, 3.2rem)' },
					lineHeight: 1.2,
					maxWidth: 920,
					color: theme.palette.text.primary,
				}}
			>
				{t('queue.guest.title.main')}
			</Typography>

			<Typography
				variant='body1'
				align='center'
				sx={{
					fontSize: { xs: '1rem', md: 'clamp(1rem, 2.1vh, 1.2rem)' },
					lineHeight: 1.65,
					color: theme.palette.text.secondary,
					maxWidth: 800,
				}}
			>
				{t('queue.guest.text.description')}
			</Typography>

			<Paper
				component='button'
				type='button'
				elevation={0}
				onClick={onCreateQueue}
				sx={{
					width: '100%',
					maxWidth: 760,
					border: 'none',
					cursor: 'pointer',
					borderRadius: { xs: 4, md: 5 },
					p: { xs: 2.5, sm: 3.5, md: 'clamp(1.8rem, 3.4vh, 4.6rem)' },
					color: theme.palette.common.white,
					background: `linear-gradient(160deg, ${theme.palette.primary.main} 0%, ${theme.palette.primary.dark} 100%)`,
					boxShadow: `0 20px 48px ${alpha(theme.palette.primary.main, 0.24)}`,
					transition: 'transform 220ms ease, box-shadow 220ms ease',
					'&:hover': {
						transform: 'translateY(-2px)',
						boxShadow: `0 26px 60px ${alpha(theme.palette.primary.main, 0.3)}`,
					},
					'&:active': {
						transform: 'translateY(0)',
					},
				}}
			>
				<Stack alignItems='center' spacing={{ xs: 2, md: 2.4 }}>
					<Box
						sx={{
							display: 'flex',
							alignItems: 'center',
							justifyContent: 'center',
							width: { xs: 80, md: 'clamp(88px, 11vh, 126px)' },
							height: { xs: 80, md: 'clamp(88px, 11vh, 126px)' },
							borderRadius: '50%',
							bgcolor: alpha(theme.palette.common.white, 0.17),
							border: `1px solid ${alpha(theme.palette.common.white, 0.22)}`,
						}}
					>
						<ConfirmationNumberRounded sx={{ fontSize: { xs: 38, md: 54 } }} />
					</Box>

					<Typography
						align='center'
						sx={{
							fontWeight: 800,
							fontSize: { xs: '1.8rem', md: 'clamp(2.1rem, 4.6vh, 2.8rem)' },
							lineHeight: 1.2,
							letterSpacing: '0.02em',
						}}
					>
						{t('queue.guest.button.take_new_number')}
					</Typography>

					<Typography
						align='center'
						sx={{
							fontWeight: 700,
							fontSize: { xs: '1.2rem', md: 'clamp(1.25rem, 2.8vh, 1.7rem)' },
							lineHeight: 1.25,
							letterSpacing: '0.08em',
							color: alpha(theme.palette.common.white, 0.84),
						}}
					>
						{t('queue.guest.button.new_registration')}
					</Typography>
				</Stack>
			</Paper>

			<Stack
				direction={{ xs: 'column', sm: 'row' }}
				alignItems='center'
				justifyContent='center'
				spacing={{ xs: 2, sm: 3 }}
				sx={{ width: '100%', pt: { xs: 1, sm: 1.5 } }}
			>
				{preparationItems.map((item, index) => (
					<Stack
						key={item.key}
						direction='row'
						alignItems='center'
						spacing={1}
						sx={{
							minWidth: { sm: 260 },
							justifyContent: 'center',
							color: theme.palette.primary.main,
						}}
					>
						<Box sx={{ display: 'inline-flex', alignItems: 'center' }}>{item.icon}</Box>
						<Typography
							sx={{
								fontWeight: 700,
								fontSize: { xs: '1rem', md: 'clamp(1rem, 1.95vh, 1.15rem)' },
								lineHeight: 1.25,
								letterSpacing: '0.01em',
								color: theme.palette.text.secondary,
							}}
						>
							{item.label}
						</Typography>
						{index < preparationItems.length - 1 && (
							<Divider
								orientation='vertical'
								flexItem
								sx={{
									mx: 2,
									display: { xs: 'none', sm: 'block' },
									borderColor: alpha(theme.palette.text.primary, 0.16),
								}}
							/>
						)}
					</Stack>
				))}
			</Stack>

			<Divider sx={{ width: '100%', borderColor: alpha(theme.palette.text.primary, 0.08) }} />
		</Stack>
	)
}

export default CreateQueueNormalSection
