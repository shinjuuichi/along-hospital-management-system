import useTranslation from '@/hooks/useTranslation'
import { Cached } from '@mui/icons-material'
import { Button } from '@mui/material'

/**
 * @typedef {Object} CustomProps
 * @property {function} props.onResetFilterClick
 */

/**
 * @param {import('@mui/material').ButtonProps & CustomProps} props
 */
const ResetFilterButton = ({ onResetFilterClick, loading = false, ...props }) => {
	const { t } = useTranslation()

	return (
		<Button
			onClick={onResetFilterClick}
			variant='outlined'
			color='secondary'
			disableElevation
			startIcon={<Cached />}
			disabled={loading}
			{...props}
		>
			{loading ? t('text.loading') : t('button.reset_filter')}
		</Button>
	)
}

export default ResetFilterButton
