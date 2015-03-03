using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Company
{
    public string Date { get; set; }
    public Decimal Open { get; set; }
    public Decimal High { get; set; }
    public Decimal Low { get; set; }
    public Decimal Close { get; set; }
    public double Volume { get; set; }
    public Decimal AdjClose { get; set; }

    public Company(string d, Decimal o, Decimal h, Decimal l, Decimal c, double v, Decimal ac)
    {
        Date = d;
        Open = o;
        High = h;
        Low = l;
        Close = c;
        Volume = v;
        AdjClose = ac;
    }
    
}
