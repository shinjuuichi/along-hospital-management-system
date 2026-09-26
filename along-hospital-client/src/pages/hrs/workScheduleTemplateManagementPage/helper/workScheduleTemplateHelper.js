export const getStaffRoles = (staff) => {
	if (!staff) return []
	const roles = Array.isArray(staff.roles) ? staff.roles : staff.role ? [staff.role] : []
	return roles.map((role) => `${role || ''}`.trim()).filter(Boolean)
}

const normalizeText = (value) => `${value || ''}`.trim().toLowerCase()

const toValidNumber = (value) => {
	const parsed = Number(value)
	return Number.isFinite(parsed) && parsed > 0 ? parsed : null
}

export const hasRole = (staff, role) => {
	const expectedRole = normalizeText(role)
	if (!expectedRole) return false

	return getStaffRoles(staff).some((staffRole) => normalizeText(staffRole) === expectedRole)
}

export const hasAnyMatchingRole = (staff, allowedRoles = []) => {
	const allowedRoleSet = new Set(
		allowedRoles.map((role) => normalizeText(role)).filter(Boolean)
	)
	if (!allowedRoleSet.size) return false

	return getStaffRoles(staff).some((staffRole) => allowedRoleSet.has(normalizeText(staffRole)))
}

export const getStaffSpecialtyId = (staff) =>
	toValidNumber(staff?.specialtyId ?? staff?.specialty?.id)

export const getLocationSpecialtyId = (location) =>
	toValidNumber(location?.specialtyId ?? location?.specialty?.id)

export const getEntitySpecialtyName = (entity, specialtyLookup) => {
	if (!entity) return ''

	const directName = entity.specialtyName || entity.specialty?.name
	if (directName) return directName

	const specialtyId = toValidNumber(entity.specialtyId ?? entity.specialty?.id)
	if (specialtyId == null) return ''

	if (specialtyLookup instanceof Map) {
		return specialtyLookup.get(specialtyId) || ''
	}

	return ''
}
