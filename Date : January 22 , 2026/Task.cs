namespace Task ;
using System;
using System.Text;

    public class DemoStringAndStringBuilder
    {
        public void Run()
        {
            Console.WriteLine(" Enter the word  : "); // for input of user

            string input = Console.ReadLine();
            StringBuilder wordModify = new StringBuilder(input);

            Console.WriteLine(" Enter the character that needs to be  searched : ");
            char chFind = Convert.ToChar(Console.ReadLine());


            Console.WriteLine("Enter the word to be inserted : "); // for user input for word to be inserted
            string wordInsert = Console.ReadLine();

             Console.WriteLine("Enter the word to be replaced first occurence only : "); // for user input for word to be inserted
            string wordReplace = Console.ReadLine();

            StringBuilder replaced = WordReplacing(wordModify, chFind, wordReplace);
            Console.WriteLine(" Replaced Result : " + replaced.ToString());

            StringBuilder result = WordInserting(wordModify, chFind, wordInsert ) ;
            Console.WriteLine(" Result : " + result.ToString());
        }

        public StringBuilder WordInserting(StringBuilder word1, char ch, string word2)
        {
            // Convert to string to find the index of the character
            string tempString = word1.ToString();
            int index = tempString.IndexOf(ch);

            // If  target char is found , insert word2 at the pos
            if ( index != -1 )
            {
                word1.Insert ( index, word2);
            }

            return word1;
        }

        public StringBuilder WordReplacing(StringBuilder word1, char ch, string word2)
        {
            // Convert to string to find the index of the character
            string tempString = word1.ToString();
            int index = tempString.IndexOf(ch);

            // If  target char is found , replace word at the pos
            if ( index != -1 )
            {
                // Find the end of the word to be replaced
                int endIndex = index;
                while ( endIndex < tempString.Length && !Char.IsWhiteSpace(tempString[endIndex]) )
                {
                    endIndex++;
                }

                // Remove the old word and insert the new word
                word1.Remove(index, endIndex - index);
                word1.Insert(index, word2);
            }

            return word1;
        }
    }
