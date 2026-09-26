export const aboutViewport = {
	once: true,
	amount: 0.2,
}

const aboutEase = [0.22, 1, 0.36, 1]

export const aboutSectionReveal = {
	hidden: { opacity: 0, y: 24 },
	show: {
		opacity: 1,
		y: 0,
		transition: {
			duration: 0.55,
			ease: aboutEase,
		},
	},
}

export const aboutItemStagger = {
	hidden: {},
	show: {
		transition: {
			staggerChildren: 0.1,
			delayChildren: 0.06,
		},
	},
}

export const aboutItemReveal = {
	hidden: { opacity: 0, y: 22 },
	show: {
		opacity: 1,
		y: 0,
		transition: {
			duration: 0.42,
			ease: aboutEase,
		},
	},
}

export const aboutSlideLeft = {
	hidden: { opacity: 0, x: -34 },
	show: {
		opacity: 1,
		x: 0,
		transition: {
			duration: 0.55,
			ease: aboutEase,
		},
	},
}

export const aboutSlideRight = {
	hidden: { opacity: 0, x: 34 },
	show: {
		opacity: 1,
		x: 0,
		transition: {
			duration: 0.55,
			ease: aboutEase,
		},
	},
}
