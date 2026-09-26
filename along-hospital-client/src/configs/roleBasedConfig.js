import LayoutAccountant from '@/layouts/LayoutAccountant'
import LayoutDoctor from '@/layouts/LayoutDoctor'
import LayoutHotlineAgent from '@/layouts/LayoutHotlineAgent'
import LayoutHR from '@/layouts/LayoutHR'
import LayoutInventoryClerk from '@/layouts/LayoutInventoryClerk'
import LayoutManager from '@/layouts/LayoutManager'
import LayoutMarketer from '@/layouts/LayoutMarketer'
import LayoutNurse from '@/layouts/LayoutNurse'
import LayoutPharmacist from '@/layouts/LayoutPharmacist'
import LayoutReceptionist from '@/layouts/LayoutReceptionist'
import { AccountBalanceWalletRounded, EventRounded, Person, WorkHistoryRounded } from '@mui/icons-material'
import React from 'react'
import { EnumConfig } from './enumConfig'
import { routeUrls } from './routeUrls'

export const getReturnUrlByRole = (role) => {
	switch (String(role).toLowerCase()) {
		case EnumConfig.Role.Manager.toLowerCase():
			return routeUrls.BASE_ROUTE.MANAGER(routeUrls.MANAGER.DASHBOARD)
		case EnumConfig.Role.Doctor.toLowerCase():
			return routeUrls.BASE_ROUTE.DOCTOR(routeUrls.DOCTOR.APPOINTMENT_MANAGEMENT)
		case EnumConfig.Role.Nurse.toLowerCase():
			return routeUrls.BASE_ROUTE.STAFF(routeUrls.STAFF.MEDICAL_HISTORY.INDEX)
		case EnumConfig.Role.HR.toLowerCase():
			return routeUrls.BASE_ROUTE.HR(routeUrls.HR.DASHBOARD)
		case EnumConfig.Role.Pharmacist.toLowerCase():
			return routeUrls.BASE_ROUTE.PHARMACIST(routeUrls.PHARMACIST.DASHBOARD)
		case EnumConfig.Role.Accountant.toLowerCase():
			return routeUrls.BASE_ROUTE.ACCOUNTANT(routeUrls.ACCOUNTANT.DASHBOARD)
		case EnumConfig.Role.Marketer.toLowerCase():
			return routeUrls.BASE_ROUTE.MARKETER(routeUrls.MARKETER.BLOG.INDEX)
		case EnumConfig.Role.Receptionist.toLowerCase():
			return routeUrls.BASE_ROUTE.STAFF(routeUrls.STAFF.MEDICAL_HISTORY.INDEX)
		case EnumConfig.Role.HotlineAgent.toLowerCase():
			return routeUrls.BASE_ROUTE.HOTLINE_AGENT(routeUrls.HOTLINE_AGENT.FEEDBACK_MANAGEMENT)
		case EnumConfig.Role.InventoryClerk.toLowerCase():
			return routeUrls.BASE_ROUTE.INVENTORY_CLERK(routeUrls.INVENTORY_CLERK.DASHBOARD)
		default:
			return '/'
	}
}

export const getLayoutByRole = (role) => {
	switch (String(role).toLowerCase()) {
		case EnumConfig.Role.Manager.toLowerCase():
			return LayoutManager
		case EnumConfig.Role.Doctor.toLowerCase():
			return LayoutDoctor
		case EnumConfig.Role.Nurse.toLowerCase():
			return LayoutNurse
		case EnumConfig.Role.HR.toLowerCase():
			return LayoutHR
		case EnumConfig.Role.Pharmacist.toLowerCase():
			return LayoutPharmacist
		case EnumConfig.Role.Accountant.toLowerCase():
			return LayoutAccountant
		case EnumConfig.Role.Marketer.toLowerCase():
			return LayoutMarketer
		case EnumConfig.Role.Receptionist.toLowerCase():
			return LayoutReceptionist
		case EnumConfig.Role.HotlineAgent.toLowerCase():
			return LayoutHotlineAgent
		case EnumConfig.Role.InventoryClerk.toLowerCase():
			return LayoutInventoryClerk
		default:
			return React.Fragment
	}
}

export const defaultStaffMenuSections = (t) => [
	{
		title: t('sidebar.shared.personal'),
		items: [
			{
				key: 'attendance',
				label: t('sidebar.items.attendance'),
				icon: EventRounded,
				url: routeUrls.BASE_ROUTE.STAFF(routeUrls.STAFF.ATTENDANCE),
			},
			{
				key: 'my-work-schedule',
				label: t('sidebar.items.my_work_schedule'),
				icon: WorkHistoryRounded,
				url: routeUrls.BASE_ROUTE.STAFF(routeUrls.STAFF.WORK_SCHEDULE),
			},
			{
				key: 'salary-advance',
				label: t('sidebar.items.salary_advance'),
				icon: AccountBalanceWalletRounded,
				url: routeUrls.BASE_ROUTE.STAFF(routeUrls.STAFF.SALARY_ADVANCE),
			},
		],
	},
]

export const defaultStaffMenuItems = (t) => [
	{
		label: t('header.user_menu.profile'),
		url: routeUrls.BASE_ROUTE.STAFF(routeUrls.STAFF.PROFILE),
		icon: Person,
	},
]
