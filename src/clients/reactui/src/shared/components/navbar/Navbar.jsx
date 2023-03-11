import "./Navbar.scss";

import React, { useEffect, useState } from "react";
import { Link, useLocation, useNavigate } from "react-router-dom";

import { removeItem } from "../../../core/utils/localStorage";
import { useDispatch, useSelector } from "react-redux";
import { Logout } from "../../../store/actions/authActions";
import LanguageSelect from "../language-select/LanguageSelect";
import { NAME } from "../../constants/claimConstants";

export default function Navbar() {
  const navigate = useNavigate();
  const hideNavbarRoutes = ["/login", "/register"];
  const { pathname } = useLocation();
  const [showNavbar, setShowNavbar] = useState(true);
  useEffect(() => {
    setShowNavbar(!hideNavbarRoutes.includes(pathname));
  }, [pathname]);

  const authState = useSelector((state) => state.auth);
  const dispatch = useDispatch();
  const handleLogout = () => {
    removeItem("token");
    dispatch(Logout());
    navigate("/login");
  };

  return (
    <>
      {showNavbar && (
        <nav className="navbar navbar-expand-lg px-3">
          <Link to="/" className="navbar-brand">
            <div className="logo"></div>
          </Link>
          <button
            className="navbar-toggler"
            type="button"
            data-toggle="collapse"
            data-target="#navbarSupportedContent"
            aria-controls="navbarSupportedContent"
            aria-expanded="false"
            aria-label="Toggle navigation"
          >
            <span className="navbar-toggler-icon"></span>
          </button>

          <div className="collapse navbar-collapse" id="navbarSupportedContent">
            <ul className="navbar-nav ms-auto">
              <li className="nav-item dropdown">
                <Link
                  className="nav-link dropdown-toggle text-white fw-bolder"
                  to="#"
                  id="navbarDropdown"
                  role="button"
                  data-bs-toggle="dropdown"
                  aria-haspopup="true"
                  aria-expanded="false"
                >
                  <i className="pi pi-user"></i>
                  {authState.user != null
                    ? " " + authState.user[NAME]
                    : " Misafir"}
                </Link>
                <div className="dropdown-menu" aria-labelledby="navbarDropdown">
                  <div
                    className="dropdown-item text-white fw-bolder"
                    onClick={() => {
                      console.log(authState);
                    }}
                  >
                    <i className="pi pi-cog"></i> Ayarlar
                  </div>
                  <div
                    className="dropdown-item text-white fw-bolder"
                    onClick={() => handleLogout()}
                  >
                    <i className="pi pi-sign-out"></i> Çıkış
                  </div>
                </div>
              </li>
              <li className="nav-item">
                <div className="nav-link language-dropdown">
                  <LanguageSelect></LanguageSelect>
                </div>
              </li>
            </ul>
          </div>
        </nav>
      )}
    </>
  );
}
