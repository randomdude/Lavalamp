using System;
using System.Windows.Forms;

namespace ruleEngine
{
    // This will magically fix any cross-thread UI problems!
    // see http://dvanderboom.wordpress.com/2008/07/01/control-invoke-pattern-using-lambdas/

// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming
    public static class controlExtensions 
    { 
        public static void Invoke(this Control control, Action action) 
        {
            control.Invoke(action); 
        } 
    }
// ReSharper restore UnusedMember.Global
// ReSharper restore InconsistentNaming

}
