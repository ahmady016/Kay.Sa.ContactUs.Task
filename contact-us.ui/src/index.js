import React from 'react'
import { render } from 'react-dom'

import { Router } from 'react-router-dom'
import { createBrowserHistory } from 'history'
import { Provider } from 'react-redux'

import { store } from './_store'

import App from './App'
import './index.css'

export const history = createBrowserHistory()

render(
	<React.StrictMode>
		<Router history={history}>
			<Provider store={store}>
				<App />
			</Provider>
		</Router>
	</React.StrictMode>,
	document.getElementById('root')
)
