import { ACTION_TYPES } from '../actions/CarRegistrationDetails';
import ActionData from '../models/ActionData';
import CarRegistrationDetails from '../models/carRegistrationDetails';

export interface CarRegistrationReducerState {
  list: Array<CarRegistrationDetails>;
}

const initialState = {
  list: []
}

export const carRegistrationDetailsReducer = ( state: CarRegistrationReducerState = initialState, action: ActionData ): any => {
  switch ( action.type ) {
    case ACTION_TYPES.FETCH_ALL:
      return {
        ...state,
        list: [...action.payload]
      }
    case ACTION_TYPES.CREATE: 
      return {
        ...state,
        list: [...state.list, action.payload]
      }
    case ACTION_TYPES.UPDATE: 
      return {
        ...state,
        list: state.list.map( x => x.id === action.payload.id ? action.payload : x )
      }
    case ACTION_TYPES.DELETE: 
      return {
        ...state,
        list: state.list.filter( x => x.id != action.payload.id )
      }
    default:
      return state;
  }
}
