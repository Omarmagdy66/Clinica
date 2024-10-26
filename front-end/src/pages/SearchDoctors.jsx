import React, { useState, useEffect } from 'react';
import axios from 'axios';
import '../styles/search.css';
import Navbar from '../components/Navbar';
import Empty from '../components/Empty';
import "../styles/user.css";
import "../styles/appointments.css"
import Footer from '../components/Footer';
import { FaClock, FaSearch } from 'react-icons/fa';
import BookingPage from './BookingPage';

function SearchPage() {

  const [searchResults, setSearchResults] = useState([]);
  const [currentPage, setCurrentPage] = useState(1);
  const PerPage = 4;
  const [specializations, setSpecializations] = useState([]);
  const [countries, setCountries] = useState([]);
  const [cities, setCities] = useState([]);
  const [selectedSpecialization, setSelectedSpecialization] = useState('');
  const [selectedCountry, setSelectedCountry] = useState('');
  const [selectedCity, setSelectedCity] = useState('');
  const [specializationNames, setSpecializationNames] = useState({});
  const [showBooking, setShowBooking] = useState(false); // State to control the booking page visibility
  const [selectedDoctor, setSelectedDoctor] = useState(null); // State to store the selected doctor's data

  useEffect(() => {
    const fetchSpecializations = async () => {
      try {
        const response = await axios.get(`/Specialization`);
        setSpecializations(response.data);
        console.log(response.data);

        response.data.map((data) => {
          getSpecialization(data.id)
        })
      } catch (error) {
        console.error('Error fetching specializations:', error);
      }
    };

    const fetchCountries = async () => {
      try {
        const response = await axios.get('Country/GetAllCountries');
        setCountries(response.data);
        console.log(response.data);

      } catch (error) {
        console.error('Error fetching countries:', error);
      }
    };

    fetchSpecializations();
    fetchCountries();
  }, []);

  const handleCountryChange = async (event) => {
    const countryId = event.target.value;
    setSelectedCountry(countryId);

    try {
      if (countryId) {
        const response = await axios.get(`/Cites/GetCityByCountryId?countryId=${countryId}`);
        setCities(response.data);
        console.log(response.data);

      } else {
        setCities([]);
      }
    } catch (error) {
      console.error('Error fetching cities:', error);
    }
  };

  const getSpecialization = async (id) => {
    if (specializationNames[id]) {
      return specializationNames[id];
    }

    try {
      const response = await axios.get(`/Specialization/${id}`);
      const specializationName = response.data.specializationName;
      console.log(response.data);

      setSpecializationNames(prevNames => ({ ...prevNames, [id]: specializationName }));
      return specializationName;
    } catch (error) {
      console.error('Error fetching specialization:', error);
      return 'Unknown Specialization';
    }
  };

  const handleSearch = async () => {
    try {
      // Construct the URL with query parameters
      const searchUrl = `http://clinica.runasp.net/api/Doctor/DoctorSearch?`;
      const queryParams = new URLSearchParams();
      if (selectedSpecialization) {
        queryParams.append('Specialization', selectedSpecialization);
      }
      if (selectedCountry) {
        queryParams.append('Country', selectedCountry);
      }
      if (selectedCity) {
        queryParams.append('city', selectedCity);
      }

      // Make the API call
      const response = await axios.get(searchUrl + queryParams.toString());
      setSearchResults(response.data);
    } catch (error) {
      console.error('Error fetching search results:', error);
      // Handle the error, e.g., show an error message to the user
    }

    console.log(searchResults);

  };

  const totalPages = Math.ceil(searchResults.length / PerPage);

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

  const paginatedSearchResults = searchResults.slice(
    (currentPage - 1) * PerPage,
    currentPage * PerPage
  );

  const handleBookNow = async (doctor) => {
    setSelectedDoctor(doctor); // Store the selected doctor's data
    setShowBooking(true); // Show the BookingPage
  };

  const handleCloseBooking = () => {
    setShowBooking(false); // Hide the BookingPage
    setSelectedDoctor(null); // Clear the selected doctor
  };

  return <>
    <Navbar />

    <div className="search-page">
      {/* Add a container for styling */}
      <h1>Search</h1>

      <div className='cont-big'>
        <div className='con-inn'>
          <label htmlFor="specialization">Specialization:</label>
          <select
            id="specialization"
            value={selectedSpecialization}
            onChange={(e) => setSelectedSpecialization(e.target.value)}
          >
            <option value="">All Specializations</option>

            {specializations.map((spec) => (
              <option key={spec.id} value={spec.id}>
                {spec.specializationName}
              </option>
            ))}
          </select>
        </div>
        <div className='con-inn'>
          <label htmlFor="country">Country:</label>
          <select
            id="country"
            value={selectedCountry}
            onChange={handleCountryChange}
          >
            <option value="">All
              Countries</option>
            {countries.map((country) => (
              <option key={country.id} value={country.id}>
                {country.name}
              </option>
            ))}
          </select>
        </div>
        <div className='con-inn'>
          <label htmlFor="city">City:</label>

          <select
            id="city"
            value={selectedCity}
            onChange={(e) => setSelectedCity(e.target.value)}
            disabled={!selectedCountry}
          >
            <option value="">All Cities</option>
            {cities.map((city) => (
              <option key={city.id} value={city.id}>
                {city.name}
              </option>
            ))}
          </select>
        </div>
        <button className='btn form-btn' onClick={handleSearch}>Search <FaSearch /></button>
      </div>
      <div className="container notif-section">
        {searchResults.length > 0 ? (
          <div className="appointments">
            <table>
              <thead>
                <tr>
                  <th>S.No</th>
                  <th>P Name</th>
                  <th>Bio</th>
                  <th>specialization</th>
                  <th>P Mobile No.</th>
                  <th>examination duration</th>
                  <th>price</th>
                  <th>Action</th>
                </tr>
              </thead>
              <tbody>
                {paginatedSearchResults.map((result, index) => (
                  <tr key={result.id}>
                    <td>{(currentPage - 1) * PerPage + index + 1}</td>
                    <td>{`${result.name}`}</td>
                    <td>{`${result.bio}`}</td>
                    <td>{specializationNames[result.specializationId] || 'Loading...'}</td>
                    <td>{result.phoneNumber}</td>
                    <td>{result.examinationduration}</td>
                    <td>{result.price}</td>
                    <td>
                      <button
                        className="btn user-btn complete-btn"
                        onClick={() => handleBookNow(result)} // Pass the doctor object
                      >
                        Book Now <FaClock />
                      </button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
            <div className="pagination">{renderPagination()}</div>
          </div>
        ):(
          <Empty message="No appointments found." />
        )}
      </div>
 {/* Booking Page (Conditional Rendering) */}
    {showBooking && (
      <BookingPage
        doctor={selectedDoctor} // Pass the selected doctor object
        onClose={handleCloseBooking}
      />
    )}
    </div>

    <div className="">
      <Footer />
    </div>

  </>

}
export default SearchPage;