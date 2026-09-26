import {
	EventAvailableRounded,
	FavoriteRounded,
	LocalPharmacyRounded,
	MonitorHeartRounded,
	ShoppingBagRounded,
} from '@mui/icons-material'

export const aboutHeroBadgeItems = [
	{ key: 'care', icon: FavoriteRounded, tone: 'primary' },
	{ key: 'booking', icon: EventAvailableRounded, tone: 'secondary' },
	{ key: 'pharmacy', icon: LocalPharmacyRounded, tone: 'info' },
]

export const aboutGoalItems = [
	{ key: 'appointment', icon: EventAvailableRounded, tone: 'primary' },
	{ key: 'commerce', icon: ShoppingBagRounded, tone: 'secondary' },
	{ key: 'care', icon: MonitorHeartRounded, tone: 'info' },
]

export const aboutTeamMembers = [
	{ key: 'member_01', avatar: '/about/team-01.png', tone: 'primary' },
	{ key: 'member_02', avatar: '/about/team-02.png', tone: 'secondary' },
	{ key: 'member_03', avatar: '/about/team-03.png', tone: 'info' },
	{ key: 'member_04', avatar: '/about/team-04.png', tone: 'success' },
	{ key: 'member_05', avatar: '/about/team-05.png', tone: 'warning' },
]
