import useTranslation from '@/hooks/useTranslation'
import { Add } from '@mui/icons-material'
import { Box, Stack, Typography } from '@mui/material'

const AddFileTileRenderField = ({ inputId }) => {
	const { t } = useTranslation()
	return (
		<label htmlFor={inputId} style={{ display: 'contents' }}>
			<Box
				sx={{
					cursor: 'pointer',
					flex: '1 1 max(50%, 160px)',
					maxWidth: 160,
					aspectRatio: 1,
					borderRadius: 2,
					boxShadow: 1,
					border: '1px dashed',
					borderColor: 'divider',
					display: 'flex',
					alignItems: 'center',
					justifyContent: 'center',
					transition: 'background-color .15s ease',
					'&:hover': { bgcolor: 'action.hover' },
				}}
			>
				<Stack alignItems='center' spacing={0.5}>
					<Add />
					<Typography variant='caption'>{t('button.add_file')}</Typography>
				</Stack>
			</Box>
		</label>
	)
}

export default AddFileTileRenderField