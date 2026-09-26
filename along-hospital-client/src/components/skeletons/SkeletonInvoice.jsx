import SkeletonBox from '@/components/skeletons/SkeletonBox'
import SkeletonTextField from '@/components/skeletons/SkeletonTextField'
import {
	Box,
	Grid,
	Paper,
	Skeleton,
	Stack,
	Table,
	TableBody,
	TableCell,
	TableHead,
	TableRow,
} from '@mui/material'

const SkeletonInvoice = ({ rows = 5 }) => {
	return (
		<Box sx={{ p: 4, bgcolor: 'background.default' }}>
			<Box sx={{ mb: 2, display: 'flex', justifyContent: 'flex-end' }}>
				<Skeleton variant='rounded' width={140} height={36} />
			</Box>

			<Paper
				elevation={3}
				sx={{
					maxWidth: 900,
					mx: 'auto',
					p: 4,
					bgcolor: 'background.paper',
				}}
			>
				<Grid container spacing={2} alignItems='center'>
					<Grid size={8}>
						<Stack spacing={1.2}>
							<Skeleton variant='text' width={260} height={36} />
							<Skeleton variant='text' width={380} height={20} />
							<Skeleton variant='text' width={240} height={20} />
						</Stack>
					</Grid>
					<Grid size={4} sx={{ textAlign: 'right' }}>
						<Stack spacing={1.2} alignItems='flex-end'>
							<Skeleton variant='text' width={140} height={28} />
							<Skeleton variant='text' width={120} height={20} />
							<Skeleton variant='text' width={200} height={20} />
						</Stack>
					</Grid>
				</Grid>

				<Box sx={{ my: 3 }}>
					<Skeleton variant='rectangular' width='100%' height={1} />
				</Box>

				<Grid container spacing={3}>
					<Grid size={6}>
						<SkeletonTextField numberOfRow={4} withTitle={true} withLabel={false} />
					</Grid>
					<Grid size={6}>
						<SkeletonTextField numberOfRow={3} withTitle={true} withLabel={false} />
					</Grid>
				</Grid>

				<Box sx={{ mt: 3 }}>
					<SkeletonTextField numberOfRow={4} withTitle={true} withLabel={false} />
				</Box>

				<Box sx={{ mt: 4 }}>
					<Skeleton variant='text' width={200} height={28} />
					<Table size='small' sx={{ mt: 1 }}>
						<TableHead>
							<TableRow>
								{Array.from({ length: 5 }).map((_, i) => (
									<TableCell key={i} align={i === 0 ? 'left' : 'right'}>
										<Skeleton variant='text' width={i === 0 ? 60 : 120} height={20} />
									</TableCell>
								))}
							</TableRow>
						</TableHead>
						<TableBody>
							{Array.from({ length: rows }).map((_, r) => (
								<TableRow key={r}>
									{Array.from({ length: 5 }).map((__, c) => (
										<TableCell key={c} align={c === 0 || c === 1 ? 'left' : 'right'}>
											<Skeleton variant='text' height={20} />
										</TableCell>
									))}
								</TableRow>
							))}
						</TableBody>
					</Table>
				</Box>

				<Grid container spacing={2} sx={{ mt: 4 }} justifyContent='flex-end'>
					<Grid size={{ xs: 12, sm: 6, md: 4 }}>
						<Box sx={{ border: '1px solid', borderColor: 'divider', p: 2, borderRadius: 1 }}>
							<SkeletonBox numberOfBoxes={4} direction='column' heights={[24, 24, 1, 32]} />
						</Box>
					</Grid>
				</Grid>

				<Box sx={{ mt: 6, display: 'flex', justifyContent: 'space-between' }}>
					{Array.from({ length: 3 }).map((_, i) => (
						<Box key={i} sx={{ textAlign: 'center', minWidth: 140 }}>
							<Skeleton variant='text' width={120} height={20} sx={{ mx: 'auto' }} />
							<Skeleton variant='text' width={160} height={16} sx={{ mx: 'auto' }} />
						</Box>
					))}
				</Box>
			</Paper>
		</Box>
	)
}

export default SkeletonInvoice
