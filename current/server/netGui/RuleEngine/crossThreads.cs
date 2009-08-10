using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Linq.Expressions;

namespace netGui.RuleEngine
{
    // This will magically fix any cross-thread UI problems!
    // see http://dvanderboom.wordpress.com/2008/07/01/control-invoke-pattern-using-lambdas/
    public static class ControlExtensions 
    { 
        public static void Invoke(this Control Control, Action Action) 
        {
            Control.Invoke(Action); 
        } 
    } 
}
