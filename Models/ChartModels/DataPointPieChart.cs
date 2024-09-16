using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ExpenseApplication.Models.ChartModels
{
    [DataContract]
    public class DataPointPieChart
    {
        public DataPointPieChart(string label, double y)
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
