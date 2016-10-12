/*    Lawrenceville Press TopListClass type IMPLEMENTATION */
/*		September 1998                                         */

//-----------------------------------------------------

/*
 *   Modified for ICS3U by Michael Trink
 *   Date: October 2010
 *   Revisions: Converted to C# 
 *   Design notes: This class file is NOT an examplar of how
 *                 to code in C#.
 *   Known issue: Program is non-deterministic with identical scores.
 * 
 *   */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SpaceInvaders
{
    class TopList
    {
        public TopList(String theFilename, int theMaxItems = 10)
        /* Post: TopListClass object contents have been loaded from file
        if it exists, or loaded with empty items if not.              */
        {
            entryList = new List<Entry>();
            maxItems = theMaxItems;
            fileName = theFilename;

            StreamReader inputFileReader;
            if (File.Exists(theFilename))
            {
                inputFileReader = File.OpenText(theFilename);
            }
            else
            {
                StreamWriter blankage = File.CreateText(theFilename);
                for (int curItem = 0; curItem < theMaxItems; curItem++)
                {
                    blankage.WriteLine("Empty");
                    blankage.WriteLine(0);
                }
                blankage.Close();
            }
            inputFileReader = File.OpenText(theFilename);

            string curName;
            long curScore;
            for (int curItem = 0; curItem < theMaxItems; curItem++)
            {
                curName = inputFileReader.ReadLine();
                curScore = long.Parse(inputFileReader.ReadLine());
                Entry nextEntry = new Entry(curScore, curName);
                entryList.Add(nextEntry);
            }

            entryList.Sort();
        }

        ~TopList()
        {
            /* Pre: TopListClass object exists
               Post: object destroyed, and data stored */
            StreamWriter outputFileWriter = File.CreateText(fileName);
            foreach (Entry myEntry in entryList)
            {
                outputFileWriter.WriteLine(myEntry.name);
                outputFileWriter.WriteLine(myEntry.score);
            }
            outputFileWriter.Close();
        }

        public string GetName(int theRank)
        /* Pre: Rank in range 1..MaxItems
           Post: Name whose score is in given Rank is returned */
        {
            Entry desiredEntry = entryList.ElementAt(maxItems - theRank - 1);
            return desiredEntry.name;
        }

        public long GetScore(int theRank)
        /* Pre: Rank in range 1..MaxItems
            Post: Score in given Rank is returned */
        {
            Entry desiredEntry = entryList.ElementAt(maxItems - theRank - 1);
            return desiredEntry.score;
        }

        public void AddItem(long theScore, string theName)
        /* Post: Score/Name added to TopList object if Score
             is greater than current lowest in list */
        {
            entryList.Add(new Entry(theScore, theName));
            entryList.Sort();
            //remove the smallest entry
            entryList.RemoveAt(entryList.Count - 1);
        }

        public void Clear()
        /* Post: TopListClass object has been cleared back to
            empty items.                                       */
        {
            entryList.Clear();
            for (int i = 0; i < maxItems; i++)
            {
                entryList.Add(new Entry());
            }
        }

        public int GetListSize()
        /* Post: Size of the list returned as specified in the
            constructor default = 10)                           */
        {
            return maxItems;
        }

        public void display(Graphics g, Font font, int xLocation, int yLocation)
        /* Post: TopList names and scores inserted in os one per line;
            os returned                                                 */
        {
            g.DrawString("Rank  Name                 Score", font, Brushes.Yellow, new Point(xLocation, yLocation));
            for (int i = 0; i < entryList.Count; i++)
            {
                g.DrawString((i + 1).ToString(), font, Brushes.Yellow, new Point(xLocation + 35, yLocation + 30 + i * 30));
                g.DrawString(entryList.ElementAt(i).name, font, Brushes.Yellow, new Point(xLocation + 75, yLocation + 30 + i * 30));
                g.DrawString((entryList.ElementAt(i).score).ToString(), font, Brushes.Yellow, new Point(xLocation + 335, yLocation + 30 + i * 30));
            }
        }

        /* This is a little inner class to store the information for each
         *  Entry. Alternatively, two lists could have been used; 
         *  The : IComparable<Entry> will allow us to use the List's Sort.
         *  We also have to implement CompareTo         
         */
        private class Entry : IComparable<Entry>
        {
            //This is the constructor for new entries
            public Entry(long theScore = 0, string theName = "Empty")
            {
                name = theName;
                score = theScore;
            }

            /*The stuff below is just to show an example of using properties
             * you could just use public variables... but that *could* be bad.
             * Properties allow us to check things before allowing variables to be 
             * set. For example, we could check to make sure a score is positive before
             * assigning it.*/
            string m_name = "Empty";
            public string name
            {
                get
                {
                    return m_name;
                }
                set
                {
                    m_name = value;
                }
            }

            long m_score = 0;

            public long score
            {
                get
                {
                    return m_score;
                }
                set
                {
                    m_score = value;
                }
            }

            /*will allow for easy sorting, when it compares entries, the scores
                will be compared   
             */
            public int CompareTo(Entry obj)
            {
                return (obj.score.CompareTo(score));
            }

        }

        //our variable to store all the entries
        private List<Entry> entryList;

        string fileName;  // File name to store data
        int maxItems;

    }
}

