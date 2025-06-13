import "../styles/doctorcard.css";
import React, { useState } from "react";
import BookAppointment from "../components/BookAppointment";
import { toast } from "react-hot-toast";

const DoctorCard = ({ ele }) => {
  const [modalOpen, setModalOpen] = useState(false);
  const [token, setToken] = useState(localStorage.getItem("token") || "");

  const handleModal = () => {
    if (token === "") {
      return toast.error("You must log in first");
    }
    setModalOpen(true);
  };

  return (
    <div className={`card`}>
      <div className={`card-img flex-center`}>
        <img
          src={
            /* ele?.image */ null ||
            "https://icon-library.com/images/anonymous-avatar-icon/anonymous-avatar-icon-25.jpg"
          }
          alt="profile"
        />
      </div>
      <h3 className="card-name">
        Dr. {ele?.name}
      </h3>
      <p className="phone">
        <strong>Email: </strong>
        {ele?.email}
      </p>
      <p className="specialization">
        <strong>Specialization: </strong>
        {ele?.specialization || 'General'}
      </p>
      {/* <p className="experience">
        <strong>Examination Duration: </strong>
        {ele?.examinationduration || "30 min"}yrs
      </p> */}
      <p className="fees">
        <strong>Fees per consultation: </strong>$ {ele?.price}
      </p>
      <p className="phone">
        <strong>Phone: </strong>
        {ele?.phoneNumber}
      </p>
      <button
        className="btn appointment-btn"
        onClick={handleModal}
      >
        Book Appointment
      </button>
      {modalOpen && (
        <BookAppointment
          setModalOpen={setModalOpen}
          ele={ele}
        />
      )}
    </div>
  );
};

export default DoctorCard;
