import "./App.css";
import "primereact/resources/themes/lara-light-indigo/theme.css";
import "primereact/resources/primereact.min.css";
import "primeicons/primeicons.css";
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap/dist/js/bootstrap.bundle.min.js";
import "toastr/build/toastr.min.css";

import { Route, Routes } from "react-router-dom";

import LoginPage from "./features/auth/login/pages/LoginPage";
import Homepage from "./shared/pages/homepage/Homepage";

import Loader from "./shared/components/loader/Loader";
import { LoaderProvider } from "./shared/contexts/LoaderContext";
import Navbar from "./shared/components/navbar/Navbar";

function App() {
  return (
    <LoaderProvider>
      <div className="App">
        <Loader />
        <Navbar />
        <Routes>
          <Route path="login" element={<LoginPage />} />
          <Route path="/" element={<Homepage />} />
          <Route path="homepage" element={<Homepage />} />
        </Routes>
      </div>
    </LoaderProvider>
  );
}

export default App;
