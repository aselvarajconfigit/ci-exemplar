import React, { useState, useEffect } from 'react';

export interface UseFormHelper {
  values: any;
  setValues: React.Dispatch<any>;
  errors: any;
  setErrors: React.Dispatch<any>;
  handleInputChange( e: React.ChangeEvent<HTMLInputElement> ): void;
}

const useForm = ( initialValues: any ): UseFormHelper => {

  const [ values, setValues ] = useState( initialValues );
  const [ errors, setErrors ] = useState( {} );
  const handleInputChange = ( e: React.ChangeEvent<HTMLInputElement> ): void => {
    const { name, value } = e.target;
    setValues( {
      ...values,
      [name]: value
    } );
  }

  return {
    values,
    setValues,
    errors,
    setErrors,
    handleInputChange
  }

}

export default useForm;
