import React, { useState, useEffect } from 'react';
import axios from 'axios';
import moment from 'moment';
import '../styles/booking.css';
import '../styles/appointments.css';
import '../styles/user.css';
import toast from 'react-hot-toast';
import { BsCalendarDate, BsClock } from 'react-icons/bs';
import jwt_decode from 'jwt-decode';
import { useNavigate } from 'react-router-dom';
import jwtDecode from 'jwt-decode';

function BookingPage({ doctor, onClose }) {
  const [clinics, setClinics] = useState([]);
  const [selectedClinic, setSelectedClinic] = useState(null);
  const [schedules, setSchedules] = useState([]);
  const [selectedSchedule, setSelectedSchedule] = useState(null);
  const [teleconsultation, setTeleconsultation] = useState(false);
  const [selectedDate, setSelectedDate] = useState(moment().format('YYYY-MM-DD'));
  const [filteredSchedules, setFilteredSchedules] = useState([]);
  const [patientData, setPatientData] = useState(null)
  const navto = useNavigate()
  // let tokenId = jwt_decode(localStorage.getItem('token')).id

  useEffect(() => {
    const fetchClinics = async () => {
      if (!doctor) return;

      try {
        const response = await axios.get(`/Clinic/GetClinicByDoctor?doctorId=${doctor.id}`);
        setClinics(response.data);
        console.log(response.data);

      } catch (error) {
        console.error('Error fetching clinics:', error);
        // TODO: Add better error handling, e.g., show a user-friendly error message to the user
      }
    };
    const token = localStorage.getItem('token');
    if (token) {
        let tokenId = jwt_decode(token).id
        getPatientData(tokenId) // Pass tokenId to getPatientData
    }
    // getPatientData()
    fetchClinics();
  }, [doctor]);

  const handleClinicChange = async (event) => {
    const clinicId = event.target.value;
    setSelectedClinic(clinicId);
    setSelectedSchedule(null);

    try {
      if (clinicId) {
        const response = await axios.get(`/DoctorSchedule/GetSchedulesByDoctorId?idDoctor=${doctor.id}`);
        const filteredSchedules = response.data.filter(schedule => schedule.clinicId === parseInt(clinicId, 10));
        console.log(response.data);

        setSchedules(filteredSchedules);
      } else {
        setSchedules([]);
      }
    } catch (error) {
      console.error('Error fetching schedules:', error);
      // TODO: Add better error handling, e.g., show a user-friendly error message to the user
    }
  };

  const handleScheduleChange = (schedule) => {
    setSelectedSchedule(schedule);
  };

  const handleTeleconsultationChange = (event) => {
    setTeleconsultation(event.target.checked);
  };

  const handleDateChange = (event) => {
    const selectedDate = event.target.value;
    const today = moment().format('YYYY-MM-DD');

    if (selectedDate < today) {
      toast.error('You cannot select a past date.');
      setSelectedDate(today); // Reset to today's date
    } else {
      setSelectedDate(selectedDate);
    }
  };

  useEffect(() => {
    const filterSchedulesByDate = () => {
      const filtered = schedules.filter(schedule =>
        moment(schedule.day).format('YYYY-MM-DD') === selectedDate
      );
      setFilteredSchedules(filtered);
    };

    filterSchedulesByDate();
  }, [schedules, selectedDate]);

  const getPatientData = async (tokenId) => {
    try {
      // console.log(tokenId);
      // let tokenId = jwt_decode(localStorage.getItem('token')).id
        
      const response = await axios.get(`http://clinica.runasp.net/api/Patient/GetPatientById?id=${tokenId}`)
      setPatientData(response.data)
      console.log(response.data);

    } catch (error) {
      console.log(error);
      // TODO: Add better error handling, e.g., show a user-friendly error message to the user
    }
  }
  const handleBookAppointment = async () => {
    if (!localStorage.getItem('token')) {
        toast.error('You Should Login Frist')
        navto('/login')
    }
    if (!selectedSchedule) {
      toast.error('Please select an available schedule.')
      return;
    }
   let tokenId = jwtDecode(localStorage.getItem('token')).id

    console.log(tokenId);
    
    try {
      const appointmentData = {
        doctorId: doctor.id,
        patientId: tokenId,
        patientNameIN: patientData.name,
        patientNumberIN: patientData.phoneNumber,
        patientEmailIN: patientData.email,
        appointmentDate: moment(selectedSchedule.day).format('YYYY-MM-DD'),
        timeSlot: selectedSchedule.availableFrom,
        teleconsultation: teleconsultation,
      };

      console.log(appointmentData);

 
      const response = await axios.post('/Appointment/AddAppointment', appointmentData, {
        headers: {
          authorization: `Bearer ${localStorage.getItem("token")}`,
        },

      });
      console.log('Appointment booked successfully:', response.data);
      toast.success('Appointment booked successfully!')
      // TODO: You might want to provide more feedback to the user after booking (e.g., show a confirmation message or redirect)
      onClose();
    } catch (error) {
      console.error('Error booking appointment:', error);
      toast.error('Failed to book appointment.')
      // TODO: Add better error handling, e.g., show a more specific error message based on the error response
    }
  };

  const getSelectedClinicAddress = () => {
    if (!selectedClinic) {
      return ''; // Or a default message like "Select a clinic to see the address"
    }

    const clinic = clinics.find(clinic => clinic.id === parseInt(selectedClinic, 10));
    return clinic ? clinic.address : ''; // Or handle the case where the clinic is not found in a better way
  };

  return (
    <div className="booking-page">
      <div className="booking-content">
        <h2>Book Appointment with <span style={{color: "tomato"}}>{doctor ? doctor.name : ''}</span></h2>
        <div className='top-schedules'>

          {/* Clinic Selection */}
          <div>
            <label htmlFor="clinic">Select Clinic:</label>
            <select
              id="clinic"
              value={selectedClinic || ''}
              onChange={handleClinicChange}
            >
              <option value="">Select Clinic</option>
              {clinics.map((clinic) => (
                <option key={clinic.id} value={clinic.id}>
                  {clinic.clinicName}
                </option>
              ))}
            </select>

            {/* Display Clinic Address */}
            <p>Address of This Clinic is: {getSelectedClinicAddress()}</p>
          </div>

          {/* Date Filter */}
          <div>
            <label htmlFor="date">Select Date:</label>
            <input
              type="date"
              className="form-input"
              id="date"
              value={selectedDate}
              onChange={handleDateChange}
              min={moment().format('YYYY-MM-DD')} // Set min attribute to today's date
            />
          </div>
        </div>

        {/* Schedule Display */}
        <div className="schedules">
          <h3>Available Schedules:</h3>
          <div className='show-shedules'>
            {filteredSchedules.length > 0 ? (
              filteredSchedules.map((schedule) => (
                <div
                  key={schedule.id}
                  className={`schedule-item ${selectedSchedule && selectedSchedule.id === schedule.id ? 'selected' : ''} ${schedule.status ? '' : 'disabled'}`}
                  onClick={() => schedule.status && handleScheduleChange(schedule)}
                >
                  <p style={{ textAlign: 'left', paddingLeft: '7px' }}>

                    <span ><BsCalendarDate />{" "} {moment(schedule.day).format('YYYY-MM-DD')}<br /></span>
                    <BsClock /> {" "}
                    <span>{schedule.availableFrom} to {schedule.availableTo}</span>
                  </p>
                  {/* You can add more schedule details here, e.g., status */}
                  {/* <p>Status: {schedule.status ? 'Available' : 'Booked'}</p> */}
                </div>
              ))
            ) : (
              <p>No schedules found for this clinic on this date.</p>
            )}
          </div>
        </div>

        {/* Teleconsultation Option */}
        <div>
          <input
            type="checkbox"
            id="teleconsultation"
            checked={teleconsultation}
            onChange={handleTeleconsultationChange}
          />
          <label htmlFor="teleconsultation"> Teleconsultation</label>
        </div>

        <div className='top-schedules ' style={{ marginTop: "10px" }}>
          {/* Book Appointment Button */}
          <button onClick={handleBookAppointment} className='btn' disabled={!selectedSchedule}>
            Book Appointment
          </button>

          {/* Close Button */}
          <button onClick={onClose} className='btn user-btn complete-btn' style={{ padding: '11px' }}>Close</button>
        </div>
      </div>
    </div>
  );
}

export default BookingPage;