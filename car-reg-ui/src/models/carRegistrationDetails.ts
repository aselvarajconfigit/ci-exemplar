export default interface CarRegistrationDetails {
  id: number;
  licensePlate: string;
  make: string;
  model: string;
  colour: string;
  engineCapacity: number;
  horsepower: number;
  registeredAt: Date;
}
