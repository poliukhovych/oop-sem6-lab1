import { useAuth0 } from "@auth0/auth0-react";
import { Navigate, Outlet } from "react-router-dom";
import { useEffect, useState } from "react";
import { jwtDecode } from "jwt-decode";
import { Spinner } from "react-bootstrap";

const ProtectedRoute = ({ allowedRoles }) => {
  const { isAuthenticated, isLoading, getAccessTokenSilently } = useAuth0();
  const [userRoles, setUserRoles] = useState([]);
  const [isChecking, setIsChecking] = useState(true);

  useEffect(() => {
    const fetchRoles = async () => {
      if (!isAuthenticated) {
        setIsChecking(false);
        return;
      }

      try {
        const accessToken = await getAccessTokenSilently();
        const decodedToken = jwtDecode(accessToken);
        const roles = decodedToken?.["https://telephoneswitch777/api/roles"] || [];
        console.log("User roles:", roles);
        setUserRoles(roles);
      } catch (error) {
        console.error("Error fetching roles:", error);
      } finally {
        setIsChecking(false);
      }
    };

    fetchRoles();
  }, [isAuthenticated, getAccessTokenSilently]);

  if (isLoading || isChecking) {
    return (
      <div className="d-flex justify-content-center align-items-center vh-100">
        <Spinner animation="border" role="status">
          <span className="visually-hidden">Loading...</span>
        </Spinner>
      </div>
    );
  }

  if (!isAuthenticated || !userRoles.some(role => allowedRoles.includes(role))) {
    return <Navigate to="/unauthorized" />;
  }

  return <Outlet />;
};

export default ProtectedRoute;
