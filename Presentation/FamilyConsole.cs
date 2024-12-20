﻿using System;
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
                var option = GetChoice(SelectTitle.Option, [Option.Create, Option.Clean, Option.GetNearest, 
                    Option.GetAges, Option.SetRelation, Option.PrintTree, Option.FindAncestors, Option.Exit]);

                switch (option)
                {
                    case Option.Create:
                        _tree.ExecuteAction(service => service.Create(BuildRelative()));
                        AnsiConsole.Clear();
                        break;
                    case Option.SetRelation:
                        var relation = GetRelation();
                        try
                        {
                            _tree.ExecuteAction(service => service.AddDependency(relation.source, relation.target, relation.relation));
                            break;
                        }
                        catch (Exception ex)
                        {
                            PrintMarkup(Input.RelationWrong);
                            break;
                        }
                    case Option.GetNearest:
                        PrintNearest();
                        break;
                    case Option.GetAges:
                        GetRelativesAges();
                        break;
                    case Option.PrintTree:
                        PrintFamilyTree();
                        break;
                    case Option.FindAncestors:
                        PrintCommonAncestors();
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
        
        public void GetRelativesAges()
        {
            List<Relative> relatives = _tree.GetAllRelativies();
            var relativeId = GetRelativeId(_ => true);
            var relative = relatives.First(r => r.Id == relativeId);
            PrintMarkup(Input.Parents, relative.FirstName);
            AnsiConsole.Write(new Rows(relative?.Relations
                .Select(r =>
                {
                    var parent = relatives.FirstOrDefault(rel => rel.Id == r.Key && r.Value == Relation.Parent);
                    return new Text(parent != null
                        ? $"{parent.GetRelativeAge(relative)}"
                        : "");
                })));
            AnsiConsole.WriteLine(string.Empty);
        }

        public (int source, int target, Relation relation) GetRelation()
        {
            List<Relative> relatives = _tree.GetAllRelativies();
            var sourceId = GetRelativeId(predicate: _ => true);
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

        public Relative BuildRelative()
        {
            string lastName = GetAnswer(Prompt.WhatIsLastName);
            string firstName = GetAnswer(Prompt.WhatIsName);
            string patronymic = GetAnswer(Prompt.WhatIsPatronymic);
            string date = GetAnswer(Prompt.WhatIsDateOfBirth);
            while (!date.Validate(@"^(0[1-9]|[12][0-9]|3[01])\.(0[1-9]|1[012])\.(19|20)\d\d$"))
            {
                PrintMarkup(Input.DateIncorrect);
                date = GetAnswer(Prompt.WhatIsDateOfBirth);
            }
            DateTime birthDate = DateTime.Parse(date);
            Sex sex = GetChoice(SelectTitle.Sex, [Sex.Female, Sex.Male]);
            return new(firstName, lastName, patronymic, birthDate, sex);
        }

        public void PrintFamilyTree()
        {
            List<Relative> relatives = _tree.GetAllRelativies();
            var relativeId = GetRelativeId(_ => true);
            var relative = relatives.First(r => r.Id == relativeId);

            PrintMarkup(Input.Tree, relative.FirstName, relative.LastName);

            var tree = new Tree($"[yellow]{relative.FirstName} {relative.LastName} ({relative.BirthDate:dd.MM.yyyy})[/]");

            var visited = new HashSet<int>();
            BuildFamilyTree(tree.AddNode(""), relative, relatives, visited, "Root");

            AnsiConsole.Write(tree);
        }

        public void BuildFamilyTree(TreeNode parentNode, Relative currentPerson, List<Relative> relatives, HashSet<int> visited, string context)
        {
            if (visited.Contains(currentPerson.Id))
                return;

            visited.Add(currentPerson.Id);

            if (context != "Child")
            {
                currentPerson.Relations.Where(r => r.Value == Relation.Parent).ForEach(r =>
                {
                    var parent = relatives.FirstOrDefault(p => p.Id == r.Key);
                    if (parent != null)
                    {
                        var parentNodeChild = parentNode.AddNode(
                            $"[blue]Родитель:[/] {parent.FirstName} {parent.LastName} ({parent.BirthDate:dd.MM.yyyy})"
                        );
                        BuildFamilyTree(parentNodeChild, parent, relatives, visited, "Parent");
                    }
                });
            }

            currentPerson.Relations.Where(r => r.Value == Relation.Spouse).ForEach(r =>
            {
                var spouse = relatives.FirstOrDefault(p => p.Id == r.Key);
                if (spouse != null)
                {
                    parentNode.AddNode(
                        $"[magenta]Супруг:[/] {spouse.FirstName} {spouse.LastName} ({spouse.BirthDate:dd.MM.yyyy})"
                    );
                }
            });

            if (context != "Parent")
            {
                currentPerson.Relations.Where(r => r.Value == Relation.Child).ForEach(r =>
                {
                    var child = relatives.FirstOrDefault(p => p.Id == r.Key);
                    if (child != null)
                    {
                        var childNode = parentNode.AddNode(
                            $"[green]Ребёнок:[/] {child.FirstName} {child.LastName} ({child.BirthDate:dd.MM.yyyy})"
                        );
                        BuildFamilyTree(childNode, child, relatives, visited, "Child");
                    }
                });
            }
        }

        public HashSet<int> GetAncestors(Relative person, List<Relative> relatives)
        {
            var ancestors = new HashSet<int>();

            var queue = new Queue<Relative>();
            queue.Enqueue(person);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                foreach (var relation in current.Relations.Where(r => r.Value == Relation.Parent))
                {
                    var parent = relatives.FirstOrDefault(p => p.Id == relation.Key);
                    if (parent != null && ancestors.Add(parent.Id))
                    {
                        queue.Enqueue(parent);  
                    }
                }
            }
            return ancestors;
        }

        public void PrintCommonAncestors()
        {
            List<Relative> relatives = _tree.GetAllRelativies();
            var relativeId = GetRelativeId(_ => true);
            var relative2Id = GetRelativeId(_ => true);
            var relative = relatives.FirstOrDefault(r => r.Id == relativeId);
            var relative2 = relatives.FirstOrDefault(r => r.Id == relative2Id);

            var ancestors1 = GetAncestors(relative, relatives);
            var ancestors2 = GetAncestors(relative2, relatives);

            var commonAncestors = new HashSet<int>(ancestors1);
            commonAncestors.IntersectWith(ancestors2);

            if (!commonAncestors.Any())
            {
                PrintMarkup(Input.NoAncestors, relative.FirstName, relative.LastName);
                AnsiConsole.WriteLine(string.Empty);
                return;
            }

            AnsiConsole.WriteLine($"Общие предки для {relative.FirstName} и {relative2.FirstName}:");

            var rows = commonAncestors.Select(ancestorId =>
            {
                var ancestor = relatives.FirstOrDefault(r => r.Id == ancestorId);
                return ancestor != null ? new Text($"{ancestor.FirstName} {ancestor.LastName} ({ancestor.BirthDate:dd.MM.yyyy})") : null;
            }).Where(x => x != null).ToList();

            AnsiConsole.Write(new Rows(rows));
            AnsiConsole.WriteLine(string.Empty);
        }

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
