-- Inserting data into the Role table
INSERT INTO "Role" ("RoleName") VALUES
('Admin'),
('Organizer'),
('Participant'),
('Sponsor'),
('Guest');

-- Inserting data into the User table
INSERT INTO "User" ("UserID", "FirstName", "LastName", "Age", "Gender", "Email", "Phone", "Password", "PasswordSalt", "UpdatedAt", "CreatedAt", "Status", "RoleID") VALUES
(NEWID(), 'John', 'Doe', 28, 'Male', 'john.doe@example.com', '1234567890', HASHBYTES('SHA2_256', 'password1'), HASHBYTES('SHA2_256', 'salt1'), GETDATE(), GETDATE(), 'Active', 1),
(NEWID(), 'Jane', 'Smith', 32, 'Female', 'jane.smith@example.com', '0987654321', HASHBYTES('SHA2_256', 'password2'), HASHBYTES('SHA2_256', 'salt2'), GETDATE(), GETDATE(), 'Active', 2),
(NEWID(), 'Alice', 'Brown', 25, 'Female', 'alice.brown@example.com', '1122334455', HASHBYTES('SHA2_256', 'password3'), HASHBYTES('SHA2_256', 'salt3'), GETDATE(), GETDATE(), 'Active', 3),
(NEWID(), 'Bob', 'Johnson', 35, 'Male', 'bob.johnson@example.com', '2233445566', HASHBYTES('SHA2_256', 'password4'), HASHBYTES('SHA2_256', 'salt4'), GETDATE(), GETDATE(), 'Active', 4),
(NEWID(), 'Charlie', 'Davis', 29, 'Male', 'charlie.davis@example.com', '3344556677', HASHBYTES('SHA2_256', 'password5'), HASHBYTES('SHA2_256', 'salt5'), GETDATE(), GETDATE(), 'Active', 5);

-- Inserting data into the Permission table
INSERT INTO "Permission" ("PermissionName", "Description", "Status") VALUES
('Create Event', 'Permission to create events', 'Active'),
('Edit Event', 'Permission to edit events', 'Active'),
('Delete Event', 'Permission to delete events', 'Active'),
('View Event', 'Permission to view events', 'Active'),
('Manage Users', 'Permission to manage users', 'Active');

-- Inserting data into the Event table
INSERT INTO "Event" ("EventID", "EventName", "Description", "Status", "StartDate", "EndDate", "CreatedBy", "Image", "Location", "CreatedAt", "UpdatedAt", "Capacity", "Approval", "Ticket") VALUES
(NEWID(), 'Tech Conference', 'Annual technology conference', 'Active', '2024-07-01', '2024-07-03', (SELECT "UserID" FROM "User" WHERE "Email" = 'john.doe@example.com'), 'image1.jpg', 'Conference Hall 1', GETDATE(), GETDATE(), 500, 1, 100.0),
(NEWID(), 'Music Festival', 'Summer music festival', 'Active', '2024-08-15', '2024-08-17', (SELECT "UserID" FROM "User" WHERE "Email" = 'jane.smith@example.com'), 'image2.jpg', 'Open Ground', GETDATE(), GETDATE(), 2000, 1, 50.0),
(NEWID(), 'Art Exhibition', 'Modern art exhibition', 'Active', '2024-09-10', '2024-09-12', (SELECT "UserID" FROM "User" WHERE "Email" = 'alice.brown@example.com'), 'image3.jpg', 'Art Gallery', GETDATE(), GETDATE(), 300, 1, 30.0),
(NEWID(), 'Food Fair', 'Gourmet food fair', 'Active', '2024-10-05', '2024-10-07', (SELECT "UserID" FROM "User" WHERE "Email" = 'bob.johnson@example.com'), 'image4.jpg', 'City Park', GETDATE(), GETDATE(), 1000, 1, 20.0),
(NEWID(), 'Science Expo', 'Annual science expo', 'Active', '2024-11-20', '2024-11-22', (SELECT "UserID" FROM "User" WHERE "Email" = 'charlie.davis@example.com'), 'image5.jpg', 'Expo Center', GETDATE(), GETDATE(), 1500, 1, 80.0);

-- Inserting data into the Tag table
INSERT INTO "Tag" ("TagName") VALUES
('Technology'),
('Music'),
('Art'),
('Food'),
('Science');

