import "./App.css";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import UserListContainer from "./User/UserListContainer";
import UserEditContainer from "./User/UserEditContainer";
import UserDetailsContainer from "./User/UserDetailsContainer";
import CurrentAccountEditContainer from "./Account/CurrentAccountEditContainer";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" exact element={<UserListContainer />} />
        <Route path="/users" exact element={<UserListContainer />} />
        <Route path="/users/:id" element={<UserEditContainer />} />
        <Route path="/users/details/:id" element={<UserDetailsContainer />} />
        <Route
          path="/accounts/addcurrent/:id"
          element={<CurrentAccountEditContainer />}
        />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
