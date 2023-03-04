import React, { Component } from 'react';
import { Route, Routes } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import { Layout } from './components/Layout';
import { QueryClient, QueryClientProvider } from 'react-query';

import './App.scss';

const client = new QueryClient();

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <QueryClientProvider client={client}>
        <Layout>
          <Routes>
            {AppRoutes.map((route, index) => {
              const { element, ...rest } = route;
              return <Route key={index} {...rest} element={element} />;
            })}
          </Routes>
        </Layout>
      </QueryClientProvider>
    );
  }
}
