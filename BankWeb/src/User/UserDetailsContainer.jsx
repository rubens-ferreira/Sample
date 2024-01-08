import { useEffect } from "react";
import { useSelector } from "react-redux";
import {
  dismissMessage,
  selectLoading,
  selectUser,
  selectMessage,
  loadUserDetails,
} from "./userSlice";
import { store } from "../store";
import Spinner from "../Other/Spinner";
import Message from "../Other/Message";
import { useParams, useNavigate } from "react-router-dom";
import UserDetails from "./UserDetails";

function UserDetailsContainer() {
  const { id } = useParams();
  const navigate = useNavigate();

  const loading = useSelector(selectLoading);
  const message = useSelector(selectMessage);
  const user = useSelector(selectUser);

  useEffect(() => {
    const userID = isNaN(Number(id)) ? 0 : Number(id);
    if (userID != 0) store.dispatch(loadUserDetails(userID));
  }, [id]);

  const handleDismissMessage = () => {
    store.dispatch(dismissMessage());
  };
  const handleClose = () => {
    navigate(`/users`);
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
            {!loading && (
              <UserDetails user={user} onClose={handleClose}></UserDetails>
            )}
          </td>
        </tr>
      </tbody>
    </table>
  );
}

export default UserDetailsContainer;
