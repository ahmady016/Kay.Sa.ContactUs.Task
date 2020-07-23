import { createSlice } from '@reduxjs/toolkit'
import _ from 'lodash'

import { UI_INIT_STATE, UI_LOADING_STATE, UI_ERROR_STATE } from '../_helpers'

const inquiriesSlice = createSlice({
  name: 'inquiries',
  initialState: {
    getInquiriesUI: UI_INIT_STATE(),
    getInquiriesPageUI: UI_INIT_STATE(),
    getInquiryUI: UI_INIT_STATE(),
    addInquiryUI: UI_INIT_STATE(),
    updateInquiryUI: UI_INIT_STATE(),
    deleteInquiryUI: UI_INIT_STATE(),
    list: {}
  },
  reducers: {
    getInquiriesSent: (state, _) => {
      state.getInquiriesUI = UI_LOADING_STATE()
    },
    getInquiriesFailed: (state, { payload }) => {
      state.getInquiriesUI = UI_ERROR_STATE(payload)
    },
    getInquiriesSucceed: (state, { payload }) => {
      state.getInquiriesUI = UI_INIT_STATE()
      state.list = _.mapKeys(payload, 'id')
    },
    getInquiriesPageSent: (state, _) => {
      state.getInquiriesPageUI = UI_LOADING_STATE()
    },
    getInquiriesPageFailed: (state, { payload }) => {
      state.getInquiriesPageUI = UI_ERROR_STATE(payload)
    },
    getInquiriesPageSucceed: (state, { payload }) => {
      state.getInquiriesPageUI = UI_INIT_STATE()
      state.list = _.mapKeys(payload.pageItems, 'id')
    },
    getInquirySent: (state, _) => {
      state.getInquiryUI = UI_LOADING_STATE()
    },
    getInquiryFailed: (state, { payload }) => {
      state.getInquiryUI = UI_ERROR_STATE(payload)
    },
    getInquirySucceed: (state, { payload }) => {
      state.getInquiryUI = UI_INIT_STATE()
      state.list[payload.id] = payload
    },
    addInquirySent: (state, _) => {
      state.addInquiryUI = UI_LOADING_STATE()
    },
    addInquiryFailed: (state, { payload }) => {
      state.addInquiryUI = UI_ERROR_STATE(payload)
    },
    addInquirySucceed: (state, { payload }) => {
      state.addInquiryUI = UI_INIT_STATE()
      state.list[payload.id] = payload
    },
    updateInquirySent: (state, _) => {
      state.updateInquiryUI = UI_LOADING_STATE()
    },
    updateInquiryFailed: (state, { payload }) => {
      state.updateInquiryUI = UI_ERROR_STATE(payload)
    },
    updateInquirySucceed: (state, { payload }) => {
      state.updateInquiryUI = UI_INIT_STATE()
      state.list[payload.id] = payload
    },
    deleteInquirySent: (state, _) => {
      state.deleteInquiryUI = UI_LOADING_STATE()
    },
    deleteInquiryFailed: (state, { payload }) => {
      state.deleteInquiryUI = UI_ERROR_STATE(payload)
    },
    deleteInquirySucceed: (state, { payload }) => {
      state.deleteInquiryUI = UI_INIT_STATE()
      console.log("payload", payload)
      state.list = _.omit(state.list, payload.id)
    }
  }
})

export const actions = inquiriesSlice.actions
export default inquiriesSlice.reducer
