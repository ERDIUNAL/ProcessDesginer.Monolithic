import toastr from "toastr";

export const handleBusinessException = (error) => {
  toastr.error(error.Detail);
};

export const handleValidationException = (error) => {
  error.Failures.forEach((fail) => {
    toastr.error(fail.ErrorMessage);
  });
};

export const handleAuthException = () => {};

export const handleDefaultException = (message) => {
  toastr.error(message);
};
