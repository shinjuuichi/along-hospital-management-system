import SearchBar from '@/components/generals/SearchBar'
import useTranslation from '@/hooks/useTranslation'
import { PersonAdd } from '@mui/icons-material'
import { Box, Button, Paper, Stack, Typography } from '@mui/material'

const CreateMedicalHistorySearchPatientSection = ({
	searchTerm,
	setSearchTerm,
	onCreateNewPatientClick,
}) => {
	const { t } = useTranslation()

	return (
		<Paper variant='outlined' sx={{ p: 2, borderRadius: 2, mb: 2 }}>
			<Typography variant='subtitle1' sx={{ mb: 1.5 }}>
				{t('medical_history.title.select_patient')}
			</Typography>
			<Stack direction='row' spacing={2} alignItems='center'>
				<Box sx={{ flex: 1 }}>
					<SearchBar
						placeholder={t('medical_history.placeholder.search_patient_in_create')}
						value={searchTerm}
						setValue={setSearchTerm}
					/>
				</Box>
				<Button variant='outlined' startIcon={<PersonAdd />} onClick={onCreateNewPatientClick}>
					{t('medical_history.button.create_patient')}
				</Button>
			</Stack>
		</Paper>
	)
}

export default CreateMedicalHistorySearchPatientSection
