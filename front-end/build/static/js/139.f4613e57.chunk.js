"use strict";(self.webpackChunkfront_end=self.webpackChunkfront_end||[]).push([[139],{5139:(e,t,n)=>{n.r(t),n.d(t,{default:()=>f});var s=n(5043),a=n(9456),r=n(6760),i=n(5560),o=n(7665),l=n(9358),c=n(8810),d=n(8271),h=n(3768),p=n(7070),m=n(8929),u=(n(8067),n(8152),n(6178)),j=n.n(u),x=n(579);const f=()=>{const[e,t]=(0,s.useState)([]),[n,u]=(0,s.useState)([]),[f,g]=(0,s.useState)(1),[A,N]=(0,s.useState)(""),b=(0,a.wA)(),{loading:S}=(0,a.d4)((e=>e.root)),{id:y}=(0,p.A)(localStorage.getItem("token")),D=async()=>{try{b((0,c.r1)(!0));const e=await(0,l.A)("/Appointment/GetAppointmentsForDoctor");console.log(e),t(e),u(e)}catch(e){console.error("Error fetching appointments:",e),h.oR.error("No Appointments Founded")}finally{b((0,c.r1)(!1))}};(0,s.useEffect)((()=>{D()}),[]);(0,s.useEffect)((()=>{(()=>{if(!A)return void u(e);const t=e.filter((e=>j()(e.appointmentDate).format("YYYY-MM-DD")===A));u(t)})()}),[e,A]);const v=Math.ceil(n.length/5),C=n.slice(5*(f-1),5*f);return(0,x.jsxs)(x.Fragment,{children:[(0,x.jsx)(o.A,{}),S?(0,x.jsx)(d.A,{}):(0,x.jsxs)("section",{className:"container notif-section",children:[(0,x.jsx)("h2",{className:"page-heading",children:"Your Appointments"}),(0,x.jsxs)("div",{className:"search-input",children:[(0,x.jsx)("label",{htmlFor:"searchDate",children:"Search by Appointment Date:"}),(0,x.jsx)("input",{type:"date",className:"form-input",id:"searchDate",value:A,onChange:e=>{N(e.target.value)}})]}),n.length>0?(0,x.jsxs)("div",{className:"appointments",children:[(0,x.jsxs)("table",{children:[(0,x.jsx)("thead",{children:(0,x.jsxs)("tr",{children:[(0,x.jsx)("th",{children:"S.No"}),(0,x.jsx)("th",{children:"P Name"}),(0,x.jsx)("th",{children:"P Email"}),(0,x.jsx)("th",{children:"P Mobile No."}),(0,x.jsx)("th",{children:"time Slot"}),(0,x.jsx)("th",{children:"Appointment Date"}),(0,x.jsx)("th",{children:"Status"}),(0,x.jsx)("th",{children:"Actions"})]})}),(0,x.jsx)("tbody",{children:C.map(((e,t)=>{return(0,x.jsxs)("tr",{children:[(0,x.jsx)("td",{children:5*(f-1)+t+1}),(0,x.jsx)("td",{children:`${e.patientNameIN}`}),(0,x.jsx)("td",{children:`${e.patientEmailIN}`}),(0,x.jsx)("td",{children:e.patientNumberIN}),(0,x.jsx)("td",{children:e.timeSlot}),(0,x.jsx)("td",{children:(n=e.appointmentDate,new Date(n).toLocaleDateString("en-US",{year:"numeric",month:"long",day:"numeric"}))}),(0,x.jsx)("td",{children:e.status}),(0,x.jsx)("td",{children:(0,x.jsx)("button",{className:"btn user-btn complete-btn",onClick:()=>(async e=>{if("Cancelld"===e.status&&h.oR.error("This appointment has been cancelled before!"),"Cancelld"!==e.status)try{await m.A.put(`Appointment/EditAppointmentStatus?id=${e.id}`,{headers:{Authorization:`Bearer ${localStorage.getItem("token")}`}}),h.oR.success("Appointment completed successfully."),D()}catch(t){console.error("Error completing appointment:",t),h.oR.error("Failed to complete appointment. Please try again.")}})(e),disabled:"Completed"===e.status,children:"Complete"})})]},e._id);var n}))})]}),(0,x.jsx)("div",{className:"pagination",children:(()=>{const e=[];for(let t=1;t<=v;t++)e.push((0,x.jsx)("button",{onClick:()=>{g(t)},children:t},t));return e})()})]}):(0,x.jsx)(r.A,{message:"No appointments found."})]}),(0,x.jsx)("div",{className:"",children:(0,x.jsx)(i.A,{})})]})}}}]);
//# sourceMappingURL=139.f4613e57.chunk.js.map