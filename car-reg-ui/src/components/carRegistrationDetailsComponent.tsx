import React, { ReactElement, useEffect } from 'react';
import { connect } from 'react-redux';
import * as actions from '../actions/CarRegistrationDetails';
import CarRegistrationDetails from '../models/carRegistrationDetails';
import CarRegistrationDetailsForm from './carRegistrationDetailsForm';
import {
  Grid, Paper, TableHead, TableRow, TableCell,
  Table, TableContainer, TableBody, withStyles,
  AppBar, Toolbar, Typography
} from '@material-ui/core';

const styles = ( theme: any ): any => ( {
  root: {
    '& .MuiTableCell-head': {
      fontSize: '1.25rem'
    }
  },
  paper: {
    margin: theme.spacing( 2 ),
    padding: theme.spacing( 2 )
  }
} );

const CarRegistrationDetailsComponent = ( props: any ): ReactElement => {
  

  useEffect( () => {
    props.fetchAllCarRegistrations()
  }, [] );

  return (
    <div>
      <AppBar style={{ marginTop: '16px', marginLeft: '16px', marginRight: '16px', width:'unset'}} position='static'>
        <Toolbar variant='dense'>
          <Typography variant='h6' color='inherit'>
          Car Registration Form
          </Typography>
        </Toolbar>
      </AppBar>
      <Paper style={{marginTop: '0px'}}className={props.classes.paper} elevation={3}>
        <Grid container>
          <Grid item xs={6}>
            <CarRegistrationDetailsForm/>
          </Grid>
          <Grid item xs={6}>
            <TableContainer>
              <Table>
                <TableHead className={props.classes.root}>
                  <TableRow>
                    <TableCell>License Plate</TableCell>
                    <TableCell>Make</TableCell>
                    <TableCell>Model</TableCell>
                    <TableCell>Colour</TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {
                    props.registrationDetailsList.map( ( item: CarRegistrationDetails, idx: number ) => {
                      return (
                        <TableRow key={idx} hover>
                          <TableCell>{item.licensePlate}</TableCell>
                          <TableCell>{item.make}</TableCell>
                          <TableCell>{item.model}</TableCell>
                          <TableCell>{item.colour}</TableCell>
                        </TableRow>
                      );
                    } )
                  }
                </TableBody>
              </Table>
            </TableContainer>
          </Grid>
        </Grid>
      </Paper>
    </div>
  )
}

const mapStateToProps = ( state: any ): any => {
  return {
    registrationDetailsList: state.carRegistrationDetailsReducer.list
  }
}

const mapActionToProps = {
  fetchAllCarRegistrations: actions.fetchAll
}

export default
connect( mapStateToProps, mapActionToProps )( withStyles( styles )( CarRegistrationDetailsComponent ) );
