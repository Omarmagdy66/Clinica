// import React, { useState, useEffect } from "react";
// import axios from "axios";
// import toast from "react-hot-toast";
// import Loading from "./Loading";
// import { setLoading } from "../redux/reducers/rootSlice.js";
// import { useDispatch, useSelector } from "react-redux";
// import Empty from "./Empty";
// import fetchData from "../helper/apiCall";
// import "../styles/user.css";

// axios.defaults.baseURL = process.env.REACT_APP_SERVER_DOMAIN;

// const AdminDoctors = () => {
//   const [doctors, setDoctors] = useState([]);
//   const [filter, setFilter] = useState("all");
//   const [searchTerm, setSearchTerm] = useState("");

//   const dispatch = useDispatch();
//   const { loading } = useSelector((state) => state.root);

//   const getAllDoctors = async () => {
//     try {
//       dispatch(setLoading(true));
//       let url = "/doctor/getalldoctors";
//       if (filter !== "all") {
//         url += `?filter=${filter}`;
//       }
//       if (searchTerm.trim() !== "") {
//         url += `${filter !== "all" ? "&" : "?"}search=${searchTerm}`;
//       }
//       const temp = await fetchData(url);
//       setDoctors(temp);
//       dispatch(setLoading(false));
//     } catch (error) {}
//   };

//   const deleteUser = async (userId) => {
//     try {
//       const confirm = window.confirm("Are you sure you want to delete?");
//       if (confirm) {
//         await toast.promise(
//           axios.put(
//             "/doctor/deletedoctor",
//             { userId },
//             {
//               headers: {
//                 authorization: `Bearer ${localStorage.getItem("token")}`,
//               },
//             }
//           ),
//           {
//             success: "Doctor deleted successfully",
//             error: "Unable to delete Doctor",
//             loading: "Deleting Doctor...",
//           }
//         );
//         getAllDoctors();
//       }
//     } catch (error) {
//       return error;
//     }
//   };

//   useEffect(() => {
//     getAllDoctors();
//   }, []);

//   const filteredDoctors = doctors.filter((doc) => {
//     if (filter === "all") {
//       return true;
//     } else if (filter === "specialization") {
//       return doc.specialization
//         .toLowerCase()
//         .includes(searchTerm.toLowerCase());
//     } else if (filter === "firstname") {
//       return (
//         doc.userId &&
//         doc.userId.firstname.toLowerCase().includes(searchTerm.toLowerCase())
//       );
//     } else {
//       return true;
//     }
//   });

//   return (
//     <>
//       {loading ? (
//         <Loading />
//       ) : (
//         <section className="user-section">
//           <div className="ayx">
//             <div className="filter">
//               <label htmlFor="filter">Filter by:</label>
//               <select
//                 id="filter"
//                 value={filter}
//                 onChange={(e) => setFilter(e.target.value)}
//               >
//                 <option value="all">All</option>
//                 <option value="firstname">Name</option>
//                 <option value="specialization">Specialization</option>
//               </select>
//             </div>

//             <div className="search">
//               <label htmlFor="search">Search:</label>
//               <input
//                 type="text"
//                 className="form-input"
//                 id="search"
//                 value={searchTerm}
//                 onChange={(e) => setSearchTerm(e.target.value)}
//                 placeholder="Search"
//               />
//             </div>
//           </div>
//           <h3 className="home-sub-heading">All Doctors</h3>
//           {filteredDoctors.length > 0 ? (
//             <div className="user-container">
//               <table>
//                 <thead>
//                   <tr>
//                     <th>S.No</th>
//                     <th>Pic</th>
//                     <th>First Name</th>
//                     <th>Last Name</th>
//                     <th>Email</th>
//                     <th>Mobile No.</th>
//                     <th>Experience</th>
//                     <th>Specialization</th>
//                     <th>Fees</th>
//                     <th>Remove</th>
//                   </tr>
//                 </thead>
//                 <tbody>
//                   {filteredDoctors.map((ele, i) => {
//                     return (
//                       <tr key={ele?._id}>
//                         <td>{i + 1}</td>
//                         <td>
//                           <img
//                             className="user-table-pic"
//                             src={ele?.userId?.pic}
//                             alt={ele?.userId?.firstname}
//                           />
//                         </td>
//                         <td>{ele?.userId?.firstname}</td>
//                         <td>{ele?.userId?.lastname}</td>
//                         <td>{ele?.userId?.email}</td>
//                         <td>{ele?.userId?.mobile}</td>
//                         <td>{ele?.experience}</td>
//                         <td>{ele?.specialization}</td>
//                         <td>{ele?.fees}</td>
//                         <td className="select">
//                           <button
//                             className="btn user-btn"
//                             onClick={() => {
//                               deleteUser(ele?.userId?._id);
//                             }}
//                           >
//                             Remove
//                           </button>
//                         </td>
//                       </tr>
//                     );
//                   })}
//                 </tbody>
//               </table>
//             </div>
//           ) : (
//             <Empty />
//           )}
//         </section>
//       )}
//     </>
//   );
// };

