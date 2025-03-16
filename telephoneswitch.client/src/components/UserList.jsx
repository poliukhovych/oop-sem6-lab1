import { ListGroup, Button, Badge } from "react-bootstrap";

function UserList({ users, onBlockUser, onUnblockUser }) {
  return (
    <div className="mb-4">
      <h3 className="mb-3">📋 Subscribers List</h3>
      <ListGroup>
        {users.map((user) => (
          <ListGroup.Item key={user.id} className="d-flex justify-content-between align-items-center">
            <span>
              {user.name} ({user.phoneNumber}){" "}
              {user.isBlocked ? <Badge bg="danger">Banned</Badge> : <Badge bg="success">Active</Badge>}
            </span>
            {user.isBlocked ? (
              <Button variant="success" size="sm" onClick={() => onUnblockUser(user.id)}>
                Unban
              </Button>
            ) : (
              <Button variant="danger" size="sm" onClick={() => onBlockUser(user.id)}>
                Ban
              </Button>
            )}
          </ListGroup.Item>
        ))}
      </ListGroup>
    </div>
  );
}

export default UserList;
