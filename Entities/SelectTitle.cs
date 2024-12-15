using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Entities
{
    public enum SelectTitle
    {
        [CustomString("Выберите [green]действие[/]:")]
        Option,

        [CustomString("Выберите [green]пол[/]:")]
        Sex,

        [CustomString("Выберите [green]сущность[/]:")]
        Entity,

        [CustomString("Кем [green]{0}[/] приходится [green]{1}[/]:")]
        Relation
    }
}
