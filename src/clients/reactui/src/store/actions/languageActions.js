import { CHANGE_LANGUAGE } from "../constants/languageCostants";

export function ChangeLanguge(language) {
  return {
    type: CHANGE_LANGUAGE,
    payload: language,
  };
}
