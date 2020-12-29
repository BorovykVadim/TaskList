import React from 'react'
import { Link, useLocation } from 'react-router-dom'
import { observer } from 'mobx-react'

import Logout from './auth/Logout'

import './App.css'

const Header = observer(({ aStore }) => {
    const location = useLocation()

    const renderButton = () => {
        if(location.pathname === '/login') {
            return <Link to='/tasks' className="loginButton" >Tasks</Link>
        } else if (location.pathname === '/tasks' || location.pathname === '/tasks/create') {
            return <Link to='/login' className="loginButton" >Login</Link>
        }
    }

    const renderLogin = () => {
        if (aStore.user) {
            return (
                <div className="header">
                    <Logout aStore={aStore} />
                </div>
            )
        } else {
            return(
                <div className="header">
                    {renderButton()}
                </div>
            )
        }
    }

    return (
        <>{renderLogin()}</>
    )
})

export default Header