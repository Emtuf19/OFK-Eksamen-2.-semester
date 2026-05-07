using _2_Semester_Eksamen.Model;
using System.Linq;

namespace UnitTestAndetSemesterEksamen
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public void Add_ShouldInsertPracticeIntoDatabase_AndShouldGetItFrom_GetAll()
        {
            // Arrange
            PracticeRepository repo = new PracticeRepository();

            Practice practice = new Practice
            {
                PracticeName = "Unit Test Practice",
                StartTime = new DateTime(2026, 5, 7, 18, 0, 0),
                EndTime = new DateTime(2026, 5, 7, 20, 0, 0)
            };

            // Act
            repo.Create(practice);

            // Assert
            List<Practice> practices = repo.GetAll();

            bool exists = practices.Any(p =>
                p.PracticeName == "Unit Test Practice" &&
                p.StartTime == practice.StartTime &&
                p.EndTime == practice.EndTime);

            Assert.IsTrue(exists);
        }

        [TestMethod]
        public void Update_ShouldUpdateExistingPractice()
        {
            // Arange
            PracticeRepository repo = new PracticeRepository();

            // Act
            Practice Practice = new Practice
            {
                PracticeID = 5,
                PracticeName = "UpdatedPractice",
                StartTime = new DateTime(2026, 6, 7, 18, 0, 0),
                EndTime = new DateTime(2026, 10, 8, 20, 0, 0)
            };

            repo.Update(Practice);

            // Assert
            List<Practice> practicesToUpdate = repo.GetAll();

            bool exists = practicesToUpdate.Any(p => 
            p.PracticeName == "UpdatedPractice" && 
            p.StartTime == Practice.StartTime && 
            p.EndTime == Practice.EndTime);

            Assert.IsTrue(exists);
        }

        [TestMethod]
        public void Delete_ExistingPractice()
        {
            // Arange
            PracticeRepository repo = new PracticeRepository();

            Practice Practice = new Practice
            {
                PracticeID = 10,
                PracticeName = "DeletePractice",
                StartTime = new DateTime(2026, 6, 7, 18, 0, 0),
                EndTime = new DateTime(2026, 10, 8, 20, 0, 0)
            };

            repo.Create(Practice);

            Practice insertedPractice = repo.GetAll().LastOrDefault(p => p.PracticeName == "DeletePractice");

            // Act
            repo.Delete(insertedPractice.PracticeID);

            // Assert
            bool exists = repo.GetAll().Any(p => p.PracticeName == insertedPractice.PracticeName);

            Assert.IsFalse(exists);
        }

        [TestMethod]
        public void ADD_ShouldAddMember()
        {
            // Arange
            MemberRepository repo = new MemberRepository();

            Member member = new Member
            {
                MemberFirstName = "FirstNameTest",
                MemberLastName = "LastNameTest",
            };

            // Act
            repo.Add(member);

            // Assert
            List<Member> members = repo.GetAll();

            bool exists = repo.GetAll().Any(m => 
            m.MemberFirstName == "FirstNameTest" && 
            m.MemberLastName == "LastNameTest");

            Assert.IsTrue(exists);

        }

    }
}
