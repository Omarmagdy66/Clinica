"use strict";(self.webpackChunkfront_end=self.webpackChunkfront_end||[]).push([[442],{2442:(s,e,r)=>{r.r(e),r.d(e,{default:()=>f});var a=r(5043),o=(r(2847),r(5560)),n=r(7665),t=r(8929),c=r(3768),d=r(8810),p=r(9456),w=r(8271),l=r(9358),u=r(7070),i=r(579);t.A.defaults.baseURL="http://clinica.runasp.net/api/";const f=function(){const{userId:s}=(0,u.A)(localStorage.getItem("token")),e=(0,p.wA)(),{loading:r}=(0,p.d4)((s=>s.root)),[f,m]=(0,a.useState)(""),[h,g]=(0,a.useState)({password:"",newpassword:"",confnewpassword:""});(0,a.useEffect)((()=>{(async()=>{try{e((0,d.r1)(!0));const r=await(0,l.A)(`/user/getuser/${s}`);g({...r,password:"",newpassword:null===r.newpassword?"":r.newpassword}),m(r.pic),e((0,d.r1)(!1))}catch(r){console.error("Error fetching user data:",r)}})()}),[e]);const y=s=>{const{name:e,value:r}=s.target;g({...h,[e]:r})};return(0,i.jsxs)(i.Fragment,{children:[(0,i.jsx)(n.A,{}),r?(0,i.jsx)(w.A,{}):(0,i.jsx)("section",{className:"register-section flex-center",children:(0,i.jsxs)("div",{className:"profile-container flex-center",children:[(0,i.jsx)("h2",{className:"form-heading",children:"Profile"}),(0,i.jsx)("img",{src:f,alt:"profile",className:"profile-pic"}),(0,i.jsxs)("form",{onSubmit:async e=>{e.preventDefault();const{password:r,newpassword:a,confnewpassword:o}=h;if(a!==o)return c.Ay.error("Passwords do not match");try{"Password changed successfully"===(await t.A.put("/user/changepassword",{userId:s,currentPassword:r,newPassword:a,confirmNewPassword:o},{headers:{Authorization:`Bearer ${localStorage.getItem("token")}`}})).data?(c.Ay.success("Password updated successfully"),g({...h,password:"",newpassword:"",confnewpassword:""})):c.Ay.error("Unable to update password")}catch(n){console.error("Error updating password:",n),n.response?c.Ay.error(n.response.data):c.Ay.error("Network error. Please try again.")}},className:"register-form",children:[(0,i.jsx)("div",{className:"form-same-row",children:(0,i.jsx)("input",{type:"password",name:"password",className:"form-input",placeholder:"Enter your current password",value:h.password,onChange:y})}),(0,i.jsxs)("div",{className:"form-same-row",children:[(0,i.jsx)("input",{type:"password",name:"newpassword",className:"form-input",placeholder:"Enter your new password",value:h.newpassword,onChange:y}),(0,i.jsx)("input",{type:"password",name:"confnewpassword",className:"form-input",placeholder:"Confirm your new password",value:h.confnewpassword,onChange:y})]}),(0,i.jsx)("button",{type:"submit",className:"btn form-btn",children:"Update"})]})]})}),(0,i.jsx)(o.A,{})]})}}}]);
//# sourceMappingURL=442.d4534f03.chunk.js.map