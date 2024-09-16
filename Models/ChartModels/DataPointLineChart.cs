using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ExpenseApplication.Models.ChartModels
{
    [DataContract]
    public class DataPointLineChart
    {
        public DataPointLineChart(string label, double y)
        {
            this.Label = label;
            this.Y = y;
        }

        [DataMember(Name = "label")]
        public string Label { get; set; }

        [DataMember(Name = "y")]
        public Nullable<double> Y { get; set; }
    }
}