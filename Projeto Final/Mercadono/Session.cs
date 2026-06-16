using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  Mercadono
{
    public static class Session
{
    public static int LoggedUserId { get; set; }
    public static string LoggedUserName { get; set; }
    public static string LoggedUserEmail { get; set; }
    public static bool IsAdmin { get; set; }
}
}
    
