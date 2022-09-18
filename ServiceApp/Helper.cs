using ServiceApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceApp
{
    public class Helper
    {
        public static bool _isAdmin = false;
        private static ServicesContext _context;
        public static ServicesContext GetContext()
        {
            if(_context == null) _context = new ServicesContext();
            return _context;
        }
    }
}
