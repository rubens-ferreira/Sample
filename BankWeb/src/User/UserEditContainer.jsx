import { useEffect } from "react";
import { useSelector } from "react-redux";
import {
  loadUser,
  saveUser,
  dismissMessage,
  selectLoading,
  selectUser,
  selectMessage,
} from "./userSlice";
import { store } from "../store";
import Spinner from "../Other/Spinner";
import Message from "../Other/Message";
import UserEdit from "./UserEdit";
import { useParams, useNavigate } from "react-router-dom";

function UserEditContainer() {
  const { id } = useParams();
  const navigate = useNavigate();

  const loading = useSelector(selectLoading);
  const message = useSelector(selectMessage);
  const user = useSelector(selectUser);

  useEffect(() => {
    const userID = isNaN(Number(id)) ? 0 : Number(id);
    if (userID != 0) store.dispatch(loadUser(userID));
  }, [id]);

  const handleDismissMessage = () => {
    store.dispatch(dismissMessage());
  };

  const handleSaveUser = (user) => {
    store.dispatch(saveUser(user)).then((result) => {
      if (result.type == "user/saveUser/fulfilled") navigate("/users");
    });
  };

  const handleNewCurrentAccount = (userID) => {
    navigate(`/accounts/addcurrent/${userID}`);
  };

  const handleClose = () => {
    navigate("/users");
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
            <h3>Edit User</h3>
          </td>
        </tr>
        <tr>
          <td>
            {!loading && (
              <UserEdit
                user={user}
                onSubmit={handleSaveUser}
                onNewCurrentAccount={handleNewCurrentAccount}
                onClose={handleClose}
              ></UserEdit>
            )}
          </td>
        </tr>
      </tbody>
    </table>
  );
}

export default UserEditContainer;
