USE [EventManage]

CREATE TABLE "Role" (
  "RoleID" INT PRIMARY KEY IDENTITY(1,1),
  "RoleName" VARCHAR(255) NOT NULL
);

CREATE TABLE "User" (
  "UserID" UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
  "FirstName" VARCHAR(255),
  "LastName" VARCHAR(255),
  "Age" INT NOT NULL,
  "Gender" VARCHAR(50),
  "Email" VARCHAR(255),
  "Phone" VARCHAR(15),
  "Password" VARBINARY(MAX),
  "PasswordSalt" VARBINARY(MAX) NOT NULL,
  "UpdatedAt" DATE,
  "CreatedAt" DATE,
  "Status" VARCHAR(50) NOT NULL,
  "RoleID" INT NOT NULL,
  FOREIGN KEY ("RoleID") REFERENCES "Role" ("RoleID")
);

CREATE TABLE "Permission" (
  "PermissionID" INT PRIMARY KEY IDENTITY(1,1),
  "PermissionName" VARCHAR(255) NOT NULL,
  "Description" VARCHAR(255),
  "Status" VARCHAR(10)
);

CREATE TABLE "Event" (
  "EventID" UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
  "EventName" VARCHAR(255) NOT NULL,
  "Description" VARCHAR(MAX),
  "Status" VARCHAR(10),
  "StartDate" DATE,
  "EndDate" DATE,
  "CreatedBy" UNIQUEIDENTIFIER,
  "Image" VARCHAR(5000),
  "Location" VARCHAR(255),
  "CreatedAt" DATE,
  "UpdatedAt" DATE,
  "Capacity" INT,
  "Approval" BIT,
  "Ticket" FLOAT,
  FOREIGN KEY ("CreatedBy") REFERENCES "User" ("UserID")
);

CREATE TABLE "Tag" (
  "TagID" INT PRIMARY KEY IDENTITY(1,1),
  "TagName" VARCHAR(255)
);

CREATE TABLE "EventTag" (
  "TagID" INT,
  "EventID" UNIQUEIDENTIFIER,
  PRIMARY KEY ("TagID", "EventID"),
  FOREIGN KEY ("EventID") REFERENCES "Event" ("EventID"),
  FOREIGN KEY ("TagID") REFERENCES "Tag" ("TagID")
);

CREATE TABLE "RoleEvent" (
  "RoleEventID" INT PRIMARY KEY IDENTITY(1,1),
  "RoleEventName" VARCHAR(255)
);

CREATE TABLE "Participant" (
  "UserID" UNIQUEIDENTIFIER,
  "EventID" UNIQUEIDENTIFIER,
  "RoleEventID" INT,
  "CheckedIn" DATETIME,
  "IsCheckedMail" BIT,
  "CreatedAt" DATETIME,
  "Status" VARCHAR(50),
  PRIMARY KEY ("UserID", "EventID"),
  FOREIGN KEY ("RoleEventID") REFERENCES "RoleEvent" ("RoleEventID"),
  FOREIGN KEY ("EventID") REFERENCES "Event" ("EventID"),
  FOREIGN KEY ("UserID") REFERENCES "User" ("UserID")
);

CREATE TABLE "Feedback" (
  "UserID" UNIQUEIDENTIFIER,
  "EventID" UNIQUEIDENTIFIER,
  "Content" VARCHAR(5000),
  "Rating" INT,
  "CreatedAt" DATE,
  PRIMARY KEY ("UserID", "EventID"),
  FOREIGN KEY ("UserID") REFERENCES "User" ("UserID"),
  FOREIGN KEY ("EventID") REFERENCES "Event" ("EventID")
);

CREATE TABLE "PaymentMethod" (
  "PaymentMethodID" INT PRIMARY KEY IDENTITY(1,1),
  "PaymentMethodName" VARCHAR(255),
  "PaymentMethodStatus" BIT
);

