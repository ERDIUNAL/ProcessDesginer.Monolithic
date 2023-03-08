import { CHANGE_LANGUAGE } from "../constants/languageCostants";
import { languageState } from "../initialValues/languageState";

export default function languageReducer(
  state = languageState,
  { type, payload }
) {
  switch (type) {
    case CHANGE_LANGUAGE:
      return { activeKey: payload };
    default:
      return state;
  }
}
