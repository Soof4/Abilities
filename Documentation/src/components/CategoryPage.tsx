import { Link } from "react-router-dom";

const CategoryPage = () => {
  return (
    <div
      className="p-3 collapse col-12 col-lg-3 p-3 d-s-block"
      id="category-page"
    >
      <p className="fs-5 fw-medium navbar-nav">Getting Started</p>
      <ul className="list-group list-group-flush navbar-nav">
        <Link to="/" className="list-group-item">
          What is this?
        </Link>
        <Link to="/ExampleProject" className="list-group-item">
          Example project
        </Link>
        <Link to="/Stats" className="list-group-item">
          Stats
        </Link>
      </ul>
    </div>
  );
};

export default CategoryPage;
