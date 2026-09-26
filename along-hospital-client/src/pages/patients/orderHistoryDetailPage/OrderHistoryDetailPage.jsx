import { ApiUrls } from '@/configs/apiUrls'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import ArrowBackIcon from '@mui/icons-material/ArrowBack'
import { Box, Button, Container, Stack, Typography } from '@mui/material'
import { useNavigate, useParams } from 'react-router-dom'

import OrderDetailInfoSection from './sections/OrderDetailInfoSection'
import OrderDetailProductsSection from './sections/OrderDetailProductsSection'
import OrderDetailSummarySection from './sections/OrderDetailSummarySection'

export default function OrderDetailPage() {
	const { t } = useTranslation()
	const { id } = useParams()
	const navigate = useNavigate()
	const { data: order, loading } = useFetch(ApiUrls.ORDER_HISTORY.DETAIL(id))

	if (loading) return <Typography sx={{ p: 4 }}>{t('order_detail.common.loading')}</Typography>

	if (!order)
		return (
			<Box sx={{ p: 4 }}>
				<Button startIcon={<ArrowBackIcon />} onClick={() => navigate(-1)} sx={{ mb: 2 }}>
					{t('order_detail.common.back')}
				</Button>
				<Typography variant='h6'>{t('order_detail.detail.not_found')}</Typography>
			</Box>
		)

	return (
		<Container maxWidth='lg' sx={{ py: 4 }}>
			<Button startIcon={<ArrowBackIcon />} onClick={() => navigate(-1)} sx={{ mb: 3 }}>
				{t('order_detail.common.back_to_history')}
			</Button>

			<Box sx={{ mb: 3 }}>
				<Typography variant='h4' sx={{ fontWeight: 700 }}>
					{t('order_detail.detail.title')}
				</Typography>
				</Box>

			<Stack spacing={3}>
				<OrderDetailInfoSection order={order} />
				<OrderDetailProductsSection order={order} />
				<OrderDetailSummarySection order={order} />
			</Stack>
		</Container>
	)
}
