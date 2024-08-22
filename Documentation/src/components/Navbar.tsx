import searchIcon from "../assets/search-icon.svg";
import "bootstrap/dist/css/bootstrap.css";
import "bootstrap/dist/js/bootstrap.js";

function Navbar() {
  return (
    <nav className="navbar bg-body-tertiary gen-border-bottom">
      <div className="container">
        <div>
          <button
            type="button"
            data-bs-toggle="collapse"
            data-bs-target="#category-page"
            className="navbar-toggler md m-1"
          >
            <span className="navbar-toggler-icon"></span>
          </button>

          <a className="navbar-brand p-3 fs-3 fw-medium m-1">Abilities</a>
        </div>
        <div>
          <form className="d-flex m-1" role="search">
            <input
              className="form-control me-2"
              type="search"
              placeholder="Search"
              aria-label="Search"
            />
            <button
              type="button"
              className="btn btn-outline-success d-flex align-items-center"
            >
              <img
                src={searchIcon}
                alt="Search"
                style={{ width: "24px", height: "24px" }}
              />
            </button>
          </form>
        </div>
      </div>
    </nav>
  );
}

export default Navbar;
