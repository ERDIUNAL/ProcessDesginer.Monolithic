import "./LoginPage.scss";

import React from "react";

import BaseInput from "../../../../shared/components/form-elements/base-input/BaseInput";

import { Link, useNavigate } from "react-router-dom";
import { Button } from "primereact/button";
import { Form, Formik } from "formik";
import * as Yup from "yup";

export default function LoginPage() {
  const navigate = useNavigate();
  const initialUserCredentials = { username: "", password: "" };
  const loginFormValidationSchema = Yup.object().shape({
    username: Yup.string().required("Kullanıcı Adı alanını boş geçemezsiniz"),
    password: Yup.string().required("Şifre alanını boş geçemezsiniz"),
  });
  const onFormSubmit = (values) => {
    //let loginService = new LoginService();
    //loginService.login(values).then((response) => {
    //  setItem("token", response.data.accessToken.token);
    //  navigate("/homepage");
    //});
    navigate("/homepage");
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
                      placeholder="Kullanıcı Adı"
                      name="username"
                      type="text"
                      icon="pi-user"
                    />
                  </div>
                  <div className="col-12 my-2">
                    <BaseInput
                      placeholder="Şifre"
                      name="password"
                      type="password"
                      icon="pi-key"
                    />
                  </div>
                  <div className="col-12 my-2">
                    <Button
                      className="w-100"
                      type="submit"
                      label="Giriş"
                      severity="info"
                    />
                  </div>
                </div>
              </Form>
            </Formik>
            <div className="w-100 text-center">
              <p>
                Don't have account?&nbsp;
                <Link to="/register">Sign Up</Link>
              </p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
