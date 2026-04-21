Use OdenseFægteKlub;
GO

--Stored Procedure til INSERT INTO Member
CREATE PROC sp_InsertIntoMember
@memberFirstName NVarChar(50),
@memberLastName NVarChar(50),
@newMemberID INT OUTPUT
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
	SET @newMemberID = SCOPE_IDENTITY();
END;

--Stored Procedure til INSERT INTO ContactInfo
GO
CREATE PROC sp_InsertIntoContactInfo
	@contactFirstName NVarChar(50),
	@contactLastName NVarChar(50),
	@contactPhoneNumber VarChar(8),
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

--Insert into member with contact person
GO
CREATE PROC sp_InsertIntoMemberWithContacts
    -- Member info
    @memberFirstName NVARCHAR(50),
    @memberLastName  NVARCHAR(50),

    -- Contact person 1 (påkrævet)
    @contact1FirstName NVARCHAR(50),
    @contact1LastName  NVARCHAR(50),
    @contact1Phone     VARCHAR(8),
    @contact1Email     NVARCHAR(100),

    -- Contact person 2 (valgfri - send NULL hvis ingen)
    @contact2FirstName NVARCHAR(50) = NULL,
    @contact2LastName  NVARCHAR(50) = NULL,
    @contact2Phone     VARCHAR(8)   = NULL,
    @contact2Email     NVARCHAR(100)= NULL,

    -- OUTPUT IDs
    @newMemberID INT OUTPUT,
    @newContact1ID INT OUTPUT,
    @newContact2ID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;
        -- 1. Insert Member
        INSERT INTO Member (MemberFirstName, MemberLastName)
        VALUES (@memberFirstName, @memberLastName);

        SET @newMemberID = SCOPE_IDENTITY();

        -- 2. Insert Contact Person #1 (påkrævet)
        INSERT INTO ContactInfo
        (
            ContactFirstName, ContactLastName,
            ContactPhoneNumber, ContactEmail,
            MemberID
        )
        VALUES
        (
            @contact1FirstName, @contact1LastName,
            @contact1Phone, @contact1Email,
            @newMemberID
        );

        SET @newContact1ID = SCOPE_IDENTITY();

        -- 3. Insert Contact Person #2 (valgfri)
        IF @contact2FirstName IS NOT NULL AND @contact2LastName IS NOT NULL
        BEGIN
            INSERT INTO ContactInfo
            (
                ContactFirstName, ContactLastName,
                ContactPhoneNumber, ContactEmail,
                MemberID
            )
            VALUES
            (
                @contact2FirstName, @contact2LastName,
                @contact2Phone, @contact2Email,
                @newMemberID
            );

            SET @newContact2ID = SCOPE_IDENTITY();
        END
        ELSE
        BEGIN
            SET @newContact2ID = NULL;
        END

        -- SUCCESS
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        
        -----------------------------------------------------------
        -- FANG TRIGGER-FEJL HER
        -----------------------------------------------------------
        DECLARE @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrSeverity INT = ERROR_SEVERITY();

        -- Hvis fejlen kommet fra triggeren
        IF @ErrMsg LIKE '%maximum of 2 contact persons%'
        BEGIN
            RAISERROR('Det er kun tilladt at tilføje maks 2 kontaktpersoner til et medlem.', 16, 1);
            RETURN;
        END

        -----------------------------------------------------------
        -- Hvis det er en anden fejl, kast den videre
        -----------------------------------------------------------
        RAISERROR(@ErrMsg, @ErrSeverity, 1);

        --THROW;
    END CATCH
END;

GO
--Stored Procedure til INSERT INTO Trainer
CREATE PROC sp_InsertIntoTrainer
@trainerFirstName NVarChar(50),
@trainerLastName NVarChar(50),
@trainerPhoneNumber VarChar(8),
@trainerEmail NVarChar(100),
@newTrainerID INT OUTPUT
AS
BEGIN
	INSERT INTO Trainer 
	(
		TrainerFirstName,
		TrainerLastName,
        TrainerPhoneNumber,
        TrainerEmail
	)
	VALUES	
	(
		@trainerFirstName,
		@trainerLastName,
        @trainerPhoneNumber,
        @trainerEmail
	);
	SET @newTrainerID = SCOPE_IDENTITY();
END;

--insert into practice
GO
CREATE PROC sp_InsertIntoPractice
@practiceName NVarChar(100),
@startTIme DateTime2,
@endTime DateTime2,
@newPracticeID INT OUTPUT
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
    SET @newPracticeID = SCOPE_IDENTITY();
END;

