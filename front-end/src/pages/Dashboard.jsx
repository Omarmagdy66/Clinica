import React from "react";
import AdminApplications from "../components/AdminApplications";
import AdminAppointments from "../components/AdminAppointments";
import AdminDoctors from "../components/AdminDoctors";
import Sidebar from "../components/Sidebar";
import Users from "../components/Users";
import Home from "../components/Home";
import Aprofile from "../components/Aprofile";
import GetAllRequestsApply from "../components/GetAllRequestsApply";
import AdminQueries from "../components/AdminQueries";

const Dashboard = (props) => {
  console.log(props);
  const { type } = props;
  return (
    <>

      <section className="layout-section">
        <div className="layout-container">
          <Sidebar />

          {
          type === "home" ?(
            <Home />
          ):type === "users" ? (
            <Users />
          ) : type === "doctors" ? (
            <AdminDoctors />
          ) : type === "GetAllRequestsApply" ? (
            <GetAllRequestsApply />
          ) :type === "GetAllQueries" ? (
            <AdminQueries/>
          ) :type === "applications" ? (
            <AdminApplications />
          ) : type === "appointments" ? (
            <AdminAppointments />
          ) : type === "aprofile" ? (
            <Aprofile />
          ): (
            <></>
          )}
        </div>
      </section>
    </>
  );
};

export default Dashboard;
