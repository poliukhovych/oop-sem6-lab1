import { useEffect, useState } from "react";
import { Container, Row, Col, Card, Button, Spinner } from "react-bootstrap";
import { useAuth0 } from "@auth0/auth0-react";
import { getUsers, addUser, getUnpaidBills, blockUser, unblockUser } from "../api/adminApi";
import AddUserForm from "../components/AddUserForm";
import UserList from "../components/UserList";
import UnpaidBillsList from "../components/UnpaidBillsList";

function AdminDashboard() {
  const [users, setUsers] = useState([]);
  const [unpaidBills, setUnpaidBills] = useState([]);
  const [loading, setLoading] = useState(false);
  const { logout, getAccessTokenSilently } = useAuth0();

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    setLoading(true);
    try {
      const accessToken = await getAccessTokenSilently();
      const usersData = await getUsers(accessToken);
      const unpaidBillsData = await getUnpaidBills(accessToken);
      setUsers(usersData);
      setUnpaidBills(unpaidBillsData);
    } catch (error) {
      console.error("Error occurred when getting data:", error);
    }
    setLoading(false);
  };

  const handleAddUser = async (name, phoneNumber) => {
    try {
      const accessToken = await getAccessTokenSilently();
      await addUser(name, phoneNumber, accessToken);
      fetchData();
    } catch (error) {
      console.error("Error adding subscriber:", error);
    }
  };

  const handleBlockUser = async (userId) => {
    try {
      const accessToken = await getAccessTokenSilently();
      await blockUser(userId, accessToken);
      fetchData();
    } catch (error) {
      console.error("Error banning:", error);
    }
  };

  const handleUnblockUser = async (userId) => {
    try {
      const accessToken = await getAccessTokenSilently();
      await unblockUser(userId, accessToken);
      fetchData();
    } catch (error) {
      console.error("Error unbanning:", error);
    }
  };

  return (
    <div className="bg-light min-vh-100 d-flex align-items-center">
      <Container className="py-5">
        <Row className="justify-content-center">
          <Col md={8}>
            <Card className="shadow-lg">
              <Card.Body>
                <Card.Title className="text-center display-5 fw-bold mb-4">
                  Admin Panel
                </Card.Title>
                <Card.Text className="text-center text-muted">
                  Manage subscribers, block users, and check unpaid bills.
                </Card.Text>

                {loading && <Spinner animation="border" className="d-block mx-auto my-3" />}

                <AddUserForm onAddUser={handleAddUser} />
                <UserList users={users} onBlockUser={handleBlockUser} onUnblockUser={handleUnblockUser} />
                <UnpaidBillsList unpaidBills={unpaidBills} />

                <div className="d-flex justify-content-center mt-4">
                  <Button variant="danger" onClick={() => logout({ returnTo: window.location.origin })}>
                    Log Out
                  </Button>
                </div>

              </Card.Body>
            </Card>
          </Col>
        </Row>
      </Container>
    </div>
  );
}

export default AdminDashboard;
