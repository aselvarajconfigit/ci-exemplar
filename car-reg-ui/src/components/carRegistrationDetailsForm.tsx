import React, { ReactElement, useState } from 'react';
import * as actions from '../actions/CarRegistrationDetails';
import { connect } from 'react-redux';
import {
  Grid, TextField, withStyles, InputAdornment, Button
} from '@material-ui/core';

import useForm from './useForm';
import { prependOnceListener } from 'cluster';

const styles = ( theme: any ): any => ( {
  root: {
    '& .MuiTextField-root': {
      margin: theme.spacing( 1 ),
      minWidth: 230
    }
  }
} );

const initialState = {
  id: 0,
  licensePlate: '',
  make: '',
  model: '',
  colour: '',
  engineCapacity: 1.0,
  horsepower: 100,
  registeredAt: Date.now()
}

const CarRegistrationDetailsForm = ( props: any ): ReactElement => {
  
  const { values, setValues, errors, setErrors, handleInputChange } = useForm( initialState );

  const validate = (): boolean => {
    const temp = {} as any;
    temp.licensePlate = values.licensePlate.length === 7 ? '' : 'License plate must be 7 characters';
    temp.make = values.make.length !== 0 ? '' : 'Car make is required';
    temp.model = values.model.length !== 0 ? '' : 'Car model is required';
    temp.colour = values.colour.length !== 0 ? '' : 'Colour is required';
    temp.engineCapacity = values.engineCapacity >= 0 ? '' : 'Engine capacity must have a positive value';
    temp.horsepower = values.horsepower >= 0 ? '' : 'Horsepower must have a positive value';
    setErrors( {
      ...temp
    } );
    
    return Object.values( temp ).every( x => x === '' );
  }

  const handleSubmit = async ( e: React.FormEvent<HTMLFormElement> ): Promise<void> => {
    e.preventDefault();
    if ( validate() ) {
      props.createCarRegistration( values, () => {
        console.log( 'inserted' ) 
      } ); 
    }
  }

  return (
    <form autoComplete='off' noValidate className={props.classes.root} onSubmit={handleSubmit}>
      <Grid container>
        <Grid item xs={6}>
          <TextField
            name='licensePlate'
            variant='outlined'
            label='License Plate'
            value={values.licensePlate}
            onChange={handleInputChange}
            error={!!errors.licensePlate}
            helperText={errors.licensePlate}/>
          <TextField
            name='make'
            variant='outlined'
            label='Make'
            value={values.make}
            onChange={handleInputChange}
            error={!!errors.make}
            helperText={errors.make}/>
          <TextField
            name='model'
            variant='outlined'
            label='Model'
            value={values.model}
            onChange={handleInputChange}
            error={!!errors.model}
            helperText={errors.model}/>
        </Grid>
        <Grid item xs={6}>
          <TextField
            name='colour'
            variant='outlined'
            label='Colour'
            value={values.colour}
            onChange={handleInputChange}
            error={!!errors.colour}
            helperText={errors.colour}/>
          <TextField
            name='engineCapacity'
            variant='outlined'
            label='Engine Capacity'
            InputProps={{endAdornment:<InputAdornment position='end'>L</InputAdornment>}}
            value={values.engineCapacity}
            onChange={handleInputChange}
            error={!!errors.engineCapacity}
            helperText={errors.engineCapacity}/>
          <TextField
            name='horsepower'
            variant='outlined'
            label='Horsepower'
            InputProps={{ type:'number', endAdornment:<InputAdornment position='end'>HP</InputAdornment>}}
            value={values.horsepower}
            onChange={handleInputChange}
            error={!!errors.horsepower}
            helperText={errors.horsepower}/>
          <div>
            <Button
              variant='contained'
              color='primary'
              type='submit'
              style={{marginLeft:'10px', marginRight:'10px'}}>
              Submit
            </Button>
            <Button
              variant='contained'
              style={{marginLeft:'10px', marginRight:'10px'}}>
              Reset
            </Button>
          </div>
        </Grid>
      </Grid>
    </form>
  );
}

const mapStateToProps = ( state: any ): any => {
  return {
    registrationDetailsList: state.carRegistrationDetailsReducer.list
  }
}

const mapActionToProps = {
  createCarRegistration: actions.create,
  updateCarRegistration: actions.Update
}


export default connect( mapStateToProps, mapActionToProps )( withStyles( styles )( CarRegistrationDetailsForm ) );
