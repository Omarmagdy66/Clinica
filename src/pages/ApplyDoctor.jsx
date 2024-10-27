
import React, { useState, useEffect } from "react";
import "../styles/contact.css";
import axios from "axios";
import toast from "react-hot-toast";
import { useNavigate } from "react-router-dom";
import Navbar from "../components/Navbar";
import Footer from "../components/Footer";

axios.defaults.baseURL = process.env.REACT_APP_SERVER_DOMAIN;

const ApplyDoctor = () => {
  const navigate = useNavigate();
  const [specializations, setSpecializations] = useState([]); // State to store specializations
  const [formDetails, setFormDetails] = useState({
    specializationId: "", // Changed to specializationId
    experience: "",
    fees: "",
    bio: "", // Added bio field
  });

  useEffect(() => {
    const fetchSpecializations = async () => {
      try {
        const response = await axios.get("/Specialization"); // Fetch specializations
        setSpecializations(response.data);
      } catch (error) {
        console.error("Error fetching specializations:", error);
        // TODO: Add better error handling, e.g., show a user-friendly error message
      }
    };

    fetchSpecializations();
  }, []);

  const inputChange = (e) => {
    const { name, value } = e.target;
    return setFormDetails({
      ...formDetails,
      [name]: value,
    });
  };

  const btnClick = async (e) => {
    e.preventDefault();
    try {
      const { specializationId, experience, fees, bio } = formDetails;

      // Validation (you might want to add more validation here)
      if (!specializationId || !experience || !fees || !bio) {
        return toast.error("Please fill in all fields.");
      }

      const requestData = {
        specializationId: parseInt(specializationId),
        examinationduration: parseInt(experience, 10), // Convert experience to integer
        bio: bio,
        price: parseFloat(fees), // Convert fees to double
      };

      console.log(requestData);
      
      await toast.promise(
        axios.post(
          "https://clinica.runasp.net/api/Doctor/RequestApplyDoctor", // Updated API endpoint
          requestData,
          {
            headers: {
              Authorization: `Bearer ${localStorage.getItem("token")}`,
            },
          }
        ),
        {
          success: "Doctor application sent successfully",
          error: "Unable to send Doctor application",
          loading: "Sending doctor application...",
        }
      );

      navigate("/");
    } catch (error) {
      console.error("Error sending doctor application:", error);
      // TODO: Add better error handling, e.g., show a more specific error message
    }
  };

  return (
    <>
      <Navbar />
      <section
        className="register-section flex-center apply-doctor"
        id="contact"
      >
        <div className="register-container flex-center contact">
          <h2 className="form-heading">Apply for Doctor</h2>
          <form className="register-form ">
            {/* Specialization Dropdown */}
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

            {/* ... (other input fields) */}
            <input
                type="number"
                name="experience"
                className="form-input"
                placeholder="Enter examination duration "
                value={formDetails.examinationduration}
                onChange={inputChange}
              />
              <input
                type="number"
                name="fees"
                className="form-input"
                placeholder="Enter your fees  (in dollars)"
                value={formDetails.fees}
                onChange={inputChange}
              />
            <textarea
              name="bio"
              className="form-input"
              placeholder="Enter your bio"
              value={formDetails.bio}
              onChange={inputChange}
              rows="4" // Increased rows for bio
            />

            {/* ... (submit button) */}
            <button
               type="submit"
               className="btn form-btn"
               onClick={btnClick}
             >
               apply
             </button>
          </form>
        </div>
      </section>
      <Footer />
    </>
  );
};

export default ApplyDoctor;