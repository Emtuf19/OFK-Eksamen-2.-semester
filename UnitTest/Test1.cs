using Anden_Semester_Eksamen;
using Anden_Semester_Eksamen.MemberRepository;

namespace UnitTest
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public void TestMethod1()
        {
            void TestGetMemberById(int id)
            {
                try
                {
                    var repo = new MemberRepository();
                    var member = repo.GetById(id);

                    if (member == null)
                    {
                        Console.WriteLine("Member not found (reader returned no rows).");
                        return;
                    }

                    Console.WriteLine($"Member: {member.MemberID} - {member.FirstName} {member.LastName}");
                    foreach (var c in member.ContactInfos)
                    {
                        Console.WriteLine($"Contact: {c.FirstName} {c.LastName} | {c.ContactPhoneNumber} | {c.ContactEmail}");
                    }
                }
                catch (SqlException ex)
                {
                    // SP raises error when member doesn't exist — you'll see it here
                    Console.WriteLine("SQL error: " + ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

            // Call it:
            TestGetMemberById(1);

        }
    }
}
