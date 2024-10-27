import React, { useEffect, useState } from "react";
import "../styles/profile.css";
import Footer from "../components/Footer";
import Navbar from "../components/Navbar";
import axios from "axios";
import toast from "react-hot-toast";   

import { setLoading } from "../redux/reducers/rootSlice.js";
import { useDispatch, useSelector } from "react-redux";
import Loading from "../components/Loading";
import fetchData from "../helper/apiCall";
import   
 jwtDecode from "jwt-decode";
import moment from "moment"; // Import moment

axios.defaults.baseURL = process.env.REACT_APP_SERVER_DOMAIN;

function ProfilePatient() {
  const userId = jwtDecode(localStorage.getItem("token")).id;
  console.log(userId);

  const dispatch = useDispatch();
  const { loading } = useSelector((state) => state.root);
  const [file, setFile] = useState("");
  const [formDetails, setFormDetails] = useState({   

    name: "", // Changed from firstname and lastname
    email: "",
    mobile: "",
    gender: "male", // Set default to "male"
    birthday: "", // Changed from age
    password: "",
    confpassword: "",
  });

  const getUser = async () => {
    try {
      dispatch(setLoading(true));
      const temp = await fetchData(`/Patient/GetPatientById?id=${userId}`);

      // Split the name into firstname and lastname (if available)
      const nameParts = temp.name ? temp.name.split(" ") : ["", ""];

      setFormDetails({
        name: temp.name || "",
        email: temp.email || "",
        mobile: temp.phoneNumber || "", // Use phoneNumber from response
        gender: temp.gender || "male",
        birthday: moment(temp.birthday).format("YYYY-MM-DD") || "", // Format birthday
        password: "",
        confpassword: "",
      });
      setFile(temp.pic);
      dispatch(setLoading(false));
    } catch (error) {
      console.error("Error fetching patient data:", error);
      // TODO: Add better error handling, e.g., show a user-friendly error message to the user
    }
  };
  useEffect(() => {
    getUser();
  }, [dispatch]);

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
      const { name, email, birthday, mobile, gender, password ,confpassword} = formDetails; // Removed confpassword


      if (!email) {
        return toast.error("Email should not be empty");
      } else if (name.length < 3) {
        return toast.error("First name must be at least 3 characters long");
      } else if (password.length < 1) {
        return toast.error("Password must be at least 5 characters long");
      } else if (password !== confpassword) {
        return toast.error("Passwords do not match");
      }
      const updatedData = {
        name: name,
        email: email,
        password: password,
        phoneNumber: mobile,
        gender: gender,
        birthday: birthday, // Use the formatted birthday
      };

        await toast.promise(
          axios.put(
            "/Patient/EditPatient",
            updatedData,
            {
              headers: {
                Authorization: `Bearer ${localStorage.getItem("token")}`,
              },
            }
          ),
        {
          pending: "Updating profile...",
          success: "Profile updated successfully",
          error: "Unable to update profile",
          loading: "Updating profile...",
        }
      );
      setFormDetails({ ...formDetails, password: "", confpassword: "" });
    } catch (error) {
      console.error("Error updating profile:", error);
      // TODO: Add better error handling, e.g., show a more specific error message to the user
    }
  };

  return (
    <>
    <Navbar />
      {loading ? (
        <Loading />
      ) : (
        <section className="register-section flex-center">
          <div className="profile-container flex-center">
            <h2 className="form-heading">Profile</h2>
            <img
              src={file}
              alt="profile"
              className="profile-pic"
            />
            <form
              onSubmit={formSubmit}
              className="register-form"
            >
              <div className="form-same-row">
              <input
                type="text"
                name="name" // Changed to "name"
                className="form-input"
                placeholder="Enter your full name"
                value={formDetails.name}
                onChange={inputChange}
              />
              </div>
              <div className="form-same-row">
                <input
                  type="email"
                  name="email"
                  className="form-input"
                  placeholder="Enter your email"
                  value={formDetails.email}
                  onChange={inputChange}
                />
                <select
                  name="gender"
                  value={formDetails.gender}
                  className="form-input"
                  id="gender"
                  onChange={inputChange}
                >
                  <option value="neither">Prefer not to say</option>
                  <option value="male">Male</option>
                  <option value="female">Female</option>
                </select>
              </div>
              <div className="form-same-row">
                <input
                  type="text"
                  name="mobile"
                  className="form-input"
                  placeholder="Enter your mobile number"
                  value={formDetails?.mobile}
                  onChange={inputChange}
                />
              </div>
              <div className="form-same-row">
              <input
                type="date" // Changed to date input
                name="birthday" // Changed to "birthday"
                className="form-input"
                placeholder="Enter your birthday"
                value={formDetails.birthday}
                onChange={inputChange}
              />
              {/* ... (other form elements) */}
            </div>
             
              <div className="form-same-row">
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
              </div>
              <button
                type="submit"
                className="btn form-btn"
              >
                update
              </button>
            </form>
          </div>
        </section>
      )}
      <Footer />
    </>
  );
}

export default ProfilePatient;
