CREATE TABLE userrak (
    id SERIAL PRIMARY KEY,
    izena VARCHAR(100) NOT NULL,
    pasahitza VARCHAR(100) NOT NULL,
    admin BOOLEAN NOT NULL
);
CREATE TABLE produktuak (
    id SERIAL PRIMARY KEY,
    izena VARCHAR(100) NOT NULL,
    prezioa DECIMAL(10, 2) NOT NULL,
    stock INT NOT NULL
);

INSERT INTO userrak (izena, pasahitza, admin)
VALUES ('Erabiltzaile', '1234', FALSE),
       ('Admin', 'admin1234', TRUE);

INSERT INTO produktuak (izena, prezioa, stock)
VALUES ('Coca-cola', 1.9, 100),
       ('Estrella Galicia', 2.5, 50),
       ('Kas Naranja', 2, 75);