-- Inserting data into the EventTag table
INSERT INTO "EventTag" ("TagID", "EventID") VALUES
((SELECT "TagID" FROM "Tag" WHERE "TagName" = 'Technology'), (SELECT "EventID" FROM "Event" WHERE "EventName" = 'Tech Conference')),
((SELECT "TagID" FROM "Tag" WHERE "TagName" = 'Music'), (SELECT "EventID" FROM "Event" WHERE "EventName" = 'Music Festival')),
((SELECT "TagID" FROM "Tag" WHERE "TagName" = 'Art'), (SELECT "EventID" FROM "Event" WHERE "EventName" = 'Art Exhibition')),
((SELECT "TagID" FROM "Tag" WHERE "TagName" = 'Food'), (SELECT "EventID" FROM "Event" WHERE "EventName" = 'Food Fair')),
((SELECT "TagID" FROM "Tag" WHERE "TagName" = 'Science'), (SELECT "EventID" FROM "Event" WHERE "EventName" = 'Science Expo'));

-- Inserting data into the RoleEvent table
INSERT INTO "RoleEvent" ("RoleEventName") VALUES
('Speaker'),
('Attendee'),
('Volunteer'),
('Organizer'),
('Sponsor');

-- Inserting data into the Participant table
INSERT INTO "Participant" ("UserID", "EventID", "RoleEventID", "CheckedIn", "IsCheckedMail", "CreatedAt", "Status") VALUES
((SELECT "UserID" FROM "User" WHERE "Email" = 'john.doe@example.com'), (SELECT "EventID" FROM "Event" WHERE "EventName" = 'Tech Conference'), (SELECT "RoleEventID" FROM "RoleEvent" WHERE "RoleEventName" = 'Speaker'), GETDATE(), 1, GETDATE(), 'Registered'),
((SELECT "UserID" FROM "User" WHERE "Email" = 'jane.smith@example.com'), (SELECT "EventID" FROM "Event" WHERE "EventName" = 'Music Festival'), (SELECT "RoleEventID" FROM "RoleEvent" WHERE "RoleEventName" = 'Attendee'), GETDATE(), 1, GETDATE(), 'Registered'),
((SELECT "UserID" FROM "User" WHERE "Email" = 'alice.brown@example.com'), (SELECT "EventID" FROM "Event" WHERE "EventName" = 'Art Exhibition'), (SELECT "RoleEventID" FROM "RoleEvent" WHERE "RoleEventName" = 'Volunteer'), GETDATE(), 1, GETDATE(), 'Registered'),
((SELECT "UserID" FROM "User" WHERE "Email" = 'bob.johnson@example.com'), (SELECT "EventID" FROM "Event" WHERE "EventName" = 'Food Fair'), (SELECT "RoleEventID" FROM "RoleEvent" WHERE "RoleEventName" = 'Organizer'), GETDATE(), 1, GETDATE(), 'Registered'),
((SELECT "UserID" FROM "User" WHERE "Email" = 'charlie.davis@example.com'), (SELECT "EventID" FROM "Event" WHERE "EventName" = 'Science Expo'), (SELECT "RoleEventID" FROM "RoleEvent" WHERE "RoleEventName" = 'Sponsor'), GETDATE(), 1, GETDATE(), 'Registered');

-- Inserting data into the Feedback table
INSERT INTO "Feedback" ("UserID", "EventID", "Content", "Rating", "CreatedAt") VALUES
((SELECT "UserID" FROM "User" WHERE "Email" = 'john.doe@example.com'), (SELECT "EventID" FROM "Event" WHERE "EventName" = 'Tech Conference'), 'Great event with lots of insightful talks!', 5, GETDATE()),
((SELECT "UserID" FROM "User" WHERE "Email" = 'jane.smith@example.com'), (SELECT "EventID" FROM "Event" WHERE "EventName" = 'Music Festival'), 'Amazing performances and atmosphere.', 4, GETDATE()),
((SELECT "UserID" FROM "User" WHERE "Email" = 'alice.brown@example.com'), (SELECT "EventID" FROM "Event" WHERE "EventName" = 'Art Exhibition'), 'Loved the art displays, very inspiring.', 5, GETDATE()),
((SELECT "UserID" FROM "User" WHERE "Email" = 'bob.johnson@example.com'), (SELECT "EventID" FROM "Event" WHERE "EventName" = 'Food Fair'), 'Delicious food from various vendors.', 4, GETDATE()),
((SELECT "UserID" FROM "User" WHERE "Email" = 'charlie.davis@example.com'), (SELECT "EventID" FROM "Event" WHERE "EventName" = 'Science Expo'), 'Very informative and well-organized.', 5, GETDATE());

