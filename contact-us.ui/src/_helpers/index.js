// Sent, Succeed, Failed Actions Names Conventions
export const UI_INIT_STATE = () => ({ loading: false, error: null })
export const UI_LOADING_STATE = () => ({ loading: true, error: null })
export const UI_ERROR_STATE = (error) => ({ loading: false, error: error })

export const toTitleCase = str => {
	return str.split(' ')
						.map(word => word.charAt(0).toUpperCase() + word.slice(1))
						.join(' ')
}

export const toQueryString = (values) => {
	return Object.entries(values)
		.map(([key, value]) => {
			if(Array.isArray(value))
				return value.map(item => `${encodeURIComponent(key)}=${encodeURIComponent(item)}`)
										.join('&')
			else
				return `${encodeURIComponent(key)}=${encodeURIComponent(value)}`
		})
		.join('&')
}
