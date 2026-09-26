export const homeViewport = {
	once: true,
	amount: 0.18,
}

export const homeSectionReveal = {
	hidden: { opacity: 0, y: 24 },
	show: {
		opacity: 1,
		y: 0,
		transition: {
			duration: 0.55,
			ease: [0.22, 1, 0.36, 1],
		},
	},
}

export const homeStagger = {
	hidden: {},
	show: {
		transition: {
			staggerChildren: 0.1,
			delayChildren: 0.06,
		},
	},
}

export const homeItemReveal = {
	hidden: { opacity: 0, y: 24 },
	show: {
		opacity: 1,
		y: 0,
		transition: {
			duration: 0.42,
			ease: [0.22, 1, 0.36, 1],
		},
	},
}
