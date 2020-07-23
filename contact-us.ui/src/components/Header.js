import React from 'react'
import { Link as RouterLink } from 'react-router-dom'

import AppBar from '@material-ui/core/AppBar'
import Toolbar from '@material-ui/core/Toolbar'
import Typography from '@material-ui/core/Typography'
import IconButton from '@material-ui/core/IconButton'
import MenuIcon from '@material-ui/icons/Menu'
import Link from '@material-ui/core/Link'

function Header({ title }) {
	return (
		<header>
			<AppBar position="static">
				<Toolbar className="flex-between">
					<div className="flex-b-30 flex">
						<IconButton edge="start" color="inherit" aria-label="menu">
							<MenuIcon />
						</IconButton>
						<Typography variant="h5">{title}</Typography>
					</div>
					<div className="flex-b-40">
						<Link className="mx-1" component={RouterLink} to="/contact-us">
							ContactUs
						</Link>
						<Link className="mx-1" component={RouterLink} to="/Inquiries">
							InquiriesList
						</Link>
					</div>
				</Toolbar>
			</AppBar>
		</header>
	)
}

export default Header
