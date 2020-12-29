import React from 'react'
import { observer, inject } from 'mobx-react'

const Logout = inject('aStore')(observer(({ aStore }) => {
    const onClickLogoutButton = () => {
        aStore.signOut()
    }

    return (
        <button className= "logoutButton" onClick={onClickLogoutButton}>
            Logout
        </button>
    )
}))

export default Logout