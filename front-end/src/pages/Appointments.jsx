import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import Empty from "../components/Empty";
import Footer from "../components/Footer";
import Navbar from "../components/Navbar";
import fetchData from "../helper/apiCall";
import { setLoading } from "../redux/reducers/rootSlice.js";
import Loading from "../components/Loading";
import { toast } from "react-hot-toast";
import jwt_decode from "jwt-decode";
import axios from "axios";
import "../styles/user.css";
import "../styles/appointments.css"
import moment from "moment";

const Appointments = () => {
  const [appointments, setAppointments] = useState([]);
  const [filteredAppointments, setFilteredAppointments] = useState([]);
  const [currentPage, setCurrentPage] = useState(1);
  const PerPage = 5;
  const [searchDate, setSearchDate] = useState("");
  const dispatch = useDispatch();
  const { loading } = useSelector((state) => state.root);
  const { id } = jwt_decode(localStorage.getItem("token"));

  const getAllAppoint = async () => {
    try {
      dispatch(setLoading(true));
      const temp = await fetchData(
        `/Appointment/GetAppointmentsForDoctor`
      );
      console.log(temp);

      setAppointments(temp);
      setFilteredAppointments(temp);
    } catch (error) {
      console.error("Error fetching appointments:", error);
      toast.error("No Appointments Founded");
    } finally {
      dispatch(setLoading(false)); // Ensure loading is set to false regardless of success or failure
    }
  };
  useEffect(() => {
    getAllAppoint();
  }, []);

  const handleSearchDateChange = (e) => {
    setSearchDate(e.target.value);
  };

  useEffect(() => {
    const filterAppointmentsByDate = () => {
      if (!searchDate) {
        setFilteredAppointments(appointments);
        return;
      }

      const filtered = appointments.filter(appointment =>
        moment(appointment.appointmentDate).format('YYYY-MM-DD') === searchDate
      );
      setFilteredAppointments(filtered);
    };

    filterAppointmentsByDate();
  }, [appointments, searchDate]);

  const totalPages = Math.ceil(filteredAppointments.length / PerPage);

  const handlePageChange = (page) => {
    setCurrentPage(page);
  };

  const renderPagination = () => {
    const pages = [];
    for (let i = 1; i <= totalPages; i++) {
      pages.push(
        <button key={i} onClick={() => handlePageChange(i)}>
          {i}
        </button>
      );
    }
    return pages;
  };

  const paginatedAppointments = filteredAppointments.slice(
    (currentPage - 1) * PerPage,
    currentPage * PerPage
  );

  const completeAppointment = async (appointment) => {
    if (appointment.status === 'Cancelld') {
      toast.error("This appointment has been cancelled before!"); // More informative message
    }

    if (appointment.status !== 'Cancelld') {
      try {
        await axios.put(
          `Appointment/EditAppointmentStatus?id=${appointment.id}`,
          {
            headers: {
              Authorization: `Bearer ${localStorage.getItem("token")}`,
            },
          }
        );
        toast.success("Appointment completed successfully.");
        getAllAppoint(); // Refetch appointments after completing one
      } catch (error) {
        console.error("Error completing appointment:", error);
        toast.error("Failed to complete appointment. Please try again.");
        // TODO: Add better error handling, e.g., show a more specific error message, maybe with details from error.response
      }
    }
  };

  const fixDate = (dateString) => {
    const date = new Date(dateString);
    const options = { year: 'numeric', month: 'long', day: 'numeric' };
    const formattedDate = date.toLocaleDateString('en-US', options)
    return formattedDate
  }

  return (
    <>
      <Navbar />
      {loading ? (
        <Loading />
      ) : (
        <section className="container notif-section">
          <h2 className="page-heading">Your Appointments</h2>

          {/* Search Input */}
          <div className="search-input">
            <label htmlFor="searchDate">Search by Appointment Date:</label>
            <input
              type="date"
              className="form-input"
              id="searchDate"
              value={searchDate}
              onChange={handleSearchDateChange}
            />
          </div>

          {filteredAppointments.length > 0 ? (
            <div className="appointments">
              <table>
                <thead>
                  <tr>
                    <th>S.No</th>
                    <th>P Name</th>
                    <th>P Email</th>
                    <th>P Mobile No.</th>
                    <th>time Slot</th>
                    <th>Appointment Date</th>
                    <th>Status</th>
                    <th>Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {paginatedAppointments.map((appointment, index) => (
                    <tr key={appointment._id}>
                      <td>{(currentPage - 1) * PerPage + index + 1}</td>
                      <td>{`${appointment.patientNameIN}`}</td>
                      <td>{`${appointment.patientEmailIN}`}</td>
                      <td>{appointment.patientNumberIN}</td>
                      <td>{appointment.timeSlot}</td>
                      <td>{fixDate(appointment.appointmentDate)}</td>
                      <td>{appointment.status}</td>
                      <td>
                        <button
                          className="btn user-btn complete-btn"
                          onClick={() => completeAppointment(appointment)}
                          disabled={appointment.status === "Completed"}
                        >
                          Complete
                        </button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
              <div className="pagination">{renderPagination()}</div>
            </div>
          ) : (
            <Empty message="No appointments found." />
          )}
        </section>
      )}
      <div className="" /* style={{position: "fixed", bottom: "0", width: "100%"}} */>
        <Footer />
      </div>
    </>
  );
};

export default Appointments;