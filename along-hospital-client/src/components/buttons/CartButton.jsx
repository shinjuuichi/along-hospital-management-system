import useTranslation from '@/hooks/useTranslation'
import { ShoppingCartOutlined } from '@mui/icons-material'
import { Badge, IconButton, Tooltip } from '@mui/material'

const CartButton = ({ count = 0, onClick }) => {
	const { t } = useTranslation()
	return (
		<Tooltip title={t('tooltip.cart')}>
			<IconButton onClick={onClick}>
				<Badge
					badgeContent={count}
					color='error'
					max={99}
					showZero
					sx={{
						'& .MuiBadge-badge': {
							minWidth: count > 9 ? '33px' : '20px',
							padding: count > 9 ? '0 6px' : '0 4px',
						},
					}}
				>
					<ShoppingCartOutlined />
				</Badge>
			</IconButton>
		</Tooltip>
	)
}

export default CartButton
