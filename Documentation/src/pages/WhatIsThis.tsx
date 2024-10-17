import { ReactNode } from "react";
import Navbar from "../components/Navbar";
import "bootstrap/dist/css/bootstrap.css";
import "bootstrap/dist/js/bootstrap.js";
import CategoryPage from "../components/CategoryPage";

function WhatIsThis(): ReactNode {
  return (
    <>
      <Navbar />
      <div className="container min-vh-100">
        <div className="row">
          <CategoryPage />
          <div className="col m-3">
            <p className="fs-3">What is this?</p>
            <p>
              Abilities is a comprehensive library designed to offer a wide
              range of functionalities for TShock plugin developers. With over
              15 built-in abilities featuring regularly balanced statistics, the
              library also provides developers with the tools to modify existing
              stats and create entirely new abilities.
            </p>
          </div>
        </div>
      </div>
    </>
  );
}

export default WhatIsThis;
