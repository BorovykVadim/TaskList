import React from 'react'
import { Route, Router, Switch } from 'react-router-dom'
import { Provider } from 'mobx-react'
import { useStrict } from 'mobx'

import history from '../history'
import Header from './Header'
import Login from './auth/Login'
import TaskList from './task/TaskList'
import CreateTask from './task/CreateTask'

import tStore from '../stores/tasksStore'
import aStore from '../stores/authStore'

const stores = { tStore, aStore }

const App = () => {
    return (
        <Provider {...stores} >
            <div>
                <Router history={history}>
                    <Header/>
                    <div>
                        <Switch>
                            <Route path='/login' exact component={() => <Login />} />
                            <Route 
                                path='/tasks'
                                exact 
                                component={() => 
                                    <TaskList />
                                }
                            />
                            <Route 
                                path='/tasks/create' 
                                exact 
                                component={() => 
                                    <CreateTask />
                                } 
                            />
                        </Switch>
                    </div>
                </Router>
            </div>
        </Provider>
    )
}

export default App