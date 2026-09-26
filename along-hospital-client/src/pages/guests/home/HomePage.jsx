import { Box } from '@mui/material'
import FeaturedDoctorsSection from '@/pages/guests/home/sections/FeaturedDoctorsSection'
import FeaturedServicesSection from '@/pages/guests/home/sections/FeaturedServicesSection'
import HeroSection from '@/pages/guests/home/sections/HeroSection'
import MedicineSection from '@/pages/guests/home/sections/MedicineSection'
import NewsSection from '@/pages/guests/home/sections/NewsSection'
import VoucherPromoSection from '@/pages/guests/home/sections/VoucherPromoSection'

const HomePage = () => {
	return (
		<Box sx={{ bgcolor: 'background.default', overflow: 'hidden' }}>
			<HeroSection />
			<FeaturedServicesSection />
			<FeaturedDoctorsSection />
			<VoucherPromoSection />
			<MedicineSection />
			<NewsSection />
		</Box>
	)
}

export default HomePage
