import { useState } from "react";

function CurrentAccountEdit({ userID, userName, onSubmit }) {
  const [newAccount, setNewAccount] = useState({
    userID: userID,
    initialCredit: 0,
  });

  const handleSubmit = (e) => {
    e.preventDefault();
    onSubmit(newAccount);
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setNewAccount((prevAccount) => ({ ...prevAccount, [name]: value }));
  };
  return (
    <form onSubmit={handleSubmit}>
      <table>
        <tbody>
          <tr>
            <td> New Current Account for {userName}</td>
          </tr>
          <tr>
            <td>Initial Credit</td>
          </tr>
          <tr>
            <td>
              <input
                type="text"
                name="initialCredit"
                value={newAccount.initialCredit}
                onChange={handleChange}
              />
            </td>
          </tr>
          <tr>
            <td>
              <button type="submit">Save</button>
            </td>
          </tr>
        </tbody>
      </table>
    </form>
  );
}

export default CurrentAccountEdit;
