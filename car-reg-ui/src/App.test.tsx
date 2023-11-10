import React from 'react';
import { render } from '@testing-library/react';
import App from './App';

test( 'renders form header', () => {
  const { getByText } = render( <App /> );
  const linkElement = getByText( /Car Registration Form/i );
  expect( linkElement ).toBeInTheDocument();
} );