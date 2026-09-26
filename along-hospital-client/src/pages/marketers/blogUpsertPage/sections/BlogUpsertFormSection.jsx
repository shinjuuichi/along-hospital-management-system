import TiptapEditor from '@/components/editors/TiptapEditor/TiptapEditor'
import useTranslation from '@/hooks/useTranslation'
import { Delete, Save } from '@mui/icons-material'
import {
	Box,
	Button,
	Card,
	CardContent,
	CircularProgress,
	FormControl,
	FormHelperText,
	IconButton,
	InputLabel,
	MenuItem,
	Select,
	Stack,
	TextField,
	Typography,
} from '@mui/material'

const BlogUpsertFormSection = ({
	isEditMode,
	detailLoading,
	onSubmit,
	formData,
	errors,
	onInputChange,
	onContentChange,
	onContentImageUpload,
	onImageChange,
	onRemoveImage,
	imagePreview,
	submitButtonLabel,
	isFormDisabled,
	onBack,
	isSubmitting,
	canRemoveImage,
	categories,
	categoriesLoading,
}) => {
	const { t } = useTranslation()

	return (
		<Card>
			<CardContent>
				{isEditMode && detailLoading && (
					<Stack direction='row' spacing={1.5} alignItems='center' sx={{ mb: 2 }}>
						<CircularProgress size={20} />
						<Typography variant='body2' color='text.secondary'>
							{t('text.loading')}
						</Typography>
					</Stack>
				)}

				<Box
					component='form'
					onSubmit={onSubmit}
					sx={{ display: 'flex', flexDirection: 'column', gap: 3 }}
				>
					<TextField
						label={t('blog.field.title')}
						value={formData.title || ''}
						onChange={(e) => onInputChange('title', e.target.value)}
						error={!!errors.title}
						helperText={errors.title}
						fullWidth
						required
						disabled={isFormDisabled}
					/>

				<FormControl fullWidth error={!!errors.blogCategoryId} required disabled={isFormDisabled}>
					<InputLabel>{t('blog.field.category')}</InputLabel>
					<Select
						value={String(formData.blogCategoryId ?? '')}
						onChange={(e) => onInputChange('blogCategoryId', e.target.value)}
						label={t('blog.field.category')}
						disabled={isFormDisabled || categoriesLoading}
					>
						{categoriesLoading || !Array.isArray(categories) ? (
							<MenuItem value=''>
								{t('text.loading')}
							</MenuItem>
						) : (
							[
								<MenuItem key='empty' value='' disabled>
									{t('blog.placeholder.select_category')}
								</MenuItem>,
								...(categories || []).map((category) => (
									<MenuItem key={category.id} value={String(category.id)}>
										{category.name}
									</MenuItem>
								))
							]
						)}
					</Select>
					{errors.blogCategoryId && <FormHelperText>{errors.blogCategoryId}</FormHelperText>}
				</FormControl>

				<Box>
					<Typography variant='body2' color='text.secondary' sx={{ mb: 1 }}>
						{t('blog.field.content')} *
					</Typography>
					<TiptapEditor
						content={formData.content || ''}
						onChange={onContentChange}
						onImageUpload={onContentImageUpload}
						placeholder={t('blog.placeholder.enter_content')}
						error={!!errors.content}
						minHeight={400}
						editable={!isFormDisabled}
					/>
					{errors.content && (
						<FormHelperText error sx={{ mt: 1 }}>
							{errors.content}
						</FormHelperText>
					)}
				</Box>

					<Box>
						<Typography variant='body2' color='text.secondary' sx={{ mb: 1 }}>
							{t('blog.field.cover_image')}
						</Typography>
						<input
							accept='image/*'
							style={{ display: 'none' }}
							id='image-upload'
							type='file'
							onChange={onImageChange}
							disabled={isFormDisabled}
						/>
						<label htmlFor='image-upload'>
							<Button variant='outlined' component='span' fullWidth disabled={isFormDisabled}>
								{t('blog.button.choose_image')}
							</Button>
						</label>
						{imagePreview && (
							<Box sx={{ position: 'relative', display: 'inline-block', mt: 2 }}>
								<Box
									component='img'
									src={imagePreview}
									alt={t('blog.field.cover_image')}
									sx={{
										width: '100%',
										maxWidth: 400,
										height: 200,
										objectFit: 'cover',
										borderRadius: 1,
										border: '1px solid',
										borderColor: 'divider',
									}}
								/>
								<IconButton
									size='small'
									color='error'
									onClick={onRemoveImage}
									disabled={!canRemoveImage || isFormDisabled}
									aria-label={t('blog.button.remove_image')}
									sx={{
										position: 'absolute',
										top: 8,
										right: 8,
										bgcolor: 'background.paper',
										boxShadow: 1,
										'&:hover': {
											bgcolor: 'error.main',
											color: 'common.white',
										},
									}}
								>
									<Delete fontSize='small' />
								</IconButton>
							</Box>
						)}
					</Box>

					<Stack direction='row' justifyContent='space-between' sx={{ mt: 2 }}>
						<Button variant='outlined' onClick={onBack} disabled={isSubmitting}>
							{t('button.back')}
						</Button>
						<Button
							type='submit'
							variant='contained'
							color='success'
							startIcon={<Save />}
							disabled={isFormDisabled}
						>
							{submitButtonLabel}
						</Button>
					</Stack>
				</Box>
			</CardContent>
		</Card>
	)
}

export default BlogUpsertFormSection