CREATE TABLE "Payment" (
  "PaymentID" UNIQUEIDENTIFIER PRIMARY KEY,
  "PaymentMethodID" INT REFERENCES "PaymentMethod" ("PaymentMethodID"),
  "PaymentOwner" VARCHAR(50),
  "UserID" UNIQUEIDENTIFIER REFERENCES "User" ("UserID"),
  "SerialNumber" VARCHAR(20),
  "PaymentStatus" BIT,
  "CreatedAt" DATETIME,
  "UpdatedAt" DATETIME
);

CREATE TABLE "Transaction" (
  "TransactionID" INT PRIMARY KEY IDENTITY(1,1),
  "UserID" UNIQUEIDENTIFIER,
  "EventID" UNIQUEIDENTIFIER,
  "PaymentID" UNIQUEIDENTIFIER REFERENCES "Payment" ("PaymentID"),
  "Money" FLOAT,
  "CreatedAt" DATETIME,
  "Type" VARCHAR(255),
  "Status" VARCHAR(20),
  FOREIGN KEY ("UserID") REFERENCES "User" ("UserID"),
  FOREIGN KEY ("EventID") REFERENCES "Event" ("EventID")
);

CREATE TABLE "EventMailSystem" (
  "EventID" UNIQUEIDENTIFIER,
  "TimeExecute" DATE,
  "MethodKey" VARCHAR(50),
  "Description" VARCHAR(250),
  "Title" VARCHAR(255),
  "Body" VARCHAR(MAX),
  "Type" VARCHAR(255),
  "CreatedBy" UNIQUEIDENTIFIER,
  FOREIGN KEY ("EventID") REFERENCES "Event" ("EventID")
);

CREATE TABLE "EventPayment" (
  "PaymentID" UNIQUEIDENTIFIER REFERENCES "Payment" ("PaymentID"),
  "EventID" UNIQUEIDENTIFIER REFERENCES "Event" ("EventID")
);

CREATE TABLE "SponsorMethod" (
  "SponsorMethodId" INT PRIMARY KEY IDENTITY(1,1),
  "SponsorMethodName" VARCHAR(255)
);

CREATE TABLE "RefreshToken" (
  "RefreshTokenId" INT PRIMARY KEY IDENTITY(1,1),
  "UserID" UNIQUEIDENTIFIER,
  "Token" NVARCHAR(500) NOT NULL,
  "CreatedAt" DATETIME,
  "ExpireAt" DATETIME,
  FOREIGN KEY ("UserID") REFERENCES "User" ("UserID")
);

CREATE TABLE "Notification" (
  "NotificationId" INT PRIMARY KEY IDENTITY(1,1),
  "UserID" UNIQUEIDENTIFIER,
  "Description" VARCHAR(100),
  "CreatedAt" DATETIME,
  "IsRead" BIT,
  FOREIGN KEY ("UserID") REFERENCES "User" ("UserID")
);

CREATE TABLE "Logo" (
  "LogoId" INT PRIMARY KEY IDENTITY(1,1),
  "SponsorBrand" VARCHAR(500),
  "LogoUrl" VARCHAR(1000)
);

CREATE TABLE "EventLogo" (
  "LogoId" INT,
  "EventID" UNIQUEIDENTIFIER,
  PRIMARY KEY ("LogoId", "EventID"),
  FOREIGN KEY ("EventID") REFERENCES "Event" ("EventID"),
  FOREIGN KEY ("LogoId") REFERENCES "Logo" ("LogoId")
);

CREATE TABLE "SponsorEvent" (
  "EventID" UNIQUEIDENTIFIER,
  "SponsorMethodId" INT,
  "UserID" UNIQUEIDENTIFIER,
  "Status" VARCHAR(50),
  "CreatedAt" DATETIME,
  "UpdatedAt" DATETIME,
  FOREIGN KEY ("EventID") REFERENCES "Event" ("EventID"),
  FOREIGN KEY ("SponsorMethodId") REFERENCES "SponsorMethod" ("SponsorMethodId"),
  FOREIGN KEY ("UserID") REFERENCES "User" ("UserID")
);
