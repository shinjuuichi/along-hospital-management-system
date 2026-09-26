import SkeletonMedicineCard from '@/components/skeletons/SkeletonMedicineCard'
import { ApiUrls } from '@/configs/apiUrls'
import useFetch from '@/hooks/useFetch'
import useReduxStore from '@/hooks/useReduxStore'
import useTranslation from '@/hooks/useTranslation'
import {
	setMedicineCategoriesStore,
	setMedicineUnitsStore,
} from '@/redux/reducers/managementReducer'
import { Box, Pagination, Stack, Typography } from '@mui/material'
import { useState } from 'react'
import { default as MedicineCardSection } from './sections/MedicineCardSection'
import ShopFilters from './sections/ShopFiltersSection'

export default function ShopPage() {
	const { t } = useTranslation()
	const [filters, setFilters] = useState({
		name: '',
		medicineCategoryId: '',
		medicineUnitId: '',
		status: 'Active',
		isPublic: true,
		page: 1,
		pageSize: 12,
	})

	const getAllMedicines = useFetch(ApiUrls.MEDICINE.INDEX, filters, [filters])

	const { data: categories, loading: loadingCategories } = useReduxStore({
		selector: (state) => state.management.medicineCategories,
		setStore: setMedicineCategoriesStore,
	})

	const { data: units } = useReduxStore({
		selector: (state) => state.management.medicineUnits,
		setStore: setMedicineUnitsStore,
	})

	const handlePageChange = (_, page) => {
		setFilters((prev) => ({ ...prev, page }))
	}

	const handleFilterClick = (newFilters) => {
		setFilters((prev) => ({ ...prev, ...newFilters, status: 'Active', page: 1 }))
	}

	const handleResetFilterClick = () => {
		setFilters({
			name: '',
			medicineCategoryId: '',
			medicineUnitId: '',
			status: 'Active',
			isPublic: true,
			page: 1,
			pageSize: 12,
		})
	}

	const medicines = getAllMedicines.data?.collection || []
	const totalPages = getAllMedicines.totalPages

	return (
		<Box
			py={4}
			sx={{
				background: (theme) => theme.palette.gradients.background,
			}}
		>
			<Typography variant='h4' sx={{ mb: 3 }}>
				{t('shop.title')}
			</Typography>

			<Box sx={{ display: 'flex', gap: 3, alignItems: 'flex-start' }}>
				<Box sx={{ width: '25%', position: 'sticky', top: 96, alignSelf: 'flex-start' }}>
					<ShopFilters
						filters={filters}
						categories={categories || []}
						units={units || []}
						loading={getAllMedicines.loading || loadingCategories}
						onFilterClick={handleFilterClick}
						onResetFilterClick={handleResetFilterClick}
					/>
				</Box>

				<Box sx={{ width: '75%' }}>
					<Box
						sx={{
							display: 'grid',
							gridTemplateColumns: {
								xs: '1fr',
								sm: 'repeat(2, 1fr)',
								md: 'repeat(3, 1fr)',
								lg: 'repeat(4, 1fr)',
							},
							gap: 3,
						}}
					>
						{getAllMedicines.loading
							? Array.from({ length: filters.pageSize }).map((_, index) => (
									<Box
										key={index}
										sx={{
											display: 'flex',
											flexDirection: 'column',
										}}
									>
										<SkeletonMedicineCard />
									</Box>
								))
							: medicines.map((medicine) => (
									<Box
										key={medicine.id}
										sx={{
											display: 'flex',
											flexDirection: 'column',
										}}
									>
										<MedicineCardSection medicine={medicine} />
									</Box>
								))}

						{!getAllMedicines.loading && medicines.length === 0 && (
							<Box
								sx={{
									p: 3,
									textAlign: 'center',
									bgcolor: 'background.paper',
									borderRadius: 1,
									width: '100%',
								}}
							>
								<Typography color='text.secondary'>{t('shop.text.no_products')}</Typography>
							</Box>
						)}
					</Box>

					{totalPages > 1 && (
						<Stack alignItems='center' sx={{ mt: 4 }}>
							<Pagination
								page={filters.page}
								count={totalPages}
								onChange={handlePageChange}
								disabled={getAllMedicines.loading}
							/>
						</Stack>
					)}
				</Box>
			</Box>
		</Box>
	)
}
