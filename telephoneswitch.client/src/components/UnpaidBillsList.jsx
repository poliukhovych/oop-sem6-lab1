import { ListGroup, Badge } from "react-bootstrap";

function UnpaidBillsList({ unpaidBills }) {
  return (
    <div>
      <h3 className="mb-3">💳 Unpaid Bills</h3>
      <ListGroup>
        {unpaidBills.map((bill) => (
          <ListGroup.Item key={bill.id} className="d-flex justify-content-between align-items-center">
            <span>Bill ID: {bill.id} - Amount: {bill.amount} hrn</span>
            <Badge bg="warning" text="dark">
              Not Paid
            </Badge>
          </ListGroup.Item>
        ))}
      </ListGroup>
    </div>
  );
}

export default UnpaidBillsList;
