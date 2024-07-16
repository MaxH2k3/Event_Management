USE [EventManage]

-- Insert sample data into Role table
INSERT INTO "Role" ("RoleName") VALUES 
('Guest'),
('User'),
('Admin');

-- Insert sample data into User table
INSERT INTO "User" ("UserID", "FullName", "Email", "Phone", "UpdatedAt", "CreatedAt", "Status", "RoleID", "Avatar") VALUES 
(NEWID(), 'John Doe', 'john.doe@example.com', '1234567890', GETDATE(), GETDATE(), 'Active', 1, 'avatar1.jpg'),
(NEWID(), 'Jane Smith', 'jane.smith@example.com', '0987654321', GETDATE(), GETDATE(), 'Active', 2, 'avatar2.jpg'),
(NEWID(), 'Alice Johnson', 'alice.johnson@example.com', '1112223333', GETDATE(), GETDATE(), 'Active', 3, 'avatar3.jpg');

-- Insert sample data into UserValidation table
INSERT INTO "UserValidation" ("UserID", "OTP", "VerifyToken", "ExpiredAt", "CreatedAt") VALUES 
((SELECT "UserID" FROM "User" WHERE "FullName"='John Doe'), '123456', 'token123', DATEADD(day, 1, GETDATE()), GETDATE()),
((SELECT "UserID" FROM "User" WHERE "FullName"='Jane Smith'), '654321', 'token456', DATEADD(day, 1, GETDATE()), GETDATE()),
((SELECT "UserID" FROM "User" WHERE "FullName"='Alice Johnson'), '789012', 'token789', DATEADD(day, 1, GETDATE()), GETDATE());

-- Insert sample data into Event table
INSERT INTO "Event" ("EventID", "EventName", "Description", "Status", "StartDate", "EndDate", "CreatedBy", "Image", "Location", "CreatedAt", "UpdatedAt", "Capacity", "Approval", "Fare", "LocationUrl", "LocationCoord", "LocationID", "LocationAddress", "Theme") VALUES 
(NEWID(), 'Music Concert', 'A great music concert.', 'Upcoming', GETDATE(), DATEADD(day, 1, GETDATE()), (SELECT "UserID" FROM "User" WHERE "FullName"='John Doe'), 'image1.jpg', 'Central Park', GETDATE(), GETDATE(), 500, 1, 20.00, 'http://location1.com', 'coord1', 'loc1', 'address1', 'Music'),
(NEWID(), 'Art Exhibition', 'An amazing art exhibition.', 'Upcoming', GETDATE(), DATEADD(day, 1, GETDATE()), (SELECT "UserID" FROM "User" WHERE "FullName"='Jane Smith'), 'image2.jpg', 'City Gallery', GETDATE(), GETDATE(), 300, 1, 15.00, 'http://location2.com', 'coord2', 'loc2', 'address2', 'Art'),
(NEWID(), 'Tech Conference', 'A tech conference for developers.', 'Upcoming', GETDATE(), DATEADD(day, 1, GETDATE()), (SELECT "UserID" FROM "User" WHERE "FullName"='Alice Johnson'), 'image3.jpg', 'Convention Center', GETDATE(), GETDATE(), 1000, 1, 50.00, 'http://location3.com', 'coord3', 'loc3', 'address3', 'Tech');

-- Insert sample data into Tag table
INSERT INTO "Tag" ("TagName") VALUES 
('Music'),
('Art'),
('Technology');

-- Insert sample data into EventTag table
INSERT INTO "EventTag" ("TagID", "EventID") VALUES 
((SELECT "TagID" FROM "Tag" WHERE "TagName"='Music'), (SELECT "EventID" FROM "Event" WHERE "EventName"='Music Concert')),
((SELECT "TagID" FROM "Tag" WHERE "TagName"='Art'), (SELECT "EventID" FROM "Event" WHERE "EventName"='Art Exhibition')),
((SELECT "TagID" FROM "Tag" WHERE "TagName"='Technology'), (SELECT "EventID" FROM "Event" WHERE "EventName"='Tech Conference'));