// export default AdminDoctors;

import React, { useEffect, useState } from "react";
import axios from "axios";
import toast from "react-hot-toast";
import Loading from "./Loading";
import { setLoading } from "../redux/reducers/rootSlice.js";
import { useDispatch, useSelector } from "react-redux";
import Empty from "./Empty";
import fetchData from "../helper/apiCall";
import "../styles/user.css";

axios.defaults.baseURL = process.env.REACT_APP_SERVER_DOMAIN;

const AdminDoctors = () => {
  const [requests, setRequests] = useState([]);
  const [filter, setFilter] = useState("all");
  const [searchTerm, setSearchTerm] = useState("");
  const [cities, setCities] = useState({}); // To store city names
  const [countries, setCountries] = useState({});
  const dispatch = useDispatch();
  const { loading } = useSelector((state) => state.root);
  const [doctors, setDoctors] = useState({});
  const getAllRequests = async () => {
    try {
      dispatch(setLoading(true));
      let url = "/Admin/GetAllRequestsClinic";

      // Filtering and search logic (you'll need to adjust this based on the actual data and filtering options)
      // if (filter !== "all") {
        // Add filter parameters to the URL based on the selected filter
        // Example: url += `?filter=${filter}`;
      // }
      // if (searchTerm.trim() !== "") {
        // Add search term to the URL
        // Example: url += `${filter !== "all" ? "&" : "?"}search=${searchTerm}`;
      // }

      const temp = await fetchData(url);
      setRequests(temp);
      dispatch(setLoading(false));
          // Fetch cities and countries after getting requests
          const cityIds = temp.map(request => request.cityId);
          const countryIds = temp.map(request => request.countryId);
          const doctorIds = temp.map(request => request.doctorId);
          await fetchCities(cityIds);
          await fetchCountries(countryIds);
          await fetchDoctors(doctorIds);
    } catch (error) {
      console.error("Error fetching requests:", error);
      toast.error("Failed to fetch clinic requests. Please try again later."); // User-friendly error message
    }
  };
  const fetchCities = async (cityIds) => {
    try {
      const cts = {};
      for (const id of cityIds) {
        if (!cities[id]) {
          const response = await axios.get(`/Cites/GetCityById?id=${id}`); // Assuming this API endpoint exists
          cts[id] = response.data.name;
        }
      }
      setCities(prevCities => ({ ...prevCities, ...cts }));
    } catch (error) {
      console.error("Error fetching cities:", error);
      toast.error("Failed to fetch cities. Please try again later."); // User-friendly error message
    }
  };

  const fetchDoctors = async (doctorIds) => {
    try {
      const docs = {};
      for (const id of doctorIds) {
        if (!doctors[id]) {
          const response = await axios.get(`/Doctor/GetDoctorById?id=${id}`);
          docs[id] = { name: response.data.name, email: response.data.email };
        }
      }
      setDoctors(prevDoctors => ({ ...prevDoctors, ...docs }));
    } catch (error) {
      console.error("Error fetching doctors:", error);
      toast.error("Failed to fetch doctors. Please try again later."); // User-friendly error message
    }
  };
  const fetchCountries = async (countryIds) => {
    try {
      const cntrs = {};
      for (const id of countryIds) {
        if (!countries[id]) {
          const response = await axios.get(`Country/GetCountryById?id=${id}`); // Assuming this API endpoint exists
          cntrs[id] = response.data.name;
        }
      }
      setCountries(prevCountries => ({ ...prevCountries, ...cntrs }));
    } catch (error) {
      console.error("Error fetching countries:", error);
      toast.error("Failed to fetch countries. Please try again later."); // User-friendly error message
    }
  };
  const handleResponse = async (requestId, flag) => {
    try {
      await axios.post(
        `/Admin/AdminResponseDoctor?id=${requestId}&flag=${flag}`,
        null,
        {
          headers: {
            authorization: `Bearer ${localStorage.getItem("token")}`,
          },
        }
      );

      // Update requests state after response
      setRequests(requests.filter(req => req.id !== requestId));
      toast.success(flag === 1 ? "Request accepted successfully!" : "Request refused successfully!");
    } catch (error) {
      console.error("Error responding to request:", error);
      toast.error("Failed to respond to request. Please try again later."); // User-friendly error message
    }
  };

  useEffect(() => {
    getAllRequests();
  }, []);

  // Filtering logic (you'll need to adjust this based on the actual data and filtering options)
  const filteredRequests = requests.filter((req) => {
    if (filter === "all") {
      return true;
    } else if (filter === "clinicName") { // Example filter by clinic name
      return req.clinicName.toLowerCase().includes(searchTerm.toLowerCase());
    } else if (filter === "address") { // Example filter by address
      return req.address.toLowerCase().includes(searchTerm.toLowerCase());
    } else {
      return true;
    }
  });

  return (
    <>
      {loading ? (
        <Loading />
      ) : (
        <section className="user-section">
          <div className="ayx">
            {/* <div className="filter">
              <label htmlFor="filter">Filter by:</label>
              <select
                id="filter"
                value={filter}
                onChange={(e) => setFilter(e.target.value)}
              >
                <option value="all">All</option>
                <option value="clinicName">Clinic Name</option>
                <option value="address">Address</option> 
              </select>
            </div> */}

            {/* ... (search input JSX) */}
          </div>
          <h3 className="home-sub-heading">All Clinic Requests</h3>
          {filteredRequests.length > 0 ? (
            <div className="user-container">
              <table>
                <thead>
                  <tr>
                  <th>S.No</th>
                    <th>Clinic Name</th>
                    <th>Address</th>
                    <th>City</th>
                    <th>Country</th>
                    <th>Doctor Name</th> {/* Added Doctor Name header */}
                    <th>Doctor Email</th> {/* Added Country header */}
                    {/* ... (other table headers - adjust as needed) */}
                    <th>Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {filteredRequests.map((request, i) => {
                                        const doctor = doctors[request.doctorId]; // Get doctor dat
                    return (
                      <tr key={request.id}>
                        <td>{i + 1}</td>
                        <td>{request.clinicName}</td> {/* Example data */}
                        <td>{request.address}</td> {/* Example data */}
                        <td>{cities[request.cityId] || "Loading..."}</td> {/* Show city name */}
                        <td>{countries[request.countryId] || "Loading..."}</td> {/* Show country name */}
                        <td>{doctor ? doctor.name : "Loading..."}</td> {/* Show doctor name */}
                        <td>{doctor ? doctor.email : "Loading..."}</td> 
                        {/* ... (other table data - adjust as needed) */}
                        <td className="select">
                          <button
                            className="btn user-btn"
                            onClick={() => handleResponse(request.id, 1)}
                          >
                            Accept
                          </button>
                          <button
                            className="btn user-btn"
                            onClick={() => handleResponse(request.id, 0)}
                          >
                            Refuse
                          </button>
                        </td>
                      </tr>
                    );
                  })}
                </tbody>
              </table>
            </div>
          ) : (
            <Empty />
          )}
        </section>
      )}
    </>
  );
};

export default AdminDoctors;