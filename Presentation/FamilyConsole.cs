using System.Linq;
using System.Reflection;
using BLL;
using CSharpx;
using Entities;
using Spectre.Console;
using Utils;

namespace Presentation
{
    public class FamilyConsole
    {
        private FamilyTree _tree;

        public FamilyConsole()
        {
            _tree = new FamilyTree();
        }

        public void Start()
        {
            var isRunning = true;
            while (isRunning)
            {
                var option = GetChoice(SelectTitle.Option, [Option.Create, Option.Clean, Option.GetNearest, Option.SetRelation, Option.Exit]);

                switch (option)
                {
                    case Option.Create:
                        _tree.ExecuteAction(service => service.Create(BuildRelative()));
                        AnsiConsole.Clear();
                        break;
                    case Option.SetRelation:
                        var relation = GetRelation();
                        _tree.ExecuteAction(service => service.AddDependency(relation.source, relation.target, relation.relation));
                        break;
                    case Option.GetNearest:
                        PrintNearest();
                        break;
                    case Option.Clean:
                        _tree.ExecuteAction(service => service.CleanTree());
                        break;
                    case Option.Exit:
                        isRunning = false;
                        break;
                }
            }
        }

        public void PrintNearest()
        {
            List<Relative> relatives = _tree.GetAllRelativies();
            var relativeId = GetRelativeId(_ => true);
            var relative = relatives.First(r => r.Id == relativeId);
            PrintMarkup(Input.Nearest, relative.FirstName);
            AnsiConsole.Write(new Rows(relative?.Relations.Select(r => new Text($"{relatives.First(rel => rel.Id == r.Key).GetFullName()} - {r.Value.ToCustomString()}"))));
            AnsiConsole.WriteLine(string.Empty);
        }

        public (int source, int target, Relation relation) GetRelation()
        {
            List<Relative> relatives = _tree.GetAllRelativies();
            var sourceId = GetRelativeId(predicate:_ => true);
            var targetId = GetRelativeId(predicate: r => r.Id != sourceId);
            var relation = GetChoice(SelectTitle.Relation, [Relation.Parent, Relation.Child, Relation.Spouse],
                relatives.First(r => r.Id == sourceId).FirstName, relatives.First(r => r.Id == targetId).FirstName);

            return (sourceId, targetId, relation);
        }

        public int GetRelativeId(Func<Relative, bool> predicate) =>
            int.Parse(AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title(SelectTitle.Entity.ToCustomString())
                    .AddChoices(_tree.GetAllRelativies().Where(predicate).Select(r => r.ToConsoleOutput())))
                .Split().ElementAtOrDefault(0) ?? throw new ArgumentException());

        public Relative BuildRelative() =>
            new(firstName: GetAnswer(Prompt.WhatIsName),
                lastName: GetAnswer(Prompt.WhatIsLastName),
                patronymic: GetAnswer(Prompt.WhatIsPatronymic),
                birthDate: DateTime.Now,
                sex: GetChoice(SelectTitle.Sex, [Sex.Female, Sex.Male]));
                

        public T GetChoice<T>(SelectTitle title, T[] choices, params object[] objs) where T : Enum =>
            AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title(string.Format(title.ToCustomString(), objs.Select(o => o.ToString()).ToArray()))
                    .AddChoices(choices.Select(c => c.ToCustomString()))).FromCustomString<T>();

        public string GetAnswer(Prompt prompt) =>
            AnsiConsole.Prompt(new TextPrompt<string>(prompt.ToCustomString()));

        public void PrintMarkup(Input input, params object[] objs) =>
            AnsiConsole.Markup(string.Format(input.ToCustomString(), objs.Select(o => o.ToString()).ToArray()) + "\n");
    }
}
