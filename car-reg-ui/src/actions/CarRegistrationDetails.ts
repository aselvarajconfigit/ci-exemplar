import CarRegistrationDetails from '../models/carRegistrationDetails';
import ActionData from '../models/ActionData';
import api from './api';


export const ACTION_TYPES = {
  CREATE: 'CREATE',
  UPDATE: 'UPDATE',
  DELETE: 'DELETE',
  FETCH_ALL: 'FETCH_ALL'
}

export const fetchAll = () => ( dispatch: ( data: ActionData ) => void ): void => {
  api.CarRegistrationDetail().fetchAll()
    .then( ( response ) => {
      dispatch( {
        type: ACTION_TYPES.FETCH_ALL,
        payload: response.data
      } );
    } )
    .catch( ( err ) => {
      console.log( err ); 
    } )
  

}

export const create = ( data: CarRegistrationDetails, onSuccess: any ) => ( dispatch: ( data: ActionData ) => void ): void => {
  data.horsepower = Number( data.horsepower );
  data.engineCapacity = Number( data.engineCapacity );
  data.registeredAt = new Date();
  api.CarRegistrationDetail().create( data )
    .then( ( response: any ) => {
      dispatch( {
        type: ACTION_TYPES.CREATE,
        payload: response.data
      } );
      onSuccess();
    } )
    .catch( ( err: any ) => {
      console.log( err ); 
    } )
}

export const Update = ( id: number, data: any, onSuccess: any ) => ( dispatch: ( data: ActionData ) => void ): void => {
  api.CarRegistrationDetail().update( id, data )
    .then( ( response: any ) => {
      dispatch( {
        type: ACTION_TYPES.UPDATE,
        payload: {id: id, ...data}
      } );
      onSuccess();
    } )
    .catch( ( err: any ) => {
      console.log( err ); 
    } )
}

export const Delete = ( id: number, onSuccess: any ) => ( dispatch: ( data: ActionData ) => void ): void => {
  api.CarRegistrationDetail().delete( id )
    .then( ( response: any ) => {
      dispatch( {
        type: ACTION_TYPES.DELETE,
        payload: id
      } );
      onSuccess();
    } )
    .catch( ( err: any ) => {
      console.log( err ); 
    } )
}
