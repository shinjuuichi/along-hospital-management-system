import { isEmptyValue } from '@/utils/handleBooleanUtil'
import { useCallback, useEffect, useMemo, useState } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import useFetch from './useFetch'

/**
 * @typedef {import('@/redux/store').RootState} RootState
 */

/**
 * @param {object} params
 * @param {string} params.url
 * @param {(state: RootState) => any} params.selector
 * @param {(payload: any) => any} params.setStore
 * @param {(storeValue: any) => any} [params.dataToGet=(x)=>x]
 * @param {(apiValue: any) => any} [params.dataToStore=(x)=>x]
 */
export default function useReduxStore({
	selector,
	setStore,
	dataToGet = (x) => x,
	dataToStore = (x) => x,
}) {
	const [initialized, setInitialized] = useState(false)

	const dispatch = useDispatch()
	const storeData = useSelector(selector)

	const finalUrl = setStore?.defaultUrl || ''
	const { loading, error, data: fetchedData, fetch } = useFetch(finalUrl, {}, [], false)

	const needFetch = isEmptyValue(storeData)
	useEffect(() => {
		if (needFetch && finalUrl) {
			fetch()
		}
	}, [needFetch, finalUrl, fetch])

	useEffect(() => {
		if (fetchedData != null && setStore) {
			const payload = dataToStore(fetchedData)
			dispatch(setStore(payload))
			setInitialized(true)
		}
	}, [fetchedData, setStore])

	const data = useMemo(() => dataToGet(storeData), [storeData])

	const resetStore = useCallback(
		(next) => {
			const payload = typeof next === 'function' ? next(storeData) : next
			if (setStore) {
				dispatch(setStore(payload))
			}
		},
		[dispatch, setStore, storeData]
	)

	return { loading, error, data, fetch, resetStore, initialized }
}
