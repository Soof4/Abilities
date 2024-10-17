import "./App.css";
import "bootstrap/dist/css/bootstrap.css";
import "bootstrap/dist/js/bootstrap.js";
import "./pages/Stats.css";
import { HashRouter as Router, Route, Routes } from "react-router-dom";
import WhatIsThis from "./pages/WhatIsThis";
import Stats from "./pages/Stats";
import ExampleProject from "./pages/ExampleProject";

const App = () => {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<WhatIsThis />} />
        <Route path="/Stats" element={<Stats />} />
        <Route path="/ExampleProject" element={<ExampleProject />} />
      </Routes>
    </Router>
  );
};

export default App;
