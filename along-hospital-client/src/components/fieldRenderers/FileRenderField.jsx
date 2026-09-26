import { getObjectValueFromStringPath } from '@/utils/handleObjectUtil'
import useTranslation from '@/hooks/useTranslation'
import { Box, Stack, Typography } from '@mui/material'
import AddFileTileRenderField from './AddFileTileRenderField'
import FileTileRenderField from './FileTileRenderField'

const FileRenderField = ({ field, setField, showError, values }) => {
	const { t } = useTranslation()

	const file = getObjectValueFromStringPath(values, field.key)
	const required = field.required ?? true
	const accept = field.accept

	const handleFileChange = (e) => {
		const f = e?.target?.files?.[0] || null
		setField(field.key, f)
		e.target.value = ''
	}

	const handleRemove = () => {
		setField(field.key, null)
	}

	const inputId = `${field.key}__picker`

	return (
		<Box>
			<Stack spacing={1.25}>
				<Typography variant='subtitle2'>
					{field.title}{' '}
					{required && (
						<Typography component='span' color='error'>
							*
						</Typography>
					)}
				</Typography>
				<Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 1.5, alignItems: 'flex-start' }}>
					{file && <FileTileRenderField fileName={file.name} onRemove={handleRemove} />}
					{!file && <AddFileTileRenderField inputId={inputId} />}
				</Box>
				{showError && required && !file && (
					<Typography variant='caption' color='error'>
						{t('error.required')}
					</Typography>
				)}
				<input
					id={inputId}
					type='file'
					accept={accept}
					style={{ display: 'none' }}
					onChange={handleFileChange}
				/>
			</Stack>
		</Box>
	)
}

export default FileRenderField
