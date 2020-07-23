import React from 'react'
import { Link as RouterLink } from 'react-router-dom'

import { useSelector } from 'react-redux'
import request from '../_helpers/request'

import CircularProgress from '@material-ui/core/CircularProgress'

import Paper from '@material-ui/core/Paper'
import Chip from '@material-ui/core/Chip'
import Avatar from '@material-ui/core/Avatar'
import Link from '@material-ui/core/Link'

import List from '@material-ui/core/List'
import ListItem from '@material-ui/core/ListItem'
import ListItemText from '@material-ui/core/ListItemText'
import ListItemAvatar from '@material-ui/core/ListItemAvatar'
import ImageIcon from '@material-ui/icons/Image'
import ListItemSecondaryAction from '@material-ui/core/ListItemSecondaryAction'
import IconButton from '@material-ui/core/IconButton'
import DeleteIcon from '@material-ui/icons/Delete'
import EditIcon from '@material-ui/icons/Edit'
import Divider from '@material-ui/core/Divider'

import Alert from '@material-ui/lab/Alert'

import styled from 'styled-components'
const ListItemIcon = styled(Avatar)`
  width: 60px !important;
  height: 60px !important;
	margin-right: 1rem;
`

const deleteInquiry = item => e => {
	if(window.confirm('Are you sure you want to delete this Inquiry ?')) {
		request({
			request: ['post', '/Inquiries/DeleteItem', item],
			baseAction: 'inquiries/deleteInquiry',
		})
	}
}

function InquiryItem({ id, name, email, phone, message }) {
	return (
		<React.Fragment key={id}>
			<ListItem dense>
				<ListItemAvatar>
					<ListItemIcon><ImageIcon fontSize="large" /></ListItemIcon>
				</ListItemAvatar>
				<ListItemText
					id={id}
					primary={name}
					secondary={
						<>
							<div className="my-05">{message}</div>
							<div className="flex-between">
								<span><strong>Email: </strong>{email}</span>
								<span><strong>Phone: </strong>{phone}</span>
							</div>
						</>
					}
				/>
				<ListItemSecondaryAction>
					<IconButton color='secondary' edge="end" aria-label="delete" onClick={deleteInquiry({ id, name, email, phone, message })}>
						<DeleteIcon />
					</IconButton>
				</ListItemSecondaryAction>
			</ListItem>
			<Divider />
		</React.Fragment>
	)
}

function InquiriesList() {
  const [pageNumber, setPageNumber] = React.useState(1)

	const { getInquiriesPageUI, inquiries } = useSelector((state) => ({
		getInquiriesPageUI: state.inquiries.getInquiriesPageUI,
		inquiries: Object.values(state.inquiries.list),
	}))

	React.useEffect(() => {
    const handleScroll = e => {
      if (window.innerHeight + window.scrollY >= document.body.offsetHeight)
        setPageNumber((pageNumber) => pageNumber + 1)
    }
    window.addEventListener('scroll', handleScroll)

		request({
      request: ['get', `/Inquiries/ListPage/existed?pageSize=10&pageNumber=${pageNumber}`],
			baseAction: 'inquiries/getInquiriesPage',
    })

    return () => window.removeEventListener('scroll', handleScroll)
	}, [])

	if (getInquiriesPageUI.loading)
		return (
			<div className="w-100 h-100-vh flex-center">
				<CircularProgress disableShrink />
			</div>
		)

	else if (getInquiriesPageUI.error)
		return (
				<Alert severity="error">{getInquiriesPageUI.error.message || 'Something went wrong!'}</Alert>
		)

	return (
		<>
			<Paper className="flex-between p-1">
				<Chip
					color="default"
					variant="outlined"
					avatar={<Avatar>{inquiries?.length}</Avatar>}
					label="All"
				/>
				<Link component={RouterLink} to="/contact-us">
					Add New Inquiry
				</Link>
			</Paper>
			<List component="div" aria-label="Inquiries list">
				{inquiries.map(inquiry => <InquiryItem key={inquiry.id} {...inquiry} /> )}
			</List>
		</>
	)
}

export default InquiriesList
