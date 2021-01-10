using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Grapher
{
    class PolynomialExpression : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<MonomialMember> Monomials{ get; set; }
        public ObservableCollection<Point2D> CalculatedPoints { get; set; }
        public int Grade { get; set; } = 0;
        public int XMinVal { get; set; } = 0;
        public int XMaxVal { get; set; } = 0;

        public PolynomialExpression()
        {
            Monomials = new ObservableCollection<MonomialMember>();
            CalculatedPoints = new ObservableCollection<Point2D>();

            this.PropertyChanged += PolynomialExpression_PropertyChanged;
        }

        private void PolynomialExpression_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName.Equals("Grade"))
            {
                int Count = Monomials.Count;

                if (Monomials.Count - 1> Grade)
                {
                    for (int i = Grade; i < Count - 1; i++)
                        Monomials.RemoveAt(Monomials.Count-1);
                }
                else if (Monomials.Count < Grade + 1)
                {
                    for (int i = Count; i < Grade + 1; i++)
                        Monomials.Add(new MonomialMember { Grade = i, Value = 0 });
                }
                    
            }
        }
    }
}
