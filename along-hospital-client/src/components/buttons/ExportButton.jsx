import useTranslation from '@/hooks/useTranslation'
import { FileDownload } from '@mui/icons-material'
import { Button } from '@mui/material'

const ExportButton = ({
	onExportClick,
	loading = false,
	translationPrefix = 'statistic_dashboard_manager',
	...props
}) => {
	const { t } = useTranslation()

	return (
		<Button
			onClick={onExportClick}
			variant='contained'
			color='primary'
			disableElevation
			startIcon={<FileDownload />}
			disabled={loading}
			{...props}
		>
			{loading ? t('text.loading') : t(`${translationPrefix}.button.export`)}
		</Button>
	)
}

export default ExportButton
