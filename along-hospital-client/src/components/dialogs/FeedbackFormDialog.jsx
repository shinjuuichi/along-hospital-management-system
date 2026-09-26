import GenericFormDialog from '@/components/dialogs/commons/GenericFormDialog'
import useTranslation from '@/hooks/useTranslation'
import { maxLen, numberRange } from '@/utils/validateUtil'
import { Box, Rating, Typography } from '@mui/material'
import { useMemo, useState } from 'react'

const StarRatingInput = ({ value, onChange, ratingLabels }) => {
	const [hover, setHover] = useState(-1)
	const show = hover !== -1 ? hover : Number(value) || 0
	return (
		<Box
			sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', gap: 0.5, width: '100%' }}
		>
			<Rating
				name='rating'
				value={Number(value) || 0}
				max={5}
				precision={1}
				size='large'
				sx={{ '& .MuiRating-icon': { fontSize: 34 } }}
				onChange={(_, newValue) => onChange(newValue)}
				onChangeActive={(_, newHover) => setHover(newHover)}
			/>
			<Typography variant='body2' color='text.secondary' align='center'>
				{ratingLabels[show] || ''}
			</Typography>
		</Box>
	)
}

const FeedbackFormDialog = ({
	open,
	onClose,
	initialValues = { rating: 5, content: '' },
	onSubmit,
	submitLabel,
	title,
	submitButtonColor = 'success',
}) => {
	const { t } = useTranslation()

	const ratingLabels = useMemo(
		() => ({
			1: t('feedback.rating_label.1'),
			2: t('feedback.rating_label.2'),
			3: t('feedback.rating_label.3'),
			4: t('feedback.rating_label.4'),
			5: t('feedback.rating_label.5'),
		}),
		[t]
	)

	const fields = [
		{
			key: 'rating',
			title: t('feedback.field.rating'),
			type: 'custom',
			validate: [numberRange(1, 5)],
			required: true,
			render: ({ value, onChange }) => (
				<StarRatingInput value={value} onChange={onChange} ratingLabels={ratingLabels} />
			),
		},
		{
			key: 'content',
			title: t('feedback.field.content'),
			validate: [maxLen(1000)],
			required: false,
			props: { placeholder: t('feedback.placeholder.content') },
		},
	]

	return (
		<GenericFormDialog
			open={open}
			onClose={onClose}
			fields={fields}
			initialValues={initialValues}
			submitLabel={submitLabel}
			submitButtonColor={submitButtonColor}
			title={title}
			onSubmit={onSubmit}
		/>
	)
}

export default FeedbackFormDialog
