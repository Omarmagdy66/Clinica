import React from "react";
import {
  FaHome,
  FaList,
  FaUser,
  FaUserMd,
  FaUsers,
  FaEnvelope,
} from "react-icons/fa";
import "../styles/sidebar.css";
import { NavLink, useNavigate } from "react-router-dom";
import { MdLogout } from "react-icons/md";
import { useDispatch } from "react-redux";
import { setUserInfo } from "../redux/reducers/rootSlice.js";

const Sidebar = () => {
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const sidebar = [
    {
      name: "Home",
      path: "/dashboard/home",
      icon: <FaHome />,
    },
    // {
    //   name: "Users",
    //   path: "/dashboard/users",
    //   icon: <FaUsers />,
    // },
    {
      name: "Clinics Requests",
      path: "/dashboard/clinicRequests",
      icon: <FaUserMd />,
    },
    {
      name: "All Requests Apply",
      path: "/dashboard/getAllRequestsApply",
      icon: <FaUserMd />,
    },
    {
      name: "All Queries",
      path: "/dashboard/getAllQueries",
      icon: <FaUserMd />,
    },
    // {
    //   name: "Appointments",
    //   path: "/dashboard/appointments",
    //   icon: <FaList />,
    // },
    // {
    //   name: "Applications",
    //   path: "/dashboard/applications",
    //   icon: <FaEnvelope />,
    // },
    // {
    //   name: "Profile",
    //   path: "/dashboard/aprofile",
    //   icon: <FaUser />,
    // },
  ];

  const logoutFunc = () => {
    dispatch(setUserInfo({}));
    localStorage.removeItem("token");
    navigate("/login");
  };

  return (
    <>
      <section className="sidebar-section flex-center">
        <div className="sidebar-container">
          <ul>
            {sidebar.map((ele, i) => {
              return (
                <li key={i}>
                  {ele.icon}
                  <NavLink to={ele.path}>{ele.name}</NavLink>
                </li>
              );
            })}
          </ul>
          <div className="logout-container">
            <MdLogout />
            <p onClick={logoutFunc}>Logout</p>
          </div>
        </div>
      </section>
    </>
  );
};

export default Sidebar;
