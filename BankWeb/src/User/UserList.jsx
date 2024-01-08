function UserList({ users, onEdit, onViewDetails, disabled }) {
  return (
    <ul>
      {users &&
        users.map((user) => (
          <li key={user.userID}>
            <table>
              <tbody>
                <tr>
                  <td>{user.name}</td>
                  <td>
                    <button
                      disabled={disabled}
                      onClick={() => onEdit(user.userID)}
                    >
                      Edit
                    </button>
                  </td>
                  <td>
                    <button
                      disabled={disabled}
                      onClick={() => onViewDetails(user.userID)}
                    >
                      View Details
                    </button>
                  </td>
                </tr>
              </tbody>
            </table>
          </li>
        ))}
    </ul>
  );
}

export default UserList;
