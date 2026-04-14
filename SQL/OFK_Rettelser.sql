USE OdenseFægteKlub
GO

/* <-- Fjern udkommentering !!
--Ændre kolonne navnet MemberName i Member tabellen til MemberFirstName
sp_rename 'Member.MemberName', 'MemberFirstName', 'COLUMN';

--Tilføjer kolonne med navnet MemberLastName i Member tabellen
ALTER TABLE Member
ADD MemberLastName NVarChar(50);
GO

--Ændre kolonne navnet ContactName i ContactInfo tabellen til ContactFirstName
sp_rename 'ContactInfo.ContactName', 'ContactFirstName', 'COLUMN';

--Tilføjer kolonne med navnet ContactLastName
ALTER TABLE ContactInfo
ADD ContactLastName NVarChar(50);
GO

--Ændre kolonne navnet TrainerName i TrainerFirstName tabellen til ContactFirstName
sp_rename 'Trainer.TrainerName', 'TrainerFirstName', 'COLUMN';

--Tilføjer kolonne med navn TrainerLastName
ALTER TABLE Trainer
ADD TrainerLastName NVarChar(50);


--Tilføjer et constraint, så medlem ikke kan have mere end 2 kontakpersoner (Virkede ikke)
--Msg 1046, Level 15, State 1, Line 34. Subqueries are not allowed in this context. Only scalar expressions are allowed.
/*ALTER TABLE ContactInfo
ADD CONSTRAINT CK_ContactInfo_MaxTwoContacts
CHECK (
    (
        SELECT COUNT(*) 
        FROM ContactInfo c 
        WHERE c.MemberID = ContactInfo.MemberID
    ) <= 2
);*/

--Tilføjer en trigger, så medlem ikke kan have mere end 2 kontakpersoner
CREATE TRIGGER trg_MaxTwoContacts
ON ContactInfo
AFTER INSERT, UPDATE
AS
BEGIN
    IF EXISTS (
        SELECT MemberID
        FROM ContactInfo
        GROUP BY MemberID
        HAVING COUNT(*) > 2
    )
    BEGIN
        RAISERROR('A member can have a maximum of 2 contact persons.', 16, 1);
        ROLLBACK TRANSACTION;
    END
END;
*/--<-- Fjern udkommentering !!

ALTER TABLE Member_Event
ADD CONSTRAINT uq_MemberEvent UNIQUE (MemberID, EventID);

ALTER TABLE Trainer_Event
ADD CONSTRAINT uq_TrainerEvent UNIQUE (TrainerID, EventID);

ALTER TABLE Member_Practice
ADD CONSTRAINT uq_MemberPractice UNIQUE (MemberID, PracticeID);

ALTER TABLE Trainer_Practice
ADD CONSTRAINT uq_TrainerPractice UNIQUE (TrainerID, PracticeID);