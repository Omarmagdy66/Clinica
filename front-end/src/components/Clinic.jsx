import React, { useEffect, useState } from "react";
import axios from "axios";
import toast from "react-hot-toast";
import Navbar from "../components/Navbar";
import Footer from "../components/Footer";
import Loading from "../components/Loading";
import { useDispatch, useSelector } from "react-redux";
import { setLoading } from "../redux/reducers/rootSlice";
import './../styles/clinica.css'
import jwtDecode from "jwt-decode";
import Empty from "../components/Empty";

axios.defaults.baseURL = "http://clinica.runasp.net/api";

function ClinicsPage() {
  const [clinics, setClinics] = useState([]);
  const [selectedClinic, setSelectedClinic] = useState("");
  const [countries, setCountries] = useState([]);
  const [cities, setCities] = useState([]);
  const [doctorClinics, setDoctorClinics] = useState([]); // State to store doctor's clinics

  const [newClinic, setNewClinic] = useState({
    clinicName: "",
    address: "",
    phoneNumber: "",
    cityId: "",
    countryId: "",
  });
  const dispatch = useDispatch();
  const { loading } = useSelector((state) => state.root);
  const { id } = jwtDecode(localStorage.getItem("token")); // Get doctor ID from token

  useEffect(() => {
    const fetchClinics = async () => {
      try {
        dispatch(setLoading(true));
        const response = await axios.get(
          "http://clinica.runasp.net/api/Clinic/GetAllClinics" // Replace with your API endpoint for getting all clinics
        );
      
        setClinics(response.data);
        dispatch(setLoading(false));
      } catch (error) {
        console.error("Error fetching clinics:", error);
        toast.error("Failed to fetch clinics. Please try again.");
        dispatch(setLoading(false));
      }
    };

    const fetchCountries = async () => {
        try {
          const response = await axios.get(
            "http://clinica.runasp.net/api/Country/GetAllCountries"
          );
          setCountries(response.data);
        } catch (error) {
          console.error("Error fetching countries:", error);
          toast.error("Failed to fetch countries.");
        }
      };

      const fetchDoctorClinics = async () => { // Fetch doctor's clinics
        try {
          const response = await axios.get(`/Clinic/GetClinicByDoctor?doctorId=${id}`);
          setDoctorClinics(response.data);
        } catch (error) {
          console.error("Error fetching doctor clinics:", error);
          toast.error("Failed to fetch doctor clinics.");
          // TODO: Add better error handling, e.g., show a user-friendly error message, maybe with details from error.response
        }
      };
      fetchDoctorClinics();

    fetchClinics();
    fetchCountries()
  }, [dispatch]);


  const handleCountryChange = async (e) => {
    const countryId = e.target.value;
    setNewClinic({ ...newClinic, countryId });

    try {
      if (countryId) {
        const response = await axios.get(
          `http://clinica.runasp.net/api/Cites/GetCityByCountryId?countryid=${countryId}`
        );
        setCities(response.data);
      } else {
        setCities([]); // Clear cities when no country is selected
      }
    } catch (error) {
      console.error("Error fetching cities:", error);
      toast.error("Failed to fetch cities for this country.");
    }
  };
  const handleInputChange = (e) => {
    setNewClinic({ ...newClinic, [e.target.name]: e.target.value });
  };

  const handleClinicChange = (e) => {
    setSelectedClinic(e.target.value);
  };


  const handleExistFromClinic = async (clinicId) => {
    try {
      await axios.delete(`/Doctor/ExitFromClinic?clinicid=${parseInt(clinicId)}`, {
        headers: {
          Authorization: `Bearer ${localStorage.getItem("token")}`,
        },
      });

      // Update doctorClinics state after existing
      setDoctorClinics(doctorClinics.filter(clinic => clinic.id !== clinicId));
      toast.success("Exited from clinic successfully!");
    } catch (error) {
      console.error("Error existing from clinic:", error);
      toast.error("Failed to exist from clinic.");
      // TODO: Add better error handling, e.g., show a more specific error message, maybe with details from error.response
    }
  };

  const addClinic = async () => {
    try {
        console.log(newClinic);
        
      dispatch(setLoading(true));
      const response = await axios.post(
        "http://clinica.runasp.net/api/Doctor/RequestClinic", // Replace with your API endpoint for adding a clinic
        newClinic,{
            headers: {
              Authorization: `Bearer ${localStorage.getItem("token")}`, // Include authentication header if needed
              'Content-Type': 'application/json'
            },
          }
      );
      setClinics([...clinics, response.data]);
      setNewClinic({
        clinicName: "",
        address: "",
        phoneNumber: "",
        cityId: "",
        countryId: "",
      });
      toast.success("Clinic added successfully!");
      dispatch(setLoading(false));
    } catch (error) {
      console.error("Error adding clinic:", error);
      toast.error("Failed to add clinic. Please try again.");
      dispatch(setLoading(false));
    }
  };

  const selectClinic = async () => {    
    try {
      // Assuming you have an API endpoint to associate a doctor with a clinic
      // Replace the endpoint and request body with your actual API call
      const response = await axios.post(
        `http://clinica.runasp.net/api/Doctor/RequestClinic?clinicid=${selectedClinic}`,
        null, // No data in the request body
        { 
            headers: { 
                Authorization: `Bearer ${localStorage.getItem("token")}`,
                'Content-Type': 'application/json' 
            }
        }
    );

      // Handle successful clinic selection (e.g., show a success message, update UI)
      if(response.data === "the Clinic is already Exits"){
        toast.error("the Clinic is already Exist!");
      }else{
      toast.success("Clinic selected successfully!");
      console.log("Clinic selection response:", response.data);
      console.log(response.data)
    }
    } catch (error) {
      console.error("Error selecting clinic:", error);
      toast.error("Failed to select clinic. Please try again.");
    }
  };
  return (
    <>
      <Navbar />
      {loading ? (
        <Loading />
      ) : (
        <section className="user-section">
          <h2 className="page-heading">Clinics</h2>

          <div className="clinic-options-container">
            {/* Left side: Dropdown to select existing clinic */}
            <div className="clinic-option clinic-option-select">
              <h3>Select Existing</h3>
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
              <button className="btn form-btn" onClick={selectClinic}>
                Select Clinic
              </button>
            </div>

            {/* Separator */}
            <div className="separator">
              <span>or</span>
            </div>

            {/* Right side: Form to add a new clinic */}
            <div className="clinic-option">
              <h3>Request To Add a New Clinic</h3>
              <div className="apply-doctor-container flex-center"> 
            <form onSubmit={addClinic} className="register-form">
              <input
                type="text"
                name="clinicName"
                className="form-input"
                placeholder="Enter clinic name"
                value={newClinic.clinicName}
                onChange={handleInputChange}
              />
              <input
                type="text"
                name="address"
                className="form-input"
                placeholder="Enter address"
                value={newClinic.address}
                onChange={handleInputChange}
              />
              <input
                type="text"
                name="phoneNumber"
                className="form-input"
                placeholder="Enter phone number"
                value={newClinic.phoneNumber}
                onChange={handleInputChange}
              />
              <select
                name="countryId"
                className="form-input"
                value={newClinic.countryId}
                onChange={handleCountryChange}
                required
                >
          <option value="">Select Country</option>
          {countries.map((country) => (
            <option key={country.id} value={country.id}>
              {country.name}
            </option>  
          ))}
        </select>

        <select
          name="cityId"
          className="form-input"
          value={newClinic.cityId}
          onChange={handleInputChange}
          required
          disabled={!newClinic.countryId} // Disable if no country is selected
        >
          <option value="">Select City</option>
          {cities.map((city) => (
            <option key={city.id} value={city.id}>
              {city.name}
            </option>
          ))}
        </select>
              <button type="button" className="btn form-btn" onClick={addClinic}>
                Add Clinic
              </button>
            </form>
          </div>
            </div>
          </div>
       
       <div>
       <h2 className="page-heading">Your Clinics</h2>

{/* Display Doctor's Clinics */}
{doctorClinics.length > 0 ? (
  <div className="appointments">
    <table>
      <thead>
        <tr>
          <th>S.No</th>
          <th>Clinic Name</th>
          <th>Address</th>
          <th>Phone Number</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        {doctorClinics.map((clinic, index) => (
          <tr key={clinic.id}>
            <td>{index + 1}</td>
            <td>{clinic.clinicName}</td>
            <td>{clinic.address}</td>
            <td>{clinic.phoneNumber}</td>
            <td>
              <button
                className="btn user-btn"
                onClick={() => handleExistFromClinic(clinic.id)}
              >
                Exist
              </button>
            </td>
          </tr>
        ))}
      </tbody>
    </table>
  </div>
) : (
  <Empty message="No clinics found." />
)}
       </div>
        </section>

        
      )}
      <Footer />
    </>
  );
}

export default ClinicsPage;