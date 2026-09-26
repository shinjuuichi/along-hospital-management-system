import useTranslation from '@/hooks/useTranslation'
import HomeIcon from '@mui/icons-material/Home'
import LocalHospitalIcon from '@mui/icons-material/LocalHospital'
import SearchOffIcon from '@mui/icons-material/SearchOff'
import { Box, Button, Container, Typography, useTheme } from '@mui/material'
import { useNavigate } from 'react-router-dom'

const NotFoundPage = () => {
	const theme = useTheme()
	const navigate = useNavigate()
	const { t } = useTranslation()

	return (
		<Box
			sx={{
				minHeight: '100vh',
				display: 'flex',
				alignItems: 'center',
				justifyContent: 'center',
				background:
					theme.palette.mode === 'light'
						? `linear-gradient(135deg, ${theme.palette.softBg} 0%, ${theme.palette.background.default} 100%)`
						: `linear-gradient(135deg, ${theme.palette.background.default} 0%, ${theme.palette.softBg} 100%)`,
				position: 'relative',
				overflow: 'hidden',
			}}
		>
			<Box
				sx={{
					position: 'absolute',
					top: '10%',
					left: '10%',
					opacity: 0.1,
					animation: 'float 6s ease-in-out infinite',
					'@keyframes float': {
						'0%, 100%': { transform: 'translateY(0px)' },
						'50%': { transform: 'translateY(-20px)' },
					},
				}}
			>
				<LocalHospitalIcon sx={{ fontSize: 120, color: theme.palette.primary.main }} />
			</Box>
			<Box
				sx={{
					position: 'absolute',
					bottom: '15%',
					right: '15%',
					opacity: 0.1,
					animation: 'float 8s ease-in-out infinite',
					animationDelay: '1s',
					'@keyframes float': {
						'0%, 100%': { transform: 'translateY(0px)' },
						'50%': { transform: 'translateY(-20px)' },
					},
				}}
			>
				<LocalHospitalIcon sx={{ fontSize: 100, color: theme.palette.primary.main }} />
			</Box>

			<Container maxWidth='md'>
				<Box
					sx={{
						textAlign: 'center',
						py: 8,
						px: 4,
					}}
				>
					<Box
						sx={{
							display: 'flex',
							justifyContent: 'center',
							mb: 3,
						}}
					>
						<Box
							sx={{
								position: 'relative',
								display: 'inline-flex',
							}}
						>
							<SearchOffIcon
								sx={{
									fontSize: 120,
									color: theme.palette.primary.main,
									opacity: 0.9,
								}}
							/>
						</Box>
					</Box>

					<Typography
						variant='h1'
						sx={{
							fontSize: { xs: '4rem', sm: '6rem', md: '8rem' },
							fontWeight: 700,
							color: theme.palette.primary.main,
							mb: 2,
							textShadow: `0 4px 20px ${theme.palette.primary.main}40`,
						}}
					>
						404
					</Typography>

					<Typography
						variant='h4'
						sx={{
							fontWeight: 600,
							color: theme.palette.text.primary,
							mb: 2,
							fontSize: { xs: '1.5rem', sm: '2rem' },
						}}
					>
						{t('not_found.title')}
					</Typography>

					<Typography
						variant='body1'
						sx={{
							color: theme.palette.text.secondary,
							mb: 5,
							maxWidth: 600,
							mx: 'auto',
							fontSize: { xs: '0.95rem', sm: '1.1rem' },
							lineHeight: 1.7,
						}}
					>
						{t('not_found.description')}
					</Typography>

					<Box
						sx={{
							display: 'flex',
							gap: 2,
							justifyContent: 'center',
							flexWrap: 'wrap',
						}}
					>
						<Button
							variant='contained'
							size='large'
							startIcon={<HomeIcon />}
							onClick={() => navigate('/')}
							sx={{
								px: 4,
								py: 1.5,
								borderRadius: 3,
								textTransform: 'none',
								fontSize: '1rem',
								fontWeight: 600,
								background: `linear-gradient(135deg, ${theme.palette.primary.main} 0%, ${theme.palette.primary.dark} 100%)`,
								boxShadow: `0 4px 14px ${theme.palette.primary.main}40`,
								'&:hover': {
									boxShadow: `0 6px 20px ${theme.palette.primary.main}60`,
									transform: 'translateY(-2px)',
									transition: 'all 0.3s ease',
								},
							}}
						>
							{t('not_found.button.back_home')}
						</Button>
					</Box>
				</Box>
			</Container>
		</Box>
	)
}

export default NotFoundPage
