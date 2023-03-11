import { createContext, useContext, useState } from "react";
import instance from "../../core/utils/axiosInterceptors";

export const LoaderContext = createContext();

export const useLoader = () => {
  return useContext(LoaderContext);
};

export const LoaderProvider = ({ children }) => {
  let requestCount = 0;
  const [isLoading, setIsLoading] = useState(false);

  const setLoadingByRequestCount = () => {
    if (requestCount > 0) {
      setIsLoading(true);
    } else {
      setIsLoading(false);
    }
  };

  const decreaseRequestCount = (config) => {
    if (
      config?.headers &&
      config.headers["X-Disable-Interceptor"] &&
      config.headers["X-Disable-Interceptor"] === "true"
    ) {
      return;
    }

    if (requestCount > 0) {
      requestCount--;
    }
  };

  const increaseRequestCount = (config) => {
    if (
      config?.headers &&
      config.headers["X-Disable-Interceptor"] &&
      config.headers["X-Disable-Interceptor"] === "true"
    ) {
      return;
    }

    requestCount++;
  };

  instance.interceptors.request.use((config) => {
    increaseRequestCount(config);
    setLoadingByRequestCount();
    return config;
  });

  instance.interceptors.response.use(
    (response) => {
      decreaseRequestCount();
      setLoadingByRequestCount();
      return response;
    },
    (error) => {
      decreaseRequestCount();
      setLoadingByRequestCount();
      return Promise.reject(error);
    }
  );

  return (
    <LoaderContext.Provider value={{ isLoading }}>
      {children}
    </LoaderContext.Provider>
  );
};
