import { Button, Stack } from '@mui/material'

const BedManagementHeaderSection = ({
	t,
	selectedIds,
	handleDeleteMany,
	handleCreate,
}) => {
	return (
		<Stack spacing={2} direction='row' alignItems='center' justifyContent='flex-end'>
			<Button
				variant='contained'
				color='primary'
				onClick={handleCreate}
			>
				{t('button.create')}
			</Button>
			<Button
				variant='outlined'
				color='error'
				disabled={!selectedIds.length}
				onClick={handleDeleteMany}
			>
				{t('button.delete_selected')}
			</Button>
		</Stack>
	)
}

export default BedManagementHeaderSection
