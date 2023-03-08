import { combineReducers } from "redux";
import authReducer from "./reducers/authReducer";
import languageReducer from "./reducers/languageReducer";

const rootReducer = combineReducers({
  auth: authReducer,
  language: languageReducer,
});

export default rootReducer;
