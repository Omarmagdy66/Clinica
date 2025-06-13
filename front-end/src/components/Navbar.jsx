import React, { useState, useEffect } from "react";
import { NavLink, useNavigate } from "react-router-dom";
import "../styles/navbar.css";
import { HashLink } from "react-router-hash-link";
import { useDispatch, useSelector } from "react-redux";
import { setUserInfo, setIsDoctor  } from "../redux/reducers/rootSlice.js";
import { FiMenu } from "react-icons/fi";
import { RxCross1 } from "react-icons/rx";
import jwt_decode from "jwt-decode";
import axios from "axios";
import { BsArrowBarDown } from "react-icons/bs";
import { FaArrowDown, FaSearch } from "react-icons/fa";
import { MdHealthAndSafety } from "react-icons/md";
import img from './../../src/images/logo.png'
axios.defaults.baseURL = process.env.REACT_APP_SERVER_DOMAIN;

const Navbar = () => {
  const [iconActive, setIconActive] = useState(false);
  
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const token = localStorage.getItem("token") || "";
  const user = token ? jwt_decode(token) : null;

  const { userInfo, isDoctor } = useSelector((state) => state.root); 
  

  const logoutFunc = () => {
    dispatch(setUserInfo({}));
    localStorage.removeItem("token");
    navigate("/login");
  }
  useEffect(() => {

    const checkDoctorStatus = async () => {
      if (userInfo) {  // Only check if a user is logged in
        try {
          const response = await axios.get("/Doctor/IsDoctor", {
            headers: {
              Authorization: `Bearer ${localStorage.getItem("token")}`,
            },
          });
          dispatch(setIsDoctor(true)); 
        } catch (error) {
          console.log(error);
          
          dispatch(setIsDoctor(false)); 
        }
      }
    };

    checkDoctorStatus();
  }, [userInfo]); // Run effect when userInfo changes


    const [showDropdown, setShowDropdown] = useState(false);
  
    const toggleDropdown = () => {
      setShowDropdown(!showDropdown);
    };
  return (
    <header>
      <nav className={iconActive ? "nav-active" : ""}>
        <h2 className="nav-logo">
<img src={img} width={"50px"} />
          <NavLink to={"/"}> CLINICA </NavLink>
        </h2>
        <ul className="nav-links">
          <li>
            <NavLink to={"/"}>Home</NavLink>
          </li>
          {user && user.role !== "doctor"  &&(<>
            <li><NavLink to={"/search/doctors"}>Search <FaSearch/> </NavLink></li>
          </>)}
          {!token &&(<>
            <li><NavLink to={"/search/doctors"}>Search <FaSearch/> </NavLink></li>
          </>)}

          {user && user.role === "doctor" && (
            <>
              {isDoctor === false &&(<>
                <li>
                  <NavLink to={"/applyfordoctor"}>Apply for doctor</NavLink>
                </li>
                <li>
                  <NavLink to={"/image-models"}>Medical Image Analysis</NavLink>
                </li>
              </>)}
            
              {isDoctor !== false &&(<>
                <li>
                <NavLink to={"/clinic"}>Join Clinic</NavLink>
              </li>
              
              <li>
                <NavLink to={"/doctor/schedules"}>Schedules</NavLink>
              </li>
              <li>
                <NavLink to={"/appointments"}>Appointments</NavLink>
              </li>
              <li>
                <NavLink to={"/notifications"}>Notifications</NavLink>
              </li>
                <li>
                <NavLink to={"/profile"}>Profile</NavLink>
              </li>
              <li><NavLink to={"/image-models"}>Medical Image Analysis</NavLink></li>
              </>)}
              <li>
                <HashLink to={"/#contact"}>Contact Us</HashLink>
              </li>
              
              {/* <li>
                <NavLink to={"/ChangePassword"}>ChangePassword</NavLink>
              </li> */}
            </>
          )}
          {user && user.role === "Patient" && (
            <>
              
              {/* <li>
                <NavLink to={"/doctors"}>Doctors</NavLink>
              </li> */}
              <li>
                <NavLink to={"/patient/appointments"}>Appointments</NavLink>
              </li>
              <li>
                <HashLink to={"/#contact"}>Contact Us</HashLink>
              </li>
              <li>
                <NavLink to={"/patient/profile"}>Profile</NavLink>
              </li>
              <li><NavLink to={"/image-models"}>Medical Image Analysis</NavLink></li>
              {/* <li>
                <NavLink to={"/ChangePassword"}>ChangePassword</NavLink>
              </li> */}
            </>
          )}
          {!token ? (
            <>
              <li>
                <NavLink className="btn" to={"/login"}>
                  Login
                </NavLink>
              </li>
              <li>
              <div className="dropdown">  
 
                <button className="btn" onClick={toggleDropdown}>
                Register as <FaArrowDown/>
                </button>
                {showDropdown && (
                  <div className="dropdown-content">
                    <NavLink to="/register/patient">Patient</NavLink>
                    <NavLink to="/register/doctor">Doctor</NavLink>
                  </div>
                )}
                </div>
              </li>
            </>
          ) : (
            <li>
              <span className="btn" onClick={logoutFunc}>
                Logout
              </span>
            </li>
          )}
        </ul>
      </nav>
      <div className="menu-icons">
        {!iconActive && (
          <FiMenu
            className="menu-open"
            onClick={() => {
              setIconActive(true);
            }}
          />
        )}
        {iconActive && (
          <RxCross1
            className="menu-close"
            onClick={() => {
              setIconActive(false);
            }}
          />
        )}
      </div>
    </header>
  );
};

export default Navbar;
