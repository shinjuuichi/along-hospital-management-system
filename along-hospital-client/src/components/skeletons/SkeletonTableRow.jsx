import { Skeleton, Stack, TableCell, TableRow } from '@mui/material'

const SkeletonTableRow = ({ colSpan = 1, rows = 3, lineCount = 3 }) => {
	const lines = Array.from({ length: lineCount })
	const rowArr = Array.from({ length: rows })

	return (
		<>
			{rowArr.map((_, i) => (
				<TableRow key={i} hover={false}>
					<TableCell colSpan={colSpan} sx={{ py: 1.5 }}>
						<Stack spacing={1}>
							{lines.map((__, idx) => (
								<Skeleton
									key={idx}
									variant='text'
									animation={'wave'}
									width={`${Math.floor(Math.random() * 20) + 80}%`}
								/>
							))}
						</Stack>
					</TableCell>
				</TableRow>
			))}
		</>
	)
}

export default SkeletonTableRow
