import "./App.css";
import "primereact/resources/themes/lara-light-indigo/theme.css";
import "primereact/resources/primereact.min.css";
import "primeicons/primeicons.css";
import "bootstrap/dist/css/bootstrap.min.css";
import "toastr/build/toastr.min.css";

import { Route, Routes } from "react-router-dom";

import LoginPage from "./features/auth/login/pages/LoginPage";
import Homepage from "./shared/pages/homepage/Homepage";

function App() {
  return (
    <div className="App">
      <Routes>
        <Route path="/" element={<LoginPage />} />
        <Route path="login" element={<LoginPage />} />
        <Route path="homepage" element={<Homepage />} />
      </Routes>
    </div>
  );
}

export default App;
