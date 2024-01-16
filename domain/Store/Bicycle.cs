using System;

namespace Store
{
    public class Bicycle
    {
        public int ID {  get; }
        public string Title { get; }
        public Bicycle(int id, string title)
        {
            ID = id;
            Title = title;

        }
    }
}