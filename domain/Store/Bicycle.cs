using System;
using System.Text.RegularExpressions;

namespace Store
{
    public class Bicycle
    {
        public int ID {  get; }

        public string Title { get; }

        public string Serial_number { get; }

        public string Producer {  get; }

        public Bicycle(int id, string title, string serial_number, string producer)
        {
            ID = id;
            Title = title;
            Serial_number = serial_number;
            Producer = producer;

        }

        internal static bool IsSerial(string s)
        {

            if(s is null)
                return false;
            s = s.Replace("-", "")
                .Replace(" ", "")
                .ToUpper();

            return Regex.IsMatch(s, @"^SERIAL:\d{7}(\d{3})?$");

        }
    }
}