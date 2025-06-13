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

const GetAllRequestsApply = () => {
  const [requests, setRequests] = useState([]);
  const [filter, setFilter] = useState("all");
  const [searchTerm, setSearchTerm] = useState("");
  const [specializations, setSpecializations] = useState({}); // To store specializations
  const [clinics, setClinics] = useState({}); // To store clinics
  const dispatch = useDispatch();
  const { loading } = useSelector((state) => state.root);

  const getAllRequests = async () => {
    try {
      dispatch(setLoading(true));
      let url = "/Admin/GetAllRequestsApply"; // Updated API endpoint

      // Filtering and search logic (you'll need to adjust this based on the actual data and filtering options)
      // if (filter !== "all") {
        // Add filter parameters to the URL based on the selected filter
        // Example: url += `?filter=${filter}`;
      // }
      // // if (searchTerm.trim() !== "") {
        // Add search term to the URL
        // Example: url += `${filter !== "all" ? "&" : "?"}search=${searchTerm}`;
      // }

      const temp = await fetchData(url);
      setRequests(temp);
      dispatch(setLoading(false));

         // Fetch specializations and clinics after getting requests
         const specializationIds = temp.map(request => request.specializationId);
        //  const clinicIds = temp.map(request => request.clinicId); // Assuming clinicId is available in the response
         await fetchSpecializations(specializationIds);
        //  await fetchClinics(clinicIds);
    } catch (error) {
      console.error("Error fetching requests:", error);
      toast.error("Failed to fetch doctor applications. Please try again later."); // User-friendly error message
    }
  };
  const fetchSpecializations = async (specializationIds) => {
    try {
      const specs = {};
      for (const id of specializationIds) {
        if (!specializations[id]) {
          const response = await axios.get(`/Specialization/${id}`);
          specs[id] = response.data.specializationName;
        }
      }
      setSpecializations(prevSpecs => ({ ...prevSpecs, ...specs }));
    } catch (error) {
      console.error("Error fetching specializations:", error);
      // TODO: Add better error handling for fetching specializations
    }
  };

  // const fetchClinics = async (clinicIds) => {
  //   try {
  //     const clins = {};
  //     for (const id of clinicIds) {
  //       if (!clinics[id]) {
  //         const response = await axios.get(`/Clinic/GetAllClinics/${id}`); // Assuming this API endpoint exists
  //         clins[id] = response.data.clinicName;
  //       }
  //     }
  //     setClinics(prevClinics => ({ ...prevClinics, ...clins }));
  //   } catch (error) {
  //     console.error("Error fetching clinics:", error);
  //     // TODO: Add better error handling for fetching clinics
  //   }
  // };

  const handleResponse = async (requestId, flag) => {
    try {
      await axios.post(
        `/Admin/AdminResponseApplyDoctor?id=${requestId}&flag=${flag}`, // Updated API endpoint
        null,
        {
          headers: {
            authorization: `Bearer ${localStorage.getItem("token")}`,
          },
        }
      );

      // Update requests state after response
      setRequests(requests.filter(req => req.id !== requestId));
      toast.success(flag === 1 ? "Application accepted successfully!" : "Application refused successfully!");
    } catch (error) {
      console.error("Error responding to application:", error);
      toast.error("Failed to respond to application. Please try again later."); // User-friendly error message
    }
  };

  useEffect(() => {
    getAllRequests();
  }, []);

  // Filtering logic (you'll need to adjust this based on the actual data and filtering options)
  const filteredRequests = requests.filter((req) => {
    if (filter === "all") {
      return true;
    } else if (filter === "name") { // Example filter by name
      return req.name.toLowerCase().includes(searchTerm.toLowerCase());
    } else if (filter === "email") { // Example filter by email
      return req.email.toLowerCase().includes(searchTerm.toLowerCase());
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
                <option value="name">Name</option> 
                <option value="email">Email</option> 
              </select>
            </div> */}

            {/* ... (search input JSX) */}
          </div>
          <h3 className="home-sub-heading">All Doctor Applications</h3> {/* Updated heading */}
          {filteredRequests.length > 0 ? (
            <div className="user-container">
              <table>
                <thead>
                  <tr>
                    <th>S.No</th>
                    <th>Name</th>
                    <th>Email</th>
                    <th>Specialization</th> {/* Added Specialization header */}
                    <th>bio</th> {/* Added Clinic header */}
                    <th>examinationduration</th> {/* Added Clinic header */}
                    {/* ... (other table headers - adjust as needed) */}
                    <th>Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {filteredRequests.map((request, i) => {
                    return (
                      <tr key={request.id}>
                        <td>{i + 1}</td>
                        <td>{request.name}</td>
                        <td>{request.email}</td>
                        <td>{specializations[request.specializationId] || "Loading..."}</td> {/* Show specialization name */}
                        <td>{request.bio}</td> {/* Show clinic name */}
                        <td>{request.examinationduration}</td> {/* Show clinic name */}
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

export default GetAllRequestsApply;