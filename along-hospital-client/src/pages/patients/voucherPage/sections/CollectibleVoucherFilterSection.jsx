import FilterButton from '@/components/buttons/FilterButton'
import ResetFilterButton from '@/components/buttons/ResetFilterButton'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import { FilterList } from '@mui/icons-material'
import { Box, Stack, Typography, useTheme } from '@mui/material'

const DRAWER_WIDTH = 280

const CollectibleVoucherFilterSection = ({ filters, onFilterChange }) => {
	const theme = useTheme()
	const { t } = useTranslation()

	const { values, handleChange, setField, registerRef } = useForm(filters)
	const { renderField } = useFieldRenderer(
		values,
		setField,
		handleChange,
		registerRef,
		false,
		'outlined',
		'small'
	)

	const fields = [
		{
			key: 'name',
			title: t('voucher.placeholder.search_voucher'),
			type: 'search',
			required: false,
		},
	]

	const handleApplyFilter = () => {
		onFilterChange(values)
	}

	const handleReset = () => {
		onFilterChange({ name: '' })
	}

	return (
		<Box
			sx={{
				width: { xs: '100%', md: DRAWER_WIDTH },
				flexShrink: 0,
				bgcolor: 'background.paper',
				borderRadius: 2,
				p: 3,
				border: `1px solid ${theme.palette.divider}`,
				height: 'fit-content',
				position: { xs: 'relative', md: 'sticky' },
				top: { xs: 0, md: 16 },
			}}
		>
			<Stack spacing={3} sx={{ height: '100%' }}>
				<Stack direction='row' alignItems='center' justifyContent='space-between'>
					<Stack direction='row' alignItems='center' spacing={1}>
						<FilterList color='primary' />
						<Typography variant='h6' fontWeight={600}>
							{t('voucher.title.filters')}
						</Typography>
					</Stack>
				</Stack>

				{fields.map((field) => (
					<Box key={field.key}>{renderField(field)}</Box>
				))}

				<Stack spacing={1.5} sx={{ mt: 'auto' }}>
					<FilterButton onFilterClick={handleApplyFilter} fullWidth />
					<ResetFilterButton onResetFilterClick={handleReset} fullWidth />
				</Stack>
			</Stack>
		</Box>
	)
}

export default CollectibleVoucherFilterSection
