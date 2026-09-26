import EmptyBox from '@/components/placeholders/EmptyBox'
import SkeletonBox from '@/components/skeletons/SkeletonBox'
import { Stack } from '@mui/material'
import OrderHistoryCardSection from './OrderHistoryCardSection'

const OrderHistoryListSection = ({
	orders = [],
	loading = false,
	onDetailClick,
	onCancelClick,
	onRepayClick,
}) => {
	if (loading) {
		return <SkeletonBox numberOfBoxes={4} rounded heights={[200]} />
	}

	if (orders.length === 0) {
		return <EmptyBox minHeight={300} />
	}

	return (
		<Stack spacing={2}>
			{orders.map((order) => (
				<OrderHistoryCardSection
					key={order.id}
					order={order}
					onDetailClick={onDetailClick}
					onCancelClick={onCancelClick}
					onRepayClick={onRepayClick}
				/>
			))}
		</Stack>
	)
}

export default OrderHistoryListSection
