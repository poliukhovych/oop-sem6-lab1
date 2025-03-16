import { useState } from "react";
import { Form, Button } from "react-bootstrap";

function AddUserForm({ onAddUser }) {
  const [name, setName] = useState("");
  const [phoneNumber, setPhoneNumber] = useState("");

  const handleSubmit = (e) => {
    e.preventDefault();
    if (!name || !phoneNumber) return;
    onAddUser(name, phoneNumber);
    setName("");
    setPhoneNumber("");
  };

  return (
    <Form onSubmit={handleSubmit} className="mb-4">
      <h3 className="mb-3">➕ Add Subscriber</h3>
      <Form.Group className="mb-2">
        <Form.Control
          type="text"
          placeholder="Name"
          value={name}
          onChange={(e) => setName(e.target.value)}
        />
      </Form.Group>
      <Form.Group className="mb-2">
        <Form.Control
          type="text"
          placeholder="Phone number"
          value={phoneNumber}
          onChange={(e) => setPhoneNumber(e.target.value)}
        />
      </Form.Group>
      <Button variant="primary" type="submit">
        Add
      </Button>
    </Form>
  );
}

export default AddUserForm;
