import { useEffect } from "react";
import { useSelector } from "react-redux";
import UserList from "./UserList";
import {
  loadUsers,
  dismissMessage,
  selectLoading,
  selectUsers,
  selectMessage,
} from "./userSlice";
import { store } from "../store";
import Spinner from "../Other/Spinner";
import Message from "../Other/Message";
import { useNavigate } from "react-router-dom";

function UserListContainer() {
  const users = useSelector(selectUsers);
  const loading = useSelector(selectLoading);
  const message = useSelector(selectMessage);
  const navigate = useNavigate();

  useEffect(() => {
    store.dispatch(loadUsers());
  }, []);

  const handleEdit = (userID) => {
    navigate(`/users/${userID}`);
  };
  const handleNew = () => {
    navigate(`/users/0`);
  };
  const handleViewDetails = (userID) => {
    navigate(`/users/details/${userID}`);
  };
  const handleDismissMessage = () => {
    store.dispatch(dismissMessage());
  };

  return (
    <table>
      <tbody>
        <tr>
          <td>
            {loading && <Spinner />}
            {message && (
              <Message message={message} onDismiss={handleDismissMessage} />
            )}
          </td>
        </tr>
        <tr>
          <td>
            <h3>Users</h3>
          </td>
        </tr>
        <tr>
          <td>
            <UserList
              users={users}
              onEdit={handleEdit}
              onViewDetails={handleViewDetails}
              disabled={loading}
            />
          </td>
        </tr>
        <tr>
          <td>
            <button onClick={handleNew}>New User</button>
          </td>
        </tr>
      </tbody>
    </table>
  );
}

export default UserListContainer;
