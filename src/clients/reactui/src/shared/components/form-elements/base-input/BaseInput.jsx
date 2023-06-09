import React from "react";
import { Field, ErrorMessage } from "formik";

import "./BaseInput.scss";

export default function BaseInput(props) {
  return (
    <>
      {props.label && <label>{props.label}</label>}
      <div className="d-flex justify-content-center flex-direction-row position-relative">
        <Field
          className="form-control"
          name={props.name}
          type={props.type}
          placeholder={props.placeholder}
        />
        {props.icon && (
          <i
            onClick={props.onIconClick}
            className={"input-icon pi " + props.icon}
          ></i>
        )}
      </div>
      <ErrorMessage name={props.name}>
        {(msg) => <div className="text-danger">{msg}</div>}
      </ErrorMessage>
    </>
  );
}
