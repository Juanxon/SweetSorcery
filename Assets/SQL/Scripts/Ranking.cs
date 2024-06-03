using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Ranking : IComparable<Ranking>
{
    //Las 4 propiedades que tendrá el ranking
    public int Id { get; set; }
    public string Name { get; set; }
    public int Score { get; set; }
    public DateTime Date { get; set; }

    public Ranking(int id, string name, int score, DateTime date)
    {
        this.Id = id;
        this.Name = name;
        this.Score = score;
        this.Date = date;
    }

    public int CompareTo(Ranking other)
    {
        //return this.Score.CompareTo(other.Score);

        //el que recibe > que el que tiene = -1
        //el que recibe < que el que tiene = 1
        //0
        if (other.Score > this.Score)
        {
            return 1;
        }
        else if (other.Score < this.Score)
        {
            return -1;
        }
        else if (other.Date > this.Date)
        {
            return -1;
        }
        else if (other.Date < this.Date)
        {
            return 1;
        }
        return 0;

        //return other.Score.CompareTo(this.Score);
    }
}
