import React from 'react'

import './Task.css'

const Task = ({ task }) => {
    var date = new Date(task.dateOfCreation)

    const renderTime = () => {
        if(task.isDone){
            return (
                <div className="field" >
                    <b>Time: </b>{task.time}s
                </div>
            )
        }
    }

    return (
        <div className="task" >
            <div className="field" >
                <b>№</b> {task.id}
            </div>
            <div className="field" >
                <b>Name:</b> {task.name}
            </div>
            <div className="field" >
                <b>Owner: </b> {task.authorName}
            </div>
            <div className="field" >
                <b>Created: </b>{`${date.getDate()}.${date.getMonth() + 1}.${date.getFullYear()} at ${date.getHours()}:${date.getMinutes()}`}
            </div>
            <div className="field" >
                <b>Status: </b>{task.isDone ? 'Done' : 'In process'}
            </div>
            {renderTime()}
        </div>
    )
}

export default Task