-- Insert sample data into RoleEvent table
INSERT INTO "RoleEvent" ("RoleEventName") VALUES 
('Host Organizor'),
('Visitor'),
('Check-in staff'),
('Sponsor');

-- Insert sample data into Participant table
INSERT INTO "Participant" ("UserID", "EventID", "RoleEventID", "CheckedIn", "IsCheckedMail", "CreatedAt", "Status") VALUES 
((SELECT "UserID" FROM "User" WHERE "FullName"='John Doe'), (SELECT "EventID" FROM "Event" WHERE "EventName"='Music Concert'), (SELECT "RoleEventID" FROM "RoleEvent" WHERE "RoleEventName"='Speaker'), GETDATE(), 1, GETDATE(), 'Confirmed'),
((SELECT "UserID" FROM "User" WHERE "FullName"='Jane Smith'), (SELECT "EventID" FROM "Event" WHERE "EventName"='Art Exhibition'), (SELECT "RoleEventID" FROM "RoleEvent" WHERE "RoleEventName"='Attendee'), GETDATE(), 1, GETDATE(), 'Confirmed'),
((SELECT "UserID" FROM "User" WHERE "FullName"='Alice Johnson'), (SELECT "EventID" FROM "Event" WHERE "EventName"='Tech Conference'), (SELECT "RoleEventID" FROM "RoleEvent" WHERE "RoleEventName"='Volunteer'), GETDATE(), 1, GETDATE(), 'Confirmed');

-- Insert sample data into Feedback table
INSERT INTO "Feedback" ("UserID", "EventID", "Content", "Rating", "CreatedAt") VALUES 
((SELECT "UserID" FROM "User" WHERE "FullName"='John Doe'), (SELECT "EventID" FROM "Event" WHERE "EventName"='Music Concert'), 'Great event!', 5, GETDATE()),
((SELECT "UserID" FROM "User" WHERE "FullName"='Jane Smith'), (SELECT "EventID" FROM "Event" WHERE "EventName"='Art Exhibition'), 'Loved it!', 4, GETDATE()),
((SELECT "UserID" FROM "User" WHERE "FullName"='Alice Johnson'), (SELECT "EventID" FROM "Event" WHERE "EventName"='Tech Conference'), 'Very informative.', 5, GETDATE());


-- Insert sample data into EventMailSystem table
INSERT INTO "EventMailSystem" ("EventID", "TimeExecute", "MethodKey", "Description", "Title", "Body", "Type", "CreatedBy") VALUES 
((SELECT "EventID" FROM "Event" WHERE "EventName"='Music Concert'), GETDATE(), 'key1', 'Reminder email', 'Concert Reminder', 'Don''t forget to attend the concert!', 'Reminder', (SELECT "UserID" FROM "User" WHERE "FullName"='John Doe')),
((SELECT "EventID" FROM "Event" WHERE "EventName"='Art Exhibition'), GETDATE(), 'key2', 'Reminder email', 'Exhibition Reminder', 'Don''t forget to attend the exhibition!', 'Reminder', (SELECT "UserID" FROM "User" WHERE "FullName"='Jane Smith')),
((SELECT "EventID" FROM "Event" WHERE "EventName"='Tech Conference'), GETDATE(), 'key3', 'Reminder email', 'Conference Reminder', 'Don''t forget to attend the conference!', 'Reminder', (SELECT "UserID" FROM "User" WHERE "FullName"='Alice Johnson'));

