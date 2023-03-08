import instance from "../../../../core/utils/axiosInterceptors";
import { BASE_API_URL } from "../../../../enviroment";

export default class loginService {
  login(credentials) {
    return instance.post(BASE_API_URL + "Auth/Login", credentials);
  }
}
