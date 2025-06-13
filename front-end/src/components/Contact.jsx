import React, { useState } from "react";
import axios from "axios"; // Import axios for the POST request
import "../styles/contact.css";

const Contact = () => {
  const [formDetails, setFormDetails] = useState({
    name: "",
    email: "",
    message: "",
  });

  const [responseMessage, setResponseMessage] = useState("");
  const [isLoading, setIsLoading] = useState(false);

  const inputChange = (e) => {
    const { name, value } = e.target;
    setFormDetails({
      ...formDetails,
      [name]: value,
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    setIsLoading(true);
    setResponseMessage("");

    try {
      const payload = {
        name: formDetails.name,
        email: formDetails.email,
        queryText: formDetails.message,
      };

      // Make the POST request to the API using axios
      const response = await axios.post('https://clinica.runasp.net/api/Query/Create', payload, {
        headers: {
          Authorization: `Bearer ${localStorage.getItem("token")}`,
          'Content-Type': 'application/json',
        },
      });

      if (response.status === 200) {
        setResponseMessage("Your message has been sent successfully!");
        setFormDetails({ name: "", email: "", message: "" });
      }
    } catch (error) {
      setResponseMessage("An error occurred. Please try again.");
      console.error(error); // Log the error for debugging
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <section className="register-section flex-center" id="contact">
      <div className="contact-container flex-center contact">
        <h2 className="form-heading">Contact Us</h2>
        <form className="register-form" onSubmit={handleSubmit}>
          <input
            type="text"
            name="name"
            className="form-input"
            placeholder="Enter your name"
            value={formDetails.name}
            onChange={inputChange}
            required
          />
          <input
            type="email"
            name="email"
            className="form-input"
            placeholder="Enter your email"
            value={formDetails.email}
            onChange={inputChange}
            required
          />
          <textarea
            name="message"
            className="form-input"
            placeholder="Enter your message"
            value={formDetails.message}
            onChange={inputChange}
            rows="8"
            cols="12"
            required
          ></textarea>

          <button type="submit" className="btn form-btn" disabled={isLoading}>
            {isLoading ? "Sending..." : "Send"}
          </button>
        </form>
        {responseMessage && <p className="response-message">{responseMessage}</p>}
      </div>
    </section>
  );
};

export default Contact;
