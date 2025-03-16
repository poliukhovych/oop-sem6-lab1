import { useAuth0 } from "@auth0/auth0-react";
import { Button } from "react-bootstrap";

function AuthButton() {
  const { loginWithRedirect, logout, isAuthenticated } = useAuth0();

  return isAuthenticated ? (
    <Button variant="outline-light" size="lg" className="fw-bold px-4 py-2"
      onClick={() => logout({ returnTo: window.location.origin })}>
      Log out
    </Button>
  ) : (
    <Button variant="outline-warning" size="lg" className="fw-bold px-4 py-2"
      onClick={() => loginWithRedirect()}>
      Log in
    </Button>
  );
}

export default AuthButton;
