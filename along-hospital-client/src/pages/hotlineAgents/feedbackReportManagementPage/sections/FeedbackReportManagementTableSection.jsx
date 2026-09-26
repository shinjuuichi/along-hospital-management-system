import GenericTable from '@/components/tables/GenericTable'
import { defaultFeedbackReportStatusStyle } from '@/configs/defaultStylesConfig'
import { EnumConfig } from '@/configs/enumConfig'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { getEnumLabelByValue } from '@/utils/handleStringUtil'
import { Button, Chip, Stack } from '@mui/material'

const FeedbackReportManagementTableSection = ({
	reports = [],
	loading,
	sort,
	setSort,
	onResolve,
	onReject,
	onViewReason,
}) => {
	const { t } = useTranslation()
	const _enum = useEnum()

	const fields = [
		{ key: 'id', title: t('feedback_report.field.id'), width: 10, sortable: true, fixedColumn: true },
		{
			key: 'status',
			title: t('feedback_report.field.status'),
			width: 20,
			render: (val) => {
				const status = val || ''
				const color = defaultFeedbackReportStatusStyle(status)
				const label = getEnumLabelByValue(_enum.feedbackReportStatusEnum, status)
				return <Chip size='small' label={label} color={color} variant='filled' />
			},
		},
		{
			key: 'reason',
			title: t('feedback_report.field.reason'),
			width: 20,
			render: (_, row) => (
				<Button size='small' variant='text' onClick={() => onViewReason?.(row)}>
					{t('feedback_report.button.view_reason')}
				</Button>
			),
		},
		{
			key: 'actions',
			title: t('feedback_report.field.actions'),
			width: 25,
			render: (_, row) => {
				return (
					<Stack direction='row' spacing={1}>
						<Button
							variant='contained'
							size='small'
							color='success'
							disabled={loading || row.status !== EnumConfig.FeedbackReportStatus.Pending}
							onClick={() => onResolve(row)}
						>
							{t('feedback_report.button.resolve')}
						</Button>
						<Button
							variant='outlined'
							size='small'
							color='error'
							disabled={loading || row.status !== EnumConfig.FeedbackReportStatus.Pending}
							onClick={() => onReject(row)}
						>
							{t('feedback_report.button.reject')}
						</Button>
					</Stack>
				)
			},
		},
	]

	return (
		<GenericTable
			data={reports}
			fields={fields}
			loading={loading}
			sort={sort}
			setSort={setSort}
			rowKey='id'
			stickyHeader
		/>
	)
}

export default FeedbackReportManagementTableSection
