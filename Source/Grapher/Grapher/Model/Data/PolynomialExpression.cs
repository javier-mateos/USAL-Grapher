using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace Grapher
{
    class PolynomialExpression : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<int> Values { get; set; }
        public int Grade { get; set; } = 0;
        public int XMinVal { get; set; } = 0;
        public int XMaxVal { get; set; } = 0;

        public PolynomialExpression()
        {
            Values = new ObservableCollection<int>();

            this.PropertyChanged += PolynomialExpression_PropertyChanged;
        }

        private void PolynomialExpression_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName.Equals("Grade"))
            {
                int Count = Values.Count;

                if (Values.Count > Grade)
                {
                    for (int i = 0; i < Count - Grade; i++)
                        Values.RemoveAt(Values.Count-1);
                }
                else if (Values.Count < Grade)
                {
                    for (int i = 0; i < Grade - Count; i++)
                        Values.Add(0);
                }
                    
            }
        }

        public override string ToString()
        {
            /*string tmp = string.Empty;

            foreach(int value in Values)
            {
                tmp += (value >= 0) ? "+ " : "- ";
                tmp += value.ToString() + "x^" + Values.IndexOf(value).ToString() + " ";
            }

            return tmp;*/

            return "pepe";
        }
    }
}
