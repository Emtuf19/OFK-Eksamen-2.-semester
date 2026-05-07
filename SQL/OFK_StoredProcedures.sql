Use OdenseFægteKlub;
GO

--Stored Procedure til INSERT INTO Member
CREATE PROC sp_InsertIntoMember
@memberFirstName NVarChar(50),
@memberLastName NVarChar(50)
AS
BEGIN
    SET NOCOUNT ON;

    --MEMBER FIRST NAME MÅ IKKE VÆRE TOM
    IF LTRIM(RTRIM(@memberFirstName)) = ''
    BEGIN
        RAISERROR('Member first name cannot be empty', 16, 1)
        RETURN;
    END;

    --MEMBER LAST NAME MÅ IKKE VÆRE TOM
    IF LTRIM(RTRIM(@memberLastName)) = ''
    BEGIN
        RAISERROR('Member last name cannot be empty', 16, 1)
        RETURN;
    END;

	INSERT INTO Member 
	(
		MemberFirstName,
		MemberLastName
	)
	VALUES	
	(
		@memberFirstName,
		@memberLastName
	);
END;

--Stored Procedure til INSERT INTO ContactInfo
GO
CREATE PROC sp_InsertIntoContactInfo
	@contactFirstName NVarChar(50),
	@contactLastName NVarChar(50),
	@contactPhoneNumber NVarChar(30),
	@contactEmail NVarChar(100),
	@memberID Int
AS
BEGIN
	INSERT INTO ContactInfo
	(
		ContactFirstName,
		ContactLastName,
		ContactPhoneNumber,
		ContactEmail,
		MemberID
	)
	VALUES
	(
		@contactFirstName,
		@contactLastName,
		@contactPhoneNumber,
		@contactEmail,
		@memberID
	);
END;

--insert into practice
GO
CREATE PROC sp_InsertIntoPractice
@practiceName NVarChar(100),
@startTIme DateTime2,
@endTime DateTime2
AS
BEGIN
    INSERT INTO Practice
    (
        PracticeName,
        StartTIme,
        EndTime
    )
    VALUES
    (
        @practiceName,
        @startTime,
        @endTime
    );
END;

--insert into event
GO
CREATE PROC sp_InsertIntoEvent
@eventName NVarChar(100),
@description NVarChar(250),
@price FLoat,
@ageGroup NVarChar(20),
@time DateTime2
AS
BEGIN
    INSERT INTO Event
    (
        EventName,
	    Description,
	    Price,
	    AgeGroup,
	    Time
    )
    VALUES
    (
        @eventName,
        @description,
        @price,
        @ageGroup,
        @time
    );
END;


GO
--Get all for practice med tilhørende medlemmer og trænere
CREATE PROCEDURE sp_GetAllPracticesWithMembersAndTrainers
AS
BEGIN
    -- 1️ Alle practices (én række pr. practice)
    SELECT
        PracticeID,
        PracticeName,
        StartTime,
        EndTime
    FROM Practice;
    --ORDER BY StartTime ASC;

    -- 2️ Alle members koblet til practices
    SELECT
        mp.PracticeID,
        m.MemberID,
        m.MemberFirstName,
        m.MemberLastName
    FROM Member_Practice mp
    INNER JOIN Member m
        ON mp.MemberID = m.MemberID;
    --ORDER BY m.MemberLastName;

    -- 3️ Alle trainers koblet til practices
    SELECT
        tp.PracticeID,
        t.TrainerID,
        t.TrainerFirstName,
        t.TrainerLastName,
        t.TrainerPhoneNumber,
        t.TrainerEmail
    FROM Trainer_Practice tp
    INNER JOIN Trainer t
        ON tp.TrainerID = t.TrainerID
END;


GO
--GetAll for events med medlemmer og trænere
CREATE PROCEDURE sp_GetAllEventsWithMembersAndTrainers
AS
BEGIN
    -- 1️ Alle events (én række pr. event)
    SELECT
        EventID,
        EventName,
        Description,
        Price,
        AgeGroup,
        Time
    FROM Event
    ORDER BY Time ASC;

    -- 2️ Alle members koblet til events
    SELECT
        me.EventID,
        m.MemberID,
        m.MemberFirstName,
        m.MemberLastName
    FROM Member_Event me
    INNER JOIN Member m
        ON me.MemberID = m.MemberID
    ORDER BY m.MemberLastName;

    -- 3️ Alle trainers koblet til events
    SELECT
        te.EventID,
        t.TrainerID,
        t.TrainerFirstName,
        t.TrainerLastName,
        t.TrainerPhoneNumber,
        t.TrainerEmail
    FROM Trainer_Event te
    INNER JOIN Trainer t
        ON te.TrainerID = t.TrainerID
    ORDER BY t.TrainerLastName;
