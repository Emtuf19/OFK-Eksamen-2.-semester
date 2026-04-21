CREATE DATABASE OdenseFægteKlub;
GO
USE OdenseFægteKlub;

--TABLES
CREATE TABLE Member
(
	MemberID Int IDENTITY(1,1) PRIMARY KEY,
	MemberName NVarCHar(50),
);

CREATE TABLE ContactInfo
(
	ContactPersonID Int IDENTITY(1,1) PRIMARY KEY,
	ContactName NVarChar(50),
	ContactPhoneNumber VarCHar(8),
	ContactEmail NVarChar(100),
	MemberID Int FOREIGN KEY REFERENCES Member(MemberID)
);

CREATE TABLE Trainer
(
	TrainerID Int IDENTITY(1,1) PRIMARY KEY,
	TrainerName NVarChar(50),
	TrainerPhoneNumber Varchar(8),
	TrainerEmail NVarChar(100)
);

CREATE TABLE Practice
(
	PracticeID Int IDENTITY(1,1) PRIMARY KEY,
	PracticeName NVarChar(100),
	StartTime DateTime2,
	EndTime DateTime2
);

CREATE TABLE Event
(
	EventID Int IDENTITY(1,1) PRIMARY KEY,
	EventName NVarChar(100),
	Description NVarChar(250),
	Price FLoat,
	AgeGroup NVarChar(20),
	Time DateTime2
);
-- mange til mange relation via kobling tabel.
CREATE TABLE Member_Event
(
	MemberID Int,
	EventID Int,
	CONSTRAINT PK_MemberEvent PRIMARY KEY (MemberID, EventID),
	CONSTRAINT FK_MemberEvent_Member FOREIGN KEY (MemberID) REFERENCES Member(MemberID),
	CONSTRAINT FK_MemberEvent_Event FOREIGN KEY (EventID) REFERENCES Event(EventID)
);
-- mange til mange relation via kobling tabel.
CREATE TABLE Member_Practice
(
	MemberID Int,
	PracticeID Int,
	CONSTRAINT PK_MemberPractice PRIMARY KEY (MemberID, PracticeID),
	CONSTRAINT FK_MemberPractice_Member FOREIGN KEY (MemberID) REFERENCES Member(MemberID),
	CONSTRAINT FK_MemberPractice_Practice FOREIGN KEY (PracticeID) REFERENCES Practice(PracticeID)
);

CREATE TABLE Trainer_Event
(
	TrainerID Int,
	EventID Int,
	CONSTRAINT PK_TrainerEvent PRIMARY KEY (TrainerID, EventID),
	CONSTRAINT FK_TrainerEvent_Trainer FOREIGN KEY (TrainerID) REFERENCES Trainer(TrainerID),
	CONSTRAINT FK_TrainerEvent_Event FOREIGN KEY (EventID) REFERENCES Event(EventID)
);

CREATE TABLE Trainer_Practice
(
	TrainerID Int,
	PracticeID Int,
	CONSTRAINT PK_TrainerPractice PRIMARY KEY (TrainerID, PracticeID),
	CONSTRAINT FK_TrainerPractice_Trainer FOREIGN KEY (TrainerID) REFERENCES Trainer(TrainerID),
	CONSTRAINT FK_TrainerPractice_Practice FOREIGN KEY (PracticeID) REFERENCES Practice(PracticeID)
);