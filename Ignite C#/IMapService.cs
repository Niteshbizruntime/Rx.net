
namespace Apache.Ignite.Examples.Services
{
    using Apache.Ignite.ExamplesDll.Services;

   
    public interface IMapService<TK, TV>
    {
       
        void Put(TK key, TV value);

       
        TV Get(TK key);

        void Clear();

      
        int Size { get; }
    }
}