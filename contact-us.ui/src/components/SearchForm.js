/* eslint-disable react-hooks/exhaustive-deps */
import React from 'react'

import request from '../_helpers/request'

import { Formik, Form, Field } from 'formik'
import * as Yup from 'yup'

import Autocomplete from '@material-ui/lab/Autocomplete'
import TextField from '@material-ui/core/TextField'
import { TextField as FormikTextField } from 'formik-material-ui'

import Grid from '@material-ui/core/Grid'
import Button from '@material-ui/core/Button'
import CircularProgress from '@material-ui/core/CircularProgress'

let whereClause = ''

const fields = [
  { name: 'name'},
  { name: 'email'},
  { name: 'phone'},
  { name: 'message'}
]

const initialSearchValues = {
  field: '',
  value: '',
}

const searchValidation = Yup.object().shape({
  field: Yup
    .string()
    .nullable()
    .required('Required'),
  value: Yup
    .string()
    .nullable()
    .required('Required'),
})

const handleSubmit = (setWhereClause, setPageNumber) => (values, { setSubmitting }) => {
  console.log("values", values)

  setPageNumber(1)
  if(values.field.name === 'message')
    setWhereClause(`${values.field.name}.contains("${values.value}")`)
  else
    setWhereClause(`${values.field.name}=="${values.value}"`)

  window.setTimeout(() => setSubmitting(false), 1000)
}

function TheSearchForm({ errors, touched, values, setFieldValue, setFieldTouched, submitForm, isSubmitting, dirty, isValid }) {
  return (
		<Form className='mt-1'>
			<Grid container spacing={3}>
				{/* field */}
				<Grid item sm={4} xs={12}>
					<Autocomplete
            selectOnFocus
            forcePopupIcon
            id="field"
            name="field"
            autoSelect
            options={fields}
            getOptionLabel={option => option.name || ''}
            value={values?.field}
            onChange={(_, value) => void setFieldValue('field', value)}
            onBlur={() => setFieldTouched('field', true)}
            renderInput={params => (
              <TextField
                name="field"
                label="Field"
                fullWidth
                {...params}
                inputProps={{ ...params.inputProps }}
                error={!!errors.field && touched.field}
                helperText={errors.field}
              />
            )}
					/>
				</Grid>
				{/* value */}
				<Grid item sm={4} xs={12}>
					<Field
						type="text"
						name="value"
						label="Value"
						component={FormikTextField}
						fullWidth
					/>
				</Grid>
				{/* submit button */}
				<Grid item sm={4} xs={12}>
					<Button
						className="w-100 mt-1"
						variant="contained"
						color="primary"
						disabled={!dirty || !isValid || isSubmitting}
						onClick={submitForm}
					>
						{isSubmitting ? <CircularProgress size={24} /> : 'Search'}
					</Button>
				</Grid>
      </Grid>
    </Form>
  )
}

function SearchForm({ setWhereClause, setPageNumber }) {
	return (
		<Formik
			initialValues={initialSearchValues}
			validationSchema={searchValidation}
			onSubmit={handleSubmit(setWhereClause, setPageNumber)}
			children={TheSearchForm}
		/>
	)
}

export default SearchForm