-- Insert sample data into RefreshToken table
INSERT INTO "RefreshToken" ("UserID", "Token", "CreatedAt", "ExpireAt") VALUES 
((SELECT "UserID" FROM "User" WHERE "FullName"='John Doe'), 'token1', GETDATE(), DATEADD(day, 30, GETDATE())),
((SELECT "UserID" FROM "User" WHERE "FullName"='Jane Smith'), 'token2', GETDATE(), DATEADD(day, 30, GETDATE())),
((SELECT "UserID" FROM "User" WHERE "FullName"='Alice Johnson'), 'token3', GETDATE(), DATEADD(day, 30, GETDATE()));

-- Insert sample data into Notification table
INSERT INTO "Notification" ("UserID", "Description", "CreatedAt", "IsRead") VALUES 
((SELECT "UserID" FROM "User" WHERE "FullName"='John Doe'), 'You have a new message', GETDATE(), 0),
((SELECT "UserID" FROM "User" WHERE "FullName"='Jane Smith'), 'Your event is coming up', GETDATE(), 0),
((SELECT "UserID" FROM "User" WHERE "FullName"='Alice Johnson'), 'New feedback received', GETDATE(), 0);

-- Insert sample data into Logo table
INSERT INTO "Logo" ("SponsorBrand", "LogoUrl") VALUES 
('Brand A', 'logo1.jpg'),
('Brand B', 'logo2.jpg'),
('Brand C', 'logo3.jpg');

-- Insert sample data into EventLogo table
INSERT INTO "EventLogo" ("LogoID", "EventID") VALUES 
((SELECT "LogoID" FROM "Logo" WHERE "SponsorBrand"='Brand A'), (SELECT "EventID" FROM "Event" WHERE "EventName"='Music Concert')),
((SELECT "LogoID" FROM "Logo" WHERE "SponsorBrand"='Brand B'), (SELECT "EventID" FROM "Event" WHERE "EventName"='Art Exhibition')),
((SELECT "LogoID" FROM "Logo" WHERE "SponsorBrand"='Brand C'), (SELECT "EventID" FROM "Event" WHERE "EventName"='Tech Conference'));

-- Insert sample data into SponsorEvent table
INSERT INTO "SponsorEvent" ("EventID", "SponsorMethodID", "UserID", "Status", "IsSponsored", "Amount", "CreatedAt", "UpdatedAt") VALUES 
((SELECT "EventID" FROM "Event" WHERE "EventName"='Music Concert'), 1, (SELECT "UserID" FROM "User" WHERE "FullName"='John Doe'), 'Confirmed', 1, 1000.00, GETDATE(), GETDATE()),
((SELECT "EventID" FROM "Event" WHERE "EventName"='Art Exhibition'), 2, (SELECT "UserID" FROM "User" WHERE "FullName"='Jane Smith'), 'Confirmed', 1, 1500.00, GETDATE(), GETDATE()),
((SELECT "EventID" FROM "Event" WHERE "EventName"='Tech Conference'), 3, (SELECT "UserID" FROM "User" WHERE "FullName"='Alice Johnson'), 'Confirmed', 1, 2000.00, GETDATE(), GETDATE());

-- Insert sample data into PaymentTransaction table
INSERT INTO PaymentTransaction (ID, TranMessage, TranStatus, TranAmount, TranDate, RemitterID, EventID) VALUES 
(NEWID(), 'Transaction 1 completed', 'Success', 50.00, GETDATE(), (SELECT "UserID" FROM "User" WHERE "FullName"='John Doe'), (SELECT "EventID" FROM "Event" WHERE "EventName"='Music Concert')),
(NEWID(), 'Transaction 2 completed', 'Success', 75.00, GETDATE(), (SELECT "UserID" FROM "User" WHERE "FullName"='Jane Smith'), (SELECT "EventID" FROM "Event" WHERE "EventName"='Art Exhibition')),
(NEWID(), 'Transaction 3 completed', 'Success', 100.00, GETDATE(), (SELECT "UserID" FROM "User" WHERE "FullName"='Alice Johnson'), (SELECT "EventID" FROM "Event" WHERE "EventName"='Tech Conference'));
