using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Entities
{
    public enum Prompt
    {
        [CustomString("Введите [green]имя[/]:")]
        WhatIsName,

        [CustomString("Введите [green]фамилию[/]:")]
        WhatIsLastName,

        [CustomString("Введите [green]отчество[/]:")]
        WhatIsPatronymic,
        
        [CustomString("Введите [green]дату рождения[/]:")]
        WhatIsDateOfBirth,
    }
}
