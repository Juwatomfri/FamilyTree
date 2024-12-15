using System.ComponentModel;
using System.Runtime.CompilerServices;
using Utils;

namespace Entities
{
    public enum Option
    {
        [CustomString("Добавить человека в древо")]
        Create,

        [CustomString("Очистить дерево")]
        Clean,

        [CustomString("Установить отношение")]
        SetRelation,

        [CustomString("Вывести ближайших родственников")]
        GetNearest, 
        
        [CustomString("Посчитать возраст родителей")]
        GetAges,
        
        [CustomString("Изобразить семейное дерево")]
        PrintTree,
        
        [CustomString("Найти общих предков")]
        FindAncestors,

        [CustomString("Выйти из приложения")]
        Exit,
    }
}
