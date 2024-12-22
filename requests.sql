--Инициализация таблиц--

CREATE TABLE RitualUrn (
    Id SERIAL PRIMARY KEY,
    Name CHARACTER VARYING(20) NOT NULL UNIQUE,
    Price NUMERIC(8, 2) NOT NULL CHECK(Price > 0),
    Image TEXT NOT NULL
);

CREATE TABLE Corpose (
    Id SERIAL PRIMARY KEY,
    Name CHARACTER VARYING(20) NOT NULL,
    Surname CHARACTER VARYING(20) NOT NULL,
    NumPassport CHAR(14) NOT NULL UNIQUE
);

CREATE TABLE Hall (
    Id SERIAL PRIMARY KEY,
    Name CHARACTER VARYING(20) NOT NULL,
    Capacity INTEGER NOT NULL CHECK(Capacity > 0),
    Price NUMERIC(8, 2) NOT NULL CHECK(Price > 0)
);

CREATE TABLE Dates(
    HallId INTEGER NOT NULL,
    FOREIGN KEY (HallId)  REFERENCES Hall (Id) ON DELETE CASCADE,
    Date CHARACTER VARYING(12) NOT NULL,
    PRIMARY KEY (HallId, Date)
);

CREATE TABLE StateOrder(
    Code SERIAL PRIMARY KEY,
    Name CHARACTER VARYING(20) NOT NULL
);

CREATE TABLE Role(
    Code SERIAL PRIMARY KEY,
    Name CHARACTER VARYING(20) NOT NULL
);

CREATE TABLE Users(
    Id SERIAL PRIMARY KEY,
    Name CHARACTER VARYING(20) NOT NULL,
    Surname CHARACTER VARYING(20) NOT NULL,
    EmailAdress TEXT NOT NULL,
    RoleCode INTEGER,
    NumPassport CHAR(14) NOT NULL UNIQUE,
    FOREIGN KEY (RoleCode) REFERENCES Role(Code) ON DELETE SET NULL
);

CREATE INDEX IF NOT EXISTS NameNumPassport ON Users (Name, NumPassport)

CREATE TABLE Orders(
    Id SERIAL PRIMARY KEY,
    DateOfActual DATE NOT NULL,
    HallId INTEGER,
    CorposeId INTEGER,
    UserId INTEGER,
    UrnId INTEGER,
    StateCode INTEGER,
    FOREIGN KEY (HallId) REFERENCES Hall(Id) ON DELETE SET NULL,
    FOREIGN KEY (CorposeId) REFERENCES Corpose(Id) ON DELETE SET NULL,
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    FOREIGN KEY (UrnId) REFERENCES RitualUrn(Id) ON DELETE SET NULL,
    FOREIGN KEY (StateCode) REFERENCES StateOrder(Code) ON DELETE SET NULL
);

CREATE TABLE OrdersAudit (
    Operation CHAR(1) NOT NULL,
    Stamp TIMESTAMP NOT Null,
    OrderId INTEGER,
    ClientNumPassport CHAR(14) NOT NULL
);

--Заполнение начальными данными--

INSERT INTO Role (Name) VALUES
('Customer'),
('Admin'),
('Employee');

INSERT INTO Users (Name, Surname, EmailAdress, RoleCode, NumPassport) VALUES
('Dima', 'Kosach', 'kosach.dmitriy@mail.ru', 2, '1111111A111PB1'),
('Biba', 'Bibkin', 'biba.bibkin@mail.ru', 1, '1111111A111PB2'),
('Boba', 'Bobikov', 'boba_bobikov@mail.ru', 3, '1111111A111PB3');

INSERT INTO StateOrder (Name) VALUES
('Decorated'),
('Approved'),
('Closed'),
('Cancelled');

--Представления--

--Объединение всей информации об заказе + --
CREATE VIEW FullOrders AS
SELECT o.Id, o.DateOfActual, o.StateCode,
cor.Name AS CorposeName, cor.Surname AS CorposeSurname, cor.NumPassport AS CoproseNumPassport,
h.Id AS HallNumber, h.Name AS HallName, h.Capacity AS HallCapacity, h.Price AS HallPrice,
urn.Name AS UrnName, urn.Price AS UrnPrice, urn.Image AS urnImage,
u.Id AS UserId, u.Name, u.Surname, u.EmailAdress, u.NumPassport, u.RoleCode
FROM Orders AS o
LEFT JOIN Corpose AS cor ON cor.Id=o.CorposeId
LEFT JOIN Hall AS h ON h.Id=o.HallId
LEFT JOIN RitualUrn AS urn ON urn.Id=o.UrnId
LEFT JOIN Users AS u ON u.Id=o.UserId;


