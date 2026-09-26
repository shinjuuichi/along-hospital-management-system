import useTranslation from '@/hooks/useTranslation'
import { FilterAlt } from '@mui/icons-material'
import { Button } from '@mui/material'

/**
 * @typedef {Object} CustomProps
 * @property {function} props.onFilterClick
 */

/**
 * @param {import('@mui/material').ButtonProps & CustomProps} props
 */
const FilterButton = ({ onFilterClick, loading = false, ...props }) => {
	const { t } = useTranslation()

	return (
		<Button
			onClick={onFilterClick}
			variant='contained'
			color='primary'
			disableElevation
			startIcon={<FilterAlt />}
			disabled={loading}
			{...props}
		>
			{loading ? t('text.loading') : t('button.filter')}
		</Button>
	)
}

export default FilterButton
