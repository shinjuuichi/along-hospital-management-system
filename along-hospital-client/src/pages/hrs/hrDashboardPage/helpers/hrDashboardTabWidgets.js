import { createSummaryWidgetItem } from '@/components/dashboard/dashboardWidgetFactory'

export const buildHRDashboardTabWidgets = (statistics, activeTab, t) => {
	const overview = statistics?.overview
	const recruitment = statistics?.recruitment
	const leave = statistics?.leave

	const tabWidgets = {
		overview: [
			createSummaryWidgetItem({
				key: 'overviewOpenJobPostings',
				title: t('statistic_dashboard_hr.widget.open_job_postings'),
				value: overview?.openJobPostings,
				accent: '#2563eb',
			}),
			createSummaryWidgetItem({
				key: 'overviewApplicationsReceived',
				title: t('statistic_dashboard_hr.widget.applications_received'),
				value: overview?.applicationsReceived,
				accent: '#7c3aed',
			}),
			createSummaryWidgetItem({
				key: 'overviewPendingLeaveRequests',
				title: t('statistic_dashboard_hr.widget.pending_leave_requests'),
				value: overview?.pendingLeaveRequests,
				accent: '#dc2626',
			}),
			createSummaryWidgetItem({
				key: 'overviewApprovedLeaveRequests',
				title: t('statistic_dashboard_hr.widget.approved_leave_requests'),
				value: overview?.approvedLeaveRequests,
				accent: '#16a34a',
			}),
			createSummaryWidgetItem({
				key: 'overviewFinalizedSchedules',
				title: t('statistic_dashboard_hr.widget.finalized_schedules'),
				value: overview?.finalizedSchedules,
				accent: '#7c3aed',
			}),
		],
		recruitment: [
			createSummaryWidgetItem({
				key: 'recruitmentOpenJobPostings',
				title: t('statistic_dashboard_hr.widget.open_job_postings'),
				value: recruitment?.openJobPostings,
				accent: '#2563eb',
			}),
			createSummaryWidgetItem({
				key: 'recruitmentApplicationsReceived',
				title: t('statistic_dashboard_hr.widget.applications_received'),
				value: recruitment?.applicationsReceived,
				accent: '#7c3aed',
			}),
			createSummaryWidgetItem({
				key: 'recruitmentPassedApplications',
				title: t('statistic_dashboard_hr.widget.passed_applications'),
				value: recruitment?.passedApplications,
				accent: '#16a34a',
			}),
			createSummaryWidgetItem({
				key: 'recruitmentFailedApplications',
				title: t('statistic_dashboard_hr.widget.failed_applications'),
				value: recruitment?.failedApplications,
				accent: '#dc2626',
			}),
			createSummaryWidgetItem({
				key: 'recruitmentInterviewPassRate',
				title: t('statistic_dashboard_hr.widget.interview_pass_rate'),
				value: recruitment?.interviewPassRate,
				format: 'percent',
				accent: '#0891b2',
			}),
		],
		leave: [
			createSummaryWidgetItem({
				key: 'leavePendingLeaveRequests',
				title: t('statistic_dashboard_hr.widget.pending_leave_requests'),
				value: leave?.pendingLeaveRequests,
				accent: '#dc2626',
			}),
			createSummaryWidgetItem({
				key: 'leaveApprovedLeaveRequests',
				title: t('statistic_dashboard_hr.widget.approved_leave_requests'),
				value: leave?.approvedLeaveRequests,
				accent: '#16a34a',
			}),
			createSummaryWidgetItem({
				key: 'leaveRejectedLeaveRequests',
				title: t('statistic_dashboard_hr.widget.rejected_leave_requests'),
				value: leave?.rejectedLeaveRequests,
				accent: '#6b7280',
			}),
		],
	}

	return tabWidgets[activeTab] ?? tabWidgets.overview
}