END;


GO
--GetAll til medlemmer med deres kontakter
CREATE PROCEDURE sp_GetAllMembers
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        m.MemberID,
        m.MemberFirstName,
        m.MemberLastName,
        c.ContactPersonID,
        c.ContactFirstName,
        c.ContactLastName,
        c.ContactPhoneNumber,
        c.ContactEmail
    FROM Member m
    LEFT JOIN ContactInfo c
        ON m.MemberID = c.MemberID;
END;

GO
--Update medlem
CREATE PROC sp_UpdateMember
@memberID Int,
@memberFirstName NVARCHAR(50) = NULL,
@memberLastName NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (
        SELECT 1 FROM Member WHERE MemberID = @MemberID
    )
    BEGIN
        RAISERROR('Member does not exist.', 16, 1);
        RETURN;
    END;

    --Ved ikke om det her skaber problemer. 
    --hvis Update giver 2 tekstbokse til at ændre navne og man efterlader den ene tom, fordi man ikke vil ændre den, får man så fejlbesked?
    IF @MemberFirstName IS NOT NULL AND LTRIM(RTRIM(@MemberFirstName)) = ''
    BEGIN
        RAISERROR('MemberFirstName cannot be empty.', 16, 1);
        RETURN;
    END;

    IF @MemberLastName IS NOT NULL AND LTRIM(RTRIM(@MemberLastName)) = ''
    BEGIN
        RAISERROR('MemberLastName cannot be empty.', 16, 1);
        RETURN;
    END;


    UPDATE Member
    SET
        MemberFirstName = COALESCE(@memberFirstName, MemberFirstName),
        MemberLastName = COALESCE(@memberLastName, MemberLastName)
    WHERE MemberID = @memberID;
END;

GO
--Update Event
CREATE PROC sp_UpdateEvent
@eventID INT,
@eventName NVarChar(100) = NULL,
@description NVarChar(250) = NULL,
@price FLoat = NULL,
@ageGroup NVarChar(20) = NULL,
@time DateTime2 = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (
        SELECT 1 FROM Event WHERE EventID = @eventID
    )
    BEGIN
        RAISERROR('Event does not exist.', 16, 1);
        RETURN;
    END;
    --EVENT NAME MÅ IKKE VÆRE TOM
    IF @eventName IS NOT NULL AND LTRIM(RTRIM(@eventName)) = ''
    BEGIN
        RAISERROR('Event name cannot be empty.', 16, 1);
        RETURN;
    END;
    --DESCRIPTION MÅ IKKE VÆRE TOM
    IF @description IS NOT NULL AND LTRIM(RTRIM(@description)) = ''
    BEGIN
        RAISERROR('Description cannot be empty.', 16, 1);
        RETURN;
    END;
    --PRICE MÅ IKKE VÆRE NEGATIV    
    IF @price IS NOT NULL AND @price < 0
    BEGIN
        RAISERROR('Price cannot be negative.', 16, 1);
        RETURN;
    END;
    --AGE GROUP MÅ IKKE VÆRE TOM
    IF @ageGroup IS NOT NULL AND LTRIM(RTRIM(@ageGroup)) = ''
    BEGIN
        RAISERROR('Age group cannot be empty.', 16, 1);
        RETURN;
    END;
    --TIME MÅ IKKE VÆRE I FORTIDEN    
    IF @time IS NOT NULL AND @time < SYSDATETIME()
    BEGIN
        RAISERROR('Event time cannot be in the past.', 16, 1);
        RETURN;
    END;

    UPDATE Event
    SET
        EventName = COALESCE(@eventName, EventName),
        Description = COALESCE(@description, Description),
        Price = COALESCE(@price, Price),
        AgeGroup = COALESCE(@ageGroup, AgeGroup),
        Time = COALESCE(@time, Time)
    WHERE EventID = @eventID;
END;

