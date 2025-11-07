CREATE TABLE userrak (
    id SERIAL PRIMARY KEY,
    izena VARCHAR(100) NOT NULL,
    pasahitza VARCHAR(100) NOT NULL,
    admin BOOLEAN NOT NULL
);

INSERT INTO userrak (izena, pasahitza, admin)
VALUES ('Erabiltzaile', '1234', FALSE),
       ('Admin', 'admin1234', TRUE);