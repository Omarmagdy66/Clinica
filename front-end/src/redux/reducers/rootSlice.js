import { createSlice } from "@reduxjs/toolkit";
import jwtDecode from "jwt-decode";
import axios from "axios";

axios.defaults.baseURL = process.env.REACT_APP_SERVER_DOMAIN;

export const rootReducer = createSlice({
  name: "root",
  initialState: {
    loading: true,
    userInfo: localStorage.getItem('token') ? jwtDecode(localStorage.getItem('token')) : null,
    isDoctor: null, // Add isDoctor state
  },
  reducers: {
    setLoading: (state, action) => {
      state.loading = action.payload;
    },
    setUserInfo: (state, action) => {
      state.userInfo = action.payload;
    },
    setIsDoctor: (state, action) => { // Add setIsDoctor reducer
      state.isDoctor = action.payload;
    },
  },
});

export const { setLoading, setUserInfo, setIsDoctor } = rootReducer.actions; 
export default rootReducer.reducer;