-- Inserting data into the PaymentMethod table
INSERT INTO "PaymentMethod" ("PaymentMethodName", "PaymentMethodStatus") VALUES
('Credit Card', 1),
('PayPal', 1),
('Bank Transfer', 1),
('Cash', 1),
('Crypto', 1);

-- Inserting data into the Payment table
INSERT INTO "Payment" ("PaymentID", "PaymentMethodID", "PaymentOwner", "UserID", "SerialNumber", "PaymentStatus", "CreatedAt", "UpdatedAt") VALUES
(NEWID(), 1, 'John Doe', (SELECT "UserID" FROM "User" WHERE "Email" = 'john.doe@example.com'), 'SN12345', 1, GETDATE(), GETDATE()),
(NEWID(), 2, 'Jane Smith', (SELECT "UserID" FROM "User" WHERE "Email" = 'jane.smith@example.com'), 'SN12346', 1, GETDATE(), GETDATE()),
(NEWID(), 3, 'Alice Brown', (SELECT "UserID" FROM "User" WHERE "Email" = 'alice.brown@example.com'), 'SN12347', 1, GETDATE(), GETDATE()),
(NEWID(), 4, 'Bob Johnson', (SELECT "UserID" FROM "User" WHERE "Email" = 'bob.johnson@example.com'), 'SN12348', 1, GETDATE(), GETDATE()),
(NEWID(), 5, 'Charlie Davis', (SELECT "UserID" FROM "User" WHERE "Email" = 'charlie.davis@example.com'), 'SN12349', 1, GETDATE(), GETDATE());

-- Inserting data into the Transaction table
INSERT INTO "Transaction" ("UserID", "EventID", "PaymentID", "Money", "CreatedAt", "Type", "Status") VALUES
((SELECT "UserID" FROM "User" WHERE "Email" = 'john.doe@example.com'), (SELECT "EventID" FROM "Event" WHERE "EventName" = 'Tech Conference'), (SELECT "PaymentID" FROM "Payment" WHERE "PaymentOwner" = 'John Doe'), 100.0, GETDATE(), 'Ticket', 'Completed'),
((SELECT "UserID" FROM "User" WHERE "Email" = 'jane.smith@example.com'), (SELECT "EventID" FROM "Event" WHERE "EventName" = 'Music Festival'), (SELECT "PaymentID" FROM "Payment" WHERE "PaymentOwner" = 'Jane Smith'), 50.0, GETDATE(), 'Ticket', 'Completed'),
((SELECT "UserID" FROM "User" WHERE "Email" = 'alice.brown@example.com'), (SELECT "EventID" FROM "Event" WHERE "EventName" = 'Art Exhibition'), (SELECT "PaymentID" FROM "Payment" WHERE "PaymentOwner" = 'Alice Brown'), 30.0, GETDATE(), 'Ticket', 'Completed'),
((SELECT "UserID" FROM "User" WHERE "Email" = 'bob.johnson@example.com'), (SELECT "EventID" FROM "Event" WHERE "EventName" = 'Food Fair'), (SELECT "PaymentID" FROM "Payment" WHERE "PaymentOwner" = 'Bob Johnson'), 20.0, GETDATE(), 'Ticket', 'Completed'),
((SELECT "UserID" FROM "User" WHERE "Email" = 'charlie.davis@example.com'), (SELECT "EventID" FROM "Event" WHERE "EventName" = 'Science Expo'), (SELECT "PaymentID" FROM "Payment" WHERE "PaymentOwner" = 'Charlie Davis'), 80.0, GETDATE(), 'Ticket', 'Completed');