GO
--Update Practice
CREATE PROC sp_UpdatePractice
@practiceID INT,
@practiceName NVarChar(100) = NULL,
@startTime DateTime2 = NULL,
@endTime DateTime2 = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (
        SELECT 1 FROM Practice WHERE PracticeID = @practiceID
    )
    BEGIN
        RAISERROR('Practice does not exist.', 16, 1);
        RETURN;
    END;

    --PRACTICE NAME MÅ IKKE VÆRE TOM
    IF @practiceName IS NOT NULL AND LTRIM(RTRIM(@practiceName)) = ''
    BEGIN
        RAISERROR('Practice name cannot be empty.', 16, 1);
        RETURN;
    END;

    --START TIME MÅ IKKE VÆRE I FORTIDEN    
    IF @startTime IS NOT NULL AND @startTime < SYSDATETIME()
    BEGIN
        RAISERROR('Practice start time cannot be in the past.', 16, 1);
        RETURN;
    END;

    --END TIME MÅ IKKE VÆRE TIDLIGERE END START TIME
    IF @endTime IS NOT NULL AND @endTime < COALESCE(@startTime, (SELECT StartTime FROM Practice WHERE PracticeID = @practiceID))
    BEGIN
        RAISERROR('Practice end time cannot be earlier than practice start time', 16, 1)
        RETURN;
    END;

    UPDATE Practice
    SET
        PracticeName = COALESCE(@practiceName, PracticeName),
        StartTime = COALESCE(@startTime, StartTime),
        EndTime = COALESCE(@endTime, EndTime)
    WHERE PracticeID = @practiceID;
END;

GO
--Update ContactInfo
CREATE PROC sp_UpdateContactInfo
@contactPersonID INT,
@contactFirstName NVarChar(50),
@contactLastName NVarChar(50),
@contactPhoneNumber NVarChar(30),
@contactEmail NVarChar(100)
AS
BEGIN
    SET NOCOUNT ON;
    
    IF NOT EXISTS (
        SELECT 1 FROM ContactInfo WHERE ContactPersonID = @contactPersonID
    )
    BEGIN
        RAISERROR('Contact person does not exist.', 16, 1);
        RETURN;
    END;

    IF @contactFirstName IS NOT NULL AND LTRIM(RTRIM(@contactFirstName)) = ''
    BEGIN
        RAISERROR('Contact persons first name cannot be empty', 16, 1)
        RETURN;
    END;

    IF @contactLastName IS NOT NULL AND LTRIM(RTRIM(@contactLastName)) = ''
    BEGIN
        RAISERROR('Contact persons last name cannot be empty', 16, 1)
        RETURN;
    END;

    IF @contactPhoneNumber IS NOT NULL AND LTRIM(RTRIM(@contactPhoneNumber)) = ''
    BEGIN
        RAISERROR('Contact persons phonenumber cannot be empty', 16, 1)
        RETURN;
    END;

    IF @contactEmail IS NOT NULL AND LTRIM(RTRIM(@contactEmail)) = ''
    BEGIN
        RAISERROR('Contact persons first name cannot be empty', 16, 1)
        RETURN;
    END;

    UPDATE ContactInfo
    SET
        ContactFirstName = COALESCE(@contactFirstName, ContactFirstName),
        ContactLastName = COALESCE(@contactLastName, ContactLastName),
        ContactPhoneNumber = COALESCE(@contactPhoneNumber, ContactPhoneNumber),
        ContactEmail = COALESCE(@contactEmail, ContactEmail)
    WHERE ContactPersonID = @contactPersonID
END;

GO
CREATE PROC sp_CancelSignUpEvent
@eventID INT,
@memberID INT
AS
BEGIN
    SET NOCOUNT ON

    DELETE FROM Member_Event
    WHERE EventID = @eventID AND MemberID = @memberID;
END;

GO
CREATE PROC sp_SignUpEvent
@eventID INT,
@memberID INT
AS
BEGIN
    SET NOCOUNT ON

    IF NOT EXISTS 
    (
        SELECT 1 FROM Member_Event
        WHERE EventID = @eventID AND MemberID = @memberID
    )
    BEGIN
        INSERT INTO Member_Event (EventID, MemberID)
        VALUES(@eventID, @memberID)
    END;
END;

GO
--Fjern medlem fra practice
CREATE PROC sp_CancelParticipation
@practiceID int,
@memberID int
AS
BEGIN
    SET NOCOUNT ON

    DELETE FROM Member_Practice
    WHERE PracticeID = @practiceID AND MemberID = @memberID;
END;

GO
CREATE OR ALTER PROC sp_SignUpPractice
@practiceID INT,
@memberID INT
AS
BEGIN
    SET NOCOUNT ON

    IF NOT EXISTS 
    (
        SELECT 1 FROM Member_Practice
        WHERE PracticeID = @practiceID AND MemberID = @memberID
    )
    BEGIN
        INSERT INTO Member_Practice(PracticeID, MemberID)
        VALUES(@practiceID, @memberID)
    END;
END;