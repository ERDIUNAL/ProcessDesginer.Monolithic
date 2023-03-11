import React, { useEffect } from "react";
import "./Loader.scss";
import { ProgressSpinner } from "primereact/progressspinner";
import { useLoader } from "../../contexts/LoaderContext";
export default function Loader() {
  const loadingState = useLoader();
  useEffect(() => {}, [loadingState]);
  return (
    <React.Fragment>
      {loadingState.isLoading === true && (
        <div className="loader">
          <ProgressSpinner></ProgressSpinner>
        </div>
      )}
    </React.Fragment>
  );
}
