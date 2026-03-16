// See https://aka.ms/new-console-template for more information

using System;
using StudentPerformanceAnalyzer ;
/*
🧩 Scenario: Student Exam Performance Analyzer
 
👤 Background:
John has taken up a freelance project for a coaching institute. The institute wants an application to manage 
student exam records. 
Each record includes the student's name, subject, and score. The application should allow adding records, 
grouping students by subject, and analyzing performance using LINQ.
Help John build this application using your C# skills.
🛠️ Functionalities
 In class Program:
C#
public static SortedDictionary<int, StudentRecord> studentRecords;

 
Show more lines
 
This sorted dictionary is already provided. It stores student exam records with an auto-incremented key.
 In class StudentRecord, implement the following properties:
Data Type	Property Name
string	StudentName
string	Subject
int	Score
 In class RecordUtility, implement the following methods:
Method 1:
 
C#
public void AddStudentRecord(string studentName, string subject, int score)

 
Show more lines
 
Adds a new student record to the studentRecords dictionary.
The key is set to one more than the current number of items in the dictionary.
Method 2:
 
C#
public SortedDictionary<string, List<StudentRecord>> GroupRecordsBySubject()

 
Show more lines
 
Groups all student records by their subject.
Returns a dictionary where:
Key = Subject name
Value = List of student records under that subject
Method 3:
 
C#
public Dictionary<string, double> GetAverageScorePerSubject()

 
Show more lines
 
Uses LINQ to calculate the average score for each subject.
Method 4:
 
C#
public StudentRecord GetTopScorer()

 
Show more lines
 
Uses LINQ to find the student with the highest score across all subjects.
Method 5:
 
C#
public List<StudentRecord> FilterStudentsByScore(int minScore)

 
Show more lines
 
Returns a list of students whose score is greater than or equal to minScore.
🧪 Sample Input/Output
1. Add Student Record
2. Group Records By Subject
3. Show Average Score Per Subject
4. Show Top Scorer
5. Filter Students By Minimum Score
6. Exit
Enter your choice
1
Enter student name
Alice
Enter subject
Math
Enter score
85
Student record added successfully
Enter your choice
1
Enter student name
Bob
Enter subject
Science
Enter score
90
Student record added successfully
Enter your choice
1
Enter student name
Charlie
Enter subject
Math
Enter score
78
Student record added successfully
Enter your choice
2
Math
Alice - 85
Charlie - 78
Science
Bob - 90
Enter your choice
3
Average Scores:
Math - 81.5
Science - 90.0
Enter your choice
4
Top Scorer:
Bob - 90
Enter your choice
5
Enter minimum score
80
Students with score >= 80:
Alice - 85
Bob - 90
Enter your choice
6
Thank you
*/


// created using dotnet new console -n Playground
// run using dotnet run console

class Program
{
    public static void Main(string[] args)
        {
            // Use Interface for Polmorphism (Decoupling)
            IRecordManagement utility = new RecordUtility();
            bool running = true;

            while (running)
            {
                Console.WriteLine("1. Add Student Record \n2. Group Records By Subject \n3. Show Average Score Per Subject \n4. Show Top Scorer \n5. Filter Students \n6. Exit");
                Console.Write("Enter your choice: ");
                
                if (!int.TryParse(Console.ReadLine(), out int choice)) continue;

                switch (choice)
                {
                    case 1:
                        Console.Write ("Enter student name: ");
                        string name = Console.ReadLine() ?? "Unknown" ;
                        Console.Write("Enter subject: ");
                        string sub = Console.ReadLine() ?? "General";
                        Console.Write("Enter score: ");
                        int.TryParse(Console.ReadLine(), out int score);
                        
                        utility.AddStudentRecord(name, sub, score);
                        Console.WriteLine( " Student record added successfully.");
                        break;

                    case 2:
                        var groups = utility.GroupRecordsBySubject();
                        foreach (var group in groups)
                        {
                            Console.WriteLine(group.Key);
                            group.Value.ForEach (s => Console.WriteLine ( $"  {s}"));
                        }
                        break;

                    case 3:
                        var averages = utility.GetAverageScorePerSubject();
                        Console.WriteLine("Average Scores :");
                        foreach ( var avg in averages ) Console.WriteLine ( $"{avg.Key} - {avg.Value:F1}");
                        break;

                    case 4:
                        var top = utility.GetTopScorer();
                        Console.WriteLine (top != null ? $"Top Scorer is : {top}" : " Unfortunately , No records found.");
                        break;

                    case 5:
                        Console.Write ("Enter minimum score that should be the lowest acceptible value of score : ");
                        int.TryParse(Console.ReadLine(), out int min);
                        var filtered = utility.FilterStudentsByScore(min);
                        Console.WriteLine ( $"Students with score >= {min} : " ) ;
                        filtered.ForEach(s => Console.WriteLine(s));
                        break;

                    case 6:
                        running = false;
                        Console.WriteLine("Exiting...");
                        break;
                }
            }
        }
}

