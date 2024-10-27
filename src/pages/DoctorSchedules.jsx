import React, { useEffect, useState } from "react";
import axios from "axios";
import toast from "react-hot-toast";
import Navbar from "../components/Navbar";
import Footer from "../components/Footer";
import Loading from "../components/Loading";
import { useDispatch, useSelector } from "react-redux";
import { setLoading } from "../redux/reducers/rootSlice";
import'./../styles/clinica.css'
import moment from "moment";
import jwtDecode from "jwt-decode";
import Empty from "../components/Empty";

axios.defaults.baseURL = "http://clinica.runasp.net/api";

function DoctorSchedules() {
  const [clinics, setClinics] = useState([]);
  const [selectedClinic, setSelectedClinic] = useState("");
  const [schedules, setSchedules] = useState([]);
  const [searchDate, setSearchDate] = useState("");
  const [currentPage, setCurrentPage] = useState(1); // State for current page
  const schedulesPerPage = 5;
  const [filteredSchedules, setFilteredSchedules] = useState([]);
  const [addSchedules, setAddSchedules] = useState(null);
  const [newSchedule, setNewSchedule] = useState({
    clinicId: "",
    day: moment().format('YYYY-MM-DD'),
    availableFrom: "00:00:00", // Set initial value to "00:00:00"
    availableTo: "00:00:00",   // Set initial value to "00:00:00"
  });
  const dispatch = useDispatch();
  const { loading } = useSelector((state) => state.root);
  const { id } = jwtDecode(localStorage.getItem("token"));

  useEffect(() => {
    const fetchClinics = async () => {
      try {
        dispatch(setLoading(true));
        const response = await axios.get(`/Clinic/GetClinicByDoctor?doctorId=${id}`);
        setClinics(response.data);
        dispatch(setLoading(false));
      } catch (error) {
        console.error("Error fetching clinics:", error);
        toast.error("Failed to fetch clinics. Please try again.");
        dispatch(setLoading(false));
        // TODO: Add better error handling, e.g., show a user-friendly error message, maybe with details from error.response
      }
    };

    fetchClinics();
  }, [dispatch]);

  useEffect(() => {
    const fetchSchedules = async () => {
      try {
        const response = await axios.get(`/DoctorSchedule/GetSchedulesByDoctorId?idDoctor=${id}`);
        setSchedules(response.data);
        setFilteredSchedules(response.data);
      } catch (error) {
        console.error("Error fetching schedules:", error);
        // toast.error("Failed to fetch schedules.");
        // TODO: Add better error handling, e.g., show a user-friendly error message, maybe with details from error.response
      }
    };

    fetchSchedules();
  }, [id,addSchedules ]);

  const handleClinicChange = (e) => {
    setSelectedClinic(e.target.value);
    setNewSchedule(prevSchedule => ({ ...prevSchedule, clinicId: e.target.value }));

    // Filter schedules based on selected clinic
    const filtered = schedules.filter(schedule => schedule.clinicId === parseInt(e.target.value, 10));
    setFilteredSchedules(filtered);
  };

  const handleSearchDateChange = (e) => {
    setSearchDate(e.target.value);
  };

  useEffect(() => {
    const filterSchedulesByDate = () => {
      if (!searchDate) {
        setFilteredSchedules(schedules);
        return;
      }

      const filtered = schedules.filter(schedule =>
        moment(schedule.day).format('YYYY-MM-DD') === searchDate
      );
      setFilteredSchedules(filtered);
    };

    filterSchedulesByDate();
  }, [schedules, searchDate]);

  const handleInputChange = (e) => {
    const { name, value } = e.target;

    // If the field is availableFrom or availableTo, format the value
    if (name === "availableFrom" || name === "availableTo") {
      // Ensure the value is in the format "HH:MM" (e.g., "10:30", "14:00")
      const formattedValue = moment(value, "HH:mm").format("HH:mm:ss");
      setNewSchedule({ ...newSchedule, [name]: formattedValue });
    } else {
      setNewSchedule({ ...newSchedule, [name]: value });
    }
  };

  const addSchedule = async () => {
    try {
      // Validate form data
      if (!newSchedule.clinicId || !newSchedule.day || !newSchedule.availableFrom || !newSchedule.availableTo) {
        return toast.error("Please fill in all fields.");
      }

      // Additional validation: availableFrom must be before availableTo
      if (newSchedule.availableFrom >= newSchedule.availableTo) {
        return toast.error("Available From must be before Available To.");
      }

      // Format day to 'yyyy-mm-dd'
      const formattedDay = moment(newSchedule.day).format('YYYY-MM-DD');


      const response = await axios.post(
        "/DoctorSchedule/AddDoctorSchedule",
        { ...newSchedule, day: formattedDay },
        {
          headers: {
            Authorization: `Bearer ${localStorage.getItem("token")}`,
            'Content-Type': 'application/json'
          },
        }
      );
      setAddSchedules(response.data)
      setSchedules([...schedules, response.data]);
      setNewSchedule({
        clinicId: "",
        day: moment().format('YYYY-MM-DD'),
        availableFrom: "00:00:00", // Reset time to "00:00:00" after adding
        availableTo: "00:00:00",   // Reset time to "00:00:00" after adding
      });
      toast.success("Schedule added successfully!");
    } catch (error) {
      console.error("Error adding schedule:", error);
      toast.error("Failed to add schedule. Please try again.");
      // TODO: Add better error handling, e.g., show a more specific error message, maybe with details from error.response
    }
  };

  const handleDeleteSchedule = async (scheduleId) => {
    try {
      // Make API call to delete the schedule
      await axios.delete(`/DoctorSchedule/DeleteDoctorSchedule?id=${scheduleId}`, {
        headers: {
          Authorization: `Bearer ${localStorage.getItem("token")}`,
        },
      });

      // Update the schedules state after successful deletion
      setSchedules(schedules.filter(schedule => schedule.id !== scheduleId));
      toast.success("Schedule deleted successfully!");
    } catch (error) {
      console.error("Error deleting schedule:", error);
      toast.error("Failed to delete schedule.");
      // TODO: Add better error handling, e.g., show a more specific error message, maybe with details from error.response
    }
  };
  const indexOfLastSchedule = currentPage * schedulesPerPage;
  const indexOfFirstSchedule = indexOfLastSchedule - schedulesPerPage;
  const currentSchedules = filteredSchedules.slice(indexOfFirstSchedule, indexOfLastSchedule); 


  const paginate = (pageNumber) => setCurrentPage(pageNumber);

  return (
    <>
      <Navbar />
      {loading ? (
        <Loading />
      ) : (
        <section className="user-section">
          <h2 className="page-heading">Your Schedules</h2>

          {/* Search Input */}
          <div className="search-input">
            <label htmlFor="searchDate">Search by Date:</label>
            <input
              type="date"
              id="searchDate"
              className="form-input"
              value={searchDate}
              onChange={handleSearchDateChange}
            />
          </div>

          <div className="clinic-options-container" style={{margin: "40px 0px"}}>
            {/* Left side: Dropdown to select existing clinic */}
            <div className="clinic-option clinic-option-select">
              <h3>Select Clinic</h3>
              <select
                className="form-input option-select"
                value={selectedClinic}
                onChange={handleClinicChange}
              >
                <option value=""> Select a Clinic</option>
                {clinics.map((clinic) => (
                  <option key={clinic.id} value={clinic.id}>
                    {clinic.clinicName}
                  </option>
                ))}
              </select>
            </div>

            {/* Separator */}
            <div className="separator">
              <span>or</span>
            </div>

            {/* Right side: Form to add a new schedule */}
            <div className="clinic-option">
              <h3>Add a New Schedule</h3>
              <div className="apply-doctor-container flex-center">
                <form onSubmit={addSchedule} className="register-form">
                  <input
                    type="date"
                    name="day"
                    className="form-input"
                    placeholder="Enter day"
                    value={newSchedule.day}
                    onChange={handleInputChange}
                    min={moment().format('YYYY-MM-DD')} // Restrict past dates
                  />
                  <input
                    type="time"
                    name="availableFrom"
                    className="form-input"
                    placeholder="Available From"
                    value={newSchedule.availableFrom}
                    onChange={handleInputChange}
                  />
                  <input
                    type="time"
                    name="availableTo"
                    className="form-input"
                    placeholder="Available To"
                    value={newSchedule.availableTo}
                    onChange={handleInputChange}
                  />
                  <button type="button" className="btn form-btn" onClick={addSchedule}>
                    Add Schedule
                  </button>
                </form>
              </div>
            </div>
          </div>

          {/* Display Schedules */}
          {currentSchedules.length > 0 ? ( // Use currentSchedules for display
        <div className="appointments">
          <table>
            <thead>
              <tr>
                <th>S.No</th>
                <th>Clinic Name</th>
                <th>Day</th>
                <th>Available From</th>
                <th>Available To</th>
                <th>Status</th>
                <th>Actions</th> 
              </tr>
            </thead>
            <tbody>
              {currentSchedules.map((schedule, index) => ( // Use currentSchedules for mapping
                <tr key={schedule.id}>
                  <td>{(currentPage - 1) * schedulesPerPage + index + 1}</td>
                  <td>{clinics.find(clinic => clinic.id === schedule.clinicId)?.clinicName || "Unknown Clinic"}</td>
                  <td>{moment(schedule.day).format('YYYY-MM-DD')}</td>
                  <td>{schedule.availableFrom}</td>
                  <td>{schedule.availableTo}</td>
                  <td>{schedule.status ? "Available" : "Booked"}</td>
                  <td>
                    <button
                      className="btn user-btn"
                      onClick={() => handleDeleteSchedule(schedule.id)}
                    >
                      Delete
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
          {/* Pagination Controls */}
          <div className="pagination">
            {Array.from({ length: Math.ceil(filteredSchedules.length / schedulesPerPage) }, (_, i) => i + 1).map(number => (
              <button key={number} onClick={() => paginate(number)} className={currentPage === number ? 'active' : ''}>
                {number}
              </button>
            ))}
          </div>
        </div>
      ) : (
        <Empty message="No schedules found." />
      )}
        </section>
      )}
      <Footer />
    </>
  );
}

export default DoctorSchedules;