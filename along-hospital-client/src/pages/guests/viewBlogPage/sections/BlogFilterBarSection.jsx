import FilterButton from '@/components/buttons/FilterButton'
import ResetFilterButton from '@/components/buttons/ResetFilterButton'
import SearchBar from '@/components/generals/SearchBar'
import useFieldRenderer from '@/hooks/useFieldRenderer'
import useForm from '@/hooks/useForm'
import useTranslation from '@/hooks/useTranslation'
import { Grid, Paper } from '@mui/material'
import { useEffect, useMemo } from 'react'

const BlogFilterBarSection = ({
	filters = {},
	setFilters = () => {},
	loading = false,
}) => {
	const { t } = useTranslation()

	const initialValues = {
		title: filters.title || '',
		creationDate: filters.creationDate || '',
		blogCategoryId: filters.blogCategoryId || '',
	}

	const { values, setField, handleChange, reset, registerRef } = useForm(initialValues)

	const { renderField } = useFieldRenderer(
		values,
		setField,
		handleChange,
		registerRef,
		false,
		'outlined',
		'small'
	)

	useEffect(() => {
		reset(initialValues)
		// eslint-disable-next-line react-hooks/exhaustive-deps
	}, [filters])

	const fields = useMemo(() => {
		return [
			{
				key: 'creationDate',
				title: t('text.date'),
				type: 'date',
				required: false,
			},
		]
	}, [t])

	const applyFilters = () => setFilters(values)

	const resetFilters = () => {
		const empty = { title: '', creationDate: '', blogCategoryId: '' }
		reset(empty)
		setFilters(empty)
	}

	return (
		<Paper elevation={0} sx={{ borderRadius: 2, bgcolor: 'transparent' }}>
			<Grid container spacing={2} alignItems='center'>
				<Grid size={{ xs: 12, md: 4 }}>
					<SearchBar
						widthPercent={100}
						value={values.title || ''}
						setValue={(value) => setField('title', value)}
						onEnterDown={applyFilters}
					/>
				</Grid>
				{fields.map((f) => (
					<Grid size={{ xs: 12, md: 2 }} key={f.key}>
						{renderField(f)}
					</Grid>
				))}
				<Grid size={{ xs: 12, md: 2 }}>
					<FilterButton
						onFilterClick={applyFilters}
						loading={loading}
						fullWidth
					/>
				</Grid>
				<Grid size={{ xs: 12, md: 2 }}>
					<ResetFilterButton onResetFilterClick={resetFilters} loading={loading} />
				</Grid>
			</Grid>
		</Paper>
	)
}

export default BlogFilterBarSection
