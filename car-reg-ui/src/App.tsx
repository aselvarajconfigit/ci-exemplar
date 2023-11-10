import React, { ReactElement } from 'react';
import './App.css';
import { store } from './actions/store';
import { Provider } from 'react-redux';
import CarRegistrationDetailsComponent from './components/carRegistrationDetailsComponent';
import { Container } from '@material-ui/core';

function App(): ReactElement {
  return (
    <Provider store={store}>
      <Container maxWidth='lg'>
        <CarRegistrationDetailsComponent />
      </Container>
    </Provider>
  );
}

export default App;
