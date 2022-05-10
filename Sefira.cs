using System;
using Humanizer;
using System.Globalization;

namespace HelloGithubClassroom
{
    interface ISefirasHaomer
    {
        int DayInOmer();//returns 1-49 if current day is in Omer and 0 if is not in Omer
        string OmerPhrase(int dayInOmer, CultureInfo cu); //returns the appropriate text for the specified dayInOmer and culture. e.g. "Today is the first day of the Omer".
        void SetStartDayForOmer(DateTime dt);//sets the start of omer for the current year. If that day has passed set for the upcoming year
    }
    public class Sefira : ISefirasHaomer
    {
        private DateTime firstDay;
        public int DayInOmer()
        {
            if (DateTime.Today >= firstDay && DateTime.Today < firstDay.AddDays(49)) //not <= because firstDay is 1, so 1 + 49 = 50, less than 50, not <=
            {                
                return (int)(DateTime.Today - firstDay).TotalDays + 1;
            }
            else { return 0; }
        }

        public string OmerPhrase(int dayInOmer, CultureInfo cu)
        {
            if (dayInOmer < 1 || dayInOmer >  49)
            {
                throw new ArgumentOutOfRangeException("Can only enter between 1 and 49 days");
            }
            if (cu.TwoLetterISOLanguageName.Equals("en"))
            {
                return generateSefiraEnglish(dayInOmer);
            }
            if (cu.TwoLetterISOLanguageName.Equals("he"))
            {
                return generateSefiraHebrew(dayInOmer);
            }
            throw new ArgumentException("Can only produce results in Hebrew and English");
        }

        public void SetStartDayForOmer(DateTime dt)
        {
            firstDay = dt;
            Console.WriteLine(firstDay);
        }

        String generateSefiraEnglish(int i)
        {

            String s = $"Today is {i.ToWords()} day";
            if (i != 1)
            {
                s += "s";
            }

            if (i / 7 >= 1)
            {
                s += $", which are {(i / 7).ToWords()} week";
                if (i / 7 > 1) { s += "s"; }
                if (i % 7 != 0)
                {
                    s += $" and {(i % 7).ToWords()} day";
                    if (i % 7 > 1) { s += "s,"; }
                    else { s += ","; }
                }
            }

            s += " of the Omer.";
            return s;

        }


        String generateSefiraHebrew(int i)
        {

            String s = $"היום ";
            if (i == 1) { s += "יום אחד"; } //1st day
            if (i > 1 && i < 11) // days 2 - 10 (Yomim as opposed to Yom)
            {

                s += $"{i.ToWords(GrammaticalGender.Masculine, CultureInfo.GetCultureInfo("he-IL"))} ימים";
            }
            if (i > 10 && i < 21) // days 11 - 20 (Yom as opposed to Yomim)
            {
                s += $"{i.ToWords(GrammaticalGender.Masculine, CultureInfo.GetCultureInfo("he-IL"))} יום";
            }

            if (i > 20) //days 21 - 49, ones come before tens
            {
                if (i % 10 == 0) { s += $"{i.ToWords(GrammaticalGender.Masculine, CultureInfo.GetCultureInfo("he-IL"))}"; } //there are no ones
                else
                {
                    String tens = $"{(i - (i % 10)).ToWords(GrammaticalGender.Masculine, CultureInfo.GetCultureInfo("he-IL"))}";
                    string ones = $"{(i % 10).ToWords(GrammaticalGender.Masculine, CultureInfo.GetCultureInfo("he-IL"))}";
                    s += ones + " ו" + tens;
                }
                s += " יום";
            }

            if (i / 7 == 1) //1st week
            {
                s += ", שהם שבוע אחד,";
            }

            if (i / 7 > 1) // weeks 2 - 7 + remaining days (after current week)
            {
                s += ", שהם ";
                s += $"{(i / 7).ToWords(GrammaticalGender.Masculine, CultureInfo.GetCultureInfo("he-IL"))}";
                s += " שבועות";
                if (i % 7 == 1) { s += " ויום אחד,"; }
                if (i % 7 > 1) { s += $" ו{(i % 7).ToWords(GrammaticalGender.Masculine, CultureInfo.GetCultureInfo("he-IL"))} ימים,"; }
                if (i % 7 == 0) { s += ","; }
            }
            s += " לעומר";
            s = s.Replace("שניים", "שני");
            s = s.Replace("שלושה", "שלשה");
            s = s.Replace("חמישה", "חמשה");
            s = s.Replace("שישה", "ששה");
            return s;

        }
    }
}
