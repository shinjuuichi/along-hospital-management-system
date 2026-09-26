import { Card, CardContent, Grid, Stack, Typography } from '@mui/material'
import { renderEmptyFallback } from '@/utils/handleStringUtil'

/**
 * DetailCard - A reusable component for displaying key-value pairs in a card layout
 *
 * @param {Array} fields - Array of field objects
 * @param {string} fields[].label - The label to display
 * @param {any} fields[].value - The value to display (or custom render result)
 * @param {function} [fields[].render] - Optional custom render function (value) => ReactNode
 * @param {number} [fields[].xs=12] - Grid xs size (default: 12)
 * @param {number} [fields[].md=6] - Grid md size (default: 6)
 * @param {boolean} [fields[].hidden] - Hide this field if true
 * @param {object} [sx] - Optional sx props for the Card
 *
 * @example
 * const fields = [
 *   { label: t('room.field.code'), value: room.code },
 *   { label: t('room.field.status'), value: room.status, render: (val) => <Chip label={val} /> },
 *   { label: t('room.field.description'), value: room.description, md: 12 },
 * ]
 * <DetailCard fields={fields} />
 */
const DetailCard = ({ fields = [], sx = {} }) => {
	return (
		<Card sx={sx}>
			<CardContent>
				<Grid container spacing={2}>
					{fields
						.filter((field) => !field.hidden)
						.map((field, index) => (
							<Grid key={index} size={{ xs: field.xs ?? 12, md: field.md ?? 6 }}>
								<Stack spacing={1}>
									<Typography variant='subtitle2' color='textSecondary'>
										{field.label}
									</Typography>
									{field.render ? (
										field.render(field.value)
									) : (
										<Typography variant='body1'>{renderEmptyFallback(field.value)}</Typography>
									)}
								</Stack>
							</Grid>
						))}
				</Grid>
			</CardContent>
		</Card>
	)
}

export default DetailCard
