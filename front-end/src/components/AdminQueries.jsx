import React, { useState, useEffect } from "react";
import axios from "axios";
import toast from "react-hot-toast";
import Loading from "./Loading";
import { setLoading } from "../redux/reducers/rootSlice.js";
import { useDispatch, useSelector } from "react-redux";
import Empty from "./Empty";
import fetchData from "../helper/apiCall";
import "../styles/user.css";

axios.defaults.baseURL = process.env.REACT_APP_SERVER_DOMAIN;

const AdminQueries = () => {
  const [queries, setQueries] = useState([]);
  const dispatch = useDispatch();
  const { loading } = useSelector((state) => state.root);

  const getAllQueries = async () => {
    try {
      dispatch(setLoading(true));
      const temp = await fetchData(`/Query/GetAll`); 
      setQueries(temp);
      dispatch(setLoading(false));
    } catch (error) {}
  };

  const markAsSolved = async (queryId) => {
    try {
      const confirm = window.confirm("Are you sure you want to mark this query as solved?");
      console.log(localStorage.getItem("token"))
      if (confirm) {
        await toast.promise(
          axios.put(
            `https://clinica.runasp.net/api/Query/SolveQuery?id=${queryId}`,
          ),
            {
            success: "Query marked as solved",
            error: "Unable to update query status",
            loading: "Updating query status...",
            }
        );
        getAllQueries(); 
      }
    } catch (error) {
      return error;
    }
  };

  const deleteQuery = async (queryId) => {
    try {
      const confirm = window.confirm("Are you sure you want to delete this query?");
      if (confirm) {
        await toast.promise(
          axios.delete(
            `/Query/Delete?id=${queryId}`, 
            {
              headers: {
                authorization: `Bearer ${localStorage.getItem("token")}`,
              },
            }
          ),
          {
            success: "Query deleted",
            error: "Unable to delete query",
            loading: "Deleting query...",
          }
        );
        getAllQueries(); 
      }
    } catch (error) {
      return error;
    }
  };

  useEffect(() => {
    getAllQueries(); 
  }, []);

  return (
    <>
      {loading ? (
        <Loading />
      ) : (
        <section className="user-section">
          <h3 className="home-sub-heading">All Queries</h3>
          {queries.length > 0 ? (
            <div className="user-container">
              <table>
                <thead>
                  <tr>
                    <th>S.No</th>
                    <th>Name</th>
                    <th>Email</th>
                    <th>Query Text</th>
                    <th>Status</th>
                    <th>Date</th>
                    <th>Action</th>
                  </tr>
                </thead>
                <tbody>
                  {queries.map((query, i) => {
                    const formattedDate = new Date(query.queryDate).toLocaleDateString('en-GB');
                    return (
                      <tr key={query.id}>
                        <td>{i + 1}</td>
                        <td>{query.name}</td>
                        <td>{query.email}</td>
                        <td>{query.queryText}</td>
                        <td>{query.queryStatus}</td>
                        <td>{formattedDate}</td> 
                        <td className="select">
                          <button
                            className="btn user-btn accept-btn"
                            onClick={() => {
                              markAsSolved(query.id);
                            }}
                            disabled={query.queryStatus === "Solved"} 
                          >
                            {query.queryStatus === "Solved" ? "Solved" : "Mark as Solved"} 
                          </button>
                          <button
                            className="btn user-btn"
                            onClick={() => {
                              deleteQuery(query.id);
                            }}
                          >
                            Delete
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

export default AdminQueries;