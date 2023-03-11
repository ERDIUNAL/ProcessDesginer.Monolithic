import { USER_LOGIN, USER_LOGOUT } from "../constants/authConstants";
import { authState } from "../initialValues/authState";

export default function authReducer(state = authState, { type, payload }) {
  switch (type) {
    case USER_LOGIN:
      return { authenticated: true, user: payload };
    case USER_LOGOUT:
      return authState;
    default:
      return state;
  }
}
