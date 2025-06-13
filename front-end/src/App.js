import { useState } from "react";
import "./styles/app.css";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import Login from "./pages/Login.jsx";
import Register from "./pages/Register.jsx";
import ForgotPassword from "./pages/ForgotPassword.jsx";
import ResetPassword from "./pages/ResetPassword.jsx";
import { Toaster } from "react-hot-toast";
import { Protected, Public, Admin } from "./middleware/route.js";
import React, { lazy, Suspense } from "react";
import Loading from "./components/Loading.jsx";
import Dashboard from "./pages/Dashboard.jsx";
import RegisterAsDoctor from "./pages/RegisterAsDoctor.jsx";
import ClinicsPage from "./components/Clinic.jsx";
import SearchDoctors from "./pages/SearchDoctors.jsx";
import PatientAppointments from "./pages/PatientAppointments.jsx";
import ProfilePatient from "./pages/ProfilePatient.jsx";
import DoctorSchedules from "./pages/DoctorSchedules.jsx";
import ChatBot from "./components/ChatBot.jsx";

const Aprofile = lazy(()=>import("./components/Aprofile.jsx"))
const Home = lazy(() => import("./pages/Home.jsx"));
const Appointments = lazy(() => import("./pages/Appointments.jsx"));
const Doctors = lazy(() => import("./pages/Doctors.jsx"));
const Profile = lazy(() => import("./pages/Profile.jsx"));
const Change = lazy(() => import("./pages/ChangePassword.jsx"));
const DasHome = lazy(() => import("./components/Home.jsx"));
const Notifications = lazy(() => import("./pages/Notifications.jsx"));
const ApplyDoctor = lazy(() => import("./pages/ApplyDoctor.jsx"));
const Error = lazy(() => import("./pages/Error.jsx"));
const ImageModels = lazy(() => import("./pages/ImageModels.jsx"));

function App() {
  const [userRole, setUserRole] = useState("");
  return (
    <Router>
      <Toaster />
      <Suspense fallback={<Loading />}>

        <Routes>
          <Route path="/login" element={<Login />} />
          <Route path="/forgotpassword" element={<ForgotPassword />} />
          <Route path="/resetpassword/:id/:token" element={<ResetPassword />} />
          <Route
            path="/register/patient"
            element={
              <Public>
                <Register />
              </Public>
            }
          />
          <Route path="/" element={<Home />} />
          <Route
            path="/register/doctor"
            element={
              <Public>
                <RegisterAsDoctor />
              </Public>
            }
          />
          <Route
            path="/search/doctors"
            element={
                <SearchDoctors />
            }
          />
          <Route path="/doctors" element={<Doctors />} />
          <Route
            path="/appointments"
            element={
              <Protected>
                <Appointments />
              </Protected>
            }
          />
          <Route
            path="/patient/profile"
            element={
              <Protected>
                <ProfilePatient />
              </Protected>
            }
          />
          <Route
            path="/patient/appointments"
            element={
              <Protected>
                <PatientAppointments />
              </Protected>
            }
          />
          <Route
            path="/notifications"
            element={
              <Protected>
                <Notifications />
              </Protected>
            }
          />
          <Route
            path="/clinic"
            element={
              <Protected>
                <ClinicsPage />
              </Protected>
            }
          />
          <Route
            path="/applyfordoctor"
            element={
              <Protected>
                <ApplyDoctor />
              </Protected>
            }
          />
          <Route
            path="/doctor/schedules"
            element={
              <Protected>
                <DoctorSchedules />
              </Protected>
            }
          />
          <Route
            path="/applyfordoctor"
            element={
              <Protected>
                <ApplyDoctor />
              </Protected>
            }
          />
          <Route
            path="/profile"
            element={
              <Protected>
                <Profile />
              </Protected>
            }
          />
          <Route
            path="/ChangePassword"
            element={
              <Protected>
                <Change />
              </Protected>
            }
          />
          <Route
            path="/dashboard/home"
            element={
              <Admin>
                <Dashboard type ={"home"} />
              </Admin>
            }
          />
          <Route
            path="/dashboard/users"
            element={
              <Admin>
                <Dashboard type={"users"} />
              </Admin>
            }
          />
          <Route
            path="/dashboard/clinicRequests"
            element={
              <Admin>
                <Dashboard type={"doctors"} />
              </Admin>
            }
          />
          <Route
            path="/dashboard/getAllRequestsApply"
            element={
              <Admin>
                <Dashboard type={"GetAllRequestsApply"} />
              </Admin>
            }
          />
          <Route
            path="/dashboard/getAllQueries"
            element={
              <Admin>
                <Dashboard type={"GetAllQueries"} />
              </Admin>
            }
          />
          <Route
            path="/dashboard/appointments"
            element={
              <Admin>
                <Dashboard type={"appointments"} />
              </Admin>
            }
          />
          <Route
            path="/dashboard/applications"
            element={
              <Admin>
                <Dashboard type={"applications"} />
              </Admin>
            }
          />
          <Route
            path="/dashboard/aprofile"
            element={
              <Admin>
                <Dashboard type={"aprofile"} />
              </Admin>
            }
          />
          <Route
            path="/image-models"
            element={
              <Protected>
                <ImageModels />
              </Protected>
            }
          />
          <Route path="*" element={<Error />} />
        </Routes>
      </Suspense>
      <ChatBot />
    </Router>
  );
}

export default App;
