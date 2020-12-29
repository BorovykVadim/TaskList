import React, { useState } from 'react'
import { observer } from 'mobx-react'

import './Login.css'

const Login = observer(({ aStore }) => {
    const [show, setShow] = useState(false)

    const [formValues, setFormValues] = useState({
        username: '',
        password: ''
    })

    const showPass = () => {
        setShow(!show)
    }

    const usernameOnChange = e => {
        setFormValues({ ...formValues, username: e.target.value })
    }

    const passwordOnChange = e => {
        setFormValues({ ...formValues, password: e.target.value })
    }

    const onSubmit = e => {
        e.preventDefault()
        aStore.signIn(formValues.username, formValues.password)
        setFormValues({ username: '',   password:''})
    }

    return (
        <div>
            <form onSubmit={onSubmit} >
                <div className="field" name="username" >
                    <label>Username: </label>
                    <input value={formValues.username} onChange={usernameOnChange} />
                </div>
                <div className="field" name="password" >
                    <label>Password: </label>
                    <input value={formValues.password} onChange={passwordOnChange} type={show ? "" : "password"} />
                </div>
                <div className="check" ><input type="checkbox" onChange={showPass} /> Show password</div>
                <button className="submitButton" type="submit">Login</button>
            </form>
        </div>
    )
})

export default Login