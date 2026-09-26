import { routeUrls } from '@/configs/routeUrls'
import useTranslation from '@/hooks/useTranslation'
import {
	aboutGoalItems,
	aboutHeroBadgeItems,
	aboutTeamMembers,
} from '@/pages/guests/aboutUsPage/helpers/aboutUsData'
import AboutUsCTASection from '@/pages/guests/aboutUsPage/sections/AboutUsCTASection'
import AboutUsGoalsSection from '@/pages/guests/aboutUsPage/sections/AboutUsGoalsSection'
import AboutUsHeroSection from '@/pages/guests/aboutUsPage/sections/AboutUsHeroSection'
import AboutUsTeamSection from '@/pages/guests/aboutUsPage/sections/AboutUsTeamSection'
import { Box } from '@mui/material'
import { useReducedMotion } from 'framer-motion'
import { useNavigate } from 'react-router-dom'

const AboutUsPage = () => {
	const { t } = useTranslation()
	const navigate = useNavigate()
	const shouldReduceMotion = useReducedMotion()

	const heroBadges = aboutHeroBadgeItems.map((item) => ({
		...item,
		label: t(`about_us.hero.badges.${item.key}`),
	}))

	const goalCards = aboutGoalItems.map((item) => ({
		...item,
		title: t(`about_us.goals.items.${item.key}.title`),
		description: t(`about_us.goals.items.${item.key}.text`),
	}))

	const teamMembers = aboutTeamMembers.map((item) => ({
		...item,
		name: t(`about_us.team.members.${item.key}.name`),
		role: t(`about_us.team.members.${item.key}.role`),
		experience: t(`about_us.team.members.${item.key}.experience`),
		bio: t(`about_us.team.members.${item.key}.bio`),
	}))

	const handleBookAppointment = () => {
		navigate(routeUrls.BASE_ROUTE.PATIENT(routeUrls.PATIENT.APPOINTMENT.CREATE))
	}

	const handleOrderMedicine = () => {
		navigate(routeUrls.HOME.MEDICINE)
	}

	return (
		<Box
			component='main'
			sx={{
				display: 'grid',
				gap: { xs: 6, md: 8 },
			}}
		>
			<AboutUsHeroSection
				badges={heroBadges}
				shouldReduceMotion={shouldReduceMotion}
				onBookAppointment={handleBookAppointment}
				onOrderMedicine={handleOrderMedicine}
			/>
			<AboutUsGoalsSection items={goalCards} shouldReduceMotion={shouldReduceMotion} />
			<AboutUsTeamSection members={teamMembers} shouldReduceMotion={shouldReduceMotion} />
			<AboutUsCTASection
				shouldReduceMotion={shouldReduceMotion}
				onBookAppointment={handleBookAppointment}
				onOrderMedicine={handleOrderMedicine}
			/>
		</Box>
	)
}

export default AboutUsPage
