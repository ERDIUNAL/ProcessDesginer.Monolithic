import "./LanguageSelect.scss";

import React, { useEffect } from "react";
import { useTranslation } from "react-i18next";
import { useDispatch, useSelector } from "react-redux";
import { ChangeLanguge } from "../../../store/actions/languageActions";

export default function LanguageSelect(props) {
  const { t, i18n } = useTranslation();
  const languageState = useSelector((state) => state.language);

  const dispatch = useDispatch();

  const onLanguageClick = (code) => {
    dispatch(ChangeLanguge(code));
    i18n.changeLanguage(code);
  };

  useEffect(() => {
    i18n.changeLanguage(languageState.activeKey);
  }, []);

  return (
    <div className="dropdown-center">
      <div
        className="dropdown-toggle d-inline text-white fw-bolder"
        id="LanguageDropdown"
        role="button"
        data-bs-toggle="dropdown"
        aria-haspopup="true"
        aria-expanded="false"
      >
        <img
          className="d-inline"
          src={`https://flagcdn.com/${t("language.select.code")}.svg`}
          alt=""
        />
        <span className="d-inline">{t("language.select.text")}</span>
      </div>
      <div className="dropdown-menu" aria-labelledby="LanguageDropdown">
        <div
          className="dropdown-item text-white fw-bolder"
          onClick={() => onLanguageClick("tr")}
        >
          <img className="d-inline" src="https://flagcdn.com/tr.svg" alt="" />
          <span className="d-inline">Türkçe</span>
        </div>
        <div
          className="dropdown-item text-white fw-bolder"
          onClick={() => onLanguageClick("en")}
        >
          <img className="d-inline" src="https://flagcdn.com/gb.svg" alt="" />
          <span className="d-inline">English</span>
        </div>
      </div>
    </div>
  );
}
