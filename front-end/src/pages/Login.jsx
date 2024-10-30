import React, { useEffect, useState } from "react";
import { NavLink, useNavigate } from "react-router-dom";
import "../styles/register.css";
import Navbar from "../components/Navbar";
import axios from "axios";
import toast from "react-hot-toast";
import { useDispatch } from "react-redux";
import { setUserInfo } from "../redux/reducers/rootSlice.js";
import jwt_decode from "jwt-decode";
import fetchData from "../helper/apiCall";
import jwtDecode from "jwt-decode";
// import  jwt  from 'jsonwebtoken';

axios.defaults.baseURL = process.env.REACT_APP_SERVER_DOMAIN;

function Login() {
  const dispatch = useDispatch();
  const [formDetails, setFormDetails] = useState({
    email: "",
    password: ""
  });
  const [userRole, setUserRole] = useState(null);
  const [adminRole, setAdminRole] = useState(null)
  const navigate = useNavigate();
  const inputChange = (e) => {
    const { name, value } = e.target;
    return setFormDetails({
      ...formDetails,
      [name]: value,
    });
  };
  const formSubmit = async (e) => {
    try {
      e.preventDefault();
      const { email, password } = formDetails;
  
      if (!email || !password) {
        return toast.error("Email and password are required");
      }
      // } else if (!role) {
      //   return toast.error("Please select a role");
      // } 
      // else if (role !== "Admin" && role !== "Doctor" && role !== "Patient") {
      //   return toast.error("Please select a valid role");
      // }
       else if (password.length < 0) {
        return toast.error("Password must be at least 5 characters long");
      }
  
      const { data } = await toast.promise(
        axios.post("https://clinica.runasp.net/api/User/Login", {
          userName:email,
          password,
          // role,
        },{
          headers: {
            'Content-Type': 'application/json'
          }}),
        
        {
          pending: "Logging in...",
          success: "Login successfully",
          error: "Unable to login user",
          loading: "Logging user...",
        }
      );
      console.log(data);
      // console.log(jwt_decode("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjkiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiQWhtZWRSYWdhYjEiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJQYXRpZW50IiwianRpIjoiNTUyNWQ4NDMtZTRiYS00MGQ0LWI5MDAtMGIzZDMyM2U1OThkIiwiZXhwIjoxNzI4ODM4NDE1LCJpc3MiOiJUZXN0LmNvbSIsImF1ZCI6IlRlc3QuY29tIn0.j3Y34mGIdQw0UbvOkz0BGf0hWxNtIxIvZO2qHVIKXms")["http://schemas.microsoft.com/ws/2008/06/identity/claims/nameidentifier"]);
      // console.log(jwt.verify("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjkiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiQWhtZWRSYWdhYjEiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJQYXRpZW50IiwianRpIjoiNTUyNWQ4NDMtZTRiYS00MGQ0LWI5MDAtMGIzZDMyM2U1OThkIiwiZXhwIjoxNzI4ODM4NDE1LCJpc3MiOiJUZXN0LmNvbSIsImF1ZCI6IlRlc3QuY29tIn0.j3Y34mGIdQw0UbvOkz0BGf0hWxNtIxIvZO2qHVIKXms","eyJhbGciOiJIUzI1NiJ9.eyJSb2xlIjoiQWRtaW4iLCJJc3N1ZXIiOiJJc3N1ZXIiLCJVc2VybmFtZSI6IkphdmFJblVzZSIsImV4cCI6MTcwODI1OTkzMCwiaWF0IjoxNzA4MjU5OTMwfQ.-SuOR1FxrCnLXJqZRmk4raQmsFnavzT1Kg1Yu2h2424"));
      console.log();
     
      localStorage.setItem("token", data.token);
      dispatch(setUserInfo(jwtDecode(data.token).id));  
      setUserRole(jwtDecode(data.token).role);
     await getUser(jwtDecode(data.token).id, userRole);
      console.log(userRole);
       if(jwtDecode(data.token).role === "Admin"){
        setAdminRole("Admin")
        navigate("/dashboard/home");

      }
    } catch (error) {
      return error;
    }
  };
  

  const getUser = async (id, role) => {
    try {
      
      if(role === 'doctor'){
        const temp = await axios.get(`https://clinica.runasp.net/api/Doctor/GetDoctorById?id=${parseInt(id)}`);
        console.log(temp);
        dispatch(setUserInfo(temp));
        
      }
      if(role === 'Patient'){
        const temp = await axios.get(`https://clinica.runasp.net/api/Patient/GetPatientById?id=${parseInt(id)}`);
        console.log(temp);
        dispatch(setUserInfo(temp));
        
      }
      console.log(role);
      
      // if(role === 'Admin'){
      //   // const temp = await axios.get(`https://clinica.runasp.net/api/Patient/GetPatientById?id=${parseInt(id)}`);
      //   // console.log(temp);
      //   dispatch(setUserInfo({
      //     role: 'Admin',
      //     roleId: 1
      //   }));
      // }

       if (role === "Admin") {
        dispatch(setUserInfo({
          role: 'Admin',
          roleId: 1
        }));
       return navigate("/dashboard/home");
      } else if (role === "Patient"){
        return navigate("/");
      } else {
        return navigate("/");
      }
    } catch (error) {
      return error;
    }
  };
  // useEffect(() => {
  //   if (userRole === "Admin") {
  //     navigate("/dashboard/home");
  //   }
  // }, [userRole]);
  return (
    <>
      <Navbar  /> 
      <section className="register-section flex-center">
        <div className="register-container flex-center">
          <h2 className="form-heading">Sign In</h2>
          <form onSubmit={formSubmit} className="register-form">
            <input
              type="text"
              name="email"
              className="form-input"
              placeholder="Enter your email"
              value={formDetails.email}
              onChange={inputChange}
            />
            <input
              type="password"
              name="password"
              className="form-input"
              placeholder="Enter your password"
              value={formDetails.password}
              onChange={inputChange}
            />
            {/* <select
              name="role"
              className="form-input"
              value={formDetails.role}
              onChange={inputChange}
            >
              <option value="">Select Role</option>
              <option value="1">Admin</option>
              <option value="3">Doctor</option>
              <option value="2">Patient</option>
            </select> */}
            <button type="submit" className="btn form-btn">
              sign in
            </button>
          </form>
          <NavLink className="login-link" to={"/forgotpassword"}>
              Forgot Password
            </NavLink>
          {/* <p>
            Not a user?{" "}
            
            <NavLink className="login-link" to={"/register"}>
              Register
            </NavLink>
          </p> */}
        </div>
      </section>
    </>
  );
}

export default Login;
