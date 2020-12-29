import React from 'react'
import { Route, Router, Switch } from 'react-router-dom'

import history from '../history'
import Header from './Header'
import Login from './auth/Login'
import TaskList from './task/TaskList'
import CreateTask from './task/CreateTask'

import { authStore, tasksStore } from '../stores'

const aStore = authStore();
const tStore = tasksStore();

const App = () => {
    return (
        <div>
            <Router history={history}>
                <Header aStore={aStore} />
                <div>
                    <Switch>
                        <Route path='/login' exact component={() => <Login aStore={aStore} />} />
                        <Route 
                            path='/tasks'
                            exact 
                            component={() => 
                                <TaskList 
                                    aStore={aStore} 
                                    tStore={tStore} 
                                />
                            }
                        />
                        <Route 
                            path='/tasks/create' 
                            exact 
                            component={() => 
                                <CreateTask 
                                    tStore={tStore}
                                />
                            } 
                        />
                    </Switch>
                </div>
            </Router>
        </div>
    )
}

export default App