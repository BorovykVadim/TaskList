import React, { useState } from 'react'
import { observer } from 'mobx-react'

const CreateTask = observer(({ tStore }) => {
    const [name, setName] = useState('')

    const onChangeName = e => {
        setName(e.target.value)
    }

    const onSubmitTaskForm = e => {
        e.preventDefault()
        tStore.createTask(name)
        setName('')
    }

    return (
        <form onSubmit={onSubmitTaskForm} >
            <div>
                <label>Name: </label>
                <input onChange={onChangeName} value={name} />
            </div>
            <button className="submitButton" type="submit">Create Task</button>
        </form>
    )
})

export default CreateTask