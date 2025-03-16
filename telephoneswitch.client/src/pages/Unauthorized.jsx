import { Container, Card, Button } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import { FaBan } from "react-icons/fa";

const Unauthorized = () => {
  const navigate = useNavigate();

  return (
    <div className="bg-light min-vh-100 d-flex align-items-center">
      <Container className="text-center">
        <Card className="shadow-lg p-5 mx-auto" style={{ maxWidth: "400px" }}>
          <Card.Body>
            <FaBan size={50} className="text-danger mb-3" />
            <Card.Title className="fw-bold display-6">Access Denied</Card.Title>
            <Card.Text className="text-muted">
              You don’t have permission to access this page.
            </Card.Text>
            <Button variant="primary" onClick={() => navigate("/login")}>
              Go to Login
            </Button>
          </Card.Body>
        </Card>
      </Container>
    </div>
  );
};

export default Unauthorized;