-- Inserting data into the EventMailSystem table
INSERT INTO "EventMailSystem" ("EventID", "TimeExecute", "MethodKey", "Description", "Title", "Body", "Type", "CreatedBy") VALUES
((SELECT "EventID" FROM "Event" WHERE "EventName" = 'Tech Conference'), '2024-06-01', 'Reminder', 'Event reminder', 'Tech Conference Reminder', 'Dont forget to attend the Tech Conference!', 'Email', (SELECT "UserID" FROM "User" WHERE "Email" = 'john.doe@example.com')),
((SELECT "EventID" FROM "Event" WHERE "EventName" = 'Music Festival'), '2024-07-01', 'Reminder', 'Event reminder', 'Music Festival Reminder', 'Dont forget to attend the Music Festival!', 'Email', (SELECT "UserID" FROM "User" WHERE "Email" = 'jane.smith@example.com')),
((SELECT "EventID" FROM "Event" WHERE "EventName" = 'Art Exhibition'), '2024-08-01', 'Reminder', 'Event reminder', 'Art Exhibition Reminder', 'Dont forget to attend the Art Exhibition!', 'Email', (SELECT "UserID" FROM "User" WHERE "Email" = 'alice.brown@example.com')),
((SELECT "EventID" FROM "Event" WHERE "EventName" = 'Food Fair'), '2024-09-01', 'Reminder', 'Event reminder', 'Food Fair Reminder', 'Dont forget to attend the Food Fair!', 'Email', (SELECT "UserID" FROM "User" WHERE "Email" = 'bob.johnson@example.com')),
((SELECT "EventID" FROM "Event" WHERE "EventName" = 'Science Expo'), '2024-10-01', 'Reminder', 'Event reminder', 'Science Expo Reminder', 'Dont forget to attend the Science Expo!', 'Email', (SELECT "UserID" FROM "User" WHERE "Email" = 'charlie.davis@example.com'));

-- Inserting data into the EventPayment table
INSERT INTO "EventPayment" ("PaymentID", "EventID") VALUES
((SELECT "PaymentID" FROM "Payment" WHERE "PaymentOwner" = 'John Doe'), (SELECT "EventID" FROM "Event" WHERE "EventName" = 'Tech Conference')),
((SELECT "PaymentID" FROM "Payment" WHERE "PaymentOwner" = 'Jane Smith'), (SELECT "EventID" FROM "Event" WHERE "EventName" = 'Music Festival')),
((SELECT "PaymentID" FROM "Payment" WHERE "PaymentOwner" = 'Alice Brown'), (SELECT "EventID" FROM "Event" WHERE "EventName" = 'Art Exhibition')),
((SELECT "PaymentID" FROM "Payment" WHERE "PaymentOwner" = 'Bob Johnson'), (SELECT "EventID" FROM "Event" WHERE "EventName" = 'Food Fair')),
((SELECT "PaymentID" FROM "Payment" WHERE "PaymentOwner" = 'Charlie Davis'), (SELECT "EventID" FROM "Event" WHERE "EventName" = 'Science Expo'));

-- Inserting data into the SponsorMethod table
INSERT INTO "SponsorMethod" ("SponsorMethodName") VALUES
('Cash'),
('In-Kind'),
('Sponsorship'),
('Donation'),
('Partnership');

-- Inserting data into the RefreshToken table
INSERT INTO "RefreshToken" ("UserID", "Token", "CreatedAt", "ExpireAt") VALUES
((SELECT "UserID" FROM "User" WHERE "Email" = 'john.doe@example.com'), 'token1', GETDATE(), DATEADD(DAY, 30, GETDATE())),
((SELECT "UserID" FROM "User" WHERE "Email" = 'jane.smith@example.com'), 'token2', GETDATE(), DATEADD(DAY, 30, GETDATE())),
((SELECT "UserID" FROM "User" WHERE "Email" = 'alice.brown@example.com'), 'token3', GETDATE(), DATEADD(DAY, 30, GETDATE())),
((SELECT "UserID" FROM "User" WHERE "Email" = 'bob.johnson@example.com'), 'token4', GETDATE(), DATEADD(DAY, 30, GETDATE())),
((SELECT "UserID" FROM "User" WHERE "Email" = 'charlie.davis@example.com'), 'token5', GETDATE(), DATEADD(DAY, 30, GETDATE()));

