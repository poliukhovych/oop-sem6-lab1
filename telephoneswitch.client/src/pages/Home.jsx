import { useAuth0 } from "@auth0/auth0-react";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Spinner } from "react-bootstrap";
import { jwtDecode } from "jwt-decode";

const Home = () => {
  const { isAuthenticated, isLoading, getAccessTokenSilently } = useAuth0();
  const navigate = useNavigate();
  const [isChecking, setIsChecking] = useState(true);

  useEffect(() => {
    if (isLoading) return;

    const checkRoles = async () => {
      if (!isAuthenticated) {
        navigate("/login");
        return;
      }

      try {
        const accessToken = await getAccessTokenSilently();
        const decodedToken = jwtDecode(accessToken);
        console.log("Decoded token:", decodedToken);

        const roles = decodedToken?.["https://telephoneswitch777/api/roles"] || [];
        console.log("User roles:", roles);

        if (roles.includes("admin")) {
          navigate("/admin");
        } else if (roles.includes("subscriber")) {
          navigate("/subscriber");
        } else {
          navigate("/unauthorized");
        }
      } catch (error) {
        console.error("Error fetching roles:", error);
        navigate("/unauthorized");
      } finally {
        setIsChecking(false);
      }
    };

    checkRoles();
  }, [isAuthenticated, isLoading, getAccessTokenSilently, navigate]);

  if (isChecking) {
    return (
      <div className="d-flex justify-content-center align-items-center vh-100">
        <Spinner animation="border" role="status">
          <span className="visually-hidden">Loading...</span>
        </Spinner>
      </div>
    );
  }

  return null;
};

export default Home;
