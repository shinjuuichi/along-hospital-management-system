import axiosConfig from '@/configs/axiosConfig'
import { isPlainObject } from '@/utils/handleBooleanUtil'
import { appendPath } from '@/utils/handleStringUtil'
import { useCallback, useEffect, useMemo, useRef, useState } from 'react'

/**
 * @param {string} url
 * @param {object} [params={}]
 * @param {Array} [dependencies=[]]
 * @param {boolean} [fetchOnMount=true]
 * @returns {{loading:boolean, error:Error|null, data:any, setData:function, fetch: () => Promise<import('axios').AxiosResponse<any> | undefined>}}
 */
export default function useFetch(url, params = {}, dependencies = [], fetchOnMount = true) {
	const [loading, setLoading] = useState(false)
	const [error, setError] = useState(null)
	const [data, setData] = useState(null)

	const isFirstRender = useRef(true)

	const memoParams = useMemo(() => params, [JSON.stringify(params)])

	const fetchData = useCallback(async () => {
		const controller = new AbortController()
		const currentReqId = Date.now()
		fetchData.reqId = currentReqId

		setError(null)
		setLoading(true)

		const isObjParams = isPlainObject(memoParams)
		const finalUrl = isObjParams ? url : appendPath(url, memoParams)
		const axiosParams = isObjParams ? memoParams : undefined

		try {
			const response = await axiosConfig.get(finalUrl, {
				params: axiosParams,
				signal: controller.signal,
			})

			if (fetchData.reqId === currentReqId) {
				setError(null)
				setData(response.data)
				return response
			}
		} catch (error) {
			if (error.name !== 'CanceledError' && fetchData.reqId === currentReqId) {
				setError(error)
				return undefined
			}
		} finally {
			if (fetchData.reqId === currentReqId) {
				setLoading(false)
			}
		}
	}, [url, memoParams])

	useEffect(() => {
		if (isFirstRender.current) {
			isFirstRender.current = false

			if (fetchOnMount) {
				fetchData()
			}

			return
		}

		if (dependencies.length > 0) {
			fetchData()
		}
	}, [fetchOnMount, fetchData, ...dependencies])

	return { loading, error, data, setData, fetch: fetchData }
}