--insert into event
GO
CREATE PROC sp_InsertIntoEvent
@eventName NVarChar(100),
@description NVarChar(250),
@price FLoat,
@ageGroup NVarChar(20),
@time DateTime2,
@newEventID INT OUTPUT
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
    SET @newEventID = SCOPE_IDENTITY();
END;

--insert into event with member and/or trainer
GO
CREATE PROC sp_InsertMemberAndOrTrainerToEvent
@memberID Int = NULL,
@trainerID Int = NULL,
@eventID Int
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        IF @memberID IS NOT NULL
        BEGIN 
            INSERT INTO Member_Event
            (
                MemberID,
                EventID
            )
            VALUES
            (
                @memberID,
                @eventID
            );
        END

        IF @trainerID IS NOT NULL
        BEGIN
            INSERT INTO Trainer_Event
            (
                TrainerID,
                EventID
            )
            VALUES
            (
                @trainerID,
                @eventID
            );
        END
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;

--insert into practice with member and/or trainer
GO
CREATE PROC sp_InsertMemberAndOrTrainerToPractice
@memberID Int = NULL,
@trainerID Int = NULL,
@practiceID Int
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        IF @memberID IS NOT NULL
        BEGIN 
            INSERT INTO Member_Practice
            (
                MemberID,
                PracticeID
            )
            VALUES
            (
                @memberID,
                @practiceID
            );
        END

        IF @trainerID IS NOT NULL
        BEGIN
            INSERT INTO Trainer_Practice
            (
                TrainerID,
                PracticeID
            )
            VALUES
            (
                @trainerID,
                @practiceID
            );
        END
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;

GO
--GetByID generisk. (Copilot er ikke glad for at lave én fælles GetByID)
CREATE PROCEDURE sp_GetById_Generic
    @TableName SYSNAME,
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @IdColumn SYSNAME;
    DECLARE @Sql NVARCHAR(MAX);

    --------------------------------------------------
    -- 1. Whitelist + ID-kolonne mapping
    --------------------------------------------------
    IF @TableName = 'Member'
        SET @IdColumn = 'MemberID';
    ELSE IF @TableName = 'Practice'
        SET @IdColumn = 'PracticeID';
    ELSE IF @TableName = 'Event'
        SET @IdColumn = 'EventID';
    ELSE
    BEGIN
        RAISERROR ('Ugyldig tabel', 16, 1);
        RETURN;
    END

    --------------------------------------------------
    -- 2. Dynamisk SQL (sikkert)
    --------------------------------------------------
    SET @Sql = N'
        SELECT *
        FROM ' + QUOTENAME(@TableName) + '
        WHERE ' + QUOTENAME(@IdColumn) + ' = @Id';

    --------------------------------------------------
    -- 3. Eksekver med parameter (anti-SQL-injection)
    --------------------------------------------------
    EXEC sp_executesql
        @Sql,
        N'@Id INT',
        @Id = @Id;
END;

GO
--GetByID Member med tilhørende kontaktpersoner
CREATE PROCEDURE sp_GetMemberByID
    @MemberID INT
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
        ON m.MemberID = c.MemberID
    WHERE m.MemberID = @MemberID;
END;

GO
--GetById til trainer
CREATE PROC sp_GetByTrainerID
@trainerID INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS ( SELECT 1 FROM Trainer WHERE TrainerID = @trainerID)
    BEGIN
        RAISERROR('Trainer does not exist.', 16, 1);
        RETURN;
    END;

    SELECT
        TrainerID,
        TrainerFirstName,
        TrainerLastName,
        TrainerPhoneNumber,
        TrainerEmail
    FROM Trainer
    WHERE TrainerID = @trainerID;
END;


GO
--GetByID for event med liste af medlemmer og trænere
CREATE PROCEDURE sp_GetEventByIDWithMembersAndTrainers
    @EventID INT
AS
BEGIN

    IF NOT EXISTS (
        SELECT 1 FROM Event WHERE EventID = @EventID
    )
    BEGIN
        RAISERROR('Event does not exist.', 16, 1);
        RETURN;
    END;

    -- 1️ Event-info (én række)
    SELECT
        EventID,
        EventName,
        Description,
        Price,
        AgeGroup,
        Time
    FROM Event
    WHERE EventID = @EventID;

    -- 2️ Medlemmer til event
    SELECT
        m.MemberID,
        m.MemberFirstName,
        m.MemberLastName
    FROM Member_Event me
    INNER JOIN Member m
        ON me.MemberID = m.MemberID
    WHERE me.EventID = @EventID;

    -- 3️ Trænere til event
    SELECT
        t.TrainerID,
        t.TrainerFirstName,
        t.TrainerLastName,
        t.TrainerPhoneNumber,
        t.TrainerEmail
    FROM Trainer_Event te
    INNER JOIN Trainer t
        ON te.TrainerID = t.TrainerID
    WHERE te.EventID = @EventID;
