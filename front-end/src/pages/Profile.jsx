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
import jwtDecode from "jwt-decode";

axios.defaults.baseURL = process.env.REACT_APP_SERVER_DOMAIN;

function Profile() {
  const userId = jwtDecode(localStorage.getItem("token")).id;
  console.log(userId);

  const dispatch = useDispatch();
  const { loading } = useSelector((state) => state.root);
  const [specializations, setSpecializations] = useState([]); // State to store specializations
  const [formDetails, setFormDetails] = useState({
    specializationId: "",
    examinationduration: "",
    name: "",
    email: "",
    phoneNumber: "",
    bio: "",
    price: 0,
    password: "",
    confpassword: "",
  });

  useEffect(() => {
    const fetchSpecializations = async () => {
      try {
        const response = await axios.get("/Specialization");
        setSpecializations(response.data);
      } catch (error) {
        console.error("Error fetching specializations:", error);
        // TODO: Add better error handling, e.g., show a user-friendly error message
      }
    };

    fetchSpecializations();
  }, []);

  const getUser = async () => {
    try {
      dispatch(setLoading(true));
      const temp = await fetchData(`/Doctor/GetDoctorById?id=${userId}`); // Updated API endpoint

      setFormDetails({
        specializationId: temp.specializationId || "",
        examinationduration: temp.examinationduration || "",
        name: temp.name || "",
        email: temp.email || "",
        phoneNumber: temp.phoneNumber || "",
        bio: temp.bio || "",
        price: temp.price || null,
        password: "",
        confpassword: "",
      });
      dispatch(setLoading(false));
    } catch (error) {
      console.error("Error fetching doctor data:", error);
      // TODO: Add better error handling, e.g., show a user-friendly error message
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
      const { specializationId, examinationduration, name, email, password, phoneNumber, bio, price, confpassword } = formDetails;

      // // Validation
      // if (!email) {
      //   return toast.error("Email should not be empty");
      // } else if (name.length < 3) {
      //   return toast.error("Name must be at least 3 characters long");
      // } else
      if(password.length>0){
      if (password.length < 5) {
        return toast.error("Password must be at least 5 characters long");
      } else if (password !== confpassword) {
        return toast.error("Passwords do not match");
      }
    }

      const updatedData = {
        specializationId: specializationId,
        examinationduration: examinationduration,
        name: name,
        email: email,
        password: password,
        phoneNumber: phoneNumber,
        bio: bio,
        price: price,
      };

      await toast.promise(
        axios.put(
          "/Doctor/EditDoctor", // Updated API endpoint
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
      // TODO: Add better error handling, e.g., show a more specific error message
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
            {/* ... (image element - you might need to adjust this) */}
            <form onSubmit={formSubmit} className="register-form">
              {/* Specialization Dropdown */}
              <div className="form-same-row">
                <select
                  name="specializationId"
                  className="form-input"
                  value={formDetails.specializationId}
                  onChange={inputChange}
                >
                  <option value="">Select Specialization</option>
                  {specializations.map((spec) => (
                    <option key={spec.id} value={spec.id}>
                      {spec.specializationName}
                    </option>
                  ))}
                </select>
              </div>

              <div className="form-same-row">
                <input
                  type="text"
                  name="name"
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
              </div>
              <div className="form-same-row">
                <input
                  type="text"
                  name="phoneNumber" // Changed to phoneNumber
                  className="form-input"
                  placeholder="Enter your mobile number"
                  value={formDetails.phoneNumber}
                  onChange={inputChange}
                />
              </div>
              <div className="form-same-row">
                <input
                  type="number" // Changed to number input
                  name="examinationduration" // Changed to examinationduration
                  className="form-input"
                  placeholder="Enter your examination duration"
                  value={formDetails.examinationduration}
                  onChange={inputChange}
                />
              </div>
              <textarea
                type="text"
                name="bio"
                className="form-input"
                placeholder="Enter your bio"
                value={formDetails.bio}
                onChange={inputChange}
                rows="2"
              ></textarea>
              <div className="form-same-row">
                <input
                  type="number" // Changed to number input
                  name="price"
                  className="form-input"
                  placeholder="Enter your price"
                  value={formDetails.price}
                  onChange={inputChange}
                />
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
              <button type="submit" className="btn form-btn">
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

export default Profile;