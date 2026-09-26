import GenericTable from '@/components/tables/GenericTable'
import { defaultFeedbackReplyStatusStyle } from '@/configs/defaultStylesConfig'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { getEnumLabelByValue, renderEmptyFallback } from '@/utils/handleStringUtil'
import VisibilityIcon from '@mui/icons-material/Visibility'
import { Chip, IconButton, Rating, Tooltip } from '@mui/material'
import { useMemo } from 'react'

const FeedbackManagementTableSection = ({
	feedbacks,
	loading,
	sort,
	setSort,
	onViewDetail = () => {},
}) => {
	const { t } = useTranslation()
	const _enum = useEnum()

	const fields = useMemo(
		() => [
			{ key: 'id', title: t('feedback.field.id'), width: 10, sortable: true, fixedColumn: true },
			{
				key: 'content',
				title: t('feedback.field.content'),
				width: 35,
				sortable: false,
				render: (value) => renderEmptyFallback(value),
			},
			{
				key: 'rating',
				title: t('feedback.field.rating'),
				width: 20,
				sortable: true,
				render: (value) => <Rating value={value} readOnly size='small' />,
			},
			{
				key: 'feedbackReplyStatus',
				title: t('feedback.field.reply_status'),
				width: 20,
				sortable: false,
				render: (value) => (
					<Chip
						label={getEnumLabelByValue(_enum.feedbackReplyStatusOptions, value)}
						color={defaultFeedbackReplyStatusStyle(value)}
						size='small'
						variant='outlined'
					/>
				),
			},
			{
				key: '',
				title: '',
				width: 5,
				render: (value, row) => (
					<Tooltip title={t('button.detail')}>
						<IconButton size='small' color='primary' onClick={() => onViewDetail(row)}>
							<VisibilityIcon fontSize='small' />
						</IconButton>
					</Tooltip>
				),
			},
		],
		[t, onViewDetail, _enum]
	)

	return (
		<GenericTable
			data={feedbacks}
			fields={fields}
			sort={sort}
			setSort={setSort}
			rowKey='id'
			loading={loading}
		/>
	)
}

export default FeedbackManagementTableSection
