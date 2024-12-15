using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Entities
{
    public enum Relation
    {
        [CustomString("Родитель")]
        Parent,

        [CustomString("Ребёнок")]
        Child,

        [CustomString("Муж/Жена")]
        Spouse
    }
}
