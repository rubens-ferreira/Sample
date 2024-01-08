import "./Message.css";
const Message = ({ message, onDismiss }) => {
  return (
    <div className="message-container">
      <div className="message-strip">
        <p className="message-text">{message}</p>
        <button className="dismiss-button" onClick={onDismiss}>
          Dismiss
        </button>
      </div>
    </div>
  );
};

export default Message;
