using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Utils;
using Entities;

namespace DAL
{
    [EnsureDbCreated]
    public class FamilyTreeService()
    {
        public List<Relative> GetRelativies() => JsonConvert.DeserializeObject<List<Relative>>(File.ReadAllText(FileBase.FilePath)) ?? [];

        public List<Relative> Create(Relative relative)
        {
            var result = GetRelativies();
            result.Add(relative with { Id = GetRelativies().Count });
            return result;
        } 

        public List<Relative> AddDependency(int source, int target, Relation relation)
        {
            var result = GetRelativies();
            result.ForEach(relative =>
            {
                if (relative.Id == source)
                    relative.Relations?.Add(target, GetReverseRelation(relation));
                if (relative.Id == target)
                    relative.Relations?.Add(source, relation);
            });
            return result;
        }

        public Relation GetReverseRelation(Relation relation) =>
            relation switch
            {
                Relation.Parent => Relation.Child,
                Relation.Child => Relation.Parent,
                Relation.Spouse => Relation.Spouse,
                _ => throw new ArgumentException(),
            };

        public List<Relative> CleanTree() => [];
    }
}
