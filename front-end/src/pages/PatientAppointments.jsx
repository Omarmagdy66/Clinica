import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import "../styles/notification.css";
import Empty from "../components/Empty";
import Footer from "../components/Footer";
import Navbar from "../components/Navbar";
import fetchData from "../helper/apiCall";
import { setLoading } from "../redux/reducers/rootSlice.js";
import Loading from "../components/Loading";
import "../styles/user.css";
import moment from "moment";
import toast from "react-hot-toast";
import axios from "axios";

const PatientAppointments = () => {
  const [appointments, setAppointments] = useState([]);
  const [doctorNames, setDoctorNames] = useState({});
  const [currentPage, setCurrentPage] = useState(1);
  const appointmentsPerPage = 6;
  const dispatch = useDispatch();
  const { loading } = useSelector((state) => state.root);

  const getAllAppointments = async () => {
    try {
      dispatch(setLoading(true));
      const temp = await fetchData("/Appointment/GetAppointmentsForPatient", {
        headers: {
          Authorization: `Bearer ${localStorage.getItem("token")}`,
        },
      });
      console.log(temp);

      dispatch(setLoading(false));
      setAppointments(temp);

      // Fetch doctor names after getting appointments
      const doctorIds = temp.map(appointment => appointment.doctorId);
      await fetchDoctorNames(doctorIds);
    } catch (error) {
      console.error("Error fetching appointments:", error);
      dispatch(setLoading(false)); // Ensure loading is set to false regardless of success or failure

      // TODO: Add better error handling, e.g., show a user-friendly error message, maybe with details from error.response
    }finally {
      dispatch(setLoading(false)); // Ensure loading is set to false regardless of success or failure
    }
  };

  const fetchDoctorNames = async (doctorIds) => {
    try {
      const names = {};
      for (const id of doctorIds) {
        if (!doctorNames[id]) {
          const response = await fetchData(`/Doctor/GetDoctorById?id=${id}`);
          names[id] = response.name;
        }
      }
      setDoctorNames(prevNames => ({ ...prevNames, ...names }));
    } catch (error) {
      console.error("Error fetching doctor names:", error);
      // TODO: Add better error handling for fetching doctor names, e.g., show a user-friendly error message, maybe with details from error.response
    }
  };

  useEffect(() => {
    getAllAppointments();
  }, [currentPage]);

  const handleCancelAppointment = async (id) => {
    try {
      // Make API call to cancel the appointment
      const response = await axios.put(`https://clinica.runasp.net/api/Appointment/CancelAppointment?id=${id}`, null, {
        headers: {
          Authorization: `Bearer ${localStorage.getItem("token")}`,
        },
      });

      // Update the appointments state with new object references
      setAppointments(prevAppointments => prevAppointments.map(app =>
        app.id === id ? { ...app, status: 'Cancelled' } : { ...app } // Create a new object to force re-render
      ));

      toast.success("Appointment cancelled successfully!");
    } catch (error) {
      console.error("Error cancelling appointment:", error);
      toast.error("Failed to cancel appointment.");
      // TODO: Add better error handling, e.g., show a more specific error message, maybe with details from error.response
    }
  };

  const totalPages = Math.ceil(appointments.length / appointmentsPerPage);

  const handlePageChange = (page) => {
    setCurrentPage(page);
  };

  const renderPagination = () => {
    const pages = [];
    for (let i = 1; i <= totalPages; i++) {
      pages.push(
        <button key={i} onClick={() => handlePageChange(i)}>{i}</button>
      );
    }
    return pages;
  };

  const paginatedAppointments = appointments.slice(
    (currentPage - 1) * appointmentsPerPage,
    currentPage * appointmentsPerPage
  );

  return (
    <>
      <Navbar />
      {loading ? (
        <Loading />
      ) : (
        <section className="container notif-section">
          <h2 className="page-heading">Your Appointments</h2>

          {appointments.length > 0 ? (
            <div className="notifications">
              <table>
                <thead>
                  <tr>
                    <th>S.No</th>
                    <th>Doctor Name</th>
                    <th>Appointment Date</th>
                    <th>Time Slot</th>
                    <th>Status</th>
                    <th>Action</th>
                  </tr>
                </thead>
                <tbody>
                  {paginatedAppointments.map((appointment, i) => (
                    <tr key={appointment.id}>
                      <td>{(currentPage - 1) * appointmentsPerPage + i + 1}</td>
                      <td>{doctorNames[appointment.doctorId] || "Loading..."}</td>
                      <td>{moment(appointment.appointmentDate).format("YYYY-MM-DD")}</td>
                      <td>{appointment.timeSlot}</td>
                      <td>{appointment.status}</td>
                      <td>
                        <button
                          className="btn user-btn"
                          onClick={() => handleCancelAppointment(appointment.id)}
                          disabled={appointment.status === "Completed" || appointment.status === "Cancelld"}
                        >
                          Cancel
                        </button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
              <div className="pagination">{renderPagination()}</div>
            </div>
          ) : (
            <Empty />
          )}
        </section>
      )}
      <Footer />
    </>
  );
};

export default PatientAppointments;