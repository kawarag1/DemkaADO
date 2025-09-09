using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemkaADO.Models
{
    public class Helper
    {
        public static demka_test2Entities _context;
        public static demka_test2Entities GetContext()
        {
            if (_context == null)
            {
                _context = new demka_test2Entities();
            }
            return _context;
        }
    }
}
