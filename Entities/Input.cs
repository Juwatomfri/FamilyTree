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
        [CustomString("Родители [green]{0}[/]:")]
        Parents,
        [CustomString("Семейно дерево для человека по имени [green]{0}[/]:")]
        Tree,
        [CustomString("Формат даты [green] неверный[/], повторите ввод:")]
        DateIncorrect,
        [CustomString("Отношение между этими людьми уже [green]есть[/] в базе")]
        RelationWrong,
        [CustomString("У этих бедолаг [green]нет[/] общих предков :")]
        NoAncestors,

    }
}
