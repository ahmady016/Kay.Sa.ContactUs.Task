import React from 'react'
import { Switch, Route, Redirect, withRouter } from 'react-router-dom'

import Container from '@material-ui/core/Container'

import Header from './components/Header'
import InquiryForm from './components/InquiryForm'
import InquiriesList from './components/InquiriesList'

function App() {
	return (
		<>
			<Header title="ContactUs App" />
			<Container>
				<Switch>
					<Route path="/contact-us" component={InquiryForm} />
          <Route path="/inquiries" component={InquiriesList} />
          <Redirect to="/contact-us" />
				</Switch>
			</Container>
		</>
  )
}

export default withRouter(App)
