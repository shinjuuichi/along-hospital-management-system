import { ApiUrls } from '@/configs/apiUrls'
import { EnumConfig } from '@/configs/enumConfig'
import useAuth from '@/hooks/useAuth'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useFetch from '@/hooks/useFetch'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import FeedbackSection from '@/pages/patients/feedbackPage/sections/FeedbackSection'
import { setCartStore } from '@/redux/reducers/patientReducer'
import { Box, CircularProgress, Container, Grid, Typography } from '@mui/material'
import { useEffect, useState } from 'react'
import { useParams } from 'react-router-dom'
import MedicineDetailImageSection from './sections/MedicineDetailImageSection'
import MedicineDetailInfoSection from './sections/MedicineDetailInfoSection'

export default function MedicineDetailPage() {
	const { t } = useTranslation()
	const { id } = useParams()
	const { auth } = useAuth()

	const { data: medicine, loading, error } = useFetch(ApiUrls.MEDICINE.GET_BY_ID(id))

	const isPatientRole = auth?.role === EnumConfig.Role.Patient || false
	const cartStore = useReduxStore({
		selector: (state) => state.patient.cart,
		setStore: isPatientRole ? setCartStore : null,
	})

	const [currentImage, setCurrentImage] = useState(null)
	const [quantity, setQuantity] = useState(1)

	useEffect(() => {
		if (medicine?.images?.length) setCurrentImage(medicine.images[0])
	}, [medicine])

	const { loading: addingToCart, submit: addToCartApi } = useAxiosSubmit({
		url: ApiUrls.CART.ADD_TO_CART,
		method: 'POST',
		onSuccess: async () => {
			if (isPatientRole) {
				await cartStore.fetch()
			}
		},
	})

	const handleAddToCart = (skuCode) => {
		if (skuCode) {
			addToCartApi({ overrideData: { skuCode, quantity } })
		}
	}

	return (
		<Container maxWidth='lg' sx={{ py: 6 }}>
			<Box>
				{loading ? (
					<Box display='flex' justifyContent='center' alignItems='center' minHeight='60vh'>
						<CircularProgress />
					</Box>
				) : medicine?.id ? (
					<Grid container spacing={6} alignItems='flex-start'>
						<Grid size={{ xs: 12, md: 5 }}>
							<MedicineDetailImageSection
								medicine={medicine}
								currentImage={currentImage}
								setCurrentImage={setCurrentImage}
							/>
						</Grid>
						<Grid size={{ xs: 12, md: 5 }}>
							<MedicineDetailInfoSection
								medicine={medicine}
								quantity={quantity}
								setQuantity={setQuantity}
								onAddToCart={handleAddToCart}
								loading={addingToCart}
							/>
						</Grid>
						<FeedbackSection medicineId={medicine.id}></FeedbackSection>
					</Grid>
				) : (
					<Box
						sx={{
							p: 3,
							textAlign: 'center',
							bgcolor: 'background.paper',
							borderRadius: 1,
							minHeight: '40vh',
						}}
					>
						<Typography color='text.secondary'>
							{error ? t('common.error.loading_data') : t('common.error.not_found')}
						</Typography>
					</Box>
				)}
			</Box>
		</Container>
	)
}
