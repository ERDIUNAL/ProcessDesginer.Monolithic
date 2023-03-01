import { USER_LOGIN } from "../constants/authConstants";
import { authState } from "../initialValues/authState";

export default function authReducer(state = authState, { type, payload }) {
  switch (type) {
    case USER_LOGIN:
      return { authenticated: true, user: payload };
    default:
      return state;
  }
}
