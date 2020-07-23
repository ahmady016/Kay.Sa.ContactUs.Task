import { configureStore, getDefaultMiddleware } from '@reduxjs/toolkit'
import { combineReducers } from 'redux'
import logger from 'redux-logger'

import inquiriesReducer from './inquiries'

const rootReducer = combineReducers({
  inquiries: inquiriesReducer,
})

export const store = configureStore({
  reducer: rootReducer,
  middleware: [
    ...getDefaultMiddleware(),
    logger
  ]
})
