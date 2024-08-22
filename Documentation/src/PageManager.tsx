import WhatIsThis from "./pages/WhatIsThis";
import ExampleProject from "./pages/ExampleProject";
import Stats from "./pages/Stats";

class PageManager {
  static WhatIsThis = () => WhatIsThis();
  static ExampleProject = () => ExampleProject();
  static Stats = () => Stats();
}

export default PageManager;
