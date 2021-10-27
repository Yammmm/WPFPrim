using System;
using System.Collections.Generic;
using System.Text;

namespace WmsPrism.Unit
{
    public  interface IRequestFocus
    {
        event EventHandler<FocusRequestedEventArgs> FocusRequested;
    }
    public class FocusRequestedEventArgs : EventArgs 
    {
        public FocusRequestedEventArgs(string propertyName)
        {
            PropertyName= propertyName;
        }

        public string PropertyName { get; private set; }
    }
}
