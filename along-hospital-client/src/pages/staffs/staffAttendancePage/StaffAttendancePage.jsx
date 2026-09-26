import ManageAttendanceBasePage from '@/components/basePages/manageAttendanceBasePage/ManageAttendanceBasePage'
import FaceCaptureDialog from '@/components/dialogs/FaceCaptureDialog'
import { ApiUrls } from '@/configs/apiUrls'
import { EnumConfig } from '@/configs/enumConfig'
import { routeUrls } from '@/configs/routeUrls'
import useAuth from '@/hooks/useAuth'
import useAxiosSubmit from '@/hooks/useAxiosSubmit'
import useConfirm from '@/hooks/useConfirm'
import useFetch from '@/hooks/useFetch'
import useTranslation from '@/hooks/useTranslation'
import { setIsIdentificationExistAuthStore } from '@/redux/reducers/authReducer'
import { Button, Stack } from '@mui/material'
import { useEffect, useState } from 'react'
import { useDispatch } from 'react-redux'
import { useNavigate } from 'react-router-dom'

const StaffAttendancePage = () => {
	const [filters, setFilters] = useState({})
	const [sort, setSort] = useState({ key: 'id', direction: 'desc' })
	const [page, setPage] = useState(1)
	const [pageSize, setPageSize] = useState(10)
	const [openFaceCapture, setOpenFaceCapture] = useState(false)
	const [checkMode, setCheckMode] = useState('CheckIn')

	const navigate = useNavigate()
	const { t } = useTranslation()
	const { auth } = useAuth()
	const confirm = useConfirm()
	const dispatch = useDispatch()

	const getMyAttendances = useFetch(
		ApiUrls.ATTENDANCE.STAFF_ATTENDANCE.INDEX,
		{ sort: `${sort.key} ${sort.direction}`, ...filters, page, pageSize },
		[sort, filters, page, pageSize]
	)

	const getAttendanceStats = useFetch(ApiUrls.ATTENDANCE.STAFF_ATTENDANCE.STATS)

	const getIdentificationExists = useFetch(
		ApiUrls.ATTENDANCE.STAFF_ATTENDANCE.CHECK_IDENTIFICATION,
		{},
		[],
		false
	)

	const checkinAttendance = useAxiosSubmit({
		url: ApiUrls.ATTENDANCE.STAFF_ATTENDANCE.CHECK_IN,
		method: 'POST',
		onSuccess: async () => {
			await getAttendanceStats.fetch()
			await getMyAttendances.fetch()
		},
	})

	const checkoutAttendance = useAxiosSubmit({
		url: ApiUrls.ATTENDANCE.STAFF_ATTENDANCE.CHECK_OUT,
		method: 'POST',
		onSuccess: async () => {
			await getAttendanceStats.fetch()
			await getMyAttendances.fetch()
		},
	})

	const resetIdentification = useAxiosSubmit({
		url: ApiUrls.ATTENDANCE.STAFF_ATTENDANCE.RESET_IDENTIFICATION,
		method: 'DELETE',
		onSuccess: async () => {
			dispatch(setIsIdentificationExistAuthStore(false))
		},
	})

	useEffect(() => {
		const fetchIdentificationExists = async () => {
			const response = await getIdentificationExists.fetch()
			if (response) {
				const isExist = response?.data ?? false
				dispatch(setIsIdentificationExistAuthStore(isExist))
			}
		}

		if (auth.isIdentificationExist === undefined) {
			fetchIdentificationExists()
		}
	}, [auth])

	const handleComplete = async (blobs) => {
		const formData = new FormData()
		formData.append('file', blobs[0], 'face.jpg')

		if (checkMode === EnumConfig.AttendanceLogType.CheckIn) {
			await checkinAttendance.submit({ overrideData: formData })
		} else if (checkMode === EnumConfig.AttendanceLogType.CheckOut) {
			await checkoutAttendance.submit({ overrideData: formData })
		}
	}

	const canCheckIn = getAttendanceStats.data?.canCheckIn ?? false
	const canCheckOut = getAttendanceStats.data?.canCheckOut ?? false
	const attendanceLoading =
		checkinAttendance.loading ||
		checkoutAttendance.loading ||
		resetIdentification.loading ||
		getMyAttendances.loading

	const handleResetIdentification = async () => {
		const isConfirmed = await confirm({
			title: t('attendance.dialog.reset_identification_title'),
			description: t('attendance.dialog.reset_identification_description'),
			confirmColor: 'error',
			confirmText: t('attendance.button.reset_identification'),
		})

		if (!isConfirmed) return

		await resetIdentification.submit()
	}

	const renderButton = () => {
		if (auth.isIdentificationExist === undefined) {
			return (
				<Button variant='contained' loading loadingPosition='start'>
					{t('text.loading')}
				</Button>
			)
		}

		if (auth.isIdentificationExist === false) {
			return (
				<Button
					color='primary'
					onClick={() => navigate(routeUrls.BASE_ROUTE.STAFF(routeUrls.STAFF.IDENTIFICATION.ENROLL))}
					variant='contained'
				>
					{t('attendance.button.enroll_face')}
				</Button>
			)
		}

		return (
			<>
				{canCheckIn && (
					<Button
						color='success'
						onClick={() => {
							setCheckMode(EnumConfig.AttendanceLogType.CheckIn)
							setOpenFaceCapture(true)
						}}
						loading={attendanceLoading}
						loadingPosition='start'
						variant='contained'
					>
						{t('attendance.button.check_in')}
					</Button>
				)}
				{canCheckOut && (
					<Button
						color='error'
						onClick={() => {
							setCheckMode(EnumConfig.AttendanceLogType.CheckOut)
							setOpenFaceCapture(true)
						}}
						loading={attendanceLoading}
						loadingPosition='start'
						variant='contained'
					>
						{t('attendance.button.check_out')}
					</Button>
				)}
				<Button
					color='warning'
					onClick={handleResetIdentification}
					loading={attendanceLoading}
					loadingPosition='start'
					variant='contained'
				>
					{t('attendance.button.reset_identification')}
				</Button>
			</>
		)
	}

	return (
		<>
			<ManageAttendanceBasePage
				filters={filters}
				setFilters={setFilters}
				sort={sort}
				setSort={setSort}
				page={page}
				setPage={setPage}
				pageSize={pageSize}
				setPageSize={setPageSize}
				attendances={getMyAttendances.data?.collection ?? []}
				loading={getMyAttendances.loading}
				totalPage={getMyAttendances.data?.totalPage}
				buttons={
					<Stack direction='row' spacing={2}>
						{renderButton()}
					</Stack>
				}
			/>
			<FaceCaptureDialog
				open={openFaceCapture}
				onClose={() => setOpenFaceCapture(false)}
				mode='single'
				onCaptureComplete={(blobs) => handleComplete(blobs)}
			/>
		</>
	)
}

export default StaffAttendancePage
