import { ApiUrls } from '@/configs/apiUrls'
import { routeUrls } from '@/configs/routeUrls'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useFetch from '@/hooks/useFetch'
import useForm from '@/hooks/useForm'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import { setBlogCategoriesStore } from '@/redux/reducers/managementReducer'
import { getImageFromCloud } from '@/utils/commons'
import { formatDateToSqlDate } from '@/utils/formatDateUtil'
import { uploadImage } from '@/utils/imageUpload'
import { ArrowBack } from '@mui/icons-material'
import { Button, Paper, Stack, Typography } from '@mui/material'
import { useEffect, useRef, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import BlogUpsertFormSection from './sections/BlogUpsertFormSection'

const BlogUpsertPage = () => {
	const { t } = useTranslation()
	const navigate = useNavigate()
	const { id: blogIdParam } = useParams()
	const blogId = blogIdParam
	const isEditMode = Boolean(blogId)

	const { values, setField, reset } = useForm({
		title: '',
		content: '',
		image: null,
		blogCategoryId: '',
		creationDate: '',
	})

	const [errors, setErrors] = useState({})
	const [imagePreview, setImagePreview] = useState(null)
	const [shouldRemoveImage, setShouldRemoveImage] = useState(false)
	const dataLoadedRef = useRef(false)

	const getBlog = useFetch(
		isEditMode && blogId ? ApiUrls.BLOG.MANAGEMENT.DETAIL(blogId) : null,
		{},
		isEditMode && blogId ? [blogId] : []
	)

	const { data: categories, loading: categoriesLoading } = useReduxStore({
		selector: (state) => state.management.blogCategories,
		setStore: setBlogCategoriesStore,
	})

	const createBlog = useAxiosSubmit({
		url: ApiUrls.BLOG.MANAGEMENT.INDEX,
		method: 'POST',
		onSuccess: async () => {
			navigate(routeUrls.BASE_ROUTE.MARKETER(routeUrls.MARKETER.BLOG.INDEX))
		},
	})

	const updateBlog = useAxiosSubmit({
		method: 'PUT',
		onSuccess: async () => {
			navigate(routeUrls.BASE_ROUTE.MARKETER(routeUrls.MARKETER.BLOG.INDEX))
		},
	})

	const handleContentChange = (html) => {
		setField('content', html)
	}

	const handleContentImageUpload = async (file) => {
		return await uploadImage(file, 'BlogContent')
	}

	useEffect(() => {
		dataLoadedRef.current = false
	}, [blogId])

	useEffect(() => {
		if (!isEditMode || !blogId || !getBlog.data || dataLoadedRef.current) return
		const blog = getBlog.data
		dataLoadedRef.current = true
		reset({
			title: blog.title ?? '',
			content: blog.content ?? '',
			image: null,
			blogCategoryId: blog.blogCategory?.id ?? '',
			creationDate: blog.creationDate ?? '',
		})
		setImagePreview(blog.image ? getImageFromCloud(blog.image) : null)
		setShouldRemoveImage(false)
	}, [getBlog.data, isEditMode, blogId, reset])

	const handleImageChange = (event) => {
		const file = event.target.files?.[0]
		if (file) {
			setField('image', file)

			const reader = new FileReader()
			reader.onload = (e) => {
				setImagePreview(e.target.result)
			}
			reader.readAsDataURL(file)
			setShouldRemoveImage(false)
		}

		if (event.target) {
			event.target.value = ''
		}
	}

	const handleRemoveImage = () => {
		setField('image', null)
		setImagePreview(null)
		setShouldRemoveImage(true)
	}

	const headerTitle = `${t(`button.${isEditMode ? 'update' : 'create'}`)} Blog`
	const blogSubmit = isEditMode ? updateBlog : createBlog
	const submitButtonLabel = blogSubmit.loading
		? isEditMode
			? t('button.submitting')
			: t('button.creating')
		: t(`button.${isEditMode ? 'update' : 'create'}`)
	const isFormDisabled = getBlog.loading || blogSubmit.loading

	const handleSubmit = async (event) => {
		event.preventDefault()
		if (isFormDisabled) return

		const newErrors = {}
		if (!values.title?.trim()) {
			newErrors.title = t('error.required')
		} else if (values.title.length > 255) {
			newErrors.title = t('error.max_length', { max: 255 })
		}

		if (!values.blogCategoryId) {
			newErrors.blogCategoryId = t('error.required')
		}

		if (!values.content?.trim()) {
			newErrors.content = t('error.required')
		} else if (values.content.length > 10000) {
			newErrors.content = t('error.max_length', { max: 10000 })
		}

		if (Object.keys(newErrors).length > 0) {
			setErrors(newErrors)
			return
		}

		const submitData = {
			Title: values.title,
			Content: values.content,
			BlogCategoryId: values.blogCategoryId,
			...(values.creationDate
				? { CreationDate: values.creationDate }
				: !isEditMode
				? { CreationDate: formatDateToSqlDate(new Date()) }
				: {}),
			...(values.image ? { Image: values.image } : {}),
			...(shouldRemoveImage && isEditMode ? { RemoveImage: 'true' } : {}),
		}

		await blogSubmit.submit({
			overrideData: submitData,
			...(isEditMode ? { overrideUrl: ApiUrls.BLOG.MANAGEMENT.DETAIL(blogId) } : {}),
		})
	}

	const handleBack = () => {
		navigate(routeUrls.BASE_ROUTE.MARKETER(routeUrls.MARKETER.BLOG.INDEX))
	}

	return (
		<Paper sx={{ py: 2, px: 3, mt: 2 }}>
			<Stack direction='row' alignItems='center' spacing={2} mb={3}>
				<Button
					startIcon={<ArrowBack />}
					onClick={handleBack}
					variant='outlined'
					disabled={blogSubmit.loading}
				>
					{t('button.back')}
				</Button>
				<Typography variant='h5' fontWeight='bold'>
					{headerTitle}
				</Typography>
			</Stack>
			<BlogUpsertFormSection
				isEditMode={isEditMode}
				detailLoading={getBlog.loading}
				t={t}
				onSubmit={handleSubmit}
				formData={values}
				errors={errors}
				onInputChange={setField}
				onContentChange={handleContentChange}
				onContentImageUpload={handleContentImageUpload}
				onImageChange={handleImageChange}
				onRemoveImage={handleRemoveImage}
				imagePreview={imagePreview}
				submitButtonLabel={submitButtonLabel}
				isFormDisabled={isFormDisabled}
				onBack={handleBack}
				isSubmitting={blogSubmit.loading}
				canRemoveImage={Boolean(imagePreview)}
				categories={categories || []}
				categoriesLoading={categoriesLoading}
			/>
		</Paper>
	)
}

export default BlogUpsertPage
