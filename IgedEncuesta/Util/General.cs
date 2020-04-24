﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IgedEncuesta.Util
{
    public class General
    {
        public Boolean ConvertStringToInt(string intString)
        {
            
            try
            {
                int result = Int32.Parse(intString);
                return true;
            }
            catch (FormatException)
            {
                Console.WriteLine($"Unable to parse '{intString}'");
                return false;
            }
        }
    }
}