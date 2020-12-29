import React, { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { observer } from 'mobx-react'

import Task from './Task'

const TaskList = observer(({ aStore, tStore }) => {

    const pageSize = 15;
    const [page, setPage] = useState(1)

    useEffect(() => {
        tStore.fetchTasks(pageSize, page)
    }, [tStore, page])

    const nextPageOnClick = () => {
        setPage(page + 1)
    }

    const prevPageOnClick = () => {
        setPage(page - 1)
    }

    const returnAddTaskButton = () => {
        if(aStore.user) {
            return <Link to='/tasks/create' className="createButton" >Add Task</Link>
        }
    }

    if(!tStore.tasks) {
        return <div>loading...</div>
    }

    const renderPrevPageButton = () => {
        if (page > 1) {
            return <button onClick={prevPageOnClick} className="page pageBtn">{`<-`}</button>
        }
    }

    const renderNextPageButton = () => {
        if ((tStore.tasksCount / pageSize) > page) {
            return <button onClick={nextPageOnClick} className="page pageBtn">{`->`}</button>
        }
    }

    return (
        <div>
            <div className="tasksList">
                {tStore.tasks.map(task => {
                    return (
                        <div key={task.id} ><Task task={task} /></div>
                    )
                })}
            </div>
            <div>
                {returnAddTaskButton()}
            </div>
            <div style={{ display: 'flex' }}>
                {renderPrevPageButton()}
                <div className="page">{page}</div>
                {renderNextPageButton()}
            </div>
        </div>
    )
})

export default TaskList