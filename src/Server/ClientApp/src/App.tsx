import * as React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import TodoList from './components/TodoList';

import './custom.css'


export default () => (
    <Layout>
        <Route exact path='/' component={TodoList} />
    </Layout>
);
