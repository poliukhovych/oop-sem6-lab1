import { useEffect, useState } from "react";
import { Container, Row, Col, Card, Button, ListGroup, Badge } from "react-bootstrap";
import { useAuth0 } from "@auth0/auth0-react";
import {
  getAvailableServices,
  getMyServices,
  addService,
  getMyBills,
  payBill,
} from "../api/subscriberApi";

function UserDashboard() {
  const [availableServices, setAvailableServices] = useState([]);
  const [myServices, setMyServices] = useState([]);
  const [bills, setBills] = useState([]);
  const { logout, getAccessTokenSilently } = useAuth0();

  useEffect(() => {
    const fetchData = async () => {
      try {
        const accessToken = await getAccessTokenSilently();
        setAvailableServices(await getAvailableServices(accessToken));
        setMyServices(await getMyServices(accessToken));
        setBills(await getMyBills(accessToken));
      } catch (error) {
        console.error("Error fetching data:", error);
      }
    };

    fetchData();
  }, [getAccessTokenSilently]);

  const handleAddService = async (serviceId) => {
    try {
      const accessToken = await getAccessTokenSilently();
      await addService(serviceId, accessToken);
      setMyServices(await getMyServices(accessToken));
      setBills(await getMyBills(accessToken));
    } catch (error) {
      console.error("Error when activating service:", error);
    }
  };

  const handlePayBill = async (billId) => {
    try {
      const accessToken = await getAccessTokenSilently();
      await payBill(billId, accessToken);
      setBills(await getMyBills(accessToken));
    } catch (error) {
      console.error("Error when paying bill:", error);
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
                  Personal Account
                </Card.Title>
                <Card.Text className="text-center text-muted">
                  Manage your subscriptions, pay bills, and stay connected.
                </Card.Text>

                <h3 className="mt-4">🛒 Available Services</h3>
                <ListGroup className="mb-4">
                  {availableServices.length > 0 ? (
                    availableServices.map((service) => (
                      <ListGroup.Item key={service.id} className="d-flex justify-content-between align-items-center">
                        <span>{service.name} - {service.price} hrn</span>
                        <Button variant="primary" onClick={() => handleAddService(service.id)}>
                          Activate
                        </Button>
                      </ListGroup.Item>
                    ))
                  ) : (
                    <ListGroup.Item>No available services</ListGroup.Item>
                  )}
                </ListGroup>

                <h3>📋 My Services</h3>
                <ListGroup className="mb-4">
                  {myServices.length > 0 ? (
                    myServices.map((service) => (
                      <ListGroup.Item key={service.id}>
                        {service.name} - {service.price} hrn
                      </ListGroup.Item>
                    ))
                  ) : (
                    <ListGroup.Item>No active services</ListGroup.Item>
                  )}
                </ListGroup>

                <h3>💳 My Bills</h3>
                <ListGroup>
                  {bills.length > 0 ? (
                    bills.map((bill) => (
                      <ListGroup.Item key={bill.id} className="d-flex justify-content-between align-items-center">
                        <span>{bill.amount} hrn</span>
                        {bill.isPaid ? (
                          <Badge bg="success">Paid</Badge>
                        ) : (
                          <Button variant="success" onClick={() => handlePayBill(bill.id)}>
                            Pay
                          </Button>
                        )}
                      </ListGroup.Item>
                    ))
                  ) : (
                    <ListGroup.Item>No bills</ListGroup.Item>
                  )}
                </ListGroup>

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

export default UserDashboard;
