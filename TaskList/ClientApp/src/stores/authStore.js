import { action, makeObservable, observable, runInAction } from 'mobx'

import history from '../history'
import apis from '../apis/task-list'

export const authStore = () => {
    return makeObservable({
        user: localStorage.getItem('authorization') ?? null,
        signIn(userName, password) {
            apis.post(`/auth/login`, { userName, password })
                .then(
                    response => {
                        runInAction(() => {
                            localStorage.setItem('authorization', JSON.stringify({
                                jwtToken: response.headers['authorization'],
                                userId: response.data.id,
                                userName: response.data.userName
                            }))
                            this.user = response.data
                            history.push('/tasks') 
                        })
                    }
                )
        },
        signOut() {
            apis.get('/auth/logout')
                .then(() => {
                    localStorage.removeItem('authorization')
                    this.user = null
                    history.push('/tasks') 
                })
        }
    }, {
        user: observable,
        signIn: action.bound,
        signOut: action.bound
    });
}