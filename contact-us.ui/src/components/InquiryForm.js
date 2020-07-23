/* eslint-disable react-hooks/exhaustive-deps */
import React from 'react'

import request from '../_helpers/request'

import { Formik, Form, Field } from 'formik'
import * as Yup from 'yup'
import { TextField } from 'formik-material-ui'

import Typography from '@material-ui/core/Typography'
import Grid from '@material-ui/core/Grid'
import Button from '@material-ui/core/Button'
import CircularProgress from '@material-ui/core/CircularProgress'

const initialInquiryValues = {
	name: '',
	email: '',
	phone: '',
	message: '',
}

const InquiryValidation = Yup.object().shape({
	name: Yup.string()
		.required('Required')
		.min(5, 'Too Short Name')
		.max(100, 'Too Long Name'),
	email: Yup.string()
		.required('Required')
		.email('not a valid Email'),
	phone: Yup.string()
		.required('Required')
		.min(11, 'Too Short Phone')
		.max(11, 'Too Long Phone'),
	message: Yup.string()
		.required('Required')
		.min(10, 'Too Short notes')
		.max(1000, 'Too Long notes'),
})

const handleSubmit = (values, { setSubmitting }) => {
	request({
		request: ['post', '/Inquiries/AddItem', values],
		baseAction: 'Inquiries/addInquiry',
		redirectTo: '/Inquiries',
		setSubmitting,
	})
}

function TheInquiryForm({ submitForm, isSubmitting, dirty, isValid }) {
	return (
		<Form className='mt-1'>
			<Typography variant="h5">Add New Inquiry</Typography>
			<Grid container spacing={3}>
				<Grid item md={6} xs={12}>
					<Field
						type="text"
						name="name"
						label="Name"
						fullWidth
						component={TextField}
					/>
				</Grid>
				<Grid item md={6} xs={12}>
					<Field
						type="text"
						name="email"
						label="Email"
						fullWidth
						component={TextField}
					/>
				</Grid>
				<Grid item md={6} xs={12}>
					<Field
						type="text"
						name="phone"
						label="Phone"
						fullWidth
						component={TextField}
					/>
				</Grid>
				<Grid item md={6} xs={12}>
					<Field
						type="text"
						name="message"
						label="Message"
						fullWidth
						multiline
						component={TextField}
					/>
				</Grid>
				<Grid item md={6} xs={12}>
					<Button
						className="w-100 mt-1"
						variant="contained"
						color="primary"
						disabled={!dirty || !isValid || isSubmitting}
            onClick={submitForm}
					>
						{isSubmitting ? <CircularProgress size={24} /> : 'Submit'}
					</Button>
				</Grid>
			</Grid>
		</Form>
	)
}

function InquiryForm() {
	return (
		<Formik
			initialValues={initialInquiryValues}
			validationSchema={InquiryValidation}
			onSubmit={handleSubmit}
			component={TheInquiryForm}
		/>
	)
}

export default InquiryForm
