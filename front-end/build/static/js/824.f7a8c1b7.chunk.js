"use strict";(self.webpackChunkfront_end=self.webpackChunkfront_end||[]).push([[824],{6161:(e,t,n)=>{n.r(t),n.d(t,{default:()=>h});var r=n(5043),s=n(8929),a=(n(4748),n(579));const i=()=>{const[e,t]=(0,r.useState)({name:"",email:"",message:""}),[n,i]=(0,r.useState)(""),[o,l]=(0,r.useState)(!1),u=n=>{const{name:r,value:s}=n.target;t({...e,[r]:s})};return(0,a.jsx)("section",{className:"register-section flex-center",id:"contact",children:(0,a.jsxs)("div",{className:"contact-container flex-center contact",children:[(0,a.jsx)("h2",{className:"form-heading",children:"Contact Us"}),(0,a.jsxs)("form",{className:"register-form",onSubmit:async n=>{n.preventDefault(),l(!0),i("");try{const n={name:e.name,email:e.email,queryText:e.message};200===(await s.A.post("http://clinica.runasp.net/api/Query/Create",n,{headers:{Authorization:`Bearer ${localStorage.getItem("token")}`,"Content-Type":"application/json"}})).status&&(i("Your message has been sent successfully!"),t({name:"",email:"",message:""}))}catch(r){i("An error occurred. Please try again."),console.error(r)}finally{l(!1)}},children:[(0,a.jsx)("input",{type:"text",name:"name",className:"form-input",placeholder:"Enter your name",value:e.name,onChange:u,required:!0}),(0,a.jsx)("input",{type:"email",name:"email",className:"form-input",placeholder:"Enter your email",value:e.email,onChange:u,required:!0}),(0,a.jsx)("textarea",{name:"message",className:"form-input",placeholder:"Enter your message",value:e.message,onChange:u,rows:"8",cols:"12",required:!0}),(0,a.jsx)("button",{type:"submit",className:"btn form-btn",disabled:o,children:o?"Sending...":"Send"})]}),n&&(0,a.jsx)("p",{className:"response-message",children:n})]})})},o=n.p+"static/media/aboutimg.af2db4b9f307d04f8745.jpg",l=()=>(0,a.jsx)(a.Fragment,{children:(0,a.jsxs)("section",{className:"container",children:[(0,a.jsx)("h2",{className:"page-heading about-heading",children:"About Us"}),(0,a.jsxs)("div",{className:"about",children:[(0,a.jsx)("div",{className:"hero-img",children:(0,a.jsx)("img",{src:o,alt:"hero"})}),(0,a.jsx)("div",{className:"hero-content",children:(0,a.jsx)("p",{children:"Lorem, ipsum dolor sit amet consectetur adipisicing elit. Quibusdam tenetur doloremque molestias repellat minus asperiores in aperiam dolor, quaerat praesentium. Lorem ipsum dolor sit amet consectetur adipisicing elit. Voluptatibus, repudiandae! Lorem ipsum dolor sit amet consectetur adipisicing elit. Provident quibusdam doloremque ex? Officia atque ab dolore? Tempore totam non ea!"})})]})]})});var u=n(5560);const c=n.p+"static/media/heroimg.8bbd2437f7c9d842026c.jpg",p=()=>(0,a.jsxs)("section",{className:"hero",children:[(0,a.jsxs)("div",{className:"hero-content",children:[(0,a.jsxs)("h1",{children:["Your Health, ",(0,a.jsx)("br",{}),"Our Responsibility"]}),(0,a.jsx)("p",{children:"Lorem, ipsum dolor sit amet consectetur adipisicing elit. Quibusdam tenetur doloremque molestias repellat minus asperiores in aperiam dolor, quaerat praesentium."})]}),(0,a.jsx)("div",{className:"hero-img",children:(0,a.jsx)("img",{src:c,alt:"hero"})})]});var d=n(7665),m=n(5751);const f=()=>(0,a.jsxs)("section",{className:"container circles",children:[(0,a.jsxs)("div",{className:"circle",children:[(0,a.jsx)(m.Ay,{start:0,end:1e3,delay:0,enableScrollSpy:!0,scrollSpyDelay:500,children:e=>{let{countUpRef:t}=e;return(0,a.jsxs)("div",{className:"counter",children:[(0,a.jsx)("span",{ref:t}),"+"]})}}),(0,a.jsxs)("span",{className:"circle-name",children:["Satisfied",(0,a.jsx)("br",{}),"Patients"]})]}),(0,a.jsxs)("div",{className:"circle",children:[(0,a.jsx)(m.Ay,{start:0,end:250,delay:0,enableScrollSpy:!0,scrollSpyDelay:500,children:e=>{let{countUpRef:t}=e;return(0,a.jsxs)("div",{className:"counter",children:[(0,a.jsx)("span",{ref:t}),"+"]})}}),(0,a.jsxs)("span",{className:"circle-name",children:["Verified",(0,a.jsx)("br",{}),"Doctors"]})]}),(0,a.jsxs)("div",{className:"circle",children:[(0,a.jsx)(m.Ay,{start:0,end:75,delay:0,enableScrollSpy:!0,scrollSpyDelay:500,children:e=>{let{countUpRef:t}=e;return(0,a.jsxs)("div",{className:"counter",children:[(0,a.jsx)("span",{ref:t}),"+"]})}}),(0,a.jsxs)("span",{className:"circle-name",children:["Specialist",(0,a.jsx)("br",{}),"Doctors"]})]})]}),h=()=>(0,a.jsxs)(a.Fragment,{children:[(0,a.jsx)(d.A,{}),(0,a.jsx)(p,{}),(0,a.jsx)(l,{}),(0,a.jsx)(f,{}),(0,a.jsx)(i,{}),(0,a.jsx)(u.A,{})]})},7904:(e,t,n)=>{n.r(t),n.d(t,{CountUp:()=>s});var r=function(){return r=Object.assign||function(e){for(var t,n=1,r=arguments.length;n<r;n++)for(var s in t=arguments[n])Object.prototype.hasOwnProperty.call(t,s)&&(e[s]=t[s]);return e},r.apply(this,arguments)},s=function(){function e(e,t,n){var s=this;this.endVal=t,this.options=n,this.version="2.8.0",this.defaults={startVal:0,decimalPlaces:0,duration:2,useEasing:!0,useGrouping:!0,useIndianSeparators:!1,smartEasingThreshold:999,smartEasingAmount:333,separator:",",decimal:".",prefix:"",suffix:"",enableScrollSpy:!1,scrollSpyDelay:200,scrollSpyOnce:!1},this.finalEndVal=null,this.useEasing=!0,this.countDown=!1,this.error="",this.startVal=0,this.paused=!0,this.once=!1,this.count=function(e){s.startTime||(s.startTime=e);var t=e-s.startTime;s.remaining=s.duration-t,s.useEasing?s.countDown?s.frameVal=s.startVal-s.easingFn(t,0,s.startVal-s.endVal,s.duration):s.frameVal=s.easingFn(t,s.startVal,s.endVal-s.startVal,s.duration):s.frameVal=s.startVal+(s.endVal-s.startVal)*(t/s.duration);var n=s.countDown?s.frameVal<s.endVal:s.frameVal>s.endVal;s.frameVal=n?s.endVal:s.frameVal,s.frameVal=Number(s.frameVal.toFixed(s.options.decimalPlaces)),s.printValue(s.frameVal),t<s.duration?s.rAF=requestAnimationFrame(s.count):null!==s.finalEndVal?s.update(s.finalEndVal):s.options.onCompleteCallback&&s.options.onCompleteCallback()},this.formatNumber=function(e){var t,n,r,a,i=e<0?"-":"";t=Math.abs(e).toFixed(s.options.decimalPlaces);var o=(t+="").split(".");if(n=o[0],r=o.length>1?s.options.decimal+o[1]:"",s.options.useGrouping){a="";for(var l=3,u=0,c=0,p=n.length;c<p;++c)s.options.useIndianSeparators&&4===c&&(l=2,u=1),0!==c&&u%l==0&&(a=s.options.separator+a),u++,a=n[p-c-1]+a;n=a}return s.options.numerals&&s.options.numerals.length&&(n=n.replace(/[0-9]/g,(function(e){return s.options.numerals[+e]})),r=r.replace(/[0-9]/g,(function(e){return s.options.numerals[+e]}))),i+s.options.prefix+n+r+s.options.suffix},this.easeOutExpo=function(e,t,n,r){return n*(1-Math.pow(2,-10*e/r))*1024/1023+t},this.options=r(r({},this.defaults),n),this.formattingFn=this.options.formattingFn?this.options.formattingFn:this.formatNumber,this.easingFn=this.options.easingFn?this.options.easingFn:this.easeOutExpo,this.startVal=this.validateValue(this.options.startVal),this.frameVal=this.startVal,this.endVal=this.validateValue(t),this.options.decimalPlaces=Math.max(this.options.decimalPlaces),this.resetDuration(),this.options.separator=String(this.options.separator),this.useEasing=this.options.useEasing,""===this.options.separator&&(this.options.useGrouping=!1),this.el="string"==typeof e?document.getElementById(e):e,this.el?this.printValue(this.startVal):this.error="[CountUp] target is null or undefined","undefined"!=typeof window&&this.options.enableScrollSpy&&(this.error?console.error(this.error,e):(window.onScrollFns=window.onScrollFns||[],window.onScrollFns.push((function(){return s.handleScroll(s)})),window.onscroll=function(){window.onScrollFns.forEach((function(e){return e()}))},this.handleScroll(this)))}return e.prototype.handleScroll=function(e){if(e&&window&&!e.once){var t=window.innerHeight+window.scrollY,n=e.el.getBoundingClientRect(),r=n.top+window.pageYOffset,s=n.top+n.height+window.pageYOffset;s<t&&s>window.scrollY&&e.paused?(e.paused=!1,setTimeout((function(){return e.start()}),e.options.scrollSpyDelay),e.options.scrollSpyOnce&&(e.once=!0)):(window.scrollY>s||r>t)&&!e.paused&&e.reset()}},e.prototype.determineDirectionAndSmartEasing=function(){var e=this.finalEndVal?this.finalEndVal:this.endVal;this.countDown=this.startVal>e;var t=e-this.startVal;if(Math.abs(t)>this.options.smartEasingThreshold&&this.options.useEasing){this.finalEndVal=e;var n=this.countDown?1:-1;this.endVal=e+n*this.options.smartEasingAmount,this.duration=this.duration/2}else this.endVal=e,this.finalEndVal=null;null!==this.finalEndVal?this.useEasing=!1:this.useEasing=this.options.useEasing},e.prototype.start=function(e){this.error||(this.options.onStartCallback&&this.options.onStartCallback(),e&&(this.options.onCompleteCallback=e),this.duration>0?(this.determineDirectionAndSmartEasing(),this.paused=!1,this.rAF=requestAnimationFrame(this.count)):this.printValue(this.endVal))},e.prototype.pauseResume=function(){this.paused?(this.startTime=null,this.duration=this.remaining,this.startVal=this.frameVal,this.determineDirectionAndSmartEasing(),this.rAF=requestAnimationFrame(this.count)):cancelAnimationFrame(this.rAF),this.paused=!this.paused},e.prototype.reset=function(){cancelAnimationFrame(this.rAF),this.paused=!0,this.resetDuration(),this.startVal=this.validateValue(this.options.startVal),this.frameVal=this.startVal,this.printValue(this.startVal)},e.prototype.update=function(e){cancelAnimationFrame(this.rAF),this.startTime=null,this.endVal=this.validateValue(e),this.endVal!==this.frameVal&&(this.startVal=this.frameVal,null==this.finalEndVal&&this.resetDuration(),this.finalEndVal=null,this.determineDirectionAndSmartEasing(),this.rAF=requestAnimationFrame(this.count))},e.prototype.printValue=function(e){var t;if(this.el){var n=this.formattingFn(e);(null===(t=this.options.plugin)||void 0===t?void 0:t.render)?this.options.plugin.render(this.el,n):"INPUT"===this.el.tagName?this.el.value=n:"text"===this.el.tagName||"tspan"===this.el.tagName?this.el.textContent=n:this.el.innerHTML=n}},e.prototype.ensureNumber=function(e){return"number"==typeof e&&!isNaN(e)},e.prototype.validateValue=function(e){var t=Number(e);return this.ensureNumber(t)?t:(this.error="[CountUp] invalid start or end value: ".concat(e),null)},e.prototype.resetDuration=function(){this.startTime=null,this.duration=1e3*Number(this.options.duration),this.remaining=this.duration},e}()},5751:(e,t,n)=>{var r=n(5043),s=n(7904);function a(e,t){var n=Object.keys(e);if(Object.getOwnPropertySymbols){var r=Object.getOwnPropertySymbols(e);t&&(r=r.filter((function(t){return Object.getOwnPropertyDescriptor(e,t).enumerable}))),n.push.apply(n,r)}return n}function i(e){for(var t=1;t<arguments.length;t++){var n=null!=arguments[t]?arguments[t]:{};t%2?a(Object(n),!0).forEach((function(t){o(e,t,n[t])})):Object.getOwnPropertyDescriptors?Object.defineProperties(e,Object.getOwnPropertyDescriptors(n)):a(Object(n)).forEach((function(t){Object.defineProperty(e,t,Object.getOwnPropertyDescriptor(n,t))}))}return e}function o(e,t,n){return(t=function(e){var t=function(e,t){if("object"!==typeof e||null===e)return e;var n=e[Symbol.toPrimitive];if(void 0!==n){var r=n.call(e,t||"default");if("object"!==typeof r)return r;throw new TypeError("@@toPrimitive must return a primitive value.")}return("string"===t?String:Number)(e)}(e,"string");return"symbol"===typeof t?t:String(t)}(t))in e?Object.defineProperty(e,t,{value:n,enumerable:!0,configurable:!0,writable:!0}):e[t]=n,e}function l(){return l=Object.assign?Object.assign.bind():function(e){for(var t=1;t<arguments.length;t++){var n=arguments[t];for(var r in n)Object.prototype.hasOwnProperty.call(n,r)&&(e[r]=n[r])}return e},l.apply(this,arguments)}function u(e,t){if(null==e)return{};var n,r,s=function(e,t){if(null==e)return{};var n,r,s={},a=Object.keys(e);for(r=0;r<a.length;r++)n=a[r],t.indexOf(n)>=0||(s[n]=e[n]);return s}(e,t);if(Object.getOwnPropertySymbols){var a=Object.getOwnPropertySymbols(e);for(r=0;r<a.length;r++)n=a[r],t.indexOf(n)>=0||Object.prototype.propertyIsEnumerable.call(e,n)&&(s[n]=e[n])}return s}var c="undefined"!==typeof window&&"undefined"!==typeof window.document&&"undefined"!==typeof window.document.createElement?r.useLayoutEffect:r.useEffect;function p(e){var t=r.useRef(e);return c((function(){t.current=e})),r.useCallback((function(){for(var e=arguments.length,n=new Array(e),r=0;r<e;r++)n[r]=arguments[r];return t.current.apply(void 0,n)}),[])}var d=["ref","startOnMount","enableReinitialize","delay","onEnd","onStart","onPauseResume","onReset","onUpdate"],m={decimal:".",delay:null,prefix:"",suffix:"",duration:2,start:0,startOnMount:!0,enableReinitialize:!0},f=function(e){var t=r.useMemo((function(){return i(i({},m),e)}),[e]),n=t.ref,a=t.startOnMount,o=t.enableReinitialize,l=t.delay,c=t.onEnd,f=t.onStart,h=t.onPauseResume,g=t.onReset,y=t.onUpdate,b=u(t,d),v=r.useRef(),x=r.useRef(),j=r.useRef(!1),V=p((function(){return function(e,t){var n=t.decimal,r=t.decimals,a=t.duration,i=t.easingFn,o=t.end,l=t.formattingFn,u=t.numerals,c=t.prefix,p=t.separator,d=t.start,m=t.suffix,f=t.useEasing,h=t.enableScrollSpy,g=t.scrollSpyDelay,y=t.scrollSpyOnce;return new s.CountUp(e,o,{startVal:d,duration:a,decimal:n,decimalPlaces:r,easingFn:i,formattingFn:l,numerals:u,separator:p,prefix:c,suffix:m,useEasing:f,useGrouping:!!p,enableScrollSpy:h,scrollSpyDelay:g,scrollSpyOnce:y})}("string"===typeof n?n:n.current,b)})),w=p((function(e){var t=v.current;if(t&&!e)return t;var n=V();return v.current=n,n})),S=p((function(){var e=function(){return w(!0).start((function(){null===c||void 0===c||c({pauseResume:E,reset:O,start:F,update:N})}))};l&&l>0?x.current=setTimeout(e,1e3*l):e(),null===f||void 0===f||f({pauseResume:E,reset:O,update:N})})),E=p((function(){w().pauseResume(),null===h||void 0===h||h({reset:O,start:F,update:N})})),O=p((function(){w().el&&(x.current&&clearTimeout(x.current),w().reset(),null===g||void 0===g||g({pauseResume:E,start:F,update:N}))})),N=p((function(e){w().update(e),null===y||void 0===y||y({pauseResume:E,reset:O,start:F})})),F=p((function(){O(),S()})),A=p((function(e){a&&(e&&O(),S())}));return r.useEffect((function(){j.current?o&&A(!0):(j.current=!0,A())}),[o,j,A,l,e.start,e.suffix,e.prefix,e.duration,e.separator,e.decimals,e.decimal,e.formattingFn]),r.useEffect((function(){return function(){O()}}),[O]),{start:F,pauseResume:E,reset:O,update:N,getCountUp:w}},h=["className","redraw","containerProps","children","style"];t.Ay=function(e){var t=e.className,n=e.redraw,s=e.containerProps,a=e.children,o=e.style,c=u(e,h),d=r.useRef(null),m=r.useRef(!1),g=f(i(i({},c),{},{ref:d,startOnMount:"function"!==typeof a||0===e.delay,enableReinitialize:!1})),y=g.start,b=g.reset,v=g.update,x=g.pauseResume,j=g.getCountUp,V=p((function(){y()})),w=p((function(t){e.preserveValue||b(),v(t)})),S=p((function(){"function"!==typeof e.children||d.current instanceof Element?j():console.error('Couldn\'t find attached element to hook the CountUp instance into! Try to attach "containerRef" from the render prop to a an Element, eg. <span ref={containerRef} />.')}));r.useEffect((function(){S()}),[S]),r.useEffect((function(){m.current&&w(e.end)}),[e.end,w]);var E=n&&e;return r.useEffect((function(){n&&m.current&&V()}),[V,n,E]),r.useEffect((function(){!n&&m.current&&V()}),[V,n,e.start,e.suffix,e.prefix,e.duration,e.separator,e.decimals,e.decimal,e.className,e.formattingFn]),r.useEffect((function(){m.current=!0}),[]),"function"===typeof a?a({countUpRef:d,start:y,reset:b,update:v,pauseResume:x,getCountUp:j}):r.createElement("span",l({className:t,ref:d,style:o},s),"undefined"!==typeof e.start?j().formattingFn(e.start):"")}},4748:()=>{}}]);
//# sourceMappingURL=824.f7a8c1b7.chunk.js.map