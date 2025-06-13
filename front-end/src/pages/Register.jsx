import React, { useState } from "react";
import { NavLink, useNavigate } from "react-router-dom";
import "../styles/register.css";
import Navbar from "../components/Navbar.jsx";
import axios from "axios";
import toast from "react-hot-toast";

axios.defaults.baseURL = process.env.REACT_APP_SERVER_DOMAIN;

function Register() {
  const [file, setFile] = useState("");
  const [selectedRole, setSelectedRole] = useState("");
  const [loading, setLoading] = useState(false);
  const [formattedBirthday, setFormattedBirthday] = useState(""); // Add this state

  const [formDetails, setFormDetails] = useState({
    name: "",
    phoneNumber: "",
    email: "",
    password: "",
    confpassword: "",
    gender: "",
    birthday: ""
  });
  const navigate = useNavigate();

  const inputChange = (e) => {
    const { name, value } = e.target;
    setFormDetails({
      ...formDetails,
      [name]: value,
    });
    if (name === "birthday") {
      const date = new Date(value);
      const year = date.getFullYear().toString().slice(-2); // Get last two digits of year
      const month = ("0" + (date.getMonth() + 1)).slice(-2); // Add leading zero if needed
      const day = ("0" + date.getDate()).slice(-2);
      setFormattedBirthday(`${year}-${month}-${day}`);
    }
  };
 

  // const onUpload = async (element) => {
  //   setLoading(true);
  //   if (
  //     element.type === "image/jpeg" ||
  //     element.type === "image/png" ||
  //     element.type === "image/jpg"
  //   ) {
  //     const data = new FormData();
  //     data.append("file", element);
  //     data.append("upload_preset", process.env.REACT_APP_CLOUDINARY_PRESET);
  //     data.append("cloud_name", process.env.REACT_APP_CLOUDINARY_CLOUD_NAME);
  //     fetch(process.env.REACT_APP_CLOUDINARY_BASE_URL, {
  //       method: "POST",
  //       body: data,
  //     })
  //       .then((res) => res.json())
  //       .then((data) => setFile(data.url.toString()));
  //     setLoading(false);
  //   } else {
  //     setLoading(false);
  //     toast.error("Please select an image in jpeg or png format");
  //   }
  // };

  const formSubmit = async (e) => {
    try {
      e.preventDefault();
  
      if (loading) return;
      // if (file === "") return;
      const { name, phoneNumber, email, password, confpassword ,gender,
        birthday} = formDetails;
      if (!name || !phoneNumber || !email || !password || !confpassword || !gender || !birthday /* || !selectedRole */) {
        return toast.error("Input field should not be empty");
      } else if (name.length < 3) {
        return toast.error("First name must be at least 3 characters long");
      } else if (!phoneNumber) {
        return toast.error("Plz Enter Phone");
      } else if (password.length < 5) {
        return toast.error("Password must be at least 5 characters long");
      } else if (password !== confpassword) {
        return toast.error("Passwords do not match");
      } else if (!birthday) {
        return toast.error("Where Is birthday?");
      } else if (!gender) {
        return toast.error("Where Is gender?");
      }


      console.log(formDetails);
      console.log(formattedBirthday);
      

  // // Create a Date object from the input value
  // const dateOfBirth = new Date(birthday); 

  // // Format the date (example for "MM/DD/YYYY" format)
  // const formattedBirthday = `${dateOfBirth.getMonth() + 1}/${dateOfBirth.getDate()}/${dateOfBirth.getFullYear()}`;

    
      const { data } = await toast.promise(
        axios.post("https://clinica.runasp.net/api/User/PatientRegister",{
          name,
          phoneNumber,
          email,
          password,
          userName: name,
          birthday,
          gender
        },{
          headers: {
            'Content-Type': 'application/json'
          }}),
        {
          pending: "Registering user...",
          success: "User registered successfully",
          error: "Unable to register user",
          loading: "Registering user...",
        }
      ); 
      console.log(data);

      // try {
      //   const da = await axios.post('https://clinica.runasp.net/api/User/PatientRegister',{ name,
      //         phoneNumber,
      //         email,
      //         password,
      //         userName: name})

      //         console.log(da);
              
      // } catch (error) {
      //   console.log(error);
        
      // }

      
      return navigate("/login");
    } catch (error) {
      console.log(error);
      
    }
  };
  

  return (
    <>
      <Navbar />
      <section className="register-section flex-center">
        <div className="register-container flex-center">
          <h2 className="form-heading">Register as Patient</h2>
          <form onSubmit={formSubmit} className="register-form">
            <input
              type="text"
              name="name"
              className="form-input"
              placeholder="Enter your name"
              value={formDetails.name}
              onChange={inputChange}
            />
            <input
              type="text"
              name="phoneNumber"
              className="form-input"
              placeholder="Enter your Phone Number"
              value={formDetails.phoneNumber}
              onChange={inputChange}
            />
            <input
              type="email"
              name="email"
              className="form-input"
              placeholder="Enter your email"
              value={formDetails.email}
              onChange={inputChange}
            />
            {/* <input
              type="file"
              onChange={(e) => onUpload(e.target.files[0])}
              name="profile-pic"
              id="profile-pic"
              className="form-input"
            /> */}
            <input
              type="password"
              name="password"
              className="form-input"
              placeholder="Enter your password"
              value={formDetails.password}
              onChange={inputChange}
            />
            <input
              type="password"
              name="confpassword"
              className="form-input"
              placeholder="Confirm your password"
              value={formDetails.confpassword}
              onChange={inputChange}
            />
            {/* <select
              name="role"
              value={selectedRole}
              onChange={(e) => setSelectedRole(e.target.value)}
              className="form-input"
            >
              <option value="">Select Role</option>
              <option value="1">Admin</option>
              <option value="3">Doctor</option>
              <option value="2">Patient</option>
            </select> */}
              <input
            type="date"
            name="birthday" 
            className="form-input"
            placeholder="Enter your date of birth"
            value={formDetails.birthday} 
            onChange={inputChange}
      />

      <div className="form-input"> 
        <select 
          name="gender"

          value={formDetails.gender} 
          onChange={inputChange}
        >
          <option value="">Select Gender</option>
          <option value="male">Male</option>
          <option value="female">Female</option>
        </select>
      </div>


            <button
              type="submit"
              className="btn form-btn"
              disabled={loading ? true : false}
            >
              sign up
            </button>
          </form>
          <p>
            Already a user?{" "}
            <NavLink className="login-link" to={"/login"}>
              Log in
            </NavLink>
          </p>
        </div>
      </section>
    </>
  );
}

export default Register;
