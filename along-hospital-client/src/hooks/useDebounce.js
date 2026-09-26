import { useEffect } from 'react'
import useTimeout from './helpers/useTimeout'

export default function useDebounce(callback, delay = 500, dependencies = []) {
	const debounceTimeout = useTimeout(callback, delay)
	useEffect(debounceTimeout.reset, [...dependencies, debounceTimeout.reset])
	useEffect(debounceTimeout.clear, [])
}

// Usage example:
// const [searchValue, setSearchValue] = useState('')
// useDebounce(() => alert(searchValue), 500, [searchValue])
