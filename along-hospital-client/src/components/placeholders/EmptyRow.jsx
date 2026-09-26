import useTranslation from '@/hooks/useTranslation'
import { ArchiveTwoTone } from '@mui/icons-material'
import { Stack, TableCell, TableRow, Typography } from '@mui/material'

const EmptyRow = ({
	colSpan = 1,
	title,
	description,
	icon = <ArchiveTwoTone sx={{ fontSize: { xs: 72, sm: 96, md: 120 } }} />,
	buttons,
	minHeight = 200,
}) => {
	const { t } = useTranslation()

	return (
		<TableRow hover={false}>
			<TableCell colSpan={colSpan} sx={{ py: 4 }}>
				<Stack
					direction='row'
					justifyContent='center'
					alignItems='center'
					spacing={3}
					sx={{ color: (t) => t.palette.text.secondary, minHeight }}
				>
					{icon}
					<Stack spacing={0.75}>
						<Typography variant='h6' sx={{ fontWeight: 600 }}>
							{title || t('text.placeholder.no_data')}
						</Typography>
						{description && (
							<Typography variant='body2' color='text.secondary'>
								{description}
							</Typography>
						)}
						{buttons && (
							<Stack direction='row' spacing={1.5} sx={{ mt: 1 }}>
								{buttons}
							</Stack>
						)}
					</Stack>
				</Stack>
			</TableCell>
		</TableRow>
	)
}

export default EmptyRow
