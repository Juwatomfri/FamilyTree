using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Entities
{
    public enum Sex
    {
        [CustomString("Мужской")]
        Male,

        [CustomString("Женский")]
        Female
    }
}
