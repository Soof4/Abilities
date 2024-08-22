import CategoryPage from "./components/CategoryPage";
import MainPage from "./components/MainPage";
import Navbar from "./components/Navbar";
import PageManager from "./PageManager";
import "./App.css";
import "bootstrap/dist/css/bootstrap.css";
import "bootstrap/dist/js/bootstrap.js";
import "./pages/Stats.css";

function App() {
  return (
    <>
      <Navbar />
      <div className="container min-vh-100">
        <div className="row">
          <CategoryPage />
          <MainPage>{PageManager.Stats()}</MainPage>
        </div>
      </div>
    </>
  );
}

export default App;
