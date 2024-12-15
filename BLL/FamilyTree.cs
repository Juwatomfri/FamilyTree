using CSharpx;
using DAL;
using Entities;
using Newtonsoft.Json;
using Utils;

namespace BLL
{
    public class FamilyTree
    {
        private FamilyTreeService RelativeService { get; } = new FamilyTreeService();

        public FamilyTree()
        {
            Type type = typeof(FamilyTreeService);
            type.GetCustomAttributes(false).ForEach(attr =>
            {
                if (attr is EnsureDbCreatedAttribute dbAttr)
                    dbAttr.Ensure();
            });
        }

        public void ExecuteAction(Func<FamilyTreeService, List<Relative>> func) =>
            File.WriteAllText(FileBase.FilePath, JsonConvert.SerializeObject(func(RelativeService)));

        public List<Relative> GetAllRelativies() => RelativeService.GetRelativies();
    }
}
