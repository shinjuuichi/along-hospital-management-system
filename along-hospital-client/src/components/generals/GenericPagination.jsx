/* eslint-disable react-hooks/exhaustive-deps */
/* eslint-disable no-unused-vars */
import useTranslation from '@/hooks/useTranslation'
import { ArrowBack, ArrowForward } from '@mui/icons-material'
import { MenuItem, Pagination, PaginationItem, Select, Stack, Typography } from '@mui/material'
import { useEffect, useMemo } from 'react'

export const GenericPagination = ({
	totalPage = 1,
	page = 1,
	setPage = (page) => {},
	siblingCount = 2,
	boundaryCount = 1,
	loading = false,
}) => {
	return (
		<Pagination
			count={Math.max(totalPage, 1)}
			page={Math.max(page, 1)}
			onChange={(event, value) => setPage(value)}
			showFirstButton
			showLastButton
			shape='rounded'
			color='primary'
			siblingCount={siblingCount}
			boundaryCount={boundaryCount}
			disabled={loading}
			renderItem={(item) => (
				<PaginationItem slots={{ previous: ArrowBack, next: ArrowForward }} {...item} />
			)}
		/>
	)
}

export const GenericTablePagination = ({
	totalPage = 0,
	page = 1,
	setPage = (page) => {},
	pageSize,
	setPageSize = (pageSize) => {},
	pageSizeOptions = [5, 10, 25],
	loading = false,
}) => {
	const { t } = useTranslation()

	const rowsPerPageNum = useMemo(
		() => parseInt(pageSize ?? pageSizeOptions?.[0] ?? 5, 10),
		[pageSize, pageSizeOptions]
	)

	useEffect(() => {
		if (page !== 1) setPage(1)
	}, [rowsPerPageNum, setPage])

	return (
		<Stack direction='row' justifyContent='space-between' alignItems='center' flexWrap={'wrap'}>
			<Stack spacing={1} direction='row' alignItems='center'>
				<Typography>{t('text.rows_per_page')}:</Typography>
				<Select
					size='small'
					disabled={loading}
					value={rowsPerPageNum}
					onChange={(e) => setPageSize(e.target.value)}
				>
					{pageSizeOptions.map((option) => (
						<MenuItem key={option} value={option}>
							{option}
						</MenuItem>
					))}
				</Select>
			</Stack>
			<GenericPagination
				totalPage={totalPage}
				page={page}
				setPage={setPage}
				siblingCount={1}
				boundaryCount={2}
				loading={loading}
			/>
		</Stack>
	)
}

// Usage example:

/*
<GenericPagination
	totalPage={totalPage}
	page={page}
	setPage={setPage}
/>

<GenericTablePagination
	totalPage={totalPage}
	page={page}
	setPage={setPage}
	pageSize={pageSize}
	setPageSize={setPageSize}
	loading={loading}
/>
*/
