CREATE TABLE Students (
    StudentID INT IDENTITY(1,1) PRIMARY KEY,
    RegistrationNo VARCHAR(50) UNIQUE NOT NULL,
    FullName VARCHAR(100) NOT NULL,
    FatherName VARCHAR(100),
    ClassName VARCHAR(50), 
    ContactNo VARCHAR(15),
    RegistrationDate DATETIME DEFAULT GETDATE()
);