using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Rover.Controls
{
    /// <summary>
    /// Implementation Notes:
    /// Exists because GridViewColumn.DisplayMemberBinding overwrites/has higher priority than XAML CellTemplates<br/>
    /// ie. DisplayMemberBinding has to be null for CellTemplates to work<br/>
    /// but SortGridView requires DisplayMemberBinding in order to sort, so this class is to provide the binding instead.<br/>
    /// </summary>
    public class SortedGridViewColumn : GridViewColumn
    {
        BindingBase _memberBinding;
        
        /// <summary>
        /// Custom property to provide the Model member binding without affecting the view
        /// </summary>
        public BindingBase MemberBinding { get => _memberBinding; set
            {
                _memberBinding = value;
                OnMemberBindingChanged();
            }}

        private void OnMemberBindingChanged()
        {
            OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(nameof(MemberBinding)));
        }
    }
}
