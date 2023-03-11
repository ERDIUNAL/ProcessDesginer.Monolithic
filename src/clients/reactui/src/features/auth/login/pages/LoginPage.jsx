import "./LoginPage.scss";

import React from "react";

import * as Yup from "yup";
import BaseInput from "../../../../shared/components/form-elements/base-input/BaseInput";
import { Form, Formik } from "formik";

import jwt_decode from "jwt-decode";
import LoginService from "../services/loginService";
import { setItem } from "../../../../core/utils/localStorage";
import { Login } from "../../../../store/actions/authActions";

import { Link, useNavigate } from "react-router-dom";
import { Button } from "primereact/button";

import { useTranslation } from "react-i18next";
import { useDispatch } from "react-redux";
import LanguageSelect from "../../../../shared/components/language-select/LanguageSelect";

import toastr from "toastr";

export default function LoginPage() {
  const dispatch = useDispatch();
  const { t } = useTranslation();

  const navigate = useNavigate();
  const initialUserCredentials = { email: "", password: "" };
  const loginFormValidationSchema = Yup.object().shape({
    email: Yup.string().required(t("login.email.error.empty")),
    password: Yup.string().required(t("login.password.error.empty")),
  });

  const onFormSubmit = (values) => {
    let loginService = new LoginService();
    loginService.login(values).then((response) => {
      setItem("token", response.data.accessToken.token);
      let userInfo = jwt_decode(response.data.accessToken.token);
      dispatch(Login(userInfo));
      navigate("/homepage");
    });
  };

  return (
    <div className="login-background">
      <div className="row content justify-content-center align-items-center">
        <div className="col-md-3 col-12">
          <div className="login-form">
            <div className="row justify-content-center">
              <div className="col-md-8 col-12">
                <div className="logo"></div>
              </div>
            </div>
            <Formik
              initialValues={initialUserCredentials}
              validationSchema={loginFormValidationSchema}
              onSubmit={onFormSubmit}
            >
              <Form>
                <div className="row">
                  <div className="col-12 my-2">
                    <BaseInput
                      placeholder={t("login.email")}
                      name="email"
                      type="text"
                      icon="pi-user"
                    />
                  </div>
                  <div className="col-12 my-2">
                    <BaseInput
                      placeholder={t("login.password")}
                      name="password"
                      type="password"
                      icon="pi-key"
                    />
                  </div>
                  <div className="col-12 my-2">
                    <Button
                      className="w-100"
                      type="submit"
                      label={t("login.submit")}
                      severity="info"
                    />
                  </div>
                </div>
              </Form>
            </Formik>
            <div className="w-100 text-center">
              <p>
                {t("login.haveaccount")}&nbsp;
                <Link to="/register">{t("login.signup")}</Link>
              </p>
            </div>
            <div className="w-100 text-center language-dropdown">
              <LanguageSelect></LanguageSelect>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
