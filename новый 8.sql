-- test database
-- dialect Postgres
CREATE TABLE Sellers (
    ID SERIAL PRIMARY KEY,
    Surname VARCHAR(50) NOT NULL,
    Name VARCHAR(50) NOT NULL
);

CREATE TABLE Products (
    ID SERIAL PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Price FLOAT NOT NULL
);

CREATE TABLE Sales (
    ID SERIAL PRIMARY KEY,
    IDSel INT REFERENCES Sellers(ID) ON DELETE CASCADE,
    IDProd INT REFERENCES Products(ID) ON DELETE CASCADE,
    Date TIMESTAMP NOT NULL,
    Quantity FLOAT NOT NULL
);

CREATE TABLE Arrivals (
    ID SERIAL PRIMARY KEY,
    IDProd INT REFERENCES Products(ID) ON DELETE CASCADE,
    Date TIMESTAMP NOT NULL,
    Quantity FLOAT NOT NULL
);

INSERT INTO Sellers (Surname, Name) VALUES
    ('Ivanov', 'Ivan'),
    ('Petrov', 'Petr'),
    ('Sidorov', 'Sidr'),
    ('Ivanov', 'Ivan');

INSERT INTO Products (Name, Price) VALUES
    ('Laptop', 1000.00),
    ('Smartphone', 500.00),
    ('Tablet', 300.00),
    ('Laptop', 1000.00);

INSERT INTO Sales (IDSel, IDProd, Date, Quantity) VALUES
    (1, 1, '2013-10-01 10:00:00', 2),
    (2, 2, '2013-10-02 11:00:00', 1),
    (1, 3, '2013-10-03 15:30:00', 3),
    (3, 1, '2013-10-05 09:00:00', 1),
    (2, 3, '2013-10-06 14:00:00', 2),
    (4, 1, '2013-10-01 10:00:00', 2),
    (4, 1, '2013-10-01 10:00:00', 4),
    (4, 4, '2013-10-01 10:00:00', 4);


INSERT INTO Arrivals (IDProd, Date, Quantity) VALUES
    (1, '2013-09-28 08:00:00', 5),
    (2, '2013-09-29 12:00:00', 3),
    (3, '2013-09-30 17:30:00', 10),
    (1, '2013-10-01 08:00:00', 2),
    (2, '2013-10-02 10:00:00', 4);

--task 1
select Sellers.Surname, Sellers.Name, sum(Sales.Quantity) as total_sales
from Sales
         join Sellers on Sales.IDSel = Sellers.ID
where Sales.Date >= '2013-10-01' and Sales.Date <= '2013-10-07'
group by Sellers.ID, Sellers.Surname, Sellers.Name
order by Sellers.Surname, Sellers.Name;

--task 2
with arrived_from_to as (
    select Arrivals.IDProd from Arrivals
    where Arrivals.Date >= '2013-09-07' and Arrivals.Date <= '2013-10-07'
    group by Arrivals.IDProd
),
arrived_total_sales as (
select Sales.IDProd, sum(Sales.Quantity) as sQ
from Sales join arrived_from_to aft on Sales.IDProd = aft.IDProd
group by Sales.IDProd
)
select Products.Name, Sellers.Surname, Sellers.Name, round(cast(sum(Sales.Quantity) / ats.sQ * 100 as numeric), 2) as percent
    from Sales
        join Sellers on Sales.IDSel = Sellers.ID
        join Products on Sales.IDProd = Products.ID
        join arrived_total_sales ats on ats.IDProd = Sales.IDProd
            where Sales.Date >= '2013-10-01' and Sales.Date <= '2013-10-07'
            group by Sellers.ID, Sales.IDProd, ats.sQ, Sellers.Surname, Sellers.Name, Products.Name
            order by Sellers.Surname, Sellers.Name, Products.Name;