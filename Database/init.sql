CREATE TABLE users (
    id SERIAL PRIMARY KEY,
    auth0_id VARCHAR(255) UNIQUE NOT NULL,
    role VARCHAR(50) NOT NULL CHECK (role IN ('admin', 'subscriber')),
    name TEXT NOT NULL,
    phone_number TEXT UNIQUE NOT NULL,
    is_blocked BOOLEAN DEFAULT FALSE
);

CREATE TABLE services (
    id SERIAL PRIMARY KEY,
    name TEXT NOT NULL,
    price DECIMAL(10, 2) NOT NULL
);

CREATE TABLE subscriber_services (
    subscriber_id INT NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    service_id INT NOT NULL REFERENCES services(id) ON DELETE CASCADE,
    PRIMARY KEY (subscriber_id, service_id)
);

CREATE TABLE bills (
    id SERIAL PRIMARY KEY,
    subscriber_id INT NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    amount DECIMAL(10, 2) NOT NULL,
    is_paid BOOLEAN DEFAULT FALSE,
    created_at TIMESTAMP DEFAULT NOW()
);