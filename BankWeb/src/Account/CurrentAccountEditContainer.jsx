import CurrentAccountEdit from "./CurrentAccountEdit";
import { useParams, useNavigate } from "react-router-dom";
import { store } from "../store";
import Spinner from "../Other/Spinner";
import Message from "../Other/Message";
import { useSelector } from "react-redux";
import {
  selectSaving,
  selectMessage,
  createNewCurrentAccount,
  dismissMessage,
} from "./accountSlice";

function CurrentAccountEditContainer() {
  const { id, userName } = useParams();
  const navigate = useNavigate();

  const saving = useSelector(selectSaving);
  const message = useSelector(selectMessage);

  const handleCreateCurrentAccount = (request) => {
    store.dispatch(createNewCurrentAccount(request)).then((result) => {
      if (result.type == "account/createNewCurrentAccount/fulfilled")
        navigate(`/users/${id}`);
    });
  };
  const handleDismissMessage = () => {
    store.dispatch(dismissMessage());
  };

  return (
    <table>
      <tbody>
        <tr>
          <td>
            {saving && <Spinner />}
            {message && (
              <Message message={message} onDismiss={handleDismissMessage} />
            )}
          </td>
        </tr>
        <tr>
          <td>
            <CurrentAccountEdit
              userID={id}
              userName={userName}
              onSubmit={handleCreateCurrentAccount}
            ></CurrentAccountEdit>
          </td>
        </tr>
      </tbody>
    </table>
  );
}

export default CurrentAccountEditContainer;