CREATE VIEW ShortOrders AS
SELECT o.Id, o.DateOfActual, o.StateCode,
cor.Name AS CorposeName, cor.Surname AS CorposeSurname,
h.Id AS HallNumber,
urn.Name AS UrnName,
u.UserId AS UserId, u.Name, u.Surname, u.NumPassport
FROM Orders AS o
LEFT JOIN Corpose AS cor ON cor.Id=o.CorposeId
LEFT JOIN Hall AS h ON h.Id=o.HallId
LEFT JOIN RitualUrn AS urn ON urn.Id=o.UrnId
LEFT JOIN UserWithRole AS u ON u.UserId=o.UserId;

--Получение пользователя с ролью текстом + --
CREATE VIEW UserWithRole AS
SELECT u.Id AS UserId, u.Name, u.Surname, u.EmailAdress, u.NumPassport,
    CASE
        WHEN u.RoleCode IS NOT NULL THEN
            (SELECT r.Name FROM Role AS r WHERE r.Code=u.RoleCode)
        ELSE 'Нет роли'
    END AS UserRole
FROM Users AS u;

--Триггеры и функции--

---Триггер проверяет, что заказ изменяют до того, как его одобрили---
CREATE OR REPLACE FUNCTION UpdateOrderCheckState()
RETURNS TRIGGER AS $$
DECLARE 
    maxStateCode INTEGER;
BEGIN
    SELECT INTO maxStateCode s.Code
    FROM StateOrder AS s
    WHERE s.Name = 'Approved';

    IF NEW.StateCode >= maxStateCode AND NEW.StateCode = OLD.StateCode THEN
        RAISE EXCEPTION 'Заказ уже нельзя изменить';
	END IF;

    NEW.DateOfActual := CURRENT_DATE;
	
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER UpdateOrderStateTrigger
BEFORE UPDATE ON Orders
FOR EACH ROW
EXECUTE FUNCTION UpdateOrderCheckState();

---триггер проверяет, дата для оформления зала больше ли сегодняшней---
CREATE OR REPLACE FUNCTION CheckDateGreaterThanToday()
RETURNS TRIGGER AS $$
BEGIN
    IF NEW.Date <= CURRENT_DATE THEN
        RAISE EXCEPTION 'Дата должна быть больше текущей';
    END IF;

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER InsertDateGreateThanTodayTrigger
BEFORE INSERT ON Dates
FOR EACH ROW EXECUTE FUNCTION CheckDateGreaterThanToday();

---Логи работы с заказами---
CREATE OR REPLACE FUNCTION ProcessOrderAudit()
RETURNS TRIGGER AS $$
DECLARE
    clientPassport CHAR(14);
BEGIN
    SELECT INTO clientPassport u.NumPassport
    FROM Users AS u WHERE u.Id = NEW.UserId;

    IF (TG_OP = 'DELETE') THEN
        INSERT INTO OrdersAudit SELECT 'D', now(), NEW.Id, clientPassport;
    ELSIF (TG_OP = 'UPDATE') THEN
        INSERT INTO OrdersAudit SELECT 'U', now(), NEW.Id, clientPassport;
    ELSIF (TG_OP = 'INSERT') THEN
        INSERT INTO OrdersAudit SELECT 'I', now(), NEW.Id, clientPassport;
    END IF;
    RETURN NULL;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER OrdersAuditTrigger
AFTER INSERT OR UPDATE OR DELETE ON Orders
FOR EACH ROW EXECUTE FUNCTION ProcessOrderAudit();

