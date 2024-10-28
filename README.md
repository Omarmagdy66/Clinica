Here's a documentation outline for your Clinica project:

---

# Clinica Documentation

Clinica is a comprehensive doctor appointment booking application that allows users to find and book doctors based on specialty, location, and availability. The project includes both a React frontend and an ASP.NET Core Web API backend, with modern styling and a user-friendly interface.

## Table of Contents

1. [Overview](#overview)
2. [Features](#features)
3. [Technologies Used](#technologies-used)
4. [Setup and Installation](#setup-and-installation)
5. [API Endpoints](#api-endpoints)
6. [Environment Variables](#environment-variables)
7. [Frontend Details](#frontend-details)
8. [Backend Details](#backend-details)
9. [Deployment](#deployment)

---

### Overview

Clinica is designed to streamline the doctor appointment booking process. Users can search for doctors by specialty, city, and area, view doctor profiles, and book appointments by selecting a clinic, date, and time slot. 

### Features

- **Doctor Search**: Users can search for doctors based on specialty, city, and area.
- **Profile Viewing**: Detailed profiles for each doctor, displaying credentials, reviews, and clinics.
- **Booking System**: Select clinic, date, and schedule an appointment with available doctors.
- **Appointment Confirmation**: Email and SMS confirmations are sent to validate the booking.

### Technologies Used

- **Frontend**: React, Formik, Styled Components, Vercel for deployment
- **Backend**: ASP.NET Core Web API, Entity Framework Core
- **Database**: SQL Server
- **Environment Management**: .env files

### Setup and Installation

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/Omarmagdy66/Clinica.git
   cd Clinica
   ```

2. **Frontend Setup**:
   ```bash
   cd client
   npm install
   npm start
   ```

3. **Backend Setup**:
   - Update the database connection string in `appsettings.json`.
   - Run migrations for database setup:
     ```bash
     dotnet ef database update
     ```
   - Start the backend server:
     ```bash
     dotnet run
     ```

### API Endpoints

- **/api/doctors**: Fetch doctors based on filters (specialization, country, city).
- **/api/appointments**: Book and confirm appointments.
- **/api/patients**: Edit patient information (requires patient ID from token).
  
### Environment Variables

- **REACT_FORMIK_SECRET**: (example: `TAR`) Environment variable used for client-side configuration in React.

### Frontend Details

The frontend is a React application deployed on Vercel, providing a responsive and attractive user interface. Key features include a booking page, search functionality, and detailed doctor profiles.

### Backend Details

The backend is built with ASP.NET Core Web API, utilizing the Repository pattern to handle data operations. Key services include:
- Doctor search filtering by specialization and location.
- Appointment scheduling and confirmation through email and SMS.

### Deployment

The project frontend is deployed on Vercel. For deployment of the backend, ensure your API base URL is correctly set in the frontend configuration.

--- 

This outline provides a comprehensive introduction to Clinica, covering setup, usage, and deployment steps.
