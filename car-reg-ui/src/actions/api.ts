import axios, { AxiosResponse } from 'axios';
import CarRegistrationDetails from '../models/carRegistrationDetails';

const baseUrl = 'http://localhost:1337/api/';

export interface CarRegistrationDetailService {
  fetchAll(): Promise<AxiosResponse<CarRegistrationDetails[]>>;
  fetchById( id: string ): Promise<AxiosResponse<CarRegistrationDetails>>;
  create( newRecord: CarRegistrationDetails ): Promise<AxiosResponse<any>>;
  update( id: number, updatedRecord: CarRegistrationDetails ): Promise<AxiosResponse<void>>;
  delete( id: number ): Promise<AxiosResponse<void>>;
}

export default {

  CarRegistrationDetail( url = `${baseUrl}carRegistrationDetails/` ): CarRegistrationDetailService {
    return {
      fetchAll: (): Promise<AxiosResponse<CarRegistrationDetails[]>> => axios.get( url ),
      fetchById: ( id: string ): Promise<AxiosResponse<CarRegistrationDetails>> => axios.get( `$url${id}` ),
      create: ( newRecord: CarRegistrationDetails ): Promise<AxiosResponse<any>> => axios.post( url, newRecord ),
      update: ( id: number, updatedRecord: CarRegistrationDetails ): Promise<AxiosResponse<void>> => axios.put( url, updatedRecord ),
      delete: ( id: number ): Promise<AxiosResponse<void>> => axios.delete( `url${id}` )
    }
  }
};
