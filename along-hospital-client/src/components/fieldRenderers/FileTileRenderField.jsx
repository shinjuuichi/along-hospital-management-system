import useTranslation from '@/hooks/useTranslation'
import { Close } from '@mui/icons-material'
import { Box, IconButton, Tooltip, Typography } from '@mui/material'

const FileTileRenderField = ({ fileName, onRemove }) => {
	const { t } = useTranslation()

	return (
		<Box
			sx={{
				position: 'relative',
				flex: '1 1 max(50%, 160px)',
				maxWidth: 160,
				aspectRatio: 1,
				borderRadius: 2,
				overflow: 'hidden',
				boxShadow: 1,
				alignItems: 'center',
				display: 'flex',
				flexDirection: 'column',
				justifyContent: 'center',
				bgcolor: 'error.lighter',
			}}
		>
			<Typography
				variant='caption'
				sx={{
					color: 'error.main',
					fontWeight: 600,
					px: 1,
					textAlign: 'center',
					wordBreak: 'break-word',
					lineHeight: 1.2,
				}}
			>
				{fileName}
			</Typography>
			<Tooltip title={t('tooltip.remove')}>
				<IconButton
					size='small'
					onClick={onRemove}
					aria-label={t('tooltip.remove')}
					sx={{
						position: 'absolute',
						top: 4,
						right: 4,
						bgcolor: 'background.paper',
						boxShadow: 1,
						'&:hover': { bgcolor: 'background.paper' },
					}}
				>
					<Close fontSize='small' />
				</IconButton>
			</Tooltip>
		</Box>
	)
}

export default FileTileRenderField