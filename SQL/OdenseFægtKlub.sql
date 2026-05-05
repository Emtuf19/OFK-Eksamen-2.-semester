CREATE DATABASE OdenseFægteKlub;
GO
USE OdenseFægteKlub;

--TABLES
CREATE TABLE Member
(
	MemberID Int IDENTITY(1,1) PRIMARY KEY,
	MemberFirstName NVarCHar(50) NOT NULL,
	MemberLastName NVarCHar(50) NOT NULL
);

CREATE TABLE ContactInfo
(
	ContactPersonID Int IDENTITY(1,1) PRIMARY KEY,
	ContactFirstName NVarChar(50) NOT NULL,
	ContactLastName NVarChar(50) NOT NULL,
	ContactPhoneNumber NVarCHar(30) NOT NULL,
	ContactEmail NVarChar(100) NOT NULL,
	MemberID Int NOT NULL,
	CONSTRAINT FK_ContactInfo_Member FOREIGN KEY (MemberID) REFERENCES Member(MemberID) ON DELETE CASCADE,
	CONSTRAINT CK_Contact_EmailFormat CHECK (ContactEmail LIKE '%_@_%._%')

);

CREATE TABLE Trainer
(
	TrainerID Int IDENTITY(1,1) PRIMARY KEY,
	TrainerFirstName NVarChar(50) NOT NULL,
	TrainerLastName NVarChar(50) NOT NULL,
	TrainerPhoneNumber NVarchar(30) NOT NULL,
	TrainerEmail NVarChar(100) NOT NULL,
	CONSTRAINT CK_Trainer_EmailFormat CHECK (TrainerEmail LIKE '%_@_%._%')
);

CREATE TABLE Practice
(
	PracticeID Int IDENTITY(1,1) PRIMARY KEY,
	PracticeName NVarChar(100) NOT NULL,
	StartTime DateTime2 NOT NULL,
	EndTime DateTime2 NOT NULL,
	CONSTRAINT CK_Practice_StartNotInPast CHECK (StartTime >= SYSDATETIME()), 
	CONSTRAINT CK_Practice_EndAfterStart CHECK (EndTime > StartTime)
);

CREATE TABLE Event
(
	EventID Int IDENTITY(1,1) PRIMARY KEY,
	EventName NVarChar(100) NOT NULL,
	Description NVarChar(250) NOT NULL,
	Price FLoat NOT NULL,
	AgeGroup NVarChar(20) NOT NULL,
	Time DateTime2 NOT NULL,
	CONSTRAINT CK_Event_PriceNotNegative CHECK (Price >= 0),
	CONSTRAINT CK_Event_TimeNotInPast CHECK (Time >= SYSDATETIME())
);
-- mange til mange relation via kobling tabel.
CREATE TABLE Member_Event
(
	MemberID Int,
	EventID Int,
	CONSTRAINT PK_MemberEvent PRIMARY KEY (MemberID, EventID),
	CONSTRAINT FK_MemberEvent_Member FOREIGN KEY (MemberID) REFERENCES Member(MemberID) ON DELETE CASCADE,
	CONSTRAINT FK_MemberEvent_Event FOREIGN KEY (EventID) REFERENCES Event(EventID) ON DELETE CASCADE,
	CONSTRAINT uq_MemberEvent UNIQUE (MemberID, EventID)
);
-- mange til mange relation via kobling tabel.
CREATE TABLE Member_Practice
(
	MemberID Int,
	PracticeID Int,
	CONSTRAINT PK_MemberPractice PRIMARY KEY (MemberID, PracticeID),
	CONSTRAINT FK_MemberPractice_Member FOREIGN KEY (MemberID) REFERENCES Member(MemberID) ON DELETE CASCADE,
	CONSTRAINT FK_MemberPractice_Practice FOREIGN KEY (PracticeID) REFERENCES Practice(PracticeID) ON DELETE CASCADE,
	CONSTRAINT uq_MemberPractice UNIQUE (MemberID, PracticeID)
);

CREATE TABLE Trainer_Event
(
	TrainerID Int,
	EventID Int,
	CONSTRAINT PK_TrainerEvent PRIMARY KEY (TrainerID, EventID),
	CONSTRAINT FK_TrainerEvent_Trainer FOREIGN KEY (TrainerID) REFERENCES Trainer(TrainerID) ON DELETE CASCADE,
	CONSTRAINT FK_TrainerEvent_Event FOREIGN KEY (EventID) REFERENCES Event(EventID) ON DELETE CASCADE,
	CONSTRAINT uq_TrainerEvent UNIQUE (TrainerID, EventID)
);

CREATE TABLE Trainer_Practice
(
	TrainerID Int,
	PracticeID Int,
	CONSTRAINT PK_TrainerPractice PRIMARY KEY (TrainerID, PracticeID),
	CONSTRAINT FK_TrainerPractice_Trainer FOREIGN KEY (TrainerID) REFERENCES Trainer(TrainerID) ON DELETE CASCADE,
	CONSTRAINT FK_TrainerPractice_Practice FOREIGN KEY (PracticeID) REFERENCES Practice(PracticeID) ON DELETE CASCADE,
	CONSTRAINT uq_TrainerPractice UNIQUE (TrainerID, PracticeID)
);

GO
CREATE TRIGGER trg_MaxTwoContacts
ON ContactInfo
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS
    (
        SELECT c.MemberID
        FROM ContactInfo c
        INNER JOIN inserted i ON c.MemberID = i.MemberID
        GROUP BY c.MemberID
        HAVING COUNT(*) > 2
    )
    BEGIN
        THROW 50000, 'maximum of 2 contact persons', 1;
    END
END;
