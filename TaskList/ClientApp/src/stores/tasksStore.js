import { action, makeObservable, observable, runInAction } from 'mobx'

import history from '../history'
import apis from '../apis/task-list'

export const tasksStore = () => {
    return makeObservable({
        tasks: null,
        tasksCount: 0,
        fetchTasks(pageSize, pageNumber) {
            apis.get(`/tasks?pageSize=${pageSize}&pageNumber=${pageNumber}`)
                .then(
                    response => {
                        runInAction(() => {
                            this.tasks = response.data.tasks
                            this.tasksCount = response.data.tasksCount
                        })
                    }
                )
        },
        createTask(taskName) {
            var token  = JSON.parse(localStorage.getItem('authorization'))
            apis.defaults.headers.common['Authorization'] = `Bearer ${token.jwtToken}`
            apis.post('/tasks', { name: taskName })
                .then(
                    response => {
                        runInAction(() => {
                            this.tasks.push(response.data)
                        })
                    }
                )
            history.push('/tasks')
        }
    }, {
        tasks: observable,
        tasksCount: observable,
        fetchTasks: action.bound,
        createTask: action.bound
    })
}