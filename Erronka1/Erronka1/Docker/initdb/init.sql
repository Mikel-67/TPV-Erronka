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
CREATE TABLE mahaiak (
    id SERIAL PRIMARY KEY,
    zenbakia VARCHAR(10) NOT NULL,
    okupatuta BOOLEAN NOT NULL DEFAULT FALSE
);
CREATE TABLE erreserbak (
    id SERIAL PRIMARY KEY,
    mahaia_id INT NOT NULL REFERENCES mahaiak(id) ON DELETE CASCADE,
    data TIMESTAMP NOT NULL
);


INSERT INTO userrak (izena, pasahitza, admin)
VALUES ('Erabiltzaile', '1234', FALSE),
       ('Admin', 'admin1234', TRUE);

INSERT INTO produktuak (izena, prezioa, stock)
VALUES ('Coca-cola', 1.9, 100),
       ('Estrella Galicia', 2.5, 50),
       ('Kas Naranja', 2, 75);

INSERT INTO mahaiak (zenbakia, okupatuta)
VALUES
('1', FALSE),
('2', FALSE),
('3', FALSE),
('4', FALSE),
('5', FALSE);