--Логирование авторизации/регистрации пользователей--
CREATE TABLE LoginLogs (
    Id SERIAL PRIMARY KEY,
    ClientNumPassport VARCHAR(50) NOT NULL,
    Action VARCHAR(50) NOT NULL,
    Timestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE OR REPLACE FUNCTION LogUserAction(passport_number VARCHAR, action_type VARCHAR) RETURNS VOID AS $$
BEGIN
    IF action_type NOT IN ('Login', 'Register') THEN
        RAISE EXCEPTION 'Invalid action type: %. Allowed types are Login or Register.', action_type;
    END IF;

    INSERT INTO LoginLogs (ClientNumPassport, Action)
    VALUES (passport_number, action_type);
END;
$$ LANGUAGE plpgsql;


---Проверяет наличие пользователя---
CREATE OR REPLACE FUNCTION IsExistUser(
    nameArg VARCHAR,
    numPassportArg CHAR
) RETURNS BOOLEAN AS $$
DECLARE isExists BOOLEAN;
BEGIN
    SELECT EXISTS(
        SELECT 1 FROM Users AS u 
        WHERE u.Name = nameArg AND u.numPassport = numPassportArg)
    INTO isExists;

    return isExists;
END;
$$ LANGUAGE plpgsql;

---CRUD для Hall(Name, Capacity, Price)---
CREATE OR REPLACE PROCEDURE CreateHall(name CHARACTER VARYING(20),
                                        capacity INTEGER,
                                        Price NUMERIC(8,2))
AS $$
BEGIN
    IF capacity <= 0 THEN
            RAISE EXCEPTION 'Вместительность должна быть больще 0';
    END IF;
    IF price <= 0 THEN
            RAISE EXCEPTION 'Стоимость должна быть больще 0';
    END IF;

    INSERT INTO Hall (Name, Capacity, Price) 
    VALUES (name, capacity, price);
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE PROCEDURE UpdateHall(hallId INTEGER,
                                        newName CHARACTER VARYING(20),
                                        newCapacity INTEGER,
                                        newPrice NUMERIC(8,2))
AS $$
BEGIN
    IF newCapacity <= 0 THEN
            RAISE EXCEPTION 'Вместительность должна быть больще 0';
    END IF;
    IF newPrice <= 0 THEN
            RAISE EXCEPTION 'Стоимость должна быть больще 0';
    END IF;

    UPDATE Hall
    SET Price = newPrice, Capacity = newCapacity, Name = newName
    WHERE Id = hallId;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE PROCEDURE DeleteHallById(hallId INTEGER)
AS $$
BEGIN
    DELETE FROM Hall 
    WHERE Id = hallId; 
END;
$$ LANGUAGE plpgsql;

--CRUD пользователей--

-- Создание пользователя
CREATE OR REPLACE PROCEDURE CreateUser(
    userName CHARACTER VARYING(20),
    userSurname CHARACTER VARYING(20),
    emailAddress TEXT,
    roleCode INTEGER,
    numPassport CHAR(14)
)
AS $$
BEGIN
    IF LENGTH(userName) = 0 THEN
        RAISE EXCEPTION 'Имя пользователя не может быть пустым';
    END IF;
    IF LENGTH(userSurname) = 0 THEN
        RAISE EXCEPTION 'Фамилия пользователя не может быть пустой';
    END IF;
    IF LENGTH(emailAddress) = 0 THEN
        RAISE EXCEPTION 'Email-адрес не может быть пустым';
    END IF;
    IF roleCode IS NOT NULL AND NOT EXISTS (SELECT 1 FROM Role WHERE Code = roleCode) THEN
        RAISE EXCEPTION 'Роль с указанным кодом не существует';
    END IF;
    IF numPassport !~ '^[0-9]{7}[A-Z][0-9]{3}(PB|BI|BA)[0-9]$' THEN
            RAISE EXCEPTION 'Номер паспорта не соответствует формату';
    END IF;

    INSERT INTO Users (Name, Surname, EmailAdress, RoleCode, NumPassport) 
    VALUES (userName, userSurname, emailAddress, roleCode, numPassport);
END;
$$ LANGUAGE plpgsql;

-- Обновление пользователя
CREATE OR REPLACE PROCEDURE UpdateUser(
    userId INTEGER,
    newUserName CHARACTER VARYING(20),
    newUserSurname CHARACTER VARYING(20),
    newEmailAddress TEXT,
    newRoleCode INTEGER
)
AS $$
BEGIN
    IF LENGTH(newUserName) = 0 THEN
        RAISE EXCEPTION 'Имя пользователя не может быть пустым';
    END IF;
    IF LENGTH(newUserSurname) = 0 THEN
        RAISE EXCEPTION 'Фамилия пользователя не может быть пустой';
    END IF;
    IF LENGTH(newEmailAddress) = 0 THEN
        RAISE EXCEPTION 'Email-адрес не может быть пустым';
    END IF;
    IF newRoleCode IS NOT NULL AND NOT EXISTS (SELECT 1 FROM Role WHERE Code = newRoleCode) THEN
        RAISE EXCEPTION 'Роль с указанным кодом не существует';
    END IF;

    UPDATE Users
    SET 
        Name = newUserName,
        Surname = newUserSurname,
        EmailAdress = newEmailAddress
    WHERE Id = userId;
END;
$$ LANGUAGE plpgsql;

-- Обновление пользователя без изменения роли
CREATE OR REPLACE PROCEDURE UpdateUserWithoutRole(
    userId INTEGER,
    newUserName CHARACTER VARYING(20),
    newUserSurname CHARACTER VARYING(20),
    newEmailAddress TEXT
)
AS $$
BEGIN
    IF LENGTH(newUserName) = 0 THEN
        RAISE EXCEPTION 'Имя пользователя не может быть пустым';
    END IF;
    IF LENGTH(newUserSurname) = 0 THEN
        RAISE EXCEPTION 'Фамилия пользователя не может быть пустой';
    END IF;
    IF LENGTH(newEmailAddress) = 0 THEN
        RAISE EXCEPTION 'Email-адрес не может быть пустым';
    END IF;

    UPDATE Users
    SET 
        Name = newUserName,
        Surname = newUserSurname,
        EmailAdress = newEmailAddress
    WHERE Id = userId;
END;
$$ LANGUAGE plpgsql;

-- Удаление пользователя по ID
CREATE OR REPLACE PROCEDURE DeleteUserById(userId INTEGER)
AS $$
BEGIN
    DELETE FROM Users 
    WHERE Id = userId; 
END;
$$ LANGUAGE plpgsql;


--CRUD трупа--

-- Создание трупа
CREATE OR REPLACE PROCEDURE CreateCorpse(
    corpseName CHARACTER VARYING(20),
    corpseSurname CHARACTER VARYING(20),
    numPassport CHAR(14)
)
AS $$
BEGIN
    IF LENGTH(corpseName) = 0 THEN
        RAISE EXCEPTION 'Имя трупа не может быть пустым';
    END IF;
    IF LENGTH(corpseSurname) = 0 THEN
        RAISE EXCEPTION 'Фамилия трупа не может быть пустой';
    END IF;
    IF numPassport !~ '^[0-9]{7}[A-Z][0-9]{3}(PB|BI|BA)[0-9]$' THEN
            RAISE EXCEPTION 'Номер паспорта не соответствует формату';
    END IF;

    INSERT INTO Corpose (Name, Surname, NumPassport) 
    VALUES (corpseName, corpseSurname, numPassport);
END;
$$ LANGUAGE plpgsql;

-- Обновление трупа
CREATE OR REPLACE PROCEDURE UpdateCorpse(
    corpseId INTEGER,
    newCorpseName CHARACTER VARYING(20),
    newCorpseSurname CHARACTER VARYING(20),
    newNumPassport CHAR(14)
)
AS $$
BEGIN
    IF LENGTH(newCorpseName) = 0 THEN
        RAISE EXCEPTION 'Имя трупа не может быть пустым';
    END IF;
    IF LENGTH(newCorpseSurname) = 0 THEN
        RAISE EXCEPTION 'Фамилия трупа не может быть пустой';
    END IF;
    IF numPassport !~ '^[0-9]{7}[A-Z][0-9]{3}(PB|BI|BA)[0-9]$' THEN
            RAISE EXCEPTION 'Номер паспорта не соответствует формату';
    END IF;

    UPDATE Corpose
    SET 
        Name = newCorpseName,
        Surname = newCorpseSurname,
        NumPassport = newNumPassport
    WHERE Id = corpseId;
END;
$$ LANGUAGE plpgsql;

-- Удаление трупа по ID
CREATE OR REPLACE PROCEDURE DeleteCorpseById(corpseId INTEGER)
AS $$
BEGIN
    DELETE FROM Corpose 
    WHERE Id = corpseId; 
END;
$$ LANGUAGE plpgsql;

--CRUD ритуальных урн--
-- Создание ритуальной урны
CREATE OR REPLACE PROCEDURE CreateRitualUrn(
    urnName CHARACTER VARYING(20),
    urnPrice NUMERIC(8, 2),
    image Text
)
AS $$
BEGIN
    IF LENGTH(urnName) = 0 THEN
        RAISE EXCEPTION 'Название ритуальной урны не может быть пустым';
    END IF;
    IF urnPrice <= 0 THEN
        RAISE EXCEPTION 'Стоимость ритуальной урны должна быть больше 0';
    END IF;

    INSERT INTO RitualUrn (Name, Price, Image) 
    VALUES (urnName, urnPrice, image);
END;
$$ LANGUAGE plpgsql;

-- Обновление ритуальной урны
CREATE OR REPLACE PROCEDURE UpdateRitualUrn(
    urnId INTEGER,
    newUrnName CHARACTER VARYING(20),
    newUrnPrice NUMERIC(8, 2),
    newImage Text
)
AS $$
BEGIN
    IF LENGTH(newUrnName) = 0 THEN
        RAISE EXCEPTION 'Название ритуальной урны не может быть пустым';
    END IF;
    IF newUrnPrice <= 0 THEN
        RAISE EXCEPTION 'Стоимость ритуальной урны должна быть больше 0';
    END IF;

    UPDATE RitualUrn
    SET 
        Name = newUrnName,
        Price = newUrnPrice,
        Image = newImage
    WHERE Id = urnId;
END;
$$ LANGUAGE plpgsql;

-- Удаление ритуальной урны по ID
CREATE OR REPLACE PROCEDURE DeleteRitualUrnById(urnId INTEGER)
AS $$
BEGIN
    DELETE FROM RitualUrn 
    WHERE Id = urnId; 
END;
$$ LANGUAGE plpgsql;


--CRUD заказов--

-- Создание заказа
CREATE OR REPLACE PROCEDURE CreateOrder(
    dateOfActual DATE,
    shallId INTEGER,
    corposeId INTEGER,
    userId INTEGER,
    urnId INTEGER,
    stateCode INTEGER
)
AS $$
BEGIN
    IF dateOfActual IS NULL THEN
        RAISE EXCEPTION 'Дата заказа не может быть пустой';
    END IF;

    DELETE FROM Dates
    WHERE HallId=shallId AND date(date)=dateOfActual;

    INSERT INTO Orders (DateOfActual, HallId, CorposeId, UserId, UrnId, StateCode) 
    VALUES (dateOfActual, shallId, corposeId, userId, urnId, stateCode);
END;
$$ LANGUAGE plpgsql;

--Изменение статуса заказа--

CREATE OR REPLACE PROCEDURE UpdateOrderStatus(orderId INTEGER, newStatusCode INTEGER)
AS $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Orders WHERE Id = orderId) THEN
        RAISE EXCEPTION 'Заказ с указанным Id не существует';
    END IF;

    IF NOT EXISTS (SELECT 1 FROM StateOrder WHERE Code = newStatusCode) THEN
        RAISE EXCEPTION 'Статус с указанным кодом не существует';
    END IF;

    UPDATE Orders
    SET StateCode = newStatusCode
    WHERE Id = orderId;
END;
$$ LANGUAGE plpgsql;

--Частичный поиск сущностей--

CREATE OR REPLACE FUNCTION PartialSearchRitualUrn(partialName VARCHAR)
RETURNS TABLE (Id INTEGER, Name VARCHAR(20), Price NUMERIC(8, 2), Image TEXT)
AS $$
BEGIN
    RETURN QUERY
    SELECT u.Id, u.Name, u.Price, u.Image
    FROM RitualUrn AS u
    WHERE u.Name ILIKE '%' || partialName || '%';
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION PartialSearchHall(partialName VARCHAR)
RETURNS TABLE (Id INTEGER, Name VARCHAR(20), Capacity INTEGER, Price NUMERIC(8, 2), Date CHARACTER VARYING(12))
AS $$
BEGIN
    RETURN QUERY
    SELECT h.Id, h.Name, h.Capacity, h.Price, d.date
    FROM Hall AS h
    LEFT OUTER JOIN Dates AS d ON h.Id=d.HallId
    WHERE h.Name ILIKE '%' || partialName || '%';
END;
$$ LANGUAGE plpgsql;

