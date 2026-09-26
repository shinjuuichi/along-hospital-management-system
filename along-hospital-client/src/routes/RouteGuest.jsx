import { routeUrls } from '@/configs/routeUrls'
import useAuth from '@/hooks/useAuth'
import LayoutGuest from '@/layouts/LayoutGuest'
import LayoutPatient from '@/layouts/LayoutPatient'
import InvoicePrintPage from '@/pages/commons/invoicePage/InvoicePrintPage'
import InvoiceRefundPrintPage from '@/pages/commons/invoicePage/InvoiceRefundPrintPage'
import PrescriptionPrintPage from '@/pages/commons/invoicePage/PrescriptionPrintPage'
import NotFoundPage from '@/pages/commons/NotFoundPage'
import AboutUsPage from '@/pages/guests/aboutUsPage/AboutUsPage'
import HomePage from '@/pages/guests/home/HomePage'
import PaymentCancelPage from '@/pages/guests/paymentResultPage/PaymentCancelPage'
import PaymentReturnPage from '@/pages/guests/paymentResultPage/PaymentReturnPage'
import SePayPaymentPage from '@/pages/patients/sePayPaymentPage/SePayPaymentPage'
import CreateQueueFromQRPage from '@/pages/guests/queues/createQueueFromQRPage/CreateQueueFromQRPage'
import CreateQueuePage from '@/pages/guests/queues/createQueuePage/CreateQueuePage'
import ShopPage from '@/pages/guests/shopPage/ShopPage'
import BlogDetailPage from '@/pages/guests/viewBlogDetailPage/BlogDetailPage'
import BlogPage from '@/pages/guests/viewBlogPage/BlogPage'
import DoctorPage from '@/pages/guests/viewDoctorPage/DoctorPage'
import JobPostingDetailPage from '@/pages/guests/viewJobPostingDetailPage/JobPostingDetailPage'
import JobPostingPage from '@/pages/guests/viewJobPostingPage/JobPostingPage'
import MedicineDetailPage from '@/pages/guests/viewMedicineDetailPage/MedicineDetailPage'
import ViewQueuePage from '@/pages/guests/queues/viewQueuePage/ViewQueuePage'
import MedicalServicePage from '@/pages/patients/medicalServicePage/MedicalServicePage'
import SpecialtyPage from '@/pages/patients/specialtyPage/SpecialtyPage'
import CollectibleVoucherListPage from '@/pages/patients/voucherPage/CollectibleVoucherListPage'
import TestTable from '@/pages/TestTable'
import { Route, Routes } from 'react-router-dom'
import ProtectedRoute from './ProtectedRoute'

const RouteGuest = () => {
	const { auth } = useAuth()

	return (
		<Routes>
			<Route element={<ProtectedRoute allowRoles={[]} />}>
				<Route element={auth?.role !== null ? <LayoutPatient /> : <LayoutGuest />}>
					<Route path='/' index element={<HomePage />} />
					<Route path={routeUrls.HOME.ABOUT_US} element={<AboutUsPage />} />
					<Route path={routeUrls.HOME.MEDICINE} element={<ShopPage />} />
					<Route path={routeUrls.HOME.SPECIALTY} element={<SpecialtyPage />} />
					<Route path={routeUrls.HOME.MEDICAL_SERVICE} element={<MedicalServicePage />} />
					<Route path={routeUrls.HOME.VOUCHERS} element={<CollectibleVoucherListPage />} />
					<Route path={routeUrls.HOME.DOCTOR} element={<DoctorPage />} />
					<Route path={routeUrls.HOME.BLOG} element={<BlogPage />} />
					<Route path={`${routeUrls.HOME.BLOG}/:id`} element={<BlogDetailPage />} />
					<Route path={routeUrls.HOME.JOB_POSTING.INDEX} element={<JobPostingPage />} />
					<Route path={routeUrls.HOME.JOB_POSTING.DETAIL(':id')} element={<JobPostingDetailPage />} />
					<Route path={`${routeUrls.HOME.MEDICINE}/:id`} element={<MedicineDetailPage />} />
					<Route path={routeUrls.HOME.INVOICE_PRINT(':id')} element={<InvoicePrintPage />} />
					<Route
						path={routeUrls.HOME.INVOICE_REFUND_PRINT(':invoiceId', ':chargeId')}
						element={<InvoiceRefundPrintPage />}
					/>
					<Route path={routeUrls.HOME.PRESCRIPTION_PRINT(':id')} element={<PrescriptionPrintPage />} />
					<Route path={routeUrls.HOME.PAYMENT.CANCEL} element={<PaymentCancelPage />} />
					<Route path={routeUrls.HOME.PAYMENT.RETURN} element={<PaymentReturnPage />} />
					<Route path={routeUrls.HOME.PAYMENT.SEPAY} element={<SePayPaymentPage />} />
					{import.meta.env.DEV && (
						<>
							<Route path='/test' element={<TestTable />} />
						</>
					)}
				</Route>

				<Route path={routeUrls.HOME.QUEUE.CREATE} element={<CreateQueuePage />} />
				<Route path={routeUrls.HOME.QUEUE.CREATE_FROM_QR} element={<CreateQueueFromQRPage />} />
				<Route path={routeUrls.HOME.QUEUE.INDEX} element={<ViewQueuePage />} />
			</Route>

			<Route path='*' element={<NotFoundPage />} />
		</Routes>
	)
}

export default RouteGuest
