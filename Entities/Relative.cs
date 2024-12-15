using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Entities
{
    public record Relative
    {
        public int Id { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string Patronymic { get; init; }
        public DateTime BirthDate { get; init; }
        public Sex Sex { get; init; }
        public Dictionary<int, Relation>? Relations { get; } = [];

        public Relative(string firstName, string lastName, string patronymic, DateTime birthDate, Sex sex) 
        {
            FirstName = firstName;
            LastName = lastName;
            Patronymic = patronymic;
            BirthDate = birthDate;
            Sex = sex;
        }

        public string GetFullName() => string.Join(' ', [LastName, FirstName, Patronymic]);

        public string ToConsoleOutput() => $"{Id} {GetFullName()}";
    }
}
