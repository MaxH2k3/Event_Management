USE [EventManage]

CREATE TABLE "Role" (
  "RoleID" INT PRIMARY KEY IDENTITY(1,1),
  "RoleName" VARCHAR(255) NOT NULL
);

CREATE TABLE "User" (
  "UserID" UNIQUEIDENTIFIER PRIMARY KEY,
  "FullName" NVARCHAR(255),
  "Email" VARCHAR(255),
  "Phone" VARCHAR(15),
  "UpdatedAt" DATETIME,
  "CreatedAt" DATETIME,
  "Status" VARCHAR(50) NOT NULL,
  "RoleID" INT NOT NULL,
  "Avatar"  VARCHAR(1000),
  FOREIGN KEY ("RoleID") REFERENCES "Role" ("RoleID")
);

CREATE TABLE "UserValidation" (
	"UserID" UNIQUEIDENTIFIER PRIMARY KEY,
	"OTP" VARCHAR(6),
	"VerifyToken" VARCHAR(MAX),
	"ExpiredAt" DATETIME,
	"CreatedAt" DATETIME,
	FOREIGN KEY ("UserID") REFERENCES "User" ("UserID")
)

CREATE TABLE "Event" (
  "EventID" UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
  "EventName" NVARCHAR(250) NOT NULL,
  "Description" NVARCHAR(MAX),
  "Status" VARCHAR(10),
  "StartDate" DATETIME,
  "EndDate" DATETIME,
  "CreatedBy" UNIQUEIDENTIFIER,
  "Image" VARCHAR(5000),
  "Location" NVARCHAR(500),
  "CreatedAt" DATETIME,
  "UpdatedAt" DATETIME,
  "Capacity" INT,
  "Approval" BIT,
  "Fare" DECIMAL(19, 2),
  "LocationUrl" varchar (2000),
  "LocationCoord" varchar (500),
  "LocationID" varchar(1000),
  "LocationAddress" nvarchar(1000),
  "Theme" varchar(20),
  FOREIGN KEY ("CreatedBy") REFERENCES "User" ("UserID")
);

CREATE TABLE "Tag" (
  "TagID" INT PRIMARY KEY IDENTITY(1,1),
  "TagName" NVARCHAR(255)
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
  "Content" text,
  "Rating" INT,
  "CreatedAt" DATETIME,
  PRIMARY KEY ("UserID", "EventID"),
  FOREIGN KEY ("UserID") REFERENCES "User" ("UserID"),
  FOREIGN KEY ("EventID") REFERENCES "Event" ("EventID")
);


CREATE TABLE "EventMailSystem" (
  "EventID" UNIQUEIDENTIFIER,
  "TimeExecute" DATE,
  "MethodKey" VARCHAR(50),
  "Description" NVARCHAR(250),
  "Title" VARCHAR(255),
  "Body" VARCHAR(MAX),
  "Type" VARCHAR(255),
  "CreatedBy" UNIQUEIDENTIFIER,
  FOREIGN KEY ("EventID") REFERENCES "Event" ("EventID")
);


CREATE TABLE "RefreshToken" (
  "RefreshTokenID" INT PRIMARY KEY IDENTITY(1,1),
  "UserID" UNIQUEIDENTIFIER,
  "Token" NVARCHAR(300) NOT NULL,
  "CreatedAt" DATETIME,
  "ExpireAt" DATETIME,
  FOREIGN KEY ("UserID") REFERENCES "User" ("UserID")
);

CREATE TABLE "Notification" (
  "NotificationId" INT PRIMARY KEY IDENTITY(1,1),
  "UserID" UNIQUEIDENTIFIER,
  "Description" NVARCHAR(255),
  "CreatedAt" DATETIME,
  "IsRead" BIT,
  FOREIGN KEY ("UserID") REFERENCES "User" ("UserID")
);

CREATE TABLE "Logo" (
  "LogoID" INT PRIMARY KEY IDENTITY(1,1),
  "SponsorBrand" VARCHAR(500),
  "LogoUrl" VARCHAR(1000)
);

CREATE TABLE "EventLogo" (
  "LogoID" INT,
  "EventID" UNIQUEIDENTIFIER,
  PRIMARY KEY ("LogoID", "EventID"),
  FOREIGN KEY ("EventID") REFERENCES "Event" ("EventID"),
  FOREIGN KEY ("LogoID") REFERENCES "Logo" ("LogoID")
);

CREATE TABLE "SponsorEvent" (
  "EventID" UNIQUEIDENTIFIER,
  "SponsorMethodID" INT,
  "UserID" UNIQUEIDENTIFIER,
  "Status" VARCHAR(50),
  "IsSponsored" BIT,
  "Amount" DECIMAL(19, 2),
  "Message" NVARCHAR(200),
  "SponsorType" varchar(20),
  "CreatedAt" DATETIME,
  "UpdatedAt" DATETIME,
  PRIMARY KEY ("EventID", "UserID"),
  FOREIGN KEY ("EventID") REFERENCES "Event" ("EventID"),
  FOREIGN KEY ("UserID") REFERENCES "User" ("UserID")
);

-- Tạo bảng PaymentTransaction
CREATE TABLE PaymentTransaction (
    ID UNIQUEIDENTIFIER PRIMARY KEY,
	PayID VARCHAR(40),
	PayerID VARCHAR(20),
    TranMessage NVARCHAR(MAX),
    TranStatus VARCHAR(50),
    TranAmount DECIMAL(19, 2),
    TranDate DATETIME,
	"RemitterID" UNIQUEIDENTIFIER,
	"EventID" UNIQUEIDENTIFIER,
	FOREIGN KEY ("EventID") REFERENCES "Event" ("EventID"),
	FOREIGN KEY ("RemitterID") REFERENCES "User" ("UserID")
);


