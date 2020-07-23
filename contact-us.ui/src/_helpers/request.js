import axios from 'axios'
import { store } from '../_store'
import { history } from '../index'

// build request and send it to the server and return the data OR error
const BASE_URL = 'http://localhost:5000/api'
let allPromises = [], allResponses = [], error
async function _send(request) {
	try {
    if (!Array.isArray(request))
			throw new Error('invalid request!!!')

		if ( Array.isArray(request) && Array.isArray(request[0]) ) {
			allPromises = request.map(request => {
				const [method, url, body = null, config = null] = request
				return axios[method](BASE_URL + url, body, config)
			})
			allResponses = await axios.all(allPromises)
			return { status: 'success', response: allResponses.map(res => res.data) }
		}
		else if ( Array.isArray(request) && typeof request[0] === 'string' ) {
			const [method, url, body = null, config = null] = request
			const { data } = await axios[method](BASE_URL + url, body, config)
			return { status: 'success', response: data }
		}
	} catch (err) {
		// The request was made and the server responded with a status code that falls out of the range of 2xx
		// error.response.data - error.response.status - error.response.headers
		if (err.response)
			return { status: 'error', response: err.response.data || err.response.status }
		// The request was made but no response was received
		// `error.request` is an instance of XMLHttpRequest in the browser and an instance of http.ClientRequest in node.js
		else if (err.request && Object.keys(err.request).length)
			return { status: 'error', response: err.request }
		// Something happened in setting up the request that triggered an Error
		else {
			error = JSON.parse(JSON.stringify(err))
			return { status: 'error', response: error.message || 'Something went wrong!' }
		}
	}
}

export default async function request({ request, baseAction, onSuccessAction, redirectTo, resultMapper, setSubmitting = v => v }) {
  store.dispatch({ type: `${baseAction}Sent` })
  let { status, response } = await _send(request)
  if (status === 'success') {
		if(response === true)
			response = { ...request[2] }

		if(resultMapper)
			response = resultMapper(response)

		if(onSuccessAction && Array.isArray(onSuccessAction))
			onSuccessAction.forEach(action => void store.dispatch({ type: action, payload: response }))
		else
			store.dispatch({ type: onSuccessAction || `${baseAction}Succeed`, payload: response })

		setSubmitting(false)

		if(typeof redirectTo === 'string')
			history.push(redirectTo)
		else if(typeof redirectTo === 'function')
			history.push(redirectTo(response))
	}
	else if (status === 'error')
    store.dispatch({ type: `${baseAction}Failed`,  payload: response })
}
