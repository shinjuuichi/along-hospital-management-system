import useTranslation from '@/hooks/useTranslation'
import { Box, TextField, Typography } from '@mui/material'

const ImageRenderField = ({ field, textFieldVariant, setField, showError, preview }) => {
	const { t } = useTranslation()
	return (
		<Box key={field.key}>
			<TextField
				name={field.key}
				variant={textFieldVariant}
				label={field.title}
				type='file'
				value={undefined}
				onChange={(e) => {
					const f = e?.target?.files?.[0] || null
					setField(field.key, f)
				}}
				required={field.required ?? true}
				error={showError}
				helperText={showError ? t('error.required') : ''}
				fullWidth
				slotProps={{
					input: { inputProps: { accept: 'image/*' } },
					inputLabel: { shrink: true },
				}}
			/>
			{preview && (
				<Box sx={{ mt: 1.5 }}>
					<Typography variant='caption' sx={{ display: 'block', mb: 0.5 }}>
						{t('text.image_preview')}
					</Typography>
					<Box
						component='img'
						src={preview}
						alt={field.title}
						sx={{ width: '100%', maxHeight: 240, objectFit: 'cover', borderRadius: 2, boxShadow: 1 }}
						onLoad={() => URL.revokeObjectURL(preview)}
						onError={(e) => {
							e.currentTarget.onerror = null
							e.currentTarget.src = '/placeholder-image.png'
						}}
					/>
				</Box>
			)}
		</Box>
	)
}

export default ImageRenderField
