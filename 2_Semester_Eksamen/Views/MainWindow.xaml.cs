using _2_Semester_Eksamen.Model;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _2_Semester_Eksamen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            ShowsNavigationUI = false;
            // Quick test: call MemberRepository.GetAll() and show results
            //try
            //{
            //    var repository = new MemberRepository();
            //    var allMembers = repository.GetAll();

            //    var sb = new StringBuilder();
            //    sb.AppendLine($"Members returned: {allMembers.Count}");
            //    foreach (var m in allMembers)
            //    {
            //        sb.AppendLine($"{m.MemberID} - {m.FirstName} {m.LastName}");
            //        foreach (var c in m.ContactPersons)
            //        {
            //            sb.AppendLine($"  Contact: {c.ContactPersonID} - {c.ContactFirstName} {c.ContactLastName} - {c.ContactPhoneNumber} - {c.ContactEmail}");
            //        }
            //    }

            //    MessageBox.Show(sb.ToString(), "GetAll() test");
            //}
            //catch (System.Exception ex)
            //{
            //    MessageBox.Show($"Error while calling GetAll(): {ex.Message}", "Error");
            //}



            //// Quick test: call and GetById() and show results
            //try
            //{
            //    var repository = new MemberRepository();

            //    // Test GetById() - change the ID to one that exists in your dummy data
            //    int testId = 3;
            //    var member = repository.GetById(testId);
            //    if (member == null)
            //    {
            //        MessageBox.Show($"GetById({testId}) returned null (no member with that ID).", "GetById() test");
            //    }
            //    else
            //    {
            //        var sbOne = new StringBuilder();
            //        sbOne.AppendLine($"Member {member.MemberID}: {member.FirstName} {member.LastName}");
            //        sbOne.AppendLine($"Contacts: {member.ContactPersons.Count}");
            //        foreach (var c in member.ContactPersons)
            //        {
            //            sbOne.AppendLine($"  Contact: {c.ContactFirstName} {c.ContactLastName} - {c.ContactPhoneNumber} - {c.ContactEmail}");
            //        }
            //        MessageBox.Show(sbOne.ToString(), "GetById() test");
            //    }
            //}
            //catch (System.Exception ex)
            //{
            //    MessageBox.Show($"Error while calling repository methods: {ex.Message}", "Error");
            //}
            // Quick test: call PracticeRepository.GetAll() and show results

            ////PracticeRepo quick test : call PracticeRepository.GetAll() and show results, including members and trainers
            //try
            //{
            //    var repository = new PracticeRepository();
            //    var practices = repository.GetAll();

            //    var sb = new StringBuilder();
            //    sb.AppendLine($"Practices returned: {practices?.Count ?? 0}");
            //    foreach (var p in practices ?? new System.Collections.Generic.List<Practice>())
            //    {
            //        sb.AppendLine($"{p.PracticeID} - {p.PracticeName}  |  {p.StartTime} - {p.EndTime}");

            //        if (p.Members == null || p.Members.Count == 0)
            //        {
            //            sb.AppendLine("  Members: (none)");
            //        }
            //        else
            //        {
            //            sb.AppendLine($"  Members ({p.Members.Count}):");
            //            foreach (var m in p.Members)
            //            {
            //                // Member properties in your model are MemberFirstName / MemberLastName
            //                sb.AppendLine($"    {m.MemberID} - {m.MemberFirstName} {m.MemberLastName}");
            //            }
            //        }

            //        if (p.Trainers == null || p.Trainers.Count == 0)
            //        {
            //            sb.AppendLine("  Trainers: (none)");
            //        }
            //        else
            //        {
            //            sb.AppendLine($"  Trainers ({p.Trainers.Count}):");
            //            foreach (var t in p.Trainers)
            //            {
            //                sb.AppendLine($"    {t.TrainerID} - {t.TrainerFirstName} {t.TrainerLastName}");
            //            }
            //        }

            //        sb.AppendLine(); // blank line between practices
            //    }

            //    MessageBox.Show(sb.ToString(), "sp_GetAllPracticesWithMembersAndTrainers - GetAll() result");
            //}
            //catch (System.Exception ex)
            //{
            //    // Show full exception to help debugging (column mismatch, connection, etc.)
            //    MessageBox.Show($"Error while calling PracticeRepository.GetAll(): {ex.Message}\n\n{ex}", "Error");
            //}

            // EventRepo quick test : call EventRepository.GetAll() and show results, including members and trainers
            //try
            //{
            //    var repository = new EventRepository();
            //    var events = repository.GetAll();

            //    var sb = new StringBuilder();
            //    sb.AppendLine($"Events returned: {events?.Count ?? 0}");
            //    foreach (var e in events ?? new System.Collections.Generic.List<Event>())
            //    {
            //        sb.AppendLine($"{e.EventID} - {e.EventName} - {e.Description} - {e.Price} - {e.AgeGroup} - {e.Time}");

            //        if (e.Members == null || e.Members.Count == 0)
            //        {
            //            sb.AppendLine("  Members: (none)");
            //        }
            //        else
            //        {
            //            sb.AppendLine($"  Members ({e.Members.Count}):");
            //            foreach (var m in e.Members)
            //            {
            //                // Member properties in your model are MemberFirstName / MemberLastName
            //                sb.AppendLine($"    {m.MemberID} - {m.MemberFirstName} {m.MemberLastName}");
            //            }
            //        }

            //        if (e.Trainers == null || e.Trainers.Count == 0)
            //        {
            //            sb.AppendLine("  Trainers: (none)");
            //        }
            //        else
            //        {
            //            sb.AppendLine($"  Trainers ({e.Trainers.Count}):");
            //            foreach (var t in e.Trainers)
            //            {
            //                sb.AppendLine($"    {t.TrainerID} - {t.TrainerFirstName} {t.TrainerLastName}");
            //            }
            //        }

            //        sb.AppendLine(); // blank line between practices
            //    }

            //    MessageBox.Show(sb.ToString(), "sp_GetAllPracticesWithMembersAndTrainers - GetAll() result");
            //}
            //catch (System.Exception ex)
            //{
            //    // Show full exception to help debugging (column mismatch, connection, etc.)
            //    MessageBox.Show($"Error while calling PracticeRepository.GetAll(): {ex.Message}\n\n{ex}", "Error");
            //}


            //// Quick test for MemberRepository.Update
            //try
            //{
            //    var repo = new MemberRepository();

            //    //change id to one that exists.
            //    int testId = 1; 
            //    var before = repo.GetById(testId);

            //    if (before == null)
            //    {
            //        MessageBox.Show($"GetById({testId}) returned null - no member with that ID.", "Update test");
            //    }
            //    else
            //    {
            //        var sb = new StringBuilder();
            //        sb.AppendLine("Before update:");
            //        sb.AppendLine($"{before.MemberID} - {before.MemberFirstName} {before.MemberLastName}");

            //        before.MemberFirstName = before.MemberFirstName + "_Edited";
            //        before.MemberLastName = before.MemberLastName + "_Edited";

            //        // Call update
            //        repo.Update(before);

            //        // Re-read from DB
            //        var after = repo.GetById(testId);
            //        sb.AppendLine("");
            //        sb.AppendLine("After update:");
            //        if (after == null)
            //            sb.AppendLine("Member disappeared after update (null).");
            //        else
            //            sb.AppendLine($"{after.MemberID} - {after.MemberFirstName} {after.MemberLastName}");

            //        MessageBox.Show(sb.ToString(), "MemberRepository.Update() test");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Error testing Update: {ex.Message}\n\n{ex}", "Error");
            //}
        }
    }

}
