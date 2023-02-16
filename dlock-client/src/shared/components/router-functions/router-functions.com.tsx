import { NavigateFunction, useNavigate } from 'react-router-dom';

let staticNavigate: NavigateFunction = (args?: any) => { }; // Null function for fallback cases

const RouterFunctions = () => {
  staticNavigate = useNavigate();
  return null;
};

export default RouterFunctions;

export { staticNavigate };