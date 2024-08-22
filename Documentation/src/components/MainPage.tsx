import { ReactNode } from "react";

interface Props {
  children: ReactNode;
}

function MainPage({ children }: Props) {
  return <div className="col m-3">{children}</div>;
}

export default MainPage;
