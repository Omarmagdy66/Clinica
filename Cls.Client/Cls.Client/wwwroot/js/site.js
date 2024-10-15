function searchDoctor() {
    // Simulate a search result and navigate to the doctor details page
    const doctorName = document.getElementById('doctor-name').value;
    const specialty = document.getElementById('specialty').value;
    const city = document.getElementById('city').value;

    if (doctorName || (specialty && city)) {
        window.location.href = ;
    } else {
        alert("Please enter search criteria");
    }
}