using System ;
using System.Collections.Generic ;
using System.Linq;

namespace StudentPerformanceAnalyzer;
// Let's first create a contract to define the essential capabilities.

public interface IRecordManagement
    {
        void AddStudentRecord(string studentName, string subject, int score); // a function for adding student's details
        SortedDictionary < string ,  List<StudentRecord>>  GroupRecordsBySubject() ;
        Dictionary < string, double> GetAverageScorePerSubject() ;
        StudentRecord GetTopScorer();
        List<StudentRecord> FilterStudentsByScore(int minScore);
    }

// Now let's create the class

public class StudentRecord
    {
        // Encapsulation: with the help of  auto-properties 
        public string StudentName { get; set; }
        public string Subject { get; set; }
        public int Score { get; set; }

        public StudentRecord(string name, string subject, int score)
        {
            StudentName = name;
            Subject = subject;
            Score = score;
        }

        public override string ToString() =>  $"{StudentName} - {Score}";
    }

public abstract class AbstractRecordHandler
    {
 
        protected static SortedDictionary<int, StudentRecord> StudentRecords = new SortedDictionary<int, StudentRecord>();


        protected int GenerateNextId()
        {
            return StudentRecords.Count + 1;
        }
    }

public class RecordUtility : AbstractRecordHandler, IRecordManagement
    {
        // Let me create a method to Add a new student record
        public void AddStudentRecord(string studentName, string subject, int score)
        {
            int newKey = GenerateNextId();
            StudentRecords.Add(newKey, new StudentRecord(studentName, subject, score));
        }

        // Creating a Method  For Grouping logic ( Returns Subject -> List of Students)
        public SortedDictionary<string, List<StudentRecord>> GroupRecordsBySubject()
        {
            var grouped = StudentRecords.Values
                .GroupBy(r => r.Subject)
                .ToDictionary(g => g.Key, g => g.ToList());

            return new SortedDictionary<string, List<StudentRecord>>(grouped);
        }

        // This Method is for Averages
        public Dictionary<string, double> GetAverageScorePerSubject()
        {
            return StudentRecords.Values
                .GroupBy(r => r.Subject)
                .ToDictionary(g => g.Key, g => g.Average(r => r.Score));
        }

        // Now this method will deal with the fiding of Top Scorer
        public StudentRecord GetTopScorer()
        {
            if (StudentRecords.Count == 0) return null;
            return StudentRecords.Values.OrderByDescending(r => r.Score).FirstOrDefault();
        }

        // Here I will LINQ Filtering
        public List<StudentRecord> FilterStudentsByScore(int minScore)
        {
            return StudentRecords.Values
                .Where(r => r.Score >= minScore)
                .ToList();
        }
    }
