import SwitchLanguageButton from '@/components/buttons/SwitchLanguageButton'
import SwitchThemeButton from '@/components/buttons/SwitchThemeButton'
import { routeUrls } from '@/configs/routeUrls'
import useTranslation from '@/hooks/useTranslation'
import { Box, Container, Stack, Typography, useMediaQuery, useTheme } from '@mui/material'
import { useNavigate } from 'react-router-dom'
import SystemLogoAndName from './commons/SystemLogoAndName'

const AuthLayout = ({ children, showHero = true }) => {
	const theme = useTheme()
	const isNarrow = useMediaQuery('(max-width:500px)')
	const { t } = useTranslation()
	const navigate = useNavigate()

	return (
		<Box
			sx={{
				minHeight: '100vh',
				display: 'flex',
				bgcolor: 'background.default',
				flexDirection: { xs: 'column', lg: 'row' },
			}}
		>
			{showHero && (
				<Box
					sx={{
						display: { xs: 'none', md: 'flex' },
						width: { xs: '100%', md: '100%', lg: '50%' },
						minHeight: { xs: '200px', md: '300px', lg: 'auto' },
						backgroundImage: `url(${window.location.origin}/auth-hero-bg.jpg)`,
						backgroundSize: 'cover',
						backgroundPosition: 'center',
						backgroundRepeat: 'no-repeat',
						flexDirection: 'column',
						alignItems: 'center',
						justifyContent: 'center',
						p: { xs: 2, sm: 4, md: 6 },
						position: 'relative',
						overflow: 'hidden',
					}}
				>
					<Box sx={{ position: 'relative', zIndex: 10, textAlign: 'center', color: 'white' }}>
						<Typography
							variant='h2'
							sx={{
								fontWeight: 700,
								mb: { xs: 2, md: 3 },
								fontSize: { xs: '2rem', sm: '2.5rem', md: '3rem' },
							}}
						>
							{t('auth.hero.title')}
						</Typography>
						<Typography
							variant='h6'
							sx={{
								maxWidth: 500,
								opacity: 0.9,
								lineHeight: 1.6,
								fontSize: { xs: '0.9rem', sm: '1rem', md: '1.25rem' },
								px: { xs: 2, sm: 0 },
							}}
						>
							{t('auth.hero.subtitle')}
						</Typography>
					</Box>
				</Box>
			)}

			<Box
				sx={{
					flex: 1,
					display: 'flex',
					flexDirection: 'column',
					alignItems: 'center',
					justifyContent: 'center',
					p: { xs: 1, sm: 3, md: 4 },
					position: 'relative',
					minHeight: { xs: 'calc(100vh - 200px)', md: 'calc(100vh - 300px)', lg: '100vh' },
				}}
			>
				<Stack
					direction='row'
					spacing={1.5}
					sx={{
						position: 'absolute',
						top: { xs: 8, sm: 16, md: 24 },
						right: { xs: 8, sm: 16, md: 24 },
						zIndex: 10,
					}}
				>
					<SwitchLanguageButton />
					<SwitchThemeButton />
				</Stack>

				<Container
					maxWidth={isNarrow ? false : 'sm'}
					disableGutters={isNarrow}
					sx={{
						width: '100%',
						px: { xs: 0.5, sm: 2 },
						boxSizing: 'border-box',
					}}
				>
					<Box sx={{ mb: { xs: 1.5, sm: 3, md: 4 } }}>
						<Stack direction='row' spacing={{ xs: 1, sm: 2 }} alignItems='center' sx={{ mb: 2 }}>
							<SystemLogoAndName onClick={() => navigate(routeUrls.HOME.INDEX)} onlyShowIcon={false} />
						</Stack>
					</Box>

					<Box
						sx={{
							bgcolor: 'background.paper',
							border: 1,
							borderColor: 'divider',
							borderRadius: { xs: 2, sm: 4 },
							boxShadow: theme.shadows[3],
							width: '100%',
							maxWidth: isNarrow ? 'min(360px, calc(100vw - 16px))' : '100%',
							mx: 'auto',
							p: { xs: 1.25, sm: 3, md: 4 },
							minWidth: 0,
							wordBreak: 'break-word',
							...(isNarrow
								? {
										'& .MuiButton-root': { width: '100%' },
										'& .MuiTextField-root': { width: '100%' },
								  }
								: {}),
						}}
					>
						{children}
					</Box>
				</Container>
			</Box>
		</Box>
	)
}

export default AuthLayout
