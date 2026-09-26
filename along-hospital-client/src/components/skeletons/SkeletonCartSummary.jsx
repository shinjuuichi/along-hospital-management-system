import { Divider, Paper, Skeleton, Stack } from '@mui/material'

const SkeletonCartSummary = () => {
	return (
		<Paper
			sx={(theme) => ({
				p: 3,
				position: { md: 'sticky' },
				top: 20,
				height: 'fit-content',
				backgroundColor: theme.palette.background.paper,
				border: `1px solid ${theme.palette.divider}`,
			})}
		>
			<Skeleton variant='text' width='60%' height={32} sx={{ mb: 2 }} />

			<Stack spacing={2}>
				{Array.from({ length: 3 }).map((_, index) => (
					<Stack key={index} direction='row' justifyContent='space-between' alignItems='center'>
						<Skeleton variant='text' width='70%' height={20} />
						<Skeleton variant='text' width={60} height={20} />
					</Stack>
				))}

				<Divider sx={{ my: 1 }} />

				<Stack direction='row' justifyContent='space-between' alignItems='center'>
					<Skeleton variant='text' width='40%' height={32} />
					<Skeleton variant='text' width={100} height={32} />
				</Stack>

				<Skeleton variant='rectangular' width='100%' height={40} sx={{ borderRadius: 1 }} />

				<Skeleton variant='rectangular' width='100%' height={40} sx={{ borderRadius: 1 }} />

				<Skeleton variant='rectangular' width='100%' height={48} sx={{ borderRadius: 1, mt: 2 }} />
			</Stack>
		</Paper>
	)
}

export default SkeletonCartSummary
