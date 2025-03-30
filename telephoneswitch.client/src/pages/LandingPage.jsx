import AuthButton from "../components/AuthButton";
import { Container, Row, Col } from "react-bootstrap";

function LandingPage() {
  return (
    <div className="bg-dark text-white min-vh-100 d-flex align-items-center">
      <Container fluid className="text-center">
        <Row className="justify-content-center">
          <Col md={8} lg={6} className="p-5 rounded shadow-lg bg-secondary bg-opacity-50">
            <h1 className="display-3 fw-bold mb-4">TelephoneSwitch</h1>
            <p className="lead mb-4">
              Your trusted digital telephone service. Connect, communicate, and stay in touch effortlessly.
            </p>
            <AuthButton />
          </Col>
        </Row>
      </Container>
    </div>
  );
}

export default LandingPage;
