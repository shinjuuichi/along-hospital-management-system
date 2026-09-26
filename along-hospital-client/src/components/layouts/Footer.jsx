import { Box, Container, Divider, Grid } from '@mui/material'
import { alpha, useTheme } from '@mui/material/styles'
import { useNavigate } from 'react-router-dom'
import FooterBottom from './footers/FooterBottom'
import FooterSection from './footers/FooterSection'

export default function Footer({ sections = [] }) {
	const theme = useTheme()
	const navigate = useNavigate()

	return (
		<Box
			component='footer'
			role='contentinfo'
			sx={{
				background: `${theme.palette.gradients.brand_reverse_45deg}`,
				color: 'primary.contrastText',
				borderTop: `1px solid ${alpha(theme.palette.primary.contrastText, 0.16)}`,
			}}
		>
			<Container maxWidth='lg' sx={{ py: { xs: 3, sm: 4 } }}>
				<Grid container spacing={{ xs: 2, sm: 3, md: 4 }} justifyContent={'center'}>
					{sections.map((sec, idx) => (
						<Grid key={sec.title + idx} size={{ xs: 6, md: 3 }}>
							<FooterSection title={sec.title} links={sec.links} onNavigate={(url) => navigate(url)} />
						</Grid>
					))}
				</Grid>
				<Divider
					aria-hidden='true'
					sx={{
						borderColor: alpha(theme.palette.primary.contrastText, 0.16),
						my: 2,
					}}
				/>
				<FooterBottom />
			</Container>
		</Box>
	)
}

/*
VÍ DỤ DỮ LIỆU & CÁCH DÙNG:
---------------------------------

// const sections = [
//   { title: 'Medical', links: [
//       { label: 'Services', url: '/services' },
//       { label: 'Doctors', url: '/doctors' },
//       { label: 'Specialties', url: '/specialties' },
//   ]},
//   { title: 'Resources', links: [
//       { label: 'Blogs', url: '/blogs' },
//       { label: 'Medicines', url: '/medicines' },
//   ]},
//   { title: 'Support', links: [
//       { label: 'Contact Us', url: '/contact' },
//       { label: 'FAQ', url: '/faq' },
//   ]},
//   { title: 'About', links: [
//       { label: 'Our Hospital', url: '/about' },
//       { label: 'Careers', url: '/careers' },
//   ]},
// ];
// <Footer sections={sections} />
*/
