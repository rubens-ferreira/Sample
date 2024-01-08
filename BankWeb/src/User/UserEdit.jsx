import { useState } from "react";

function UserEdit({ user, onSubmit, onNewCurrentAccount, onClose }) {
  const [editedUser, setEditedUser] = useState({
    userID: user?.userID ?? 0,
    name: user?.name || "",
    currentAccountID: user?.currentAccountID,
  });

  const handleChange = (e) => {
    const { name, value } = e.target;
    setEditedUser((prevUser) => ({ ...prevUser, [name]: value }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    onSubmit(editedUser);
  };

  const handleNewCurrentAccount = () => {
    onNewCurrentAccount(editedUser.userID);
  };

  const handleClose = () => onClose();

  return (
    <form onSubmit={handleSubmit}>
      <table>
        <tbody>
          <tr>
            <td>Name:</td>
          </tr>
          <tr>
            <td>
              <input
                type="text"
                name="name"
                value={editedUser.name}
                onChange={handleChange}
              />
            </td>
          </tr>
          <tr>
            <td>
              <button type="submit">Save</button>
            </td>
          </tr>
          <tr>
            <td>
              Current Account:
              <b>{editedUser.currentAccountID ?? " no account assigned"}</b>
            </td>
          </tr>
          <tr>
            <td>
              <button
                onClick={handleNewCurrentAccount}
                disabled={editedUser.userID == 0}
              >
                Add Current Account
              </button>
              {"   "}
              <button onClick={handleClose}>Close</button>
            </td>
          </tr>
        </tbody>
      </table>
    </form>
  );
}

export default UserEdit;