-- Inserting data into the Notification table
INSERT INTO "Notification" ("UserID", "Description", "CreatedAt", "IsRead") VALUES
((SELECT "UserID" FROM "User" WHERE "Email" = 'john.doe@example.com'), 'Welcome to the Tech Conference!', GETDATE(), 0),
((SELECT "UserID" FROM "User" WHERE "Email" = 'jane.smith@example.com'), 'Welcome to the Music Festival!', GETDATE(), 0),
((SELECT "UserID" FROM "User" WHERE "Email" = 'alice.brown@example.com'), 'Welcome to the Art Exhibition!', GETDATE(), 0),
((SELECT "UserID" FROM "User" WHERE "Email" = 'bob.johnson@example.com'), 'Welcome to the Food Fair!', GETDATE(), 0),
((SELECT "UserID" FROM "User" WHERE "Email" = 'charlie.davis@example.com'), 'Welcome to the Science Expo!', GETDATE(), 0);

-- Inserting data into the Logo table
INSERT INTO "Logo" ("SponsorBrand", "LogoUrl") VALUES
('TechCorp', 'techcorp-logo.png'),
('MusicFest', 'musicfest-logo.png'),
('ArtHouse', 'arthouse-logo.png'),
('Foodies', 'foodies-logo.png'),
('ScienceWorld', 'scienceworld-logo.png');

-- Inserting data into the EventLogo table
INSERT INTO "EventLogo" ("LogoId", "EventID") VALUES
((SELECT "LogoId" FROM "Logo" WHERE "SponsorBrand" = 'TechCorp'), (SELECT "EventID" FROM "Event" WHERE "EventName" = 'Tech Conference')),
((SELECT "LogoId" FROM "Logo" WHERE "SponsorBrand" = 'MusicFest'), (SELECT "EventID" FROM "Event" WHERE "EventName" = 'Music Festival')),
((SELECT "LogoId" FROM "Logo" WHERE "SponsorBrand" = 'ArtHouse'), (SELECT "EventID" FROM "Event" WHERE "EventName" = 'Art Exhibition')),
((SELECT "LogoId" FROM "Logo" WHERE "SponsorBrand" = 'Foodies'), (SELECT "EventID" FROM "Event" WHERE "EventName" = 'Food Fair')),
((SELECT "LogoId" FROM "Logo" WHERE "SponsorBrand" = 'ScienceWorld'), (SELECT "EventID" FROM "Event" WHERE "EventName" = 'Science Expo'));

-- Inserting data into the SponsorEvent table
INSERT INTO "SponsorEvent" ("EventID", "SponsorMethodId", "UserID", "Status", "CreatedAt", "UpdatedAt") VALUES
((SELECT "EventID" FROM "Event" WHERE "EventName" = 'Tech Conference'), (SELECT "SponsorMethodId" FROM "SponsorMethod" WHERE "SponsorMethodName" = 'Cash'), (SELECT "UserID" FROM "User" WHERE "Email" = 'john.doe@example.com'), 'Approved', GETDATE(), GETDATE()),
((SELECT "EventID" FROM "Event" WHERE "EventName" = 'Music Festival'), (SELECT "SponsorMethodId" FROM "SponsorMethod" WHERE "SponsorMethodName" = 'In-Kind'), (SELECT "UserID" FROM "User" WHERE "Email" = 'jane.smith@example.com'), 'Approved', GETDATE(), GETDATE()),
((SELECT "EventID" FROM "Event" WHERE "EventName" = 'Art Exhibition'), (SELECT "SponsorMethodId" FROM "SponsorMethod" WHERE "SponsorMethodName" = 'Sponsorship'), (SELECT "UserID" FROM "User" WHERE "Email" = 'alice.brown@example.com'), 'Approved', GETDATE(), GETDATE()),
((SELECT "EventID" FROM "Event" WHERE "EventName" = 'Food Fair'), (SELECT "SponsorMethodId" FROM "SponsorMethod" WHERE "SponsorMethodName" = 'Donation'), (SELECT "UserID" FROM "User" WHERE "Email" = 'bob.johnson@example.com'), 'Approved', GETDATE(), GETDATE()),
((SELECT "EventID" FROM "Event" WHERE "EventName" = 'Science Expo'), (SELECT "SponsorMethodId" FROM "SponsorMethod" WHERE "SponsorMethodName" = 'Partnership'), (SELECT "UserID" FROM "User" WHERE "Email" = 'charlie.davis@example.com'), 'Approved', GETDATE(), GETDATE());
