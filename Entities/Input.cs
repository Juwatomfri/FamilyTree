using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Entities
{
    public enum Input
    {
        [CustomString("Ближайшие родственники [green]{0}[/]:")]
        Nearest,
        [CustomString("Формат даты [green] неверный[/], повторите ввод:")]
        DateIncorrect,
        [CustomString("Возраст [green] неверный[/], повторите ввод:")]
        ParentsAges
    }
}