END;


GO
--GetByID for practice med liste af medlemmer og liste af trænere
CREATE PROCEDURE sp_GetPracticeByIDWithMembersAndTrainers
    @PracticeID INT
AS
BEGIN

    IF NOT EXISTS (
        SELECT 1 FROM Practice WHERE PracticeID = @PracticeID
    )
    BEGIN
        RAISERROR('Practice does not exist.', 16, 1);
        RETURN;
    END;

    -- 1️ Practice-info (én række)
    SELECT
        PracticeID,
        PracticeName,
        StartTime,
        EndTime
    FROM Practice
    WHERE PracticeID = @PracticeID;

    -- 2️ Medlemmer til practice
    SELECT
        m.MemberID,
        m.MemberFirstName,
        m.MemberLastName
    FROM Member_Practice mp
    INNER JOIN Member m
        ON mp.MemberID = m.MemberID
    WHERE mp.PracticeID = @PracticeID;

    -- 3️ Trænere til practice
    SELECT
        t.TrainerID,
        t.TrainerFirstName,
        t.TrainerLastName,
        t.TrainerPhoneNumber,
        t.TrainerEmail
    FROM Trainer_Practice tp
    INNER JOIN Trainer t
        ON tp.TrainerID = t.TrainerID
    WHERE tp.PracticeID = @PracticeID;
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

    -- 2️ Alle members koblet til practices
    SELECT
        mp.PracticeID,
        m.MemberID,
        m.MemberFirstName,
        m.MemberLastName
    FROM Member_Practice mp
    INNER JOIN Member m
        ON mp.MemberID = m.MemberID;

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
        ON tp.TrainerID = t.TrainerID;
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
    FROM Event;

    -- 2️ Alle members koblet til events
    SELECT
        me.EventID,
        m.MemberID,
        m.MemberFirstName,
        m.MemberLastName
    FROM Member_Event me
    INNER JOIN Member m
        ON me.MemberID = m.MemberID;

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
        ON te.TrainerID = t.TrainerID;
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
--GetAll til trænere
CREATE PROC sp_GetAllTrainers
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        TrainerID,
        TrainerFirstName,
        TrainerLastName,
        TrainerPhoneNumber,
        TrainerEmail
    FROM Trainer;
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
    IF @endTime IS NOT NULL AND @endTime < COALESCE(@startTime, StartTime)
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
--Update Trainer
CREATE PROC sp_UpdateTrainer
@trainerID INT,
@trainerFirstName NVARCHAR(50) = NULL,
@trainerLastName NVARCHAR(50) = NULL,
@trainerPhoneNumber VARCHAR(8) = NULL,
@trainerEmail NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (
        SELECT 1 FROM Trainer WHERE TrainerID = @trainerID
    )
    BEGIN
        RAISERROR('Trainer does not exist.', 16, 1);
        RETURN;
    END;

    IF @trainerFirstName IS NOT NULL AND LTRIM(RTRIM(@trainerFirstName)) = ''
    BEGIN
        RAISERROR('Trainer first name cannot be empty', 16, 1)
        RETURN;
    END;

    IF @trainerLastName IS NOT NULL AND LTRIM(RTRIM(@trainerLastName)) = ''
    BEGIN
        RAISERROR('Trainer last name cannot be empty', 16, 1)
        RETURN;
    END;

    IF @trainerPhoneNumber IS NOT NULL AND LTRIM(RTRIM(@trainerPhoneNumber)) = ''
    BEGIN
        RAISERROR('Trainer phonenumber cannot be empty', 16, 1)
        RETURN;
    END;

    IF @trainerEmail IS NOT NULL AND LTRIM(RTRIM(@trainerEmail)) = ''
    BEGIN
        RAISERROR('Trainer email cannot be empty', 16, 1)
        RETURN;
    END;

    UPDATE Trainer
    SET
        TrainerFirstName = COALESCE(@trainerFirstName, TrainerFirstName),
        TrainerLastName = COALESCE(@trainerLastName, TrainerLastName),
        TrainerPhoneNumber = COALESCE(@trainerPhoneNumber, TrainerPhoneNumber),
        TrainerEmail = COALESCE(@trainerEmail, TrainerEmail)
    WHERE TrainerID = @trainerID
END;

GO
--Update ContactInfo
CREATE PROC sp_UpdateContactInfo
@contactPersonID INT,
@contactFirstName NVarChar(50),
@contactLastName NVarChar(50),
@contactPhoneNumber VarChar(8),
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