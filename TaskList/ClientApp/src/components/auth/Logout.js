import React from 'react'
import { observer } from 'mobx-react'

const Logout = observer(({ aStore }) => {
    const onClickLogoutButton = () => {
        aStore.signOut()
    }

    return (
        <button className= "logoutButton" onClick={onClickLogoutButton}>
            Logout
        </button>
    )
})

export default Logout