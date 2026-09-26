import { ApiUrls } from '@/configs/apiUrls'
import { routeUrls } from '@/configs/routeUrls'
import useFetch from '@/hooks/useFetch'
import MedicalServiceDetailDialog from '@/pages/patients/medicalServicePage/sections/MedicalServiceDetailDialog'
import MedicalServiceListSection from '@/pages/patients/medicalServicePage/sections/MedicalServiceListSection'
import { Stack } from '@mui/material'
import { useMemo, useState } from 'react'
import { useNavigate } from 'react-router-dom'

const LETTERS = ['ALL', ...'ABCDEFGHIJKLMNOPQRSTUVWXYZ'.split('')]

const normalizeLetter = (value) => {
	if (!value || typeof value !== 'string') return '#'
	const letter = value.trim().charAt(0).toUpperCase()
	return /[A-Z]/.test(letter) ? letter : '#'
}

const pickDescription = (service) => {
	return service?.description || ''
}

const MedicalServicePage = () => {
	const navigate = useNavigate()

	const { loading, data: medicalServices } = useFetch(ApiUrls.MEDICAL_SERVICE.GET_ALL, {}, [], true)

	const [searchTerm, setSearchTerm] = useState('')
	const [activeLetter, setActiveLetter] = useState('ALL')
	const [selectedService, setSelectedService] = useState(null)

	const lettersWithData = useMemo(() => {
		return new Set((medicalServices || []).map((item) => normalizeLetter(item?.name)))
	}, [medicalServices])

	const filteredServices = useMemo(() => {
		const needle = searchTerm.trim().toLowerCase()
		return (medicalServices || []).filter((service) => {
			const matchesSearch =
				needle.length === 0 ||
				service?.name?.toLowerCase().includes(needle) ||
				pickDescription(service).toLowerCase().includes(needle)

			if (!matchesSearch) return false

			if (activeLetter === 'ALL') return true
			return normalizeLetter(service?.name) === activeLetter
		})
	}, [medicalServices, searchTerm, activeLetter])

	const groupedServices = useMemo(() => {
		const map = new Map()
		filteredServices.forEach((service) => {
			const letter = normalizeLetter(service?.name)
			if (!map.has(letter)) map.set(letter, [])
			map.get(letter).push(service)
		})

		return Array.from(map.entries())
			.sort(([a], [b]) => a.localeCompare(b))
			.map(([letter, items]) => ({
				letter,
				items: items.sort((a, b) => (a?.name || '').localeCompare(b?.name || '')),
			}))
	}, [filteredServices])

	const handleViewDetail = (service) => {
		setSelectedService(service)
	}

	const handleCloseDialog = () => {
		setSelectedService(null)
	}

	const handleBookAppointment = () => {
		handleCloseDialog()
		navigate(routeUrls.BASE_ROUTE.PATIENT(routeUrls.PATIENT.APPOINTMENT.CREATE))
	}

	return (
		<Stack spacing={5} py={4}>
			<MedicalServiceListSection
				letters={LETTERS}
				activeLetter={activeLetter}
				onLetterChange={(letter) => setActiveLetter(letter)}
				searchTerm={searchTerm}
				onSearchChange={setSearchTerm}
				groupedServices={groupedServices}
				onViewDetail={handleViewDetail}
				loading={loading}
				availableLetters={lettersWithData}
			/>
			<MedicalServiceDetailDialog
				open={Boolean(selectedService)}
				service={selectedService}
				onClose={handleCloseDialog}
				onBook={handleBookAppointment}
			/>
		</Stack>
	)
}

export default MedicalServicePage
