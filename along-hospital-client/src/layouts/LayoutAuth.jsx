import AuthLayout from '@/components/layouts/AuthLayout'
import { Outlet } from 'react-router-dom'

const LayoutAuth = () => {
	return (
		<AuthLayout showHero={true}>
			<Outlet />
		</AuthLayout>
	)
}

export default LayoutAuth
