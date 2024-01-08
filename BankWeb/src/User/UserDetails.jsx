import "./UserDetails.css";

const UserDetails = ({ user, onClose }) => {
  if (!user) {
    return <div>Loading...</div>;
  }

  return (
    <table>
      <tbody>
        <tr>
          <td>
            <h1>User Details</h1>
          </td>
        </tr>
        <tr>
          <td>
            <table>
              <thead>
                <tr>
                  <th>User ID</th>
                  <th>Name</th>
                  <th>Surname</th>
                  <th>Current Account ID</th>
                </tr>
              </thead>
              <tbody>
                <tr>
                  <td>{user.userID}</td>
                  <td>{user.name}</td>
                  <td>{user.surName}</td>
                  <td>{user.currentAccountID}</td>
                </tr>
              </tbody>
            </table>
          </td>
        </tr>
        <tr>
          <td>
            <h2>Accounts</h2>
          </td>
        </tr>
        <tr>
          <td>
            {user.accounts &&
              user.accounts.map((account) => (
                <div key={account.accountID}>
                  <table>
                    <thead>
                      <tr>
                        <th>Account ID:</th>
                      </tr>
                      <tr>
                        <th>{account.accountID}</th>
                      </tr>
                      <tr>
                        <th>Transaction ID</th>
                        <th>Amount</th>
                      </tr>
                    </thead>
                    <tbody>
                      {account.transactions &&
                        account.transactions.map((transaction) => (
                          <tr key={transaction.transactionID}>
                            <td>{transaction.transactionID}</td>
                            <td>{transaction.amount}</td>
                          </tr>
                        ))}
                    </tbody>
                  </table>
                </div>
              ))}
          </td>
        </tr>
        <tr>
          <td>
            <button onClick={onClose}>Close View</button>
          </td>
        </tr>
      </tbody>
    </table>
  );
};

export default UserDetails;
