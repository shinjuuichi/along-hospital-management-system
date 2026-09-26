import { defaultQueueStatusThemeColor } from '@/configs/defaultStylesConfig'
import { EnumConfig } from '@/configs/enumConfig'
import useEnum from '@/hooks/useEnum'
import useTranslation from '@/hooks/useTranslation'
import { getEnumLabelByValue } from '@/utils/handleStringUtil'
import { Paper, Stack, Typography, alpha, useTheme } from '@mui/material'

const QueueDetailManagementFooterSection = ({ summary = {} }) => {
	const theme = useTheme()
	const { t } = useTranslation()
	const _enum = useEnum()

	const summaryItems = [
		{
			key: 'total',
			label: t('text.total'),
			value: summary?.total ?? 0,
		},
		{
			key: EnumConfig.QueueStatus.Waiting,
			label: getEnumLabelByValue(_enum.queueStatusOptions, EnumConfig.QueueStatus.Waiting),
			value: summary?.waiting ?? 0,
		},
		{
			key: EnumConfig.QueueStatus.Called,
			label: getEnumLabelByValue(_enum.queueStatusOptions, EnumConfig.QueueStatus.Called),
			value: summary?.called ?? 0,
		},
		{
			key: EnumConfig.QueueStatus.InProgress,
			label: getEnumLabelByValue(_enum.queueStatusOptions, EnumConfig.QueueStatus.InProgress),
			value: summary?.inProgress ?? 0,
		},
		{
			key: EnumConfig.QueueStatus.AwaitingResults,
			label: getEnumLabelByValue(_enum.queueStatusOptions, EnumConfig.QueueStatus.AwaitingResults),
			value: summary?.awaitingResults ?? 0,
		},
		{
			key: EnumConfig.QueueStatus.Completed,
			label: getEnumLabelByValue(_enum.queueStatusOptions, EnumConfig.QueueStatus.Completed),
			value: summary?.completed ?? 0,
		},
		{
			key: EnumConfig.QueueStatus.Cancelled,
			label: getEnumLabelByValue(_enum.queueStatusOptions, EnumConfig.QueueStatus.Cancelled),
			value: summary?.cancelled ?? 0,
		},
	]

	return (
		<Paper
			elevation={0}
			sx={{
				px: { xs: 1, md: 1.5 },
				py: 0.85,
				borderRadius: 3,
				border: `1px solid ${alpha(theme.palette.primary.main, 0.2)}`,
				backgroundColor: alpha(theme.palette.primary.main, 0.03),
			}}
		>
			<Stack direction='row' spacing={0.9} alignItems='center' useFlexGap flexWrap='wrap'>
				{summaryItems.map((item) => (
					<Stack
						key={item.key}
						direction='row'
						spacing={0.55}
						alignItems='baseline'
						sx={{
							px: 0.8,
							py: 0.35,
							borderRadius: 1.2,
							bgcolor: alpha(theme.palette.background.paper, 0.9),
							border: `1px solid ${alpha(theme.palette.divider, 0.9)}`,
						}}
					>
						<Typography
							variant='caption'
							sx={{
								fontWeight: 700,
								fontSize: '0.68rem',
								textTransform: 'uppercase',
								letterSpacing: '0.05em',
								color: alpha(theme.palette.text.secondary, 0.9),
							}}
						>
							{item.label}
						</Typography>
						<Typography
							sx={{
								fontWeight: 800,
								fontSize: { xs: '0.95rem', md: '1.02rem' },
								lineHeight: 1,
								color: defaultQueueStatusThemeColor(theme, item.key) || theme.palette.text.primary,
							}}
						>
							{item.value}
						</Typography>
					</Stack>
				))}
			</Stack>
		</Paper>
	)
}

export default QueueDetailManagementFooterSection
