using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Searcher.Panels
{
    public static class Helper
    {
        public static bool IsValidObject(DependencyObject obj)
        {
            // The dependency object is valid if it has no errors, 
            //and all of its children (that are dependency objects) are error-free.
            return !Validation.GetHasError(obj) &&
                   LogicalTreeHelper.GetChildren(obj)
                       .OfType<DependencyObject>()
                       .All(child => IsValidObject(child));
        }

        public static bool IsValid(this UserControl ctrl)
        {
            return IsValidObject(ctrl);
        }
    }
}