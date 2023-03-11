import axios from "axios";
import { BASE_API_URL } from "../../enviroment";
import {
  AUTH_EXCEPTION,
  BUSINESS_EXCEPTION,
  VALIDATION_EXCEPTION,
} from "../enums/exceptionTypes";
import {
  handleAuthException,
  handleBusinessException,
  handleValidationException,
  handleDefaultException,
} from "./exeptionHandlers";
import { getItem } from "./localStorage";

const instance = axios.create({
  baseURL: BASE_API_URL,
});

instance.interceptors.request.use((config) => {
  config.headers.Authorization = `Bearer ${getItem("token")}`;

  return config;
});

instance.interceptors.response.use(
  (response) => {
    return response;
  },
  (error) => {
    console.log(error);

    if (error.code === "ERR_NETWORK") {
      handleDefaultException(error.message);
    } else {
      switch (error.response.data.Type) {
        case BUSINESS_EXCEPTION:
          handleBusinessException(error.response.data);
          break;
        case VALIDATION_EXCEPTION:
          handleValidationException(error.response.data);
          break;
        case AUTH_EXCEPTION:
          handleAuthException();
          break;
        default:
          break;
      }
    }

    return Promise.reject(error);
  }
);

export default instance;
