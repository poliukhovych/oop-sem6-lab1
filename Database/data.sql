INSERT INTO users (name, role, phone_number, is_blocked) VALUES
('admin adminenko', 'admin', 00001, FALSE),
('Ivan Petrenko', 'subscriber', 12345, FALSE),
('Olga Sydorenko', 'subscriber', 77777, FALSE),
('Petro Ivanenko', 'subscriber', 99999, FALSE);

INSERT INTO services (name, price) VALUES
('Voice calls', 50.00),
('Internet 100 Mbit/s', 100.00),
('SMS package (100 SMS)', 30.00);

INSERT INTO subscriber_services (subscriber_id, service_id) VALUES
(2, 1),
(2, 2),
(3, 2),
(4, 1),
(4, 3);

INSERT INTO bills (subscriber_id, amount, is_paid) VALUES
(2, 120.00, FALSE),
(3, 100.00, FALSE),
(4, 80.00, TRUE);
