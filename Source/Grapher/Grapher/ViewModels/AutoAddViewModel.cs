using System.ComponentModel;

namespace Grapher
{
    class AutoAddViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Private Members

        private AutoAddWindow Wnd { get; set; }

        #endregion


        #region Public Members

        public PolynomialExpression PolyExp { get; set; }
        public string StringExp { get; set; }

        #endregion


        public AutoAddViewModel(PolynomialExpression poly)
        {
            /* Poly */
            PolyExp = poly;

            /* Events */
            PolyExp.Monomials.CollectionChanged += Values_CollectionChanged;

            /* Window*/
            Wnd = new AutoAddWindow();
            Wnd.DataContext = this;

            Wnd.Show();
        }

        private void Values_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    foreach (MonomialMember mem in e.NewItems)
                    {
                        mem.PropertyChanged += Mem_PropertyChanged;
                        PolyExpDisplay();
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    foreach (MonomialMember mem in e.OldItems)
                    {
                        mem.PropertyChanged -= Mem_PropertyChanged;
                        PolyExpDisplay();
                    }
                    break;
                default:
                    break;
            }
        }

        private void Mem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PolyExpDisplay();
        }

        private void PolyExpDisplay()
        {
            string tmp = string.Empty;

            for (int i = PolyExp.Monomials.Count-1; i >= 0; i--)
            {
                MonomialMember member = PolyExp.Monomials[i];

                if (member.Value == 0)
                    continue;

                if(i != PolyExp.Monomials.Count-1)
                    tmp += (member.Value >= 0) ? "+ " : " ";
                
                if (member.Grade != 0)
                    tmp += member.Value.ToString() + "x^" + member.Grade.ToString() + " ";
                else
                    tmp += member.Value.ToString();
            }

            StringExp = tmp;
        }
    